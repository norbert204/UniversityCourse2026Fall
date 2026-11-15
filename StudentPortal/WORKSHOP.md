# Blazor Walkthrough (90 Minutes)

Build the portal in small steps rather than presenting the finished code all at once. Keep styling optional; use browser defaults until the final five minutes.

| Time      | Step                                               | Blazor concept                                     |
|-----------|----------------------------------------------------|----------------------------------------------------|
| 0-10 min  | Create the Blazor app and run the starter page.    | Project structure and Razor components             |
| 10-20 min | Register `HttpClient` and define the API models.   | Dependency injection and JSON                      |
| 20-35 min | Load and display students and lessons.             | `OnInitializedAsync`, `@foreach`, and conditionals |
| 35-50 min | Add a student form and send a `POST` request.      | `EditForm`, validation, and event handlers         |
| 50-65 min | Add a lesson form and list.                        | Reusing the same pattern                           |
| 65-80 min | Select a lesson and enroll a student.              | Component state and related API calls              |
| 80-85 min | Add basic spacing and mobile form layout.          | A few CSS rules                                    |
| 85-90 min | Run both apps and troubleshoot the API connection. | Local testing                                      |

The API is configured at `http://localhost:5000/`. Start it before the portal. Both projects target .NET 10; the .NET 10 SDK and runtime are required to run them.


Diasor:

SPA: Single page application -> A böngésző egyszer betölti az alkalmazást, majd navigáláskor csak az oldal tartalma frissül, nem töltődik be újra az egész weboldal.
