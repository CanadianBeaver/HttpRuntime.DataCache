using Microsoft.VisualStudio.TestTools.UnitTesting;
using DevBian.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;
using System.Web.Caching;
using System.Threading;

namespace DevBian.Caching.Tests
{
  [TestClass()]
  public class DataCacheTests
  {
    [TestInitialize]
    public void Setup()
    {
      DataCache.RemoveAllData();
    }

    [TestMethod()]
    public void GetDataTest()
    {
      // Arrange
      int result = 1;
      DataCache.InsertData("a", result);

      // Act
      result = DataCache.GetData<int>("a");

      // Assert
      Assert.AreEqual(1, result);
    }

    [TestMethod()]
    [Ignore("Not implemented")]
    public void GetDeepCopiedDataTest()
    {
      Assert.Fail();
    }

    [TestMethod()]
    public void InsertDataTest()
    {
      // Arrange
      DataCache.InsertData("ddd1", 0);
      DataCache.InsertData("ddd2", 0);
      DataCache.InsertData("ddd3", 0);
      DataCache.InsertData("ddd4", 0);
      DataCache.InsertData("sss", 0);
      DataCache.InsertData("aaa", 0);

      // Assert
      Assert.AreEqual(HttpRuntime.Cache.Count, 6);

      // Act
      Thread.Sleep(TimeSpan.FromSeconds(1));

      // Assert
      Assert.AreNotEqual(HttpRuntime.Cache.Get("ddd1"), null);
    }

    [TestMethod()]
    public void InsertAbsoluteExpirationDataTest()
    {
      // Arrange
      DataCache.InsertAbsoluteExpirationData("ddd1", 0, TimeSpan.FromSeconds(3));
      DataCache.InsertData("ddd2", 0);
      DataCache.InsertData("ddd3", 0);
      DataCache.InsertData("ddd4", 0);
      DataCache.InsertData("sss", 0);
      DataCache.InsertData("aaa", 0);

      // Assert
      Assert.AreEqual(HttpRuntime.Cache.Count, 6);

      // Act
      Thread.Sleep(TimeSpan.FromSeconds(1));

      // Assert
      Assert.AreNotEqual(HttpRuntime.Cache.Get("ddd1"), null);

      // Act
      Thread.Sleep(TimeSpan.FromSeconds(1));

      // Assert
      Assert.AreNotEqual(HttpRuntime.Cache.Get("ddd1"), null);

      // Act
      Thread.Sleep(TimeSpan.FromSeconds(2));

      // Assert
      Assert.AreEqual(HttpRuntime.Cache.Get("ddd1"), null);
    }

    [TestMethod()]
    public void InsertSlidingExpirationDataTest()
    {
      // Arrange
      DataCache.InsertSlidingExpirationData("ddd1", 0, TimeSpan.FromSeconds(3));
      DataCache.InsertData("ddd2", 0);
      DataCache.InsertData("ddd3", 0);
      DataCache.InsertData("ddd4", 0);
      DataCache.InsertData("sss", 0);
      DataCache.InsertData("aaa", 0);

      // Assert
      Assert.AreEqual(HttpRuntime.Cache.Count, 6);

      // Act
      Thread.Sleep(TimeSpan.FromSeconds(1));

      // Assert
      Assert.AreNotEqual(HttpRuntime.Cache.Get("ddd1"), null);

      // Act
      Thread.Sleep(TimeSpan.FromSeconds(2));

      // Assert
      Assert.AreNotEqual(HttpRuntime.Cache.Get("ddd1"), null);

      // Act
      Thread.Sleep(TimeSpan.FromSeconds(2));

      // Assert
      Assert.AreNotEqual(HttpRuntime.Cache.Get("ddd1"), null);

      // Act
      Thread.Sleep(TimeSpan.FromSeconds(4));

      // Assert
      Assert.AreEqual(HttpRuntime.Cache.Get("ddd1"), null);
    }

    [TestMethod()]
    [Ignore("Not implemented")]
    public void InsertExpirationDataTest()
    {
      Assert.Fail();
    }

    [TestMethod()]
    [Ignore("Not implemented")]
    public void InsertExpirationDataTest1()
    {
      Assert.Fail();
    }

    [TestMethod()]
    public void RemoveDataTest()
    {
      // Arrange
      DataCache.InsertData("ddd1", 0);
      DataCache.InsertData("ddd2", 0);
      DataCache.InsertData("ddd3", 0);
      DataCache.InsertData("ddd4", 0);
      DataCache.InsertData("sss", 0);
      DataCache.InsertData("aaa", 0);

      // Assert
      Assert.AreEqual(HttpRuntime.Cache.Count, 6);

      // Act
      DataCache.RemoveData("ddd");

      // Assert
      Assert.AreEqual(HttpRuntime.Cache.Count, 6);

      // Act
      DataCache.RemoveData("ddd1");

      // Assert
      Assert.AreEqual(HttpRuntime.Cache.Count, 5);

      // Act
      DataCache.RemoveData("ss");

      // Assert
      Assert.AreEqual(HttpRuntime.Cache.Count, 5);

      // Act
      DataCache.RemoveData("sss");

      // Assert
      Assert.AreEqual(HttpRuntime.Cache.Count, 4);
    }

    [TestMethod()]
    public void RemoveAllDataTest()
    {
      // Arrange
      DataCache.InsertData("ddd1", 0);
      DataCache.InsertData("ddd2", 0);
      DataCache.InsertData("ddd3", 0);
      DataCache.InsertData("ddd4", 0);
      DataCache.InsertData("sss", 0);
      DataCache.InsertData("aaa", 0);

      // Assert
      Assert.AreEqual(HttpRuntime.Cache.Count, 6);

      // Act
      DataCache.RemoveAllData();

      // Assert
      Assert.AreEqual(HttpRuntime.Cache.Count, 0);
    }

    [TestMethod()]
    public void RemoveAllDataNameStartTest()
    {
      // Arrange
      DataCache.InsertData("ddd1", 0);
      DataCache.InsertData("ddd2", 0);
      DataCache.InsertData("ddd3", 0);
      DataCache.InsertData("ddd4", 0);
      DataCache.InsertData("sss", 0);
      DataCache.InsertData("aaa", 0);

      // Assert
      Assert.AreEqual(HttpRuntime.Cache.Count, 6);

      // Act
      DataCache.RemoveAllData("ddd");

      // Assert
      Assert.AreEqual(HttpRuntime.Cache.Count, 2);

      // Act
      DataCache.RemoveAllData("sss");

      // Assert
      Assert.AreEqual(HttpRuntime.Cache.Count, 1);

    }
  }
}