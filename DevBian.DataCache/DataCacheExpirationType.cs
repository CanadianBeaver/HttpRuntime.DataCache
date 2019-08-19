using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Caching;

namespace DevBian
{
  /// <summary>
  /// Тип хранения объектов в кэше, без ограничения по времени, абсолютное кэширование на заданное время, 
  /// скользящее кэширование на заданное время.
  /// </summary>
  public enum DataCacheExpirationType
  {
    NoExpiration,
    AbsoluteExpiration,
    SlidingExpiration
  }
}