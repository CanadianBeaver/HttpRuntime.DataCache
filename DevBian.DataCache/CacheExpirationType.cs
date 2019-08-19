using System;

namespace DevBian.Caching
{
  /// <summary>
  /// Тип хранения объектов в кэше
  /// </summary>
  public enum CacheExpirationType
  {
    /// <summary>
    /// Без ограничения по времени, значение свойства <seealso cref="DataCache.ExpirationTime"/> будет проигнорировано
    /// </summary>
    NoExpiration,

    /// <summary>
    /// Абсолютное кэширование на заданное время в свойстве <seealso cref="DataCache.ExpirationTime"/>
    /// </summary>
    AbsoluteExpiration,

    /// <summary>
    /// Скользящее кэширование на заданное время в свойстве <seealso cref="DataCache.ExpirationTime"/>
    /// </summary>
    SlidingExpiration
  }
}