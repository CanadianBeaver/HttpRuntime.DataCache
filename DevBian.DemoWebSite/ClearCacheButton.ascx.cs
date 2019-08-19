using System;

using DevBian.Caching;

namespace DevBian.DemoWebSite
{
  public partial class ClearCacheButton : System.Web.UI.UserControl
  {
    protected void buttonClear_Click(object sender, EventArgs e)
    {
      DataCache.RemoveAllData();
    }
  }
}