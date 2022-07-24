# HBD.EfCore.Extensions

## Nuget package

```cmd
PM> Install-Package HBD.EfCore.Extensions
```

## Quick Startup

1.  Change inherited of your DbContext from
    `Microsoft.EfCore.DbContext` to
    `HBD.EfCore.Extensions.DbContext`.

2.  Implement your `EntityTypeConfiguration<TEntity>` by following the
    instruction below.

3.  Add Configuration to your Startup class.

## Introduction

As you know the Entity Framework Core is a lightweight, extensible, and
cross-platform version of the popular Entity Framework data access technology.
However, in order to make the EF work, We need to define, config a few things
below:

1.  Define the Entities

```csharp
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id{get;set;}

    public string FullName => $"{FirstName} {LastName}";

    [Required]
    [MaxLength(256)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(256)]
    public string LastName { get; set; }
}
```

1.  Define Mappers

```csharp
internal class UserMapper : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => u.FirstName);
        //more here
    }
}
```

1.  Define DbContext and add the Mapper in.

```csharp
public partial class MyContext : Microsoft.EfCore.DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMapper());
    }
}
```

Let see, if we have a hundred Entities, we need to do all steps above a hundred
times and the third step is an important step without applying the configuration
to the DbContext, we are not able to use that entities. To speed up the
development process and cut down the manual works, I developed this extension
for EF Core which helps to scan and apply the IEntityTypeConfiguration from
assemblies to DbContext automatically.

## How HBD.EfCore.Extensions Works.

### 1. The Generic of Entity Type Configuration

Let see, if we are 2 numbers of entities which have the same Entity Type
Configuration as almost of configuration can be done via DataAnnotations.
However, we still need to define a class from `IEntityTypeConfiguration` for
every entity and add into `OnModelCreating` of DbContext. However, if using this
the extension you can define a generic EntityTypeConfiguration that define the
configuration for all basic entities which extension will scan it from the
Assembly and add to `OnModelCreating` automatically.

Sample code: Define a generic Entity Type Configuration

```csharp
//1. Define BaseEntity
//The recommendation, you should define a BaseEntity on your project
//which include all basic properties which will be applied for all entities on the project.
//The Extension provides a few generic class which help you to do the things faster.
public abstract class BaseEntity: Entity { }

//2. Define Generic Entity Type Configuration
internal class MyEntityTypeConfiguration<TEntity> : EntityTypeConfiguration<TEntity>
    where TEntity: BaseEntity
{
    public void Map(EntityTypeBuilder<TEntity> builder)
    { /*Apply the configuration here for generic entities*/ }
}

//3. Switch from Microsoft.EfCore.DbContext to HBD.EfCore.Extensions.DbContext
public partial class MyContext : Microsoft.EfCore.DbContext
{
   //No need to override the OnModelCreating(ModelBuilder modelBuilder)
}

//4. Apply the configuration when App Start
//Normally, the DbContext will be registered with Dependency Injection when App start.
//So below is a sample code to register the assembles to the extensions as well
var db = new MyDbContext(new DbContextOptionsBuilder()
    .RegisterEntities(op=>op.FromAssemblies(typeof(MyDbContext).Assembly))
    .Options)
```

Run the application and show the result, All the entities had been mapped to the
DbContext automatically using generic `MyEntityTypeConfiguration<TEntity>`.

Now, you want to add new
entities? Just go-ahead to add the entity classes it will be mapped to DbContext automatically.

### 2. Specific Entity Type Configuration

But, The life is not easy and for some special entities, that the generic may not able
to adopt all the requirement of the entities, in this case, it requires a
specific Entity Type Configuration to be defined.

```csharp
//1. Specific Configuration for User Entity
internal class UserTypeConfiguration : EntityTypeConfiguration<User>
{
    public void Map(EntityTypeBuilder<User> builder)
    { /*Apply the configuration here for the User entity*/ }
}
```

That's all, You just need to define the specific EntityTypeConfiguration for
those special entities. The extension is smart enough to scan and all them to
the DbContext and obviously, it will apply the Generic Entity Type Configuration
to those others entities.

### 3. Static Data Loading

Usually, In the application, We also have some static data tables which shall be
initialized when creating the database. EF Core proved a `HasData` method allow
us to define the static data for particular entities. But, again the static data
need to be provided during `OnModelCreating`. Instead of combined to together
with Entity Type Configuration, This extension provides a new generic interface
named `IDataSeedingConfiguration<>` allow you to define the Static Data for an
entity and this definition will be scanned and loadded to the DbContext at runtime
automatically.

```csharp
//1. Define the Data Seeding
internal class AccountDataSeeding: IDataSeedingConfiguration<Account>{
    public TEntity[] Data => new [
        new Account {Id= 1, UserName = "admin", Password="123"},
        new Account {Id= 2, UserName = "user", Password="123"}
    ];
}
```

## Domain Driven Design Support

If you are following the DDD design with your EF Core. For each business
activity, we may need to load all relevant data of a Navigation root, that can
be done via include all relevant navigation properties into a Queryable.

```csharp
//Get all diabled users including their addesses.
var users = dbContext.Set<User>()
            .Include(u => u.Addresses)
            .Where(u => u.IsDisabled)
            .ToList();
```

By doing that you might face a few common problems below:

1.  The including code for a query are duplicated between the Biz actions.

2.  Need to write different code to validate a business condition for a query
    and In-memory entities.

To resolve the above problems, I would like to share Specification
implementation for DDD entities as below.​

### I. Spec generic abstract class

