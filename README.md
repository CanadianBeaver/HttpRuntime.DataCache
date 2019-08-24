The ASP.NET WebForms Cache was implemented very conveniently for temporary keeping the operational data. In the early years of the .NET platform, developers often used to work with `System.Web` namespace, even in WinForms applications.

The code template, that is constantly being offered in articles about ASP.NET caching, is very simple and practical:

```csharp
// try to get an instance of object from cache
DataSet ds = HttpRuntime.Cache["KeyName"] as DataSet;
// check the result and recreate it, if it is null
if (ds == null)
{
    ds = QueryDataFromDatabase();
    HttpRuntime.Cache.Insert("KeyName", ds);
}
// using the instance of object that has been populated from cache or from storage
DataRow dr = ds.Tables[0].Rows[0];
```

However, the ASP.NET Cache implementation does not include some desirable features.

### ASP.NET cache settings

Although the [Cache.Insert(String, Object)](https://docs.microsoft.com/en-us/dotnet/api/system.web.caching.cache.insert?view=netframework-1.1#System_Web_Caching_Cache_Insert_System_String_System_Object_) method, that adds a value into the cache, is suitable for most cases, it is often desirable to applay different settings for caching data based on the web application's settings stored in the Web.config file, and sometimes even completely disabling the cache for the entire application. In this case, the source code should not be changed and application should run without recompiling.

### Generic methods and default values

The ASP.NET cache always returns instances of the `object` class, no matter what the actual type of the instance is. In most cases, this is not a problem, because the `Nullable types` can be used instead of the `Value types`. Perhaps you would like to have generic methods for retrieving data from the cache:

```csharp
// this is a dafault extraction data from cache
myClassName item = HttpRuntime.Cache["Key1"] as myClassName; 

// this is a desired extraction data with generic methods
myClassName item = DataCache.Get<myClassName>("Key1"); 
```

If the generic methods are not satisfied for you, then it is possible to use the default version. In fact, these two options are identical. But the generic methods offer additional features. For example, the default value could be define if the cache does not contains a value to be retrieved:

```csharp
myClassName defaultValue = new myClassName(/* init properties */);
myClassName item = DataCache.Get<myClassName>("Key1", defaultValue); 
/* if there is nothing in the cache, then the item will be defaultValue */
```

### Deep copied data

The ASP.NET cache always returns the object stored in the cache. This means that changes to any property of the extracted object will also change the object stored in the cache, since these are the same objects. In some cases, you may want to modify the retrieved object, but leave the object in the cache unchanged. One of the convenient functionality is to retrieve a copy of an object from the cache which can be changed without worrying about the object in the cache.

### Regions of cheched data 

The ASP.NET cache is similar to a `NameValueCollection`. It is simple and elegant solution for a small web application, but it becomes difficult to generate the keys when application grows up. For example, consider caching two related tables, such as Vendors and Models. In this case web application will keep all data from the Vendors table in one cached object and many cached objects for associated records of the Models table. The key for Vendors supposed to be a `Vendors` and the keys for Models supposed to be `Models.[VendorID]`. When Vendors table has changed, cached instance for Vendors and only associated instances for Models have to be removed from cache. This means that data should be cached and removed from cache by regions.

## Implementation

The described functions are optional and can be easily implemented in any web application. [Proxy template](https://en.wikipedia.org/wiki/Proxy_pattern) is a good option for implementing all the described features. The resulted library can be used on all .NET Frameworks, starting version 2.0. The .NET Framework 4.0 introduces the `System.Runtime.Caching` namespace and several classes with a new caching model. Therefore, using the standard ASP.NET cache and its improvements is not required.

Sad but true.

### Properties and settings

```csharp
/// <summary>
/// Is cache used or not
/// </summary>
public static bool IsCacheEnable;

/// <summary>
/// How to store objects in the cache
/// </summary>
public static CacheExpirationType ExpirationType;

/// <summary>
/// How long objects should be stored in the cache 
/// </summary>
public static TimeSpan ExpirationTime;
```

Enum `CacheExpirationType` defines how to store objects in the cache

```csharp
/// <summary>
/// How to store objects in the cache
/// </summary>
public enum CacheExpirationType
{
    /// <summary>
    /// Without time limit, the value of the <seealso cref = "DataCache.ExpirationTime" /> property will be ignored
    /// </summary>
    NoExpiration,

    /// <summary>
    /// The <seealso cref="DataCache.ExpirationTime"/> at which the inserted object expires and is removed from the cache
    /// </summary>
    AbsoluteExpiration,

    /// <summary>
    /// The interval <seealso cref="DataCache.ExpirationTime"/> between the time the inserted object is last accessed and the time at which that object expires
    /// </summary>
    SlidingExpiration
}
```

### Methods to extract data from the cache

```csharp
/// <summary>
/// Retrieves the specified item from the Cache object
/// </summary>
/// <typeparam name="T">The type for the cache item to retrieve</typeparam>
/// <param name="key">The identifier for the cache item to retrieve</param>
/// <param name="defaultValue">The default value if the object is not in the cache</param>
/// <returns>The retrieved cache item, or <paramref name="defaultValue"/> if the key is not found</returns>
public static T GetData<T>(string key, T defaultValue = default(T))

/// <summary>
/// Retrieves the deep copied of specified item from the Cache object
/// </summary>
/// <typeparam name="T">The type for the cache item to retrieve</typeparam>
/// <param name="key">The identifier for the cache item to retrieve</param>
/// <param name="defaultValue">The default value if the object is not in the cache</param>
/// <returns>The retrieved cache item, or <paramref name="defaultValue"/> if the key is not found</returns>
public static T GetDeepCopiedData<T>(string key, T defaultValue = default(T))
```

### Methods to store data in the cache

```csharp
/// <summary>
/// Inserts an item into the cache with a cache key to reference its location, using default values provided by the settings
/// </summary>
/// <param name="key">The cache key used to reference the item</param>
/// <param name="value">The object to be inserted into the cache</param>
public static void InsertData(string key, object value)

/// <summary>
/// Inserts an item into the cache with a cache key to reference its location, using the absolute expiration time
/// </summary>
/// <param name="key">The cache key used to reference the item</param>
/// <param name="value">The object to be inserted into the cache</param>
/// <param name="expirationTime">How long the object should be stored in the cache</param>
public static void InsertAbsoluteExpirationData(string key, object value, TimeSpan expirationTime)

/// <summary>
/// Inserts an item into the cache with a cache key to reference its location, using the sliding expiration time
/// </summary>
/// <param name="key">The cache key used to reference the item</param>
/// <param name="value">The object to be inserted into the cache</param>
/// <param name="expirationTime">How long the object should be stored in the cache</param>
public static void InsertSlidingExpirationData(string key, object value, TimeSpan expirationTime)

/// <summary>
/// Inserts an item into the cache with a cache key to reference its location, using the type of expiration and default value for expiration time
/// </summary>
/// <param name="key">The cache key used to reference the item</param>
/// <param name="value">The object to be inserted into the cache</param>
/// <param name="expirationType">How to store objects in the cache</param>
public static void InsertExpirationData(string key, object value, CacheExpirationType expirationType)

/// <summary>
/// Inserts an item into the cache with a cache key to reference its location, using the type of expiration and expiration time
/// </summary>
/// <param name="key">The cache key used to reference the item</param>
/// <param name="value">The object to be inserted into the cache</param>
/// <param name="expirationType">How to store objects in the cache</param>
/// <param name="expirationTime">How long the object should be stored in the cache</param>
public static void InsertExpirationData(string key, object value, CacheExpirationType expirationType, TimeSpan expirationTime)
```

### Methods to remove data from the cache

```csharp
/// <summary>
/// Removes the specified item from the application's cache 
/// </summary>
/// <param name="key">An identifier for the cache item to remove</param>
public static void RemoveDataByKey(string key)

/// <summary>
/// Removes all items from the application's cache that starts with key
/// </summary>
/// <param name="keyStartsWith">An starts with identifier for the cache item to remove</param>
public static void RemoveAllDataByKey(string keyStartsWith)

/// <summary>
/// Removes all items from the application's cache 
/// </summary>
public static void RemoveAllData()
```

## Usage example

#### 1. Settings the default parameters when application starts

```csharp
public class Global : System.Web.HttpApplication
{
    protected void Application_Start(object sender, EventArgs e)
    {
        Settings settings = Settings.Default;
        DataCache.IsCacheEnable = settings.IsCacheEnable;
        DataCache.ExpirationType = settings.ExpirationType;
        DataCache.ExpirationTime = settings.ExpirationTime;
    }
}
```

#### 2. Implement the standard template working with cache on the Page or in DAL

```csharp
public partial class DefaultPage : System.Web.UI.Page
{
    public const string STR_CACHENAME = "something";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            SomethingDataModel val = DataCache.GetData<SomethingDataModel>(STR_CACHENAME);
            if (val == null) 
            {
                SomethingDataModel val = new SomethingDataModel(); // get data from ...
                DataCache.InsertData(STR_CACHENAME, val);
            }
            DataBind(val); // use data on the page
        }
    }
}
```

If you want to define time settings that are different from specified by default in the cache, use one of the overridden `Insert{Which}Data` methods.

#### 3. In the admin panel or when the database was updated remove also cached values by key or by key that starts with

```csharp
public partial class DefaultPage : System.Web.UI.Page
{
    public const string STR_CACHENAME = "something";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.IsPostBack)
        {
            DAL.Update(); // update the database ...
            DataCache.RemoveAllDataByKey(STR_CACHENAME); // clear cache
            Response.Redirect("Default.aspx"); // proccessing the request
        }
    }
}
```

## Support or Contact

Having questions? [Contact me](https://github.com/CanadianBeaver) and I will help you sort it out.

<style>.inner { min-width: 800px !important; max-width: 60% !important;}</style>
