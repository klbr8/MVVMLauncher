# Project Files

This document provides the current directory and file structure for the
launcher project. It is updated dynamically as files are added, removed,
or modified. If a file is not listed here, it does not exist in the project yet.

---

## Directory Tree
📁 RebirthLauncher
├─📁 Constants
│  └─📄 LauncherConstants.cs
├─📁 Converters
│  ├─📄 BooleanToVisibilityConverter.cs
│  ├─📄 RoundEndRadiusConverter.cs
│  └─📄 UrlToBitmapImageConverter.cs
├─📁 Dialogs
│  ├─📄 InitializingDialog.xaml
│  └─📄 SplashScreen.xaml
├─📁 Enums
│  └─📄 AppState.cs
├─📁 Helpers
│  └─📄 FileHelper.cs
├─📁 Models
│  ├─📄 BrowseButton.cs
│  ├─📄 DownloadProgress.cs
│  ├─📄 FileProgress.cs
│  ├─📄 Manifest.cs
│  ├─📄 ReleaseVersion.cs
│  ├─📄 Server.cs
│  └─📄 ServerList.cs
├─📁 Resources
│  └─📄 Converters.xaml
├─📁 Services
│  └─📄 Bootstrap.cs
├─📁 Styles
│  ├─📄 ButtonStyles.xaml
│  └─📄 OverlayStyles.xaml
├─📁 Utilities
│  └─📄 HashHelper.cs
├─📁 ViewModels
│  ├─📄 InitializingDialogViewModel.cs
│  └─📄 SplashScreenViewModel.cs
├─📄 App.xaml
├─📄 AssemblyInfo.cs
├─📄 Charter.md
├─📄 Implementation.md
├─📄 MainWindow.xaml
├─📄 Project_Files.md
└─📄 Workflow.md

---

## URL Construction Rule

To access the raw GitHub URL for any file listed in the directory tree, use the following format:

https://raw.githubusercontent.com/klbr8/MVVMLauncher/refs/heads/master/RebirthLauncher/<Subfolders>/<FileName>

### Guidelines
- Always begin the relative path with `RebirthLauncher`.
- Use forward slashes (`/`) instead of backslashes (`\`) when constructing URLs.
- Append subfolders and the file name exactly as shown in the directory tree.
- Do not include `src/` or any other prefixes not present in the repository structure.

### Examples
- **File:** `RebirthLauncher\Services\Bootstrap.cs`  
  **URL:** `https://raw.githubusercontent.com/klbr8/MVVMLauncher/refs/heads/master/RebirthLauncher/Services/Bootstrap.cs`

- **File:** `RebirthLauncher\Constants\LauncherConstants.cs`  
  **URL:** `https://raw.githubusercontent.com/klbr8/MVVMLauncher/refs/heads/master/RebirthLauncher/Constants/LauncherConstants.cs`

- **File:** `RebirthLauncher\Views\InitializingDialog.xaml`  
  **URL:** `https://raw.githubusercontent.com/klbr8/MVVMLauncher/refs/heads/master/RebirthLauncher/Views/InitializingDialog.xaml`