This interface had been added allow to define a specification for an entity not
only for Db querying but also for in-memory validation.

Which this implementation also helps to have a better biz knowledge organization
for an application.

### II. How is Spec work?

Which sample above, Re-write it with Spec pattern as below.

1.  **Define Disabled Users Spec which including Address.**

    ```csharp
    public class DisabledUserWithAddress : Spec<User>
    {
        public override IQueryable<User> Includes(IQueryable<User> query)
            => query.Include(u => u.Addresses);

        public override Expression<Func<User, bool>> ToExpression()
                => u => u.IsDisabled;
    }
    ```

2.  **Query data for a Spec**

    ```csharp
    //Get all diabled users including their addesses.
    var users = dbContext.ForSpec(new DisabledUserWithAddress()).ToList();
    ```

3.  **Verify in memory user whether that user is matched**

    `DisabledUserWithAddress` **condition or not.**

    ```csharp
    var isDisabled = new DisabledUserWithAddress().IsSatisfied(user);
    ```

4.  **Spec combinations**

The Spec is also supporting a few combinations below:

**NotMe**

Let's say if you want to get all active users in Db? You don't need to create a
new one instead just call the `NotMe` method:

```csharp
    //Get all diabled users including their addesses.
    var users = dbContext.ForSpec(new DisabledUserWithAddress().NotMe()).ToList();
```

**And**

```csharp
    //Active user Spec
    var activeSpec = new DisabledUserWithAddress().NotMe();
    //Have Login Name is "Duy"
    var loginNameIsDuy = new LoginNameSpec("Duy");
    //Combine 2 sepc:Active users AND LoginName == "Duy"
    var users = dbContext.ForSpec(activeSpec.And(loginNameIsDuy)).ToList();
```

**Or**

```csharp
    //Active user Spec
    var activeSpec = new DisabledUserWithAddress().NotMe();
    //Have Login Name is "Duy"
    var loginNameIsDuy = new LoginNameSpec("Duy");
    //Combine 2 sepc: Active Users OR LoginName == "Duy"
    var users = dbContext.ForSpec(activeSpec.Or(loginNameIsDuy)).ToList();
```

**ButNot**

```csharp
    //Active user Spec
    var activeSpec = new DisabledUserWithAddress().NotMe();
    //Have Login Name is "Duy"
    var loginNameIsDuy = new LoginNameSpec("Duy");
    //Combine 2 sepc: Active Users AND LoginName != "Duy"
    var users = dbContext.ForSpec(activeSpec.ButNot(loginNameIsDuy)).ToList();
```

## Ordering Builder

## Lifecycle Hooks Handling

There are a few interfaces had added that allow entity aware when it is saved to
Db.

**1. `ISavingAwareness` interface**

By Implement this interface to an Entity, the method `OnSaving` will be called
right before the entity being saved to Db. The use case of this feature allows
you to perform all calculation for all computed properties in one place. This
method will be called before the validation.

**Use-case 1**: Calculate Age of User when Birthday set

```csharp
public class User: BaseEntity, ISavingAwareness
{
    #region Public Properties
    public DateTime? Birthday {get;set;}
    public int Age {get;private set;}
     ...
    #endregion Public Properties

    public void OnSaving(EntityState state, DbContext dbContext)
    {
        if(state == EntityState.Delete || BirthDay == null)
        {
            //Do some other validation for deleting.
            return;
        }

        //Calculate Age for Insert and Update
        var today = DateTime.Today;
        // Calculate the age.
        var age = today.Year - Birthday.Value.Year;
        // Go back to the year the person was born in case of a leap year
        if (Birthday > today.AddYears(-age))
            age--;
        //Assgin the value to Property
        Age = age;
    }
}
```

**Use-case 2:** Re-Calculate only when property got changed.

In use case 1 the Age property got updated whenever User got changed even
Birthday is not changed. If the calculation property depends on navigation
properties the calculation may wrong as the data of the navigation
properties were not loaded propriety.

In this use case, I would like to propose the other way to calculate the computed
the property when the related got changed.

Let review the `Payments` and `TotalPayment` of the User as below.

```c#
 public class User: ISavingAwareness
 {
        public virtual HashSet<Payment> Payments { get; private set; }

          [Column(TypeName = "Money")]
        public decimal TotalPayment { get; private set; }

        public void OnSaving(EntityState state, DbContext dbContext)
        {
            if (state == EntityState.Deleted) return;

            if (dbContext.Entry(this).HasChangeOn(i => i.Payments))
            {
                TotalPayment = Payments.Any() ? Payments.Sum(i => i.Amount) : 0;
            }
        }
  }
```

The Total payment property got re-calculated whenever Payments collection has changed. However, we need to ensure the Payments is loaded along with User when making changes. **If not the total amount won't be calculated.**

```c#
 var user = Db.Set<User>().Include(u => u.Payments).Last();

 //Update user Payment
 user.Payments.Add(new Payment { Amount = 100 });
 //Save changes
 await Db.SaveChangesAsync();
```

> **Instead of loading all the navigation properties for every Biz action, you should use Spec<T> to load required navigation properties based on the need of the Biz action in order to improve the performance of the application.**

## Recommendation

1.  The entities SHOULD be implemented of `IEntity<>`

2.  The Mapper type SHOULD be implemented of `IEntityTypeConfiguration<>`

3.  Your DbContext SHOULD be implemented of
    `HBD.EfCore.Extensions.DbContext`

## Source Code

[Github](https://github.com/baoduy/HBD.EfCore.Extensions)

Hope the library useful. [drunkcoding.net](http://drunkcoding.net)
