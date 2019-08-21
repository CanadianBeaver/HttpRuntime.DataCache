using System;
using System.Web.Caching;

namespace DevBian.DemoWebSite
{
  public partial class PopulateCacheButton : System.Web.UI.UserControl
  {
    protected void buttonPopulate_Click(object sender, EventArgs e)
    {
      SomethingDataModel val = new SomethingDataModel()
      {
        ID = 1,
        SomethingName = "Name",
        SomethingDescription = null,
      };
      DataCache.InsertData(Default.STR_CACHENAME, val);
    }
  }
}