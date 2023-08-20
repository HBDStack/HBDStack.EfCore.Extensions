# HBDStack.EfCore.Extensions

Part of the HBDStack suite, `HBDStack.EfCore.Extensions` provides a collection of extension methods and utilities to enhance the functionality of Entity Framework Core in your projects.

## Overview

`HBDStack.EfCore.Extensions` aims to improve developers' experience with Entity Framework Core by offering additional functionalities, thus abstracting some complexities away and promoting code readability and maintainability.

## Features

- **Extension Methods**: Simplify common tasks in Entity Framework Core with a collection of practical extension methods.

- **Auto Config the entities with generic IEntityTypeConfiguration<>**: This feature eliminates the need to manually define each IEntityTypeConfiguration<> for every entity. Instead, this library auto-configures the entities leveraging both, the default settings and those specified in generic instances of IEntityTypeConfiguration<>

- **Static Data Loading**: For enhanced maintainability, this feature separates data seeding and entity configurations (IEntityTypeConfiguration) into distinct classes. The library will automatically scan and load all defined classes that implement IDataSeedingConfiguration<> during the data migration process.

- **Dynamic Order Support**: This library supplies a set of extension methods that grant the capability to dynamically order entities when they are fetched from the database. This functionality proves highly useful in scenarios such as ASP.NET applications where ordering parameters can be determined and passed dynamically from the User Interface level.

## How to Use

To utilize `HBDStack.EfCore.Extensions`, start by adding it as a reference to your .NET project. Please refer [here](HBD.EfCore.Extensions.md) for mor details.

## Contributions

Contributions are very much welcome! Make sure to get familiar with the design principles and objectives of HBDStack before making alterations. Propose your changes through a pull request and provide an explanation of your changes in the pull request comments.

## License

`HBDStack.EfCore.Extensions` is distributed under the MIT license. You can find more details in the LICENCE file within this repository.

## Disclaimer

While `HBDStack.EfCore.Extensions` offers additional functionalities to simplify and enhance your use of Entity Framework Core, it does not implicitly enforce specific business rules or data validations. Ensuring the proper implementation of business rules and data checks is still dependent on the user or application developer.