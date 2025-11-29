# Implementation Plan

This document defines the step‑by‑step planned implementation of the
launcher. Each section represents a discrete unit of work, to be refined
and completed before moving to the next.

---

## Step 1: Bootstrapper (Bootstrap.cs in Services namespace)

### Purpose
Initialize the launcher, display a splash/loading screen, and prepare the
application state before MainWindow is instantiated.

### Responsibilities
- **Mike**: Implement Bootstrap.cs, dialogs, and persistence logic; perform
  unit testing before moving to the next step.
- **Anders**: Refine flow, advise on structure, ensure MVVM discipline, and
  highlight testable units.

### Flow / Logic
1. **Command line arguments**
   - Parse arguments at startup.
   - If uninstall switch is present:
     - Prompt user for confirmation.
     - Ask if the user also wishes to uninstall the CoH client.
     - Advise that deleting the launcher’s working folder must be done
       manually to fully remove the launcher.
     - Proceed with uninstall logic if confirmed.

2. **Launcher update check**
   - Connect to version.txt to compare current version against available
     version.
   - Updates are mandatory when newer versions are detected.
   - Attempt to download and install the update with retries.
   - If download fails, notify the user but continue with other functions
     (game does not depend on launcher version).

3. **Settings.xml check**
   - Look for settings.xml in Local/AppData/{AppName}.
   - If missing:
     - Open InitializingDialog (View/ViewModel).
     - If dialog returns invalid or canceled data:
       - Do not write settings.xml.
       - Advise user and exit program.
     - If dialog returns valid folder:
       - Copy to InstallPath property.
       - Persist to settings.xml.
   - Bootstrapper halts until a valid InstallPath exists.

4. **Config.xml / Servers.xml check**
   - Look for config.xml (or servers.xml) in Local/AppData/{AppName}.
   - If missing:
     - Call method with URL to retrieve a Server object.
     - If valid, add to ObservableCollection<Server>.
     - Serialize collection to settings.xml.
   - Multiple servers are supported; user can add/remove servers later.
   - On first run, the default server is added automatically and should
     persist across subsequent runs.

5. **Completion**
   - Once settings.xml exists and is valid, bootstrapping is complete.
   - Instantiate MainWindow and its ViewModel.
   - On subsequent runs, bootstrapper typically only parses command line
     arguments and loads/deserializes settings.xml.

### Dependencies
- Models, converters, enumerations, and helper classes already provided.
- LauncherConstants.cs in Constants namespace for adjustable values.
- InitializingDialog View/ViewModel.
- Project_Files.md for current repo structure.

### Unit Testing
- Verify command line parsing (normal vs uninstall mode).
- Simulate update check with version.txt (newer, same, download failure).
- Test settings.xml creation, persistence, and invalid/canceled dialog
  paths.
- Test server retrieval and serialization logic.
- Confirm MainWindow only instantiates when prerequisites are met.

---

## Step 2: [Placeholder]
*(Reserved for next step in the plan.)*