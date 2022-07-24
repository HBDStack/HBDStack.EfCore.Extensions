# How to upgrade to HBD.EfCore.DDD v3.5

Below are breaking changes of v3.5

- Moved `IDomainEvent` from `HBD.GenericEventRunner.DomainParts` to `HBD.EfCore.DDD.Domains` => This only namespace changes is required.
- The `callingEntity` parameter of all `EventHandler` type changed from `EntityEventsBase` to object.