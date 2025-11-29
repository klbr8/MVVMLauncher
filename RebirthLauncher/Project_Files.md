# Project Files

This document provides the current directory and file structure for the
launcher project. It is updated dynamically as files are added, removed,
or modified.

---

## Directory Tree
📁 RebirthLauncher
├─📁 Constants
│  └─📄 LauncherConstants.cs
├─📁 Converters
│  ├─📄 BooleanToVisibilityConverter.cs
│  ├─📄 RoundEndRadiusConverter.cs
│  └─📄 UrlToBitmapImageConverter.cs
├─📁 Enums
│  └─📄 AppState.cs
├─📁 Models
│  ├─📄 BrowseButton.cs
│  ├─📄 DownloadProgress.cs
│  ├─📄 FileProgress.cs
│  ├─📄 Manifest.cs
│  ├─📄 Server.cs
│  └─📄 ServerList.cs
├─📁 Resources
│  └─📄 Converters.xaml
├─📁 Services
├─📁 Styles
│  ├─📄 ButtonStyles.xaml
│  └─📄 OverlayStyles.xaml
├─📁 Utilities
│  └─📄 ReleaseVersion.cs
├─📁 ViewModels
├─📄 App.xaml
├─📄 AssemblyInfo.cs
├─📄 Charter.md
├─📄 Implementation.md
├─📄 MainWindow.xaml
└─📄 Workflow.md

---

## URL Construction Rule
To access the raw GitHub URL for any file listed above, use the following
format:

https://raw.githubusercontent.com/klbr8/MVVMLauncher/refs/heads/master/<RelativePath>

- `<RelativePath>` = path shown in the directory tree, starting from the
  repo root.

Example:
- File: `src/Services/Bootstrap.cs`
- URL: `https://raw.githubusercontent.com/klbr8/MVVMLauncher/refs/heads/master/src/Services/Bootstrap.cs`


