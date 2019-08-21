## DataCache

The ASP.NET cache in WebForms for storing data in memory was implemented very conveniently. In the early years of the .NET platform, developers often used to work with `System.Web` namespace, even in WinForms applications.

The code template, that is constantly being offered in articles about caching in ASP.NET WebForms, is very simple and practical:

```csharp
myClassName item = HttpRuntime.Cache["Key1"] as myClassName;
if (item == null)
{
    item = /* get value from database or from web service or from somewhere else */;
    HttpRuntime.Insert("Key1", item);
}
// using the item that has been populated from cache or from storage
```

However, the ASP.NET cache implementation does not include some good features.

#### Default settings

Although the [Cache.Insert(String, Object)](https://docs.microsoft.com/en-us/dotnet/api/system.web.caching.cache.insert?view=netframework-1.1#System_Web_Caching_Cache_Insert_System_String_System_Object_) method, that adds a value into the cache, is suitable for most cases, it is often desirable to applay different settings, perhaps based on the web application's settings stored in the `Web.config` file, and sometimes even completely disabling the cache for the entire application. In this case, the source code should not be changed and application should run without recompiling.





### Implementation

#### Properties and settings

```csharp
public static bool IsCacheEnable;
```

Whether the cache will be used or not to store the data.

```csharp
public static CacheExpirationType ExpirationType;
```

How the data will be stored in the cache. Possible options are: 
- NoExpiration,
- AbsoluteExpiration,
- SlidingExpiration.

More details https://docs.microsoft.com/en-us/dotnet/api/system.web.caching.cache.insert?view=netframework-4.8#System_Web_Caching_Cache_Insert_System_String_System_Object_System_Web_Caching_CacheDependency_System_DateTime_System_TimeSpan_

```csharp
public static TimeSpan ExpirationTime;
```

The interval between the time the inserted object was last accessed and the time at which that object expires.

#### Methods for extracting cached data



#### Methods for storing the data in cache



#### Usage example

```csharp
```

### Support or Contact

Having questions? [Contact me](https://github.com/CanadianBeaver) and I will help you sort it out.
 
<style> .inner { min-width: 800px !important; max-width: 60% !important; }</style>
