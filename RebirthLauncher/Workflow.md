# Workflow Integration

This document defines how Mike’s manual responsibilities and development
practices integrate with Anders’ chartered responsibilities. It serves as
the evolving companion to Charter.md, and references Project_Files.md for
the current directory/file structure.

---

## Purpose
To ensure disciplined collaboration between Mike and Anders by documenting:
- Manual responsibilities (editing, building, testing, maintaining file
  listings).
- Development practices (error handling, validation, logging, testing).
- Project overview and planned implementation details.

---

## Project Overview
The project is a WPF application designed to serve as a launcher for City
of Heroes. Its general purpose is to provide a user‑friendly interface that
manages setup, validation, and launching of the game. The application
emphasizes:
- **Maintainability**: structured around MVVM principles for clean
  separation of concerns.
- **Feature completeness**: designed to deliver a full set of required
  launcher functionality; future updates are expected to focus primarily on
  bug fixes and stability rather than new features.
- **User experience**: simple, intuitive flows for setup and launch, with
  clear feedback on progress and errors.
- **Discipline**: consistent coding practices, resource management, and
  documentation to ensure long‑term stability.

---

## Workflow Integration

### Anders Responsibilities
- Provide complete, structured responses to the current step or request.
- Ask clarifying questions immediately if context is missing.
- Withhold implementation‑related questions unless prompted, but advise
  when such questions exist.
- Remain within the defined focus; remind Mike to refocus if conversation
  drifts.
- Avoid citations and unnecessary progression beyond the current step.

### Mike Responsibilities
- **Editing/Building/Testing**: Perform all builds and tests manually;
  Anders will not assume automated pipelines.
- **File Listing Maintenance**: Keep Project_Files.md updated with
  directory/file structure and raw GitHub URLs.
- **Focus Control**: Retain the right to redirect or override focus outside
  the current step; accept reminders to refocus when needed.
- **Implementation Decisions**: Define application flow and planned
  implementation details; Anders will refine and support but not dictate.
- **Resource Discipline**: Ensure resource dictionaries, converters, and
  styles are organized and maintained consistently.
- *(Placeholder: add any other manual responsibilities you want explicitly
  documented.)*

---

## Development Practices
- **MVVM Architecture**: Strict adherence to MVVM principles for separation
  of concerns. Use .Net 8.0 and C# 12 features where appropriate. 
- **Error Handling**: Incorporate structured error reporting and
  user‑friendly feedback.
- **Nullability**: Adhere to strict nullability checks and practices
  throughout the codebase. No need to apply #nullable directives as the default
  project setting is enabled.
- **Logging**: Log all exceptions. Any exceptions not explicitly handled
  should be rethrown after logging. Verbosity will be described per step in Implementation.md.
- **Validation Discipline**: Ensure initialization steps, settings
  persistence, and server handling are validated consistently.
- **Testing**: Unit testing must be performed for each unit‑testable class,
  ViewModel, and service before moving on to the next component.
- **Documentation**: Keep charter static; update workflow and file listings
  dynamically as project evolves. Use XMLDOC comments throughout the codebase.

---

## Document Roles

To maintain clarity and discipline, each document in the project serves a
specific role:

- **Charter.md**  
  Static foundation. Defines responsibilities, tone, style, and rules of
  collaboration. This file does not change once established.

- **Workflow.md**  
  Semi‑dynamic. Describes project overview, integration of responsibilities,
  and development practices. Provides context for how Anders and Mike work
  together. Points outward to Implementation.md and Project_Files.md.

- **Implementation.md**  
  Dynamic, step‑by‑step plan. Each section represents a discrete unit of
  work (e.g., bootstrapper, dialogs, server management). Updated one step at
  a time, refined and tested before moving forward.

- **Project_Files.md**  
  Dynamic repo map. Contains the current directory tree and the URL
  construction rule for deriving raw GitHub links. Always kept up to date so
  Anders can reference the latest code without guessing paths.

Together, these documents form a complete framework: Charter anchors the
rules, Workflow integrates practices, Implementation drives step‑by‑step
progress, and Project_Files ensures Anders always has the latest repo map.

---

## Implementation Reference
See [Implementation.md](Implementation.md) for step‑by‑step planned
implementation details.