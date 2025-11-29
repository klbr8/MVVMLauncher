// RebirthLauncher/Dialogs/InitializingDialog.xaml.cs
using System.IO;
using System.Windows;
using RebirthLauncher.ViewModels;
using Microsoft.Win32; // OpenFolderDialog

namespace RebirthLauncher.Dialogs
{
    public partial class InitializingDialog : Window
    {
        private InitializingDialogViewModel? _vm;

        public InitializingDialog()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current?.MainWindow;

            if (DataContext is InitializingDialogViewModel existingVm)
            {
                AttachViewModel(existingVm);
            }
            else
            {
                var vm = new InitializingDialogViewModel();
                AttachViewModel(vm);
                DataContext = vm;
            }
        }

        private void AttachViewModel(InitializingDialogViewModel vm)
        {
            if (_vm != null)
            {
                _vm.RequestClose -= OnRequestClose;
            }

            _vm = vm;
            _vm.RequestClose += OnRequestClose;

            // Provide a folder picker implementation using .NET 8 OpenFolderDialog.
            // When the picker returns a folder, update the VM so the textbox reflects it immediately.
            _vm.FolderPicker = () =>
            {
                try
                {
                    var dlg = new OpenFolderDialog
                    {
                        Title = "Select the game install folder",
                        // Optionally set InitialDirectory from current VM value
                        InitialDirectory = string.IsNullOrWhiteSpace(vm.InstallPath) ? null : vm.InstallPath
                    };

                    var ok = dlg.ShowDialog(this);
                    return ok == true ? dlg.FolderName : null;
                }
                catch
                {
                    return null;
                }
            };
        }

        private void OnRequestClose(bool? result)
        {
            // Ensure UI thread
            Dispatcher.Invoke(() =>
            {
                // If user pressed OK, validate/create the folder before closing.
                if (result == true && _vm != null)
                {
                    var path = _vm.InstallPath?.Trim();
                    if (string.IsNullOrWhiteSpace(path))
                    {
                        MessageBox.Show(this, "Please choose a valid install folder.", "Invalid Path", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return; // keep dialog open
                    }

                    try
                    {
                        // Normalize and get full path
                        var full = Path.GetFullPath(path);

                        // Create directory if it doesn't exist (user intent to create is respected)
                        if (!Directory.Exists(full))
                        {
                            Directory.CreateDirectory(full);
                        }

                        // Update VM with normalized path so the textbox shows the canonical path
                        _vm.InstallPath = full;

                        // Unsubscribe and close with DialogResult = true
                        _vm.RequestClose -= OnRequestClose;
                        DialogResult = true;
                        Close();
                        return;
                    }
                    catch (IOException ioEx)
                    {
                        MessageBox.Show(this, $"Unable to create or access the folder:\n{ioEx.Message}", "Folder Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return; // keep dialog open
                    }
                    catch
                    {
                        MessageBox.Show(this, "An unexpected error occurred while preparing the folder.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return; // keep dialog open
                    }
                }

                // Cancel or no VM: just close
                if (_vm != null)
                {
                    _vm.RequestClose -= OnRequestClose;
                }
                DialogResult = result;
                Close();
            });
        }

        protected override void OnClosed(System.EventArgs e)
        {
            if (_vm != null)
            {
                _vm.RequestClose -= OnRequestClose;
                _vm = null;
            }
            base.OnClosed(e);
        }
    }
}