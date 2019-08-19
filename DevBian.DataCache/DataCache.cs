using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Caching;

namespace DevBian
{
  public static class DataCache
  {
    /// <summary>
    /// Признак использования кэша для хранения данных
    /// </summary>
    public static bool IsCacheEnable;

    /// <summary>
    /// Тип хранения объектов в кэше
    /// </summary>
    public static DataCacheExpirationType ExpirationType;

    /// <summary>
    /// Время хранения объектов в кэше
    /// </summary>
    public static TimeSpan ExpirationTime;

    /// <summary>
    /// Статический конструктор инициализирует начальные значения свойств класса
    /// </summary>
    static DataCache()
    {
      DataCache.IsCacheEnable = true;
      DataCache.ExpirationType = DataCacheExpirationType.SlidingExpiration;
      DataCache.ExpirationTime = TimeSpan.FromMinutes(20);
    }

    /// <summary>
    /// Возврашает ссылку на объект ASP.NET кэша, если кэширование разрешено.
    /// При запрещении ведения кэша, всегда возврашает null.
    /// </summary>
    private static Cache GetCache()
    {
      if (DataCache.IsCacheEnable) return HttpRuntime.Cache;
      return null;
    }

    /// <summary>
    /// Возвращает типизированный объект из глобального кэша приложений по уникальному имени
    /// </summary>
    /// <typeparam name="T">Тип возвращаемого объекта</typeparam>
    /// <param name="cacheName">Имя объекта в кэше</param>
    /// <param name="defaultResult">Значение, возвращаемое по умолчанию, если запрашиваемого объекта нет в кэше</param>
    /// <returns>Извлеченный объект, или <paramref name="defaultResult"/> - если в кэше указанного объекта не обнаружено</returns>
    public static T GetData<T>(string cacheName, T defaultResult = default(T))
    {
      var cache = DataCache.GetCache();
      if (cache != null)
      {
        object result = cache.Get(cacheName);
        if (result is T) return (T)result;
        else if (result == null) return defaultResult;
        else
          try
          {
            return (T)Convert.ChangeType(result, typeof(T));
          }
          catch
          {
            return defaultResult;
          }
      }
      return defaultResult;
    }

    /// <summary>
    /// Создаёт новый типизированный объект и заполняет его свойтсвами объекта из кэша. 
    /// Идентичен методу <seealso cref="GetData{T}(string, T)"/>, но вместо возращение объекта из кэша 
    /// дублирует его значения в новом объекте.
    /// </summary>
    /// <typeparam name="T">Тип возвращаемого объекта</typeparam>
    /// <param name="cacheName">Имя объекта в кэше</param>
    /// <param name="defaultResult">Значение, возвращаемое по умолчанию, если запрашиваемого объекта нет в кэше</param>
    /// <returns>Извлеченный объект, или <paramref name="defaultResult"/> - если в кэше указанного объекта не обнаружено</returns>
    public static T GetDeepCopiedData<T>(string cacheName, T defaultResult = default(T))
    {
      var cache = DataCache.GetCache();
      if (cache != null)
      {
        object result = cache.Get(cacheName);
        if (result is T) return DataCache.DeepCopy<T>((T)result);
        else if (result == null) return defaultResult;
        else
          try
          {
            return DataCache.DeepCopy<T>((T)Convert.ChangeType(result, typeof(T)));
          }
          catch
          {
            return defaultResult;
          }
      }
      return defaultResult;
    }

    /// <summary>
    /// Дублирует объект для отвязки от кэша используя побайтное копирование
    /// </summary>
    /// <typeparam name="T">Тип возвращаемого объекта</typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    private static T DeepCopy<T>(T obj)
    {
      using (var ms = new MemoryStream())
      {
        var bf = new BinaryFormatter();
        bf.Serialize(ms, obj);
        ms.Seek(0, SeekOrigin.Begin);
        return (T)bf.Deserialize(ms);
      }
    }

    /// <summary>
    /// Добавляет объект в глобальный кэш приложений.
    /// Если объект по такому имени уже существует, то он будет переписан новым значением.
    /// </summary>
    /// <param name="cacheName">Имя добавляемого объекта в кэше</param>
    /// <param name="cacheValue">Значение добавляемого объекта в кэше</param>
    public static void InsertData(string cacheName, object cacheValue)
    {
      var cache = DataCache.GetCache();
      if (cache != null)
      {
        if (DataCache.ExpirationType == DataCacheExpirationType.SlidingExpiration)
          cache.Insert(cacheName, cacheValue, null, Cache.NoAbsoluteExpiration, DataCache.ExpirationTime);
        else if (DataCache.ExpirationType == DataCacheExpirationType.AbsoluteExpiration)
          cache.Insert(cacheName, cacheValue, null, DateTime.UtcNow.Add(DataCache.ExpirationTime), Cache.NoSlidingExpiration);
        else
          cache.Insert(cacheName, cacheValue);
      }
    }

    /// <summary>
    /// Добавляет объект в глобальный кэш приложений перекрывая параметры хранения по умолчанию.
    /// Если объект по такому имени уже существует, то он будет переписан новым значением.
    /// </summary>
    /// <param name="cacheName">Имя добавляемого объекта в кэше</param>
    /// <param name="cacheValue">Значение добавляемого объекта в кэше</param>
    /// <param name="expirationType">Тип хранения объекта в кэше</param>
    /// <param name="expirationTime">Время хранения объекта в кэше</param>
    public static void InsertData(string cacheName, object cacheValue, DataCacheExpirationType expirationType, TimeSpan expirationTime)
    {
      var cache = DataCache.GetCache();
      if (cache != null)
      {
        if (expirationType == DataCacheExpirationType.SlidingExpiration)
          cache.Insert(cacheName, cacheValue, null, Cache.NoAbsoluteExpiration, expirationTime);
        else if (expirationType == DataCacheExpirationType.AbsoluteExpiration)
          cache.Insert(cacheName, cacheValue, null, DateTime.UtcNow.Add(expirationTime), Cache.NoSlidingExpiration);
        else
          cache.Insert(cacheName, cacheValue);
      }
    }

    /// <summary>
    /// Удаляет объект из глобального кэша приложений по уникальному имени
    /// </summary>
    /// <param name="cacheName">Имя объекта в кэше</param>
    public static void RemoveData(string cacheName)
    {
      var cache = DataCache.GetCache();
      if (cache != null) cache.Remove(cacheName);
    }

    /// <summary>
    /// Удаляет объекты из глобального кэша приложений по частичному совпадению имени.
    /// Удаляются все объекты, имена которых начинаются с указанного значения.
    /// </summary>
    /// <param name="cacheNameStart">Частичное имя объекта в кэше</param>
    public static void RemoveAllData(string cacheNameStart)
    {
      var cache = DataCache.GetCache();
      if (cache != null)
      {
        IDictionaryEnumerator eCache = cache.GetEnumerator();
        while (eCache.MoveNext())
        {
          string key = eCache.Key as string;
          if (!string.IsNullOrEmpty(key) && key.StartsWith(cacheNameStart))
            cache.Remove(key);
        }
      }
    }

  }
}