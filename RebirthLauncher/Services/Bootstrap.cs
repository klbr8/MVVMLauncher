// RebirthLauncher/Services/Bootstrap.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using RebirthLauncher.Models;
using RebirthLauncher.Utilities;
using RebirthLauncher.Constants;
using RebirthLauncher.Dialogs;

namespace RebirthLauncher.Services
{
    public class Bootstrap
    {
        private readonly HttpClient _http;
        private bool _loggingEnabled;
        private readonly string _settingsPath = LauncherConstants.SettingsFilePath;
        private readonly string _serversPath = LauncherConstants.ServersFilePath;
        private readonly string _logPath = LauncherConstants.LogFilePath;

        public Bootstrap()
        {
            _http = ((App)Application.Current).HttpClient;
        }

        public async Task RunAsync(string[] args, CancellationToken cancellationToken = default)
        {
            ParseCommandLineArgs(args);

            try
            {
                if (IsUninstallMode(args))
                {
                    await RunUninstallAsync();
                    return;
                }

                LogInfo("Bootstrap starting");

                var installPath = EnsureSettings();
                if (string.IsNullOrWhiteSpace(installPath))
                {
                    ShowErrorAndExit("Install path not provided. Exiting.");
                    return;
                }

                var servers = LoadServerList();
                if (servers == null || servers.Count == 0)
                {
                    LogWarning("No valid servers found in servers.xml; attempting embedded default.");
                    servers = LoadEmbeddedDefaultServers();
                    if (servers == null || servers.Count == 0)
                    {
                        ShowErrorAndExit("No servers available. Exiting.");
                        return;
                    }
                }

                Server validatedServer = null;
                Manifest validatedManifest = null;

                foreach (var server in servers)
                {
                    if (cancellationToken.IsCancellationRequested) break;

                    try
                    {
                        var manifestBytes = await FetchBytesWithRetryAsync(server.ManifestUrl, cancellationToken);
                        if (manifestBytes == null)
                        {
                            LogWarning($"Failed to fetch manifest for server {server.Name}");
                            continue;
                        }

                        var md5 = ComputeMd5Hex(manifestBytes);
                        if (!string.IsNullOrWhiteSpace(server.ManifestHash) && !string.Equals(md5, server.ManifestHash, StringComparison.OrdinalIgnoreCase))
                        {
                            LogWarning($"Manifest hash mismatch for server {server.Name} (expected {server.ManifestHash}, got {md5})");
                            // Treat as unvalidated; skip to next server
                            continue;
                        }

                        // Parse manifest (trusted). If parsing fails, skip server.
                        var manifest = DeserializeManifest(manifestBytes);
                        if (manifest == null)
                        {
                            LogWarning($"Failed to parse manifest for server {server.Name}");
                            continue;
                        }

                        // Mark validated and stop
                        validatedServer = server;
                        validatedManifest = manifest;
                        LogInfo($"Validated server {server.Name}");
                        break;
                    }
                    catch (Exception ex)
                    {
                        LogException(ex, $"Validating server {server.Name}");
                        // try next server
                    }
                }

                if (validatedServer == null)
                {
                    ShowErrorAndExit("Unable to validate any server. Exiting.");
                    return;
                }

                // Version check
                var versionOk = await CheckRemoteVersionAsync(cancellationToken);
                if (!versionOk)
                {
                    var proceed = AskUserToProceedDespiteVersion();
                    if (!proceed)
                    {
                        LogInfo("User chose not to proceed after version check.");
                        return;
                    }
                }

                // Start main UI
                StartMainWindow(validatedServer, validatedManifest, installPath);
            }
            catch (Exception ex)
            {
                LogException(ex, "Unhandled exception in bootstrap");
                ShowErrorAndExit("An unexpected error occurred during startup. See log for details.");
            }
            finally
            {
                LogInfo("Bootstrap finished");
            }
        }

        private void ParseCommandLineArgs(string[] args)
        {
            _loggingEnabled = args?.Any(a => string.Equals(a, "-l", StringComparison.OrdinalIgnoreCase)) ?? false;
            // -u handled in RunAsync entry
            if (_loggingEnabled) LogInfo("Logging enabled via CLI");
        }

        private bool IsUninstallMode(string[] args)
        {
            return args?.Any(a => string.Equals(a, "-u", StringComparison.OrdinalIgnoreCase)) ?? false;
        }

