# HBDStack.EfCore.Events

This project is a part of the HBDStack suite, aimed at providing event-based features for applications using Microsoft's Entity Framework Core.

## Overview

`HBDStack.EfCore.Events` provides a way to trigger and handle events in your Entity Framework Core applications. These events can be used for creating triggers, handling updates, validating entities, and more.

## Features

- **Event Triggers**: Automatically fire events based on changes in your data like onCreate, onUpdate or onDelete.

- **Custom Event Handlers**: Easy to write and add your own custom event handlers suitable for any business rules.

- **Pre & Post Save Actions**: Ability to hook pre-save and post-save actions in your database operations.

## How to Use

Start by adding the `HBDStack.EfCore.Events` project to your solution. Implement and register your event handlers as per your application needs. You can write specific business logic in these handlers which will get executed when the corresponding event gets triggered.

## Contributions

Contributions are very welcome! Please become familiar with the design, principles and objectives of HBDStack before making any changes. To propose a change, submit a pull request and explain the purpose and reasoning behind your changes in the pull request comment.

## License

`HBDStack.EfCore.Events` operates under the MIT license. You can find more details in the LICENCE file in this repository.

## Disclaimer

While `HBDStack.EfCore.Events` provides a way to respond to changes in your Entity Framework Core data, it does not implement any specific business logic. Enforcement of proper business rules and constraints is still the responsibility of the user or application developer, as is proper handling and management of events.