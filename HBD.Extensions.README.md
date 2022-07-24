# HBD.EfCore.Extensions

As you know EfCore is the best library to manage your databases accessing for all .Net Core Applications.
I have built a few extension to help developers to speed up the development. They will focus to the Biz logic implementation and leave all the configuration complexity to these library.
I love DDD principals and you too so that Theses library are fully compatible and tested with all DDD principal implementation. 

- Auto-scan and import all `IEntityTypeConfiguration` from assemblies to a DbContext.

  ```c#
  services.AddDbContext<YourDbContext>(op => op.UseAutoConfigModel(i => i.ScanFrom(typeof(YourDbContext).Assembly)))
  ```

- If you don't want to create `IEntityTypeConfiguration` for every Entity. You can define one generic type of `IEntityTypeConfiguration`. The extension will help to map all Entity to this type automatically without any extra steps need you to do.

- Auto-scan and import all `IDataSeedingConfiguration` from assemblies to a DbContext. Allows you to separate Model configuration and Data Seeding to different class by providing implementation as below.

  ```c#
  //1. Define the Data Seeding
  internal class AccountDataSeeding: IDataSeedingConfiguration<Account>{
      public TEntity[] Data => new [
          new Account {Id= 1, UserName = "admin", Password="123"},
          new Account {Id= 2, UserName = "user", Password="123"}
      ];
  }
  ```

- Support to build re-usable Query Specification and Ordering Condition in separate classes. This supports the DDD design when writing the QueryServices at the Application Service layer (DDD principal).

  [For more information about this library please refer here](/HBD.EfCore.Extensions.md)

## HBD.EfCore.Hooks

This extensions allows to inject the logic to handle before and after save activities.

#### To use this extensions

1. Create a an implement for one of below interfaces to handle the saving activities.

   ```c#
   //Handle before save
   public interface IBeforeSaveHook{}
   //Handle after save
   public interface IAfterSaveHook{}
   //Handle before save async
   public interface IBeforeSaveHookAsync{}
   //Handle after save async
   public interface IAfterSaveHookAsync{}
   
   //Handle before and after save
   public interface IHook{}
   //Handle before and after save async
   public interface IHookAsync{}
   ```
   
2. Register the hook runner to your dependence injection.

```c#
// To ensure DataAuthorization work property you need to register all below dependency services to your DI.
services
    //2. Register your hooks
    .AddHook<YourHook1>()
    .AddHook<YourHook2>()
    //3. Your DbContext with Hook Interceptor
    .AddDbContextWithHook<YourDbContext>(builder => builder.UseSqliteMemory());
```

If you would like to manage the AddDbContext by yourselves then using the below service to reginster Hook Interceptor to your DbContextBuilder.

```c#
// To ensure DataAuthorization work property you need to register all below dependency services to your DI.
services
    //1. Register your hooks
    .AddHook<YourHook1>()
    .AddHook<YourHook2>()
     //2. Add HookRunner from HBD.EfCore.Hooks
     .AddHookRunner()
     //3. Your DbContext with Hook Interceptor
     .AddDbContext<TDbContext>((provider, options) =>
     {
        ///Your condig here
        //...         
        options.AddHookInterceptor(provider);
     });
```

## HBD.EfCore.DataAuthorization

