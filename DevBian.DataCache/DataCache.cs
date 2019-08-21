using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Caching;

namespace System.Web.Caching
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
    public static CacheExpirationType ExpirationType;

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
      DataCache.ExpirationType = CacheExpirationType.SlidingExpiration;
      DataCache.ExpirationTime = TimeSpan.FromMinutes(20);
    }

    /// <summary>
    /// Возвращает типизированный объект из глобального кэша приложений по уникальному имени
    /// </summary>
    /// <typeparam name="T">Тип возвращаемого объекта</typeparam>
    /// <param name="key">Имя объекта в кэше</param>
    /// <param name="defaultValue">Значение, возвращаемое по умолчанию, если запрашиваемого объекта нет в кэше</param>
    /// <returns>Извлеченный объект, или <paramref name="defaultValue"/> - если в кэше указанного объекта не обнаружено</returns>
    public static T GetData<T>(string key, T defaultValue = default(T))
    {
      if (DataCache.IsCacheEnable)
      {
        object result = HttpRuntime.Cache.Get(key);
        if (result is T) return (T)result;
        else if (result == null) return defaultValue;
        else
          try
          {
            return (T)Convert.ChangeType(result, typeof(T));
          }
          catch
          {
            return defaultValue;
          }
      }
      return defaultValue;
    }

    /// <summary>
    /// Создаёт новый типизированный объект и заполняет его свойствами объекта из кэша. 
    /// Идентичен методу <seealso cref="GetData{T}(string, T)"/>, но вместо возвращения объекта из кэша 
    /// дублирует его значения в новом объекте.
    /// </summary>
    /// <typeparam name="T">Тип возвращаемого объекта</typeparam>
    /// <param name="key">Имя объекта в кэше</param>
    /// <param name="defaultValue">Значение, возвращаемое по умолчанию, если запрашиваемого объекта нет в кэше</param>
    /// <returns>Извлеченный объект, или <paramref name="defaultValue"/> - если в кэше указанного объекта не обнаружено</returns>
    public static T GetDeepCopiedData<T>(string key, T defaultValue = default(T))
    {
      if (DataCache.IsCacheEnable)
      {
        object result = HttpRuntime.Cache.Get(key);
        if (result is T) return DataCache.DeepCopy<T>((T)result);
        else if (result == null) return defaultValue;
        else
          try
          {
            return DataCache.DeepCopy<T>((T)Convert.ChangeType(result, typeof(T)));
          }
          catch
          {
            return defaultValue;
          }
      }
      return defaultValue;
    }

    /// <summary>
    /// Дублирует объект для отвязки от кэша используя побайтное копирование
    /// </summary>
    /// <typeparam name="T">Тип возвращаемого объекта</typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    private static T DeepCopy<T>(T obj)
    {
      using (MemoryStream ms = new MemoryStream())
      {
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(ms, obj);
        ms.Seek(0, SeekOrigin.Begin);
        return (T)bf.Deserialize(ms);
      }
    }

    /// <summary>
    /// Добавляет объект в глобальный кэш приложений используя параметры кэширования заданные по умолчанию.
    /// Если объект по такому имени уже существует, то он будет переписан новым значением.
    /// </summary>
    /// <param name="key">Имя добавляемого объекта в кэше</param>
    /// <param name="value">Значение добавляемого объекта в кэше</param>
    public static void InsertData(string key, object value)
    {
      if (DataCache.IsCacheEnable)
      {
        if (DataCache.ExpirationType == CacheExpirationType.SlidingExpiration)
        {
          HttpRuntime.Cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, DataCache.ExpirationTime);
        }
        else if (DataCache.ExpirationType == CacheExpirationType.AbsoluteExpiration)
        {
          HttpRuntime.Cache.Insert(key, value, null, DateTime.UtcNow.Add(DataCache.ExpirationTime), Cache.NoSlidingExpiration);
        }
        else
        {
          HttpRuntime.Cache.Insert(key, value);
        }
      }
    }

    /// <summary>
    /// Добавляет объект в глобальный кэш приложений с абсолютным кэшированием на заданное время.
    /// Если объект по такому имени уже существует, то он будет переписан новым значением.
    /// </summary>
    /// <param name="key">Имя добавляемого объекта в кэше</param>
    /// <param name="value">Значение добавляемого объекта в кэше</param>
    /// <param name="expirationTime">Время хранения объекта в кэше</param>
    public static void InsertAbsoluteExpirationData(string key, object value, TimeSpan expirationTime)
    {
      if (DataCache.IsCacheEnable)
      {
        HttpRuntime.Cache.Insert(key, value, null, DateTime.UtcNow.Add(expirationTime), Cache.NoSlidingExpiration);
      }
    }

    /// <summary>
    /// Добавляет объект в глобальный кэш приложений со скользящим кэшированием на заданное время.
    /// Если объект по такому имени уже существует, то он будет переписан новым значением.
    /// </summary>
    /// <param name="key">Имя добавляемого объекта в кэше</param>
    /// <param name="value">Значение добавляемого объекта в кэше</param>
    /// <param name="expirationTime">Время хранения объекта в кэше</param>
    public static void InsertSlidingExpirationData(string key, object value, TimeSpan expirationTime)
    {
      if (DataCache.IsCacheEnable)
      {
        HttpRuntime.Cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, expirationTime);
      }
    }

    /// <summary>
    /// Добавляет объект в глобальный кэш приложений с заданными параметрами кэширования.
    /// Если объект по такому имени уже существует, то он будет переписан новым значением.
    /// </summary>
    /// <param name="key">Имя добавляемого объекта в кэше</param>
    /// <param name="value">Значение добавляемого объекта в кэше</param>
    /// <param name="expirationType">Тип хранения объекта в кэше</param>
    public static void InsertExpirationData(string key, object value, CacheExpirationType expirationType)
    {
      if (DataCache.IsCacheEnable)
      {
        if (expirationType == CacheExpirationType.AbsoluteExpiration)
        {
          HttpRuntime.Cache.Insert(key, value, null, DateTime.UtcNow.Add(DataCache.ExpirationTime), Cache.NoSlidingExpiration);
        }
        else if (expirationType == CacheExpirationType.SlidingExpiration)
        {
          HttpRuntime.Cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, DataCache.ExpirationTime);
        }
        else
        {
          HttpRuntime.Cache.Insert(key, value);
        }
      }
    }

    /// <summary>
    /// Добавляет объект в глобальный кэш приложений с заданными параметрами кэширования.
    /// Если объект по такому имени уже существует, то он будет переписан новым значением.
    /// </summary>
    /// <param name="key">Имя добавляемого объекта в кэше</param>
    /// <param name="value">Значение добавляемого объекта в кэше</param>
    /// <param name="expirationType">Тип хранения объекта в кэше</param>
    /// <param name="expirationTime">Время хранения объекта в кэше</param>
    public static void InsertExpirationData(string key, object value, CacheExpirationType expirationType, TimeSpan expirationTime)
    {
      if (DataCache.IsCacheEnable)
      {
        if (expirationType == CacheExpirationType.AbsoluteExpiration)
        {
          HttpRuntime.Cache.Insert(key, value, null, DateTime.UtcNow.Add(expirationTime), Cache.NoSlidingExpiration);
        }
        else if (expirationType == CacheExpirationType.SlidingExpiration)
        {
          HttpRuntime.Cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, expirationTime);
        }
        else
        {
          HttpRuntime.Cache.Insert(key, value);
        }
      }
    }

    /// <summary>
    /// Удаляет объект из глобального кэша приложений по уникальному имени
    /// </summary>
    /// <param name="key">Имя объекта в кэше</param>
    public static void RemoveDataByKey(string key)
    {
      if (DataCache.IsCacheEnable)
      {
        HttpRuntime.Cache.Remove(key);
      }
    }

    /// <summary>
    /// Удаляет объекты из глобального кэша приложений по частичному совпадению имени.
    /// Удаляются все объекты, имена которых начинаются с указанного значения.
    /// </summary>
    /// <param name="keyStartsWith">Частичное имя объекта в кэше</param>
    public static void RemoveAllDataByKey(string keyStartsWith)
    {
      if (DataCache.IsCacheEnable)
      {
        IDictionaryEnumerator eCache = HttpRuntime.Cache.GetEnumerator();
        while (eCache.MoveNext())
        {
          string key = eCache.Key as string;
          if (!string.IsNullOrEmpty(key) && key.StartsWith(keyStartsWith))
          {
            HttpRuntime.Cache.Remove(key);
          }
        }
      }
    }

    /// <summary>
    /// Удаляет все объекты из глобального кэша приложений
    /// </summary>
    public static void RemoveAllData()
    {
      if (DataCache.IsCacheEnable)
      {
        IDictionaryEnumerator eCache = HttpRuntime.Cache.GetEnumerator();
        while (eCache.MoveNext())
        {
          string key = eCache.Key as string;
          HttpRuntime.Cache.Remove(key);
        }
      }
    }

  }
}