# Homeworks

### Prerequisites
You will need the following tools:

* [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)
* [.Net 8.0 or later](https://dotnet.microsoft.com/download/)
* EF Core

## Layered Architecture
Homeworks implements NLayer **Hexagonal architecture** (Core, Application, Infrastructure and Presentation Layers) and **Domain Driven Design** (Entities, Repositories, Domain/Application Services, DTO's...). Also implements and provides a good infrastructure to implement **best practices** such as Dependency Injection, logging, validation, exception handling, localization and so on.
Aimed to be a **Clean Architecture** also called **Onion Architecture**, with applying **SOLID principles** in order to use for a project template. Also implements and provides a good infrastructure to implement **best practices** like **loosely-coupled, dependency-inverted** architecture
The below image represents aspnetrun approach of development architecture of run repository series;

![DDD_png_pure](https://user-images.githubusercontent.com/1147445/54773098-e1efe700-4c19-11e9-9150-74f7e770de42.png)

### Structure of Project
Repository include layers divided by **4 project**;
* Core
    * Entities    
    * Interfaces
    * Specifications
    * ValueObjects
    * Exceptions
* Application    
    * Interfaces    
    * Services
    * Dtos
    * Mapper
    * Exceptions
* Infrastructure
    * Data
    * Repository
    * Services
    * Migrations
    * Logging
    * Exceptions
* Web
    * ViewModels
    * AutoMapper

### Core Layer
Development of Domain Logic with abstraction. Interfaces drives business requirements with light implementation. The Core project is the **center of the Clean Architecture** design, and all other project dependencies should point toward it.. 

#### Entities
Includes Entity Framework Core Entities which creates sql table with **Entity Framework Core Code First Aproach**. Some Aggregate folders holds entity and aggregates.
You can see example of **code-first** Entity definition as below;

#### Interfaces
Abstraction of Repository - Domain repositories - Specifications etc.. This interfaces include database operations without any application and ui responsibilities.

#### Specifications
This folder is implementation of **[specification pattern](https://en.wikipedia.org/wiki/Specification_pattern)**. Creates custom scripts with using **ISpecification** interface. Using BaseSpecification managing Criteria, Includes, OrderBy, Paging.
This specs runs when EF commands working with passing spec. This specs implemented SpecificationEvaluator.cs and creates query in ApplySpecification method.This helps create custom queries.

### Infrastructure Layer
Implementation of Core interfaces in this project with **Entity Framework Core** and other dependencies.
Most of your application's dependence on external resources should be implemented in classes defined in the Infrastructure project. These classes must implement the interfaces defined in Core. If you have a very large project with many dependencies, it may make sense to have more than one Infrastructure project (eg Infrastructure.Data), but in most projects one Infrastructure project that contains folders works well.
This could be includes, for example, **e-mail providers, file access, web api clients**, etc. For now this repository only dependend sample data access and basic domain actions, by this way there will be no direct links to your Core or UI projects.

#### Data
Includes **Entity Framework Core Context** and tables in this folder. When new entity created, it should add to context and configure in context.
The Infrastructure project depends on Microsoft.**EntityFrameworkCore.SqlServer** and EF.Core related nuget packages, you can check nuget packages of Infrastructure layer. If you want to change your data access layer, it can easily be replaced with a lighter-weight ORM like Dapper. 

#### Migrations
EF add-migration classes.

#### Repository
EF Repository and Specification implementation. This class responsible to create queries, includes, where conditions etc..

#### Services
Custom services implementation, like email, cron jobs etc.

### Application Layer
Development of **Domain Logic with implementation**. Interfaces drives business requirements and implementations in this layer.
Application layer defines that user required actions in app services classes as below way;

In this layer we can add validation , authorization, logging, exception handling etc. -- cross cutting activities should be handled in here.

### Web Layer
Development of UI Logic with implementation. Interfaces drives business requirements and implementations in this layer.

### Test Layer
For each layer, there is a test project which includes intended layer dependencies and mock classes. So that means Core-Application-Infrastructure and Web layer has their own test layer. By this way this test projects also divided by **unit, functional and integration tests** defined by in which layer it is implemented. 
Test projects using **xunit and Mock libraries**.  xunit, because that's what ASP.NET Core uses internally to test the product. Moq, because perform to create fake objects clearly and its very modular.

## Disclaimer

Please note that the architectural approach used in this project is intentionally complex and not reflective of what would typically be necessary for a task of this scope. This complexity was adopted solely for educational purposes to provide hands-on experience with certain design patterns and advanced features. We acknowledge that this might not be the optimal approach for the given task and appreciate your understanding and feedback.

## Authors

* **Mehmet Ozkaya** - *clean architecture Tempalte* - [mehmetozkaya](https://github.com/mehmetozkaya)

See also the list of [contributors](https://github.com/aspnetrun/run-core/contributors) who participated in this project. Check also [gihtub page of repository](https://aspnetrun.github.io/run-aspnetcore/)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
