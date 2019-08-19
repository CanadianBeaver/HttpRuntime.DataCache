using System;

using DevBian.Caching;
using DevBian.DemoWebSite.Properties;

namespace DevBian.DemoWebSite
{
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
}