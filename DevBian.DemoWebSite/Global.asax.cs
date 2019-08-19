using DevBian.Caching;

using System;

namespace DevBian.DemoWebSite
{
  public class Global : System.Web.HttpApplication
  {
    protected void Application_Start(object sender, EventArgs e)
    {
      var settings = Properties.Settings.Default;
      DataCache.IsCacheEnable = settings.IsCacheEnable;
      DataCache.ExpirationType = settings.ExpirationType;
      DataCache.ExpirationTime = settings.ExpirationTime;
    }
  }
}