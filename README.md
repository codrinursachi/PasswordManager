# PasswordManager

## Motivation

Managing passwords securely is a critical challenge in the digital age. PasswordManager was created to provide a simple, secure, and user-friendly solution for individuals who want full control over their credentials, without relying on cloud-based or third-party services. The project emphasizes privacy, local encryption, and extensibility for power users and developers.

PasswordManager is a secure, modern, and extensible desktop application for managing your passwords and sensitive credentials. Built with C# and WPF, it provides a user-friendly interface, strong encryption, and a modular architecture for easy maintenance and extension.

## Features

-   **Secure password storage** with encryption
-   **User authentication** and hashing
-   **Password generator** with customizable options
-   **Password import/export**
-   **Category and tag organization**
-   **Favorites and search**
-   **Automatic backup and restore**
-   **Auto-lock on inactivity**
-   **Modern WPF UI with custom controls**
-   **Unit tests for core logic**

## Project Structure

```
PasswordManager.sln                # Solution file
README.md                          # Project documentation
PasswordManager/                   # Main application
	App.xaml, App.xaml.cs            # Application entry point
	Models/                          # Data models (Password, Category, etc.)
	Services/                        # Business logic and services
	Interfaces/                      # Service and repository interfaces
	Repositories/                    # Data access and user repository
	ViewModels/                      # MVVM view models
	Views/                           # XAML UI views
	CustomControls/                  # Reusable WPF controls
	Data/                            # Database context
	State/, Styles/, Images/         # App state, styles, and resources
PasswordManagerTests/              # Unit tests
```

## Quick Start

1. **Clone the repository:**
    ```sh
    git clone https://github.com/codrinursachi/PasswordManager.git
    ```
2. **Open the solution** in Visual Studio 2022+ or VS Code.
3. **Restore NuGet packages** (should happen automatically).
4. **Build the solution:**
    - Visual Studio: `Ctrl+Shift+B`
    - VS Code/CLI: `dotnet build PasswordManager/PasswordManager.csproj`
5. **Run the application:**
    - Visual Studio: Press `F5`
    - VS Code/CLI: `dotnet run --project PasswordManager/PasswordManager.csproj`

---

### Prerequisites

-   Windows OS
-   [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
-   Visual Studio 2022+ (recommended) or VS Code with C# extensions

### Build and Run

1. **Clone the repository:**
    ```sh
    git clone https://github.com/codrinursachi/PasswordManager.git
    ```
2. **Open the solution** in Visual Studio or VS Code.
3. **Restore NuGet packages** (should happen automatically).
4. **Build the solution** (`Ctrl+Shift+B` in Visual Studio).
5. **Run the application** (F5 or `dotnet run` in the PasswordManager folder).

## Usage

1. **First launch:** Set your master password. This password is required to access your vault.
2. **Add passwords:** Click "Add" to store new credentials. Fill in username, password, URL, category, tags, and notes.
3. **Edit/delete:** Select an entry to edit or remove it.
4. **Organize:** Use categories and tags to group passwords for easy retrieval.
5. **Generate passwords:** Use the built-in generator for strong, random passwords.
6. **Search & favorites:** Quickly find passwords and mark important ones as favorites.
7. **Backup/restore:** Use the backup feature to save and restore your vault locally.
8. **Auto-lock:** The app will auto-lock after inactivity for your security.

### Testing

Unit tests are located in the `PasswordManagerTests` project. To run tests:

```
dotnet test
```

## Contributing

Contributions are welcome! To contribute:

1. Fork the repository
2. Create a new branch for your feature or bugfix
3. Make your changes with clear commit messages
4. Ensure all tests pass (`dotnet test`)
5. Submit a pull request with a description of your changes

For major changes, please open an issue first to discuss what you would like to change.

## License

This project is licensed under the MIT License.

---

**Author:** Codrin Ursachi
**Last updated:** December 23, 2025