        private async Task RunUninstallAsync()
        {
            try
            {
                LogInfo("Running uninstall flow");
                // Minimal uninstall: remove settings and servers files created by launcher, keep user game files
                if (File.Exists(_settingsPath)) File.Delete(_settingsPath);
                if (File.Exists(_serversPath)) File.Delete(_serversPath);
                if (File.Exists(_logPath)) File.Delete(_logPath);

                // Show a simple confirmation dialog
                MessageBox.Show("Launcher uninstalled. Settings and logs removed.", "Uninstall", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LogException(ex, "Uninstall");
                MessageBox.Show("Uninstall encountered an error. See log for details.", "Uninstall Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            await Task.CompletedTask;
        }

        private string EnsureSettings()
        {
            try
            {
                var settings = ReadSettings();
                if (settings != null && !string.IsNullOrWhiteSpace(settings.InstallPath))
                {
                    return settings.InstallPath;
                }

                // Show initializing dialog to collect install path
                var chosen = ShowInitializingDialog();
                if (string.IsNullOrWhiteSpace(chosen))
                {
                    LogInfo("User cancelled install path selection");
                    return null;
                }

                // Persist settings
                WriteSettings(new Settings { InstallPath = chosen });
                return chosen;
            }
            catch (Exception ex)
            {
                LogException(ex, "EnsureSettings");
                return null;
            }
        }

        private Settings ReadSettings()
        {
            try
            {
                if (!File.Exists(_settingsPath)) return null;
                var serializer = new XmlSerializer(typeof(Settings));
                using (var fs = File.OpenRead(_settingsPath))
                {
                    return (Settings)serializer.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                LogException(ex, "ReadSettings");
                return null;
            }
        }

        private void WriteSettings(Settings settings)
        {
            try
            {
                var dir = Path.GetDirectoryName(_settingsPath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                var serializer = new XmlSerializer(typeof(Settings));
                using (var fs = File.Create(_settingsPath))
                {
                    serializer.Serialize(fs, settings);
                }
            }
            catch (Exception ex)
            {
                LogException(ex, "WriteSettings");
            }
        }

        private string ShowInitializingDialog()
        {
            try
            {
                // Use existing dialog ViewModel to show folder picker and return chosen path
                var vm = new InitializingDialogViewModel();
                var dlg = new InitializingDialog { DataContext = vm };
                var result = dlg.ShowDialog();
                if (result == true)
                {
                    var chosen = vm.InstallPath;
                    if (!string.IsNullOrWhiteSpace(chosen) && !Directory.Exists(chosen))
                    {
                        Directory.CreateDirectory(chosen);
                    }
                    return chosen;
                }
                return null;
            }
            catch (Exception ex)
            {
                LogException(ex, "ShowInitializingDialog");
                return null;
            }
        }

        private List<Server> LoadServerList()
        {
            try
            {
                if (!File.Exists(_serversPath))
                {
                    LogWarning("servers.xml not found");
                    return new List<Server>();
                }

                var serializer = new XmlSerializer(typeof(ServerList));
                using (var fs = File.OpenRead(_serversPath))
                {
                    var list = (ServerList)serializer.Deserialize(fs);
                    var servers = list?.Servers ?? new List<Server>();
                    // Sanitize and validate minimal fields
                    var valid = new List<Server>();
                    foreach (var s in servers)
                    {
                        s.Name = s.Name?.Trim();
                        s.ManifestUrl = s.ManifestUrl?.Trim();
                        if (string.IsNullOrWhiteSpace(s.Name) || string.IsNullOrWhiteSpace(s.ManifestUrl))
                        {
                            LogWarning($"Skipping invalid server entry (missing Name or ManifestUrl)");
                            continue;
                        }
                        valid.Add(s);
                    }
                    return valid;
                }
            }
            catch (Exception ex)
            {
                LogException(ex, "LoadServerList");
                return new List<Server>();
            }
        }

        private List<Server> LoadEmbeddedDefaultServers()
        {
            try
            {
                // If you have an embedded resource or hardcoded default, return it here.
                // For now return empty list; replace with your default server construction.
                return new List<Server>();
            }
            catch (Exception ex)
            {
                LogException(ex, "LoadEmbeddedDefaultServers");
                return new List<Server>();
            }
        }

        private async Task<byte[]> FetchBytesWithRetryAsync(string url, CancellationToken cancellationToken)
        {
            const int attempts = 2;
            const int delayMs = 500;

            for (int i = 0; i < attempts; i++)
            {
                try
                {
                    var resp = await _http.GetAsync(url, cancellationToken).ConfigureAwait(false);
                    if (!resp.IsSuccessStatusCode)
                    {
                        LogWarning($"HTTP {(int)resp.StatusCode} fetching {url}");
                        // fall through to retry or return null
                    }
                    else
                    {
                        return await resp.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                    }
                }
                catch (Exception ex) when (i < attempts - 1)
                {
                    LogException(ex, $"Fetch attempt {i + 1} failed for {url}");
                    await Task.Delay(delayMs, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    LogException(ex, $"Fetch failed for {url}");
                    return null;
                }
            }

            return null;
        }

        private string ComputeMd5Hex(byte[] data)
        {
            try
            {
                using (var md5 = MD5.Create())
                {
                    var hash = md5.ComputeHash(data);
                    var sb = new StringBuilder(hash.Length * 2);
                    foreach (var b in hash) sb.Append(b.ToString("x2"));
                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {
                LogException(ex, "ComputeMd5Hex");
                return null;
            }
        }

        private Manifest DeserializeManifest(byte[] bytes)
        {
            try
            {
                using (var ms = new MemoryStream(bytes))
                {
                    var serializer = new XmlSerializer(typeof(Manifest));
                    return (Manifest)serializer.Deserialize(ms);
                }
            }
            catch (Exception ex)
            {
                LogException(ex, "DeserializeManifest");
                return null;
            }
        }

        private async Task<bool> CheckRemoteVersionAsync(CancellationToken cancellationToken)
        {
            try
            {
                var url = LauncherConstants.VersionUrl;
                var bytes = await FetchBytesWithRetryAsync(url, cancellationToken).ConfigureAwait(false);
                if (bytes == null) return true; // if we can't fetch, allow proceed (non-blocking)
                var remote = Encoding.UTF8.GetString(bytes).Trim();
                var local = ReleaseVersion.Current?.ToString() ?? string.Empty;
                if (string.Equals(remote, local, StringComparison.OrdinalIgnoreCase)) return true;

                // remote differs
                LogInfo($"Remote version {remote} differs from local {local}");
                return false;
            }
            catch (Exception ex)
            {
                LogException(ex, "CheckRemoteVersion");
                return true;
            }
        }

        private bool AskUserToProceedDespiteVersion()
        {
            var res = MessageBox.Show("A newer launcher version is available. Continue anyway?", "Launcher Update", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return res == MessageBoxResult.Yes;
        }

        private void StartMainWindow(Server server, Manifest manifest, string installPath)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var main = new MainWindow();
                // Pass data into MainWindow or its ViewModel as needed
                if (main.DataContext is MainWindowViewModel vm)
                {
                    vm.Initialize(server, manifest, installPath);
                }
                main.Show();
            });
        }

        private void ShowErrorAndExit(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Application.Current.Shutdown();
        }

        private void LogInfo(string message)
        {
            if (!_loggingEnabled) return;
            AppendLog("INFO", message, null);
        }

        private void LogWarning(string message)
        {
            if (!_loggingEnabled) return;
            AppendLog("WARN", message, null);
        }

        private void LogException(Exception ex, string context)
        {
            if (!_loggingEnabled) return;
            AppendLog("ERROR", context, ex);
        }

        private void AppendLog(string level, string context, Exception ex)
        {
            try
            {
                var dir = Path.GetDirectoryName(_logPath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                // Simple rotation
                const long maxBytes = LauncherConstants.LogMaxBytes;
                if (File.Exists(_logPath))
                {
                    var fi = new FileInfo(_logPath);
                    if (fi.Length > maxBytes)
                    {
                        var rotated = _logPath + ".1";
                        if (File.Exists(rotated)) File.Delete(rotated);
                        File.Move(_logPath, rotated);
                    }
                }

                using (var sw = File.AppendText(_logPath))
                {
                    sw.WriteLine($"{DateTime.UtcNow:O} [{level}] {context}");
                    if (ex != null)
                    {
                        sw.WriteLine(ex.ToString());
                    }
                }
            }
            catch
            {
                // Swallow logging errors to avoid recursive failures
            }
        }
    }
}