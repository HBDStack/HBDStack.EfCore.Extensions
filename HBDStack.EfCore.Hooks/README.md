# HBDStack.EfCore.Hooks

`HBDStack.EfCore.Hooks` is a project within the HBDStack suite that provides easy-to-use hooks with Entity Framework Core, enabling additional actions to be performed before or after database operations.

## Overview

`HBDStack.EfCore.Hooks` is designed to provide an easy way to hook additional operations into the Entity Framework Core's save pipeline. This allows developers to define actions that will be performed before or after an entity is added, modified, or deleted from the database.

## Features

- **Before and After Hooks:** The project allows the execution of custom logic both before and after an operation is performed on an entity, giving developers a greater degree of control over their data.

- **Flexible Hook Registration:** Hooks can be registered on per-entity basis, meaning different actions can be performed depending on the entity type being altered.

- **Async support:** The hooks support asynchronous operations, aligning them with the best practices of modern .NET programming.

## How to Use

To use `HBDStack.EfCore.Hooks`, first include it in your project's dependencies. You then register and implement hooks as per your application's requirements. Have a look at the comprehensive [HowTo file](https://github.com/HBDStack/HBDStack.EfCore.Extensions/blob/main/HBD.EfCore.Hooks.md) for detailed instructions and usage scenarios!

## Contributions

Contributions are most welcome! Should you like to contribute, please become familiar with HBDStack's existing design principles. Submit your proposed changes as a pull request, and include clear explanations on the reasoning behind them in the request's comments.

## License

`HBDStack.EfCore.Hooks` is licensed under the MIT license - see the LICENCE file in this repository for further details.

## Disclaimer

While hooks implemented with `HBDStack.EfCore.Hooks` can perform actions around database operations, they do not create or enforce specific business logic. Applying appropriate business rules and constraints within the hooks, as well as safe and responsible use of hooks, remains the responsibility of the user or application developer.