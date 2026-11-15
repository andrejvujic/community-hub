# CommunityHub

CommunityHub is a desktop application for managing residential buildings and local communities. It brings building residents, building managers, citizens, and neighborhood coordinators into one system for handling everyday community tasks: apartment access, shared-room reservations, resident assemblies, neighborhood events, facility bookings, donations, budgets, and service requests.

The project was developed as university coursework for **Software Specification and Modeling** and **Human-Computer Interaction**. It is a layered monolith built around Clean Architecture, the Dependency Inversion Principle (DIP), and SOLID design principles. The HCI part of the project focuses on role-specific workflows, clear feedback, validation, contextual help, localization, and consistent navigation.

## Main features

After signing in, each user is taken to an interface for their role.

- **Building residents** can request apartment access, browse buildings, reserve common rooms, create and match community ads, manage time slots, follow notifications, report issues, take part in resident assemblies, and use in-app video tutorials.
- **Building managers** can manage buildings, apartments, memberships, access requests, common rooms, reported issues, notice-board content, and resident assemblies.
- **Citizens** can join neighborhoods, attend meetings, organize and volunteer for events, donate to neighborhood budgets, reserve or vote for local facilities, review coordinators, and view trust records.
- **Neighborhood coordinators** can process access requests, manage neighborhood budgets and expenses, schedule meetings, coordinate facilities and service providers, review statistics, and generate reports.

The application also includes Serbian and English localization, light and dark themes, tooltips, quick-help dialogs, a guided help wizard, confirmation dialogs, input validation, charts, and PDF reports.

## Architecture

CommunityHub is deployed as a single application, with responsibilities separated into layers:

```text
CommunityHub/
├── src/
│   ├── CommunityHub.Application/
│   │   ├── Domain/                 # Entities, domain rules, and repository contracts
│   │   ├── Services/               # Application use cases and orchestration
│   │   ├── MappersDto/             # DTOs and mappings between layers
│   │   └── Database/               # PostgreSQL access, repository implementations, and scripts
│   └── CommunityHub.Ui/
│       ├── Views/                  # WPF windows and pages
│       ├── ViewModels/             # Presentation logic and commands
│       ├── Helpers/                # Navigation, alerts, converters, and tooltips
│       └── Resources/              # Themes, styles, localization, images, fonts, and videos
└── CommunityHub.slnx
```

The domain layer defines repository interfaces, while the database layer supplies their PostgreSQL implementations. Application services depend on those abstractions through the project injector. This keeps business rules separate from persistence and UI code and applies DIP within the monolith. The WPF application follows MVVM for most presentation flows, using view models, commands, bindings, and reusable services for navigation and dialogs.

## Technologies

- **C# and .NET 10** for the application and domain logic
- **WPF and XAML** for the Windows desktop interface
- **PostgreSQL** for relational data storage
- **Npgsql** for PostgreSQL access from .NET
- **MVVM** for presentation structure and data binding
- **MaterialDesignThemes** and **Extended WPF Toolkit** for UI components and styling
- **LiveCharts2** for charts and statistics
- **PDFsharp/MigraDoc** and **QuestPDF** for report generation
- **RESX resources** for Serbian and English localization
- **Git and GitHub** for version control and team collaboration

## My contributions

The Git history records my work under the names **Andrej Vujic**, **Andrej Vujić**, and **Andrej**. My contributions span the full application stack, with most of my feature work centered on the building-resident part of the system. Several technical components I introduced became shared infrastructure used by the rest of the team.

### Technical foundations

- Designed the database access layer around `DatabaseConnection`, an `IDisposable` wrapper over Npgsql that manages connection lifetime, transactions, PostgreSQL enum registration, reader callbacks, scalar queries, and non-query commands in one place.
- Created `DatabaseQuery`, a small parameterized-query builder that turns SQL and named parameters into `IDbCommand` instances. This keeps command construction consistent and prevents repositories from duplicating parameter-binding code.
- Designed a lightweight object-relational mapping system. The generic `EntityMapper<T>` contract maps `IDataReader` rows to domain entities and maps entities back to query-parameter dictionaries. I also added prefix-aware data readers so mappers can safely compose nested objects from joined result sets.
- Built the first entity mappers and migrated repositories to the shared query and mapping infrastructure. The team then used the same approach across the rest of the data model; the current codebase uses `DatabaseConnection` and `DatabaseQuery` in more than 40 repositories.
- Created the generic `BaseService<TEntity, TDto>` mapping layer to keep domain entities inside the application layer while exposing DTOs to the UI. It now provides shared entity-to-DTO and DTO-to-entity conversion for 20 application services.
- Created a custom stack-based navigation system for the resident interface. It supports `Push`, `PushReplacement`, `Pop`, and `PopAll`; a page can return a value when it is popped, and the caller receives it through an `onPop` callback after WPF finishes navigating back.
- Introduced `IMainNavigationConsumer` so view models depend on a navigation abstraction instead of a concrete WPF page, keeping navigation testable and consistent with DIP.
- Created `BaseViewModel` for the resident UI, centralizing `INotifyPropertyChanged`, navigation, and alert access. Resident view models reuse it instead of implementing that plumbing independently.
- Designed a reusable host/consumer alert system with typed alerts, dynamic action buttons, and callbacks. Resident pages use it for confirmations, validation failures, warnings, and success feedback.
- Created the project injector and repository contracts that map abstractions to concrete persistence implementations. This infrastructure is used throughout services and view models, including features developed by other team members.
- Designed the extensible notification-details model and shared notification repository flow, allowing a notification to carry a typed detail payload while keeping common notification storage and behavior in one place.
- Refactored database access and application code to improve separation of concerns, move behavior into the correct layer, and apply Clean Architecture, DIP, and SOLID principles more consistently.

### Features and user experience

- Built and evolved the PostgreSQL schema, seed data, domain entities, DTOs, mappers, repositories, and services for users, buildings, apartments, memberships, and access requests.
- Implemented resident building search and browsing, apartment-access workflows, and role-based login and navigation.
- Developed the community ads workflow, including ad creation, matching, time slots, reservations, subscriptions, and related notifications.
- Implemented common-room browsing and rental requests, along with date validation and PDF reports for rental-request data.
- Developed resident assemblies, attendance registration, automatic status updates, accepted and suggested topics, and topic-management screens.
- Added notification pages and notification-detail flows for resident activity.
- Created the in-app tutorial section and video player, including playback controls, loading states, progress tracking, seeking, and tutorial videos.
- Contributed reusable UI behavior, validation, dialogs, converters, styling, and usability fixes across resident screens.

## Getting started

### Prerequisites

- Windows 10 or later
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL
- Visual Studio with the .NET desktop development workload, or another environment that can build WPF projects

### Database setup

1. Create a PostgreSQL database named `communityhub`.
2. Run [`create_all.sql`](src/CommunityHub.Application/Database/Scripts/create_all.sql) against that database.
3. Run [`seed.sql`](src/CommunityHub.Application/Database/Scripts/seed.sql) to add development data.
4. If your PostgreSQL credentials differ from the defaults, update the connection string in [`appsettings.json`](src/CommunityHub.Application/appsettings.json).

The default configuration expects PostgreSQL at `localhost:5432`, with the username and password both set to `postgres`.

### Run the application

From the repository root:

```powershell
dotnet restore CommunityHub.slnx
dotnet build CommunityHub.slnx
dotnet run --project src/CommunityHub.Ui/CommunityHub.Ui.csproj
```

You can also open `CommunityHub.slnx` in Visual Studio, set `CommunityHub.Ui` as the startup project, and run it with `F5`.

### Sample accounts

The seed script creates accounts for each implemented role. All sample accounts below use the password `123`.

| Role | Username |
| --- | --- |
| Building manager | `milan.jovanovic` |
| Building resident | `stefan.matic` |
| Neighborhood coordinator | `aleksandar.savic` |
| Citizen | `petar.simic` |

## Database scripts

- [`create_all.sql`](src/CommunityHub.Application/Database/Scripts/create_all.sql) creates the PostgreSQL types, tables, constraints, and relationships.
- [`seed.sql`](src/CommunityHub.Application/Database/Scripts/seed.sql) inserts sample users and community data for development and demonstrations.
- [`drop_all.sql`](src/CommunityHub.Application/Database/Scripts/drop_all.sql) removes the project schema. It is intended only for resetting a local development database.