This is an hook implementation allows to handle the [data authorisation](https://www.thereformedprogrammer.net/part-2-handling-data-authorization-asp-net-core-and-entity-framework-core/). To use this functionality you need:

1. Mark your entity with IDataKeyEntity. The Hook will update DataKey property automatically from `IDataKeyProvider` when entity got created.

   ```c#
   public class YourEntity: IDataKeyEntity
   {
    	public Guid? DataKey { get; private set; }
     public void UpdateDataKey(Guid dataKey) => DataKey = dataKey;
   }
   ```

2. Update your DbContext to support Data Authorisation as bellow.

   ```c#
   public class YourDbContext : DbContext, IDataKeyContext
   {
       internal readonly IDataKeyProvider _dataKeyProvider;
   
       public YourDbContext(DbContextOptions options,
            [AllowNull] IEnumerable<IDataKeyProvider> dataKeyProviders)
               : base(options)
       {
          _dataKeyProvider = dataKeyProviders?.SingleOrDefault();
       }
   
       public IEnumerable<Guid> ImpersonateKeys => _dataKeyProvider?.GetImpersonateKeys() ?? Enumerable.Empty<Guid>();
   
       public Guid OwnershipKey => _dataKeyProvider?.GetOwnershipKey() ?? Guid.Empty;
   }
   ```

3. Implement and Register your Data Key Provider as below. 
   
To implement DataKeyProvider you need to provider at lest the `GetOwnershipKey` this key will be updated to the `DataKey` property of all `IDataKeyEntity` when it created to tell that this data is belong to person who own this key. This usually is the current user identity *(primary key, ...)*.

Implement your DataKeyProvider as sample below:

   ```c#
   public class TestDataKeyProvider : IDataKeyProvider
   {
      //This is combined all the keys of all entities that current login user able to access.
      //You should have a system to manage the impersonate permission of the users and store the impersonate keys into a table.
      //This should be loaded when your applicated started.
     public IEnumerable<Guid> GetImpersonateKeys() => new[] {GetOwnershipKey()};
   
     //This is the key of current login users. Which will be added to all entities that created by current users.
     //Depend on your business requirement the key may be different. It shall be user ID or Department ID of your company.
     public Guid GetOwnershipKey()=>new Guid("bc2c7648-6e0e-41f9-adff-b344302fdc8d");
   }
   ```

When reading entities up. it will be filtered by ownership key or impersonate keys. Impersonate keys are those key belong to ther users. However, had been authorised to this users for read and operation. *(Approval on behaft.)*

```c#
// To ensure DataAuthorization work property you need to register all below dependency services to your DI.
services
    //1. Add Your Data Key from HBD.EfCore.DataAuthorization
    .AddDataKeyProvider<YourDataKeyProvider>()
    //2. Add HookRunner from HBD.EfCore.Hooks
    .AddHookRunner()
    .AddDbContext<YourDbContext>(builder => builder.UseSqliteMemory()
         //3. Register Global Query Filter for all IDataKeyEntity from HBD.EfCore.Extensions
         .UseAutoConfigModel());
   ```

## HBD.EfCore.Repos

The repositories pattern for EfCore. These repositories will help you to build QueryService faster, or even you can import and use them directly by calling.

You can register them as generic repositories without any customization.

 ```c#
services.AddGenericRepositories<YourDbContext>();
 ```
Or customize the repository with your application business by inherit below implemented repositories.

### ReadOnlyRepository<TEntity>

This class is implementing `IReadOnlyRepository<TEntity>` allows reading the Entity from the Database.

All Queries return from this class will call `AsNoTracking` by default to ensure no write action had been performing accidentally when you call DbContext.SaveChanges.

Below are exposed methods from the interface.

```c#
 public interface IReadOnlyRepository<TEntity> where TEntity : class
 {
    public IQueryable<TEntity> GetQuery();
    public ValueTask<List<TEntity>> ReadAsync(Expression<Func<TEntity, bool>> filter, IQueryBuilder<TEntity> ordering = null);
    public ValueTask<List<TEntity>> ReadAsync<TKey>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TKey>> ordering = null);
    public ValueTask<TEntity> ReadSingleAsync(params object[] id);
    public ValueTask<TEntity> ReadSingleAsync(Expression<Func<TEntity, bool>> filter);
}
```

### Repository<TEntity>

Similar to the read-only repository. However, the query is a tracking change query, and there are a few more methods provided below for CURD entities from Databases.

```c#
public interface IRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
{
    void Add(TEntity entity);
    ValueTask<int> AddAndSaveAsync(TEntity entity);
    ValueTask<int> DeleteAndSaveAsync(params object[] id);
    Task DeleteAsync(params object[] id);
    ValueTask<int> SaveAsync(CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    ValueTask<int> UpdateAndSaveAsync(TEntity entity);
}
```

### DtoRepository<TEntity>

Most of the time, you need to map the Entity to a model or Dto before return to the client at your API level.

This repository provides similar functionality to ReadOnlyRepository above. However, also help to map the Entity to DTO directly by using Automapper. This class will help to simplify a lot of implementation of your application.

```c#
public interface IDtoRepository<TEntity> where TEntity : class
{
    public IAsyncEnumerable<TDto> ReadAsync<TDto>(Expression<Func<TEntity, bool>> filter = null, IQueryBuilder<TDto> ordering = null) where TDto : class;
    public IAsyncEnumerable<TDto> ReadIgnoreFiltersAsync<TDto>(Expression<Func<TEntity, bool>> filter = null, IQueryBuilder<TDto> ordering = null) where TDto : class;
    public ValueTask<IPageable<TDto>> ReadPageAsync<TDto>(int pageIndex, int pageSize, IQueryBuilder<TDto> ordering, Expression<Func<TEntity, bool>> filter = null) where TDto : class;
    public ValueTask<IPageable<TDto>> ReadPageIgnoreFiltersAsync<TDto>(int pageIndex, int pageSize, IQueryBuilder<TDto> ordering, Expression<Func<TEntity, bool>> filter = null) where TDto : class;
    public ValueTask<TDto> ReadSingleAsync<TDto>(Expression<Func<TEntity, bool>> filter) where TDto : class;
}
```

Hope the libraries useful. [drunkcoding.net](http://drunkcoding.net)

