# Implementation Plan

This document defines the step‑by‑step planned implementation of the
launcher. Each section represents a discrete unit of work, to be refined
and completed before moving to the next.

---

## Document Roles Reference

- **Charter.md** → Static foundation. Defines responsibilities, tone, style,
  and rules of collaboration.
- **Workflow.md** → Semi‑dynamic. Provides project overview, integration of
  responsibilities, and development practices. Points outward to
  Implementation.md and Project_Files.md.
- **Implementation.md** → Dynamic, step‑by‑step plan. Each section represents
  a discrete unit of work. Updated one step at a time, refined and tested
  before moving forward.
- **Project_Files.md** → Dynamic repo map. Contains the current directory
  tree and the URL construction rule for deriving raw GitHub links.

This file (Implementation.md) is where planned steps are documented and
refined before coding and testing. It should always be read alongside
Workflow.md and Project_Files.md. If supporting services and helpers are
needed, they should be created in their respective namespaces after
discussing in chat. This document may not describe every low-level detail so,
please keep a list of clarifying questions as you read through it and advise
when there are questions to address before proceeding.

---

## Step 1: Bootstrapper (Bootstrap.cs in Services namespace)

### Purpose
Initialize the launcher, display a splash/loading screen, and prepare the
application state before MainWindow is instantiated.

### Flow / Logic
1. **Command line arguments**
   - Show splash screen immediately on startup.
     - Splash screen is a simple WPF Window with an image and
       an indeterminate progress bar.

   - Parse arguments at startup.
     - valid switches:
       - `/uninstall` : triggers uninstall logic.
       - `/log` : enables logging for the session. 
   - If uninstall switch is present:
     - Prompt user for confirmation.
     - Ask if the user also wishes to uninstall the CoH client.
     - Advise that deleting the launcher’s working folder must be done
       manually to fully remove the launcher.
     - Proceed with uninstall logic if confirmed.
   - If logging switch is present:
     - Enable logging for the session
     - There should be a static boolean property in LauncherConstants.cs
       indicating whether logging is enabled.
     - Logging implementation is simply writing to a text file in the
       launcher's working directory when exceptions are thrown. 
     - Rethrow unhandled exceptions after logging. 

2. **Launcher update check**
   - Check for .old files from previous updates and delete them.
   - Connect to version.txt to compare current version against available
     version.
   - Url for version.txt is configurable in LauncherConstants.cs.
   - Updates are mandatory when newer versions are detected.
   - Attempt to download and install the update with retries.
     - Maximum retries and delay between attempts are configurable in
       LauncherConstants.cs.
     - Update file is a .zip archive that overwrites existing
       files. It is always located at the same URL path as version.txt,
       and always named "RebirthLauncher_Update.zip".
     - The update process follows the steps download update zip, rename
       the current process with .old extension, extract zip to current folder, restart the launcher. 
     - If download fails, notify the user but continue with other functions
     (game does not depend on launcher version).
   - If update is successful:
     - Restart the launcher.
   - If no update is needed or after successful update, continue.

3. **Settings.xml check**
   - Look for settings.xml in Local/AppData/{AppName}.
   - If missing:
     - Open InitializingDialog (View/ViewModel).
     - InitializingDialog prompts user to select the CoH client
       installation folder. It displays a textbox with the default
       path "C:\CityOfHeroes", and a browse button that opens a wpf
       openfolderdialog.
       - User can accept default or select a different folder.
       - When user confirms selection:
         - Validate that the folder exists, if it does not, check
           if it is a valid path then create the folder.
         - If folder is invalid (e.g. contains invalid characters) notify
           user and allow them to reselect.
     - If InitializingDialog returns invalid or canceled data:
       - Do not write settings.xml.
       - Advise user and exit program.
     - If dialog returns valid folder:
       - Persist to settings.xml.

4. **Servers.xml check**
   - Look for servers.xml in Local/AppData/{AppName}.
   - If missing:
     - Call method with URL from LauncherConstants to retrieve an xml file and deserialize as a Manifest object.
     - In this init case we assume the manifest is valid as we own it.
     - Create a Server object from the relevant properties of the Manifest object.
     - Add that Server object to a ServerList object.
     - Serialize ServerList to settings.xml.
   - Multiple servers are supported; user can add/remove servers 
     later. (This is informational and does not need to be implemented now.)
   - If servers.xml exists:
     - Continue to next step.

5. **Completion**
   - Once settings.xml exists and is valid, bootstrapping is complete.
   - Close splash screen.
   - Instantiate MainWindow and its ViewModel.
   - On subsequent runs, bootstrapper typically only parses command line
     arguments as other checks should pass.

### Dependencies
- Most Models, converters, enumerations, and helper classes are already provided.
- LauncherConstants.cs in Constants namespace for adjustable values.
- InitializingDialog View/ViewModel.
- Project_Files.md for current repo structure.

### Unit Testing
- Do not write/design Bootstrapper with testability in mind. It is inherently difficult to
  unit test due to its nature as an application entry point. Focus on manual testing.
- Verify command line parsing (normal vs uninstall mode).
- Simulate update check with version.txt (newer, same, download failure).
- Test settings.xml creation, persistence, and invalid/canceled dialog
  paths.
- Test server retrieval and serialization logic.
- Confirm MainWindow only instantiates when prerequisites are met.

---

## Step 2: [Placeholder]
*(Reserved for next step in the plan.)*