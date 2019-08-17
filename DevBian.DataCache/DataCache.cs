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
		public static bool IsCacheEnable { get; set; } = true;

		/// <summary>
		/// 
		/// </summary>
		public static DateTime AbsoluteExpiration { get; set; } = Cache.NoAbsoluteExpiration;

		/// <summary>
		/// 
		/// </summary>
		public static TimeSpan SlidingExpiration { get; set; } = Cache.NoSlidingExpiration;

		/// <summary>
		/// Возврашает ссылку на объект ASP.NET кэша, если кэширование разрешено.
		/// При запрещении ведения кэша, всегда возврашает null.
		/// </summary>
		private static Cache Cache
		{
			get
			{
				if (DataCache.IsCacheEnable) return HttpRuntime.Cache;
				return null;
			}
		}

		/// <summary>
		/// Возвращает типизированный объект из глобального кэша приложений по уникальному имени
		/// </summary>
		/// <typeparam name="T">Тип возвращаемого объекта</typeparam>
		/// <param name="CacheName">Имя объекта в кэше</param>
		/// <param name="DefaultResult"></param>
		/// <returns>Извлеченный объект, или DefaultResult - если в кэше указанного объекта не обнаружено</returns>
		public static T GetData<T>(string CacheName, T DefaultResult = default(T))
		{
			Cache cache = DataCache.Cache;
			if (cache != null)
			{
				object result = cache.Get(CacheName);
				if (result is T) return (T)result;
				else if (result == null) return DefaultResult;
				else
					try
					{
						return (T)Convert.ChangeType(result, typeof(T));
					}
					catch
					{
						return DefaultResult;
					}
			}
			return DefaultResult;
		}

		/// <summary>
		/// Добавляет объект в глобальный кэш приложений.
		/// Если объект по такому имени уже существует, то он будет переписан новым значением.
		/// </summary>
		/// <param name="CacheName">Имя добавляемого объекта в кэше</param>
		/// <param name="CacheValue">Значение добавляемого объекта в кэше</param>
		public static void InsertData(string CacheName, object CacheValue, CacheDependency Dependency = null, DateTime? AbsoluteExpiration = null, TimeSpan? SlidingExpiration = null)
		{
			Cache cache = DataCache.Cache;
			if (cache != null)
			{
				if (AbsoluteExpiration.HasValue)
					cache.Insert(CacheName, CacheValue, Dependency, AbsoluteExpiration.Value, Cache.NoSlidingExpiration);
				else if (SlidingExpiration.HasValue)
					cache.Insert(CacheName, CacheValue, Dependency, Cache.NoAbsoluteExpiration, SlidingExpiration.Value);
				else
					cache.Insert(CacheName, CacheValue, Dependency, DataCache.AbsoluteExpiration, DataCache.SlidingExpiration);
			}
		}

		/// <summary>
		/// Удаляет объект из глобального кэша приложений по уникальному имени
		/// </summary>
		/// <param name="CacheName">Имя объекта в кэше</param>
		public static void RemoveData(string CacheName)
		{
			Cache cache = DataCache.Cache;
			if (cache != null) cache.Remove(CacheName);
		}

		/// <summary>
		/// Удаляет объекты из глобального кэша приложений по частичному совпадению имени.
		/// Удаляются все объекты, имена которых начинаются с указанного значения.
		/// </summary>
		/// <param name="CacheNameStart">Частичное имя объекта в кэше</param>
		public static void RemoveAllData(string CacheNameStart)
		{
			Cache cache = DataCache.Cache;
			if (cache != null)
			{
				IDictionaryEnumerator eCache = cache.GetEnumerator();
				while (eCache.MoveNext())
				{
					string key = eCache.Key as string;
					if (!string.IsNullOrEmpty(key) && key.StartsWith(CacheNameStart))
						cache.Remove(key);
				}
			}
		}

		/// <summary>
		/// Дублирует объект для отвязки от кэша
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static T DeepCopy<T>(T obj)
		{
			using (var ms = new MemoryStream())
			{
				var bf = new BinaryFormatter();
				bf.Serialize(ms, obj);
				ms.Seek(0, SeekOrigin.Begin);
				return (T)bf.Deserialize(ms);
			}
		}

	}
}