## DataCache

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
