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
- **Error Handling**: Incorporate structured error reporting and
  user‑friendly feedback.
- **Logging**: Log all exceptions. Any exceptions not explicitly handled
  should be rethrown after logging.
- **Validation Discipline**: Ensure initialization steps, settings
  persistence, and server handling are validated consistently.
- **Testing**: Unit testing must be performed for each unit‑testable class,
  ViewModel, and service before moving on to the next component.
- **Documentation**: Keep charter static; update workflow and file listings
  dynamically as project evolves.

---

## Implementation Reference
See [Implementation.md](Implementation.md) for step‑by‑step planned
implementation details.