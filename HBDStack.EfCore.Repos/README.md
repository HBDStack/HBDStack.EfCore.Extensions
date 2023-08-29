# HBD.EfCore.Repositories

`HBD.EfCore.Repositories` is a project within the HBDStack suite, providing a standard way of abstracting interactions with the underlying data stores in Entity Framework Core applications.

## Overview

`HBD.EfCore.Repositories` helps developers adhere to the Repository pattern in their Entity Framework Core applications by providing standard repository implementations and useful abstractions.

## Features

- **Standard Repository Implementations**: Ready to use repositories for common database operations, such as Create, Read, Update, Delete (CRUD).

- **Unit Of Work Support**: Use the unit of work pattern to ensure that changes to your data are managed in a consistent and predictable way.

- **Configurability**: Configurable to suit your application's specific needs while maintaining the benefits of a standardized approach to data access.

- **Ease of Integration**: Designed for easy integration with any application using Entity Framework Core.

## How to Use

To use `HBD.EfCore.Repositories`, include it as a dependency in your project. The repositories provided by this project can be used as is, or as a base for developing repositories tailored to your specific application needs. More specific instructions can be found in the code comments or any additional documentation provided with the project.

## Contributions

Contributions are most welcome! If you wish to contribute, please familiarize yourself with the HBDStack suite's design principles. When you're ready, submit a pull request with your changes, and include a detailed explanation of your changes.

## License

`HBD.EfCore.Repositories` is licensed under the MIT License. For more details, please refer to the LICENCE file in this repository.

## Disclaimer

While `HBD.EfCore.Repositories` provides a standard and efficient way to interact with your data using Entity Framework Core, it doesn't dictate or enforce specific business rules or validation needs on your repositories. Ensuring that business rules and data validation are correctly implemented remains the responsibility of the user or the application developer.