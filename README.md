# PasswordManager

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

## Getting Started

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

### Usage

1. On first launch, set your master password.
2. Add, edit, or delete password entries.
3. Organize passwords by category and tags.
4. Use the password generator for strong passwords.
5. Search, favorite, and manage your credentials securely.

### Testing

Unit tests are located in the `PasswordManagerTests` project. To run tests:

```
dotnet test
```

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

## License

This project is licensed under the MIT License.

---

**Author:** Codrin Ursachi
**Last updated:** December 23, 2025
