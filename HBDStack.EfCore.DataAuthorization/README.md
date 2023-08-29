# HBDStack.EfCore.DataAuthorization

This project is a part of the HBDStack suite and is specifically focused on providing data authorization functionalities for applications built with Entity Framework Core.

## Overview

`HBDStack.EfCore.DataAuthorization` is aimed at simplifying and standardizing the approach to data authorization in your applications. This library provides features for enforcing security constraints on data, ensuring that users can only access and modify the data that they are authorized to.

## Features

- **Granular authorization controls:** You can define detailed data authorization rules at entity level, down to individual properties if needed. This ensures your data is protected at all levels.

- **Integration with Identity:** Works seamlessly with ASP.NET Core Identity for user management and authorization.

- **Query Filtering:** Automatically applies authorization rules at the query level, reducing the risk of accidental data exposure.

## How to Use

To use `HBDStack.EfCore.DataAuthorization`, reference it in your .NET Core project. You need to configure the data authorization rules in your DbContext or startup configuration. More detailed usage instructions will depend on the exact contents and interfaces exposed by this library.

## Contributions

We welcome contributions! Before making any changes, please familiarize yourself with the overall design and objectives of the HBDStack. If you wish to propose changes, submit a pull request and explain the purpose of your changes in the pull request comment.

## License

`HBDStack.EfCore.DataAuthorization` is licensed under MIT license. See the LICENCE file in this repository for more details.

## Disclaimer

Data authorization rules defined with `HBDStack.EfCore.DataAuthorization` are only as effective as the overall security and integrity of your application. Always ensure to follow best security practices and regularly audit your application for potential vulnerabilities.