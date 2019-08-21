using System;
using System.Web.Caching;

namespace DevBian.DemoWebSite
{
  public partial class Default : System.Web.UI.Page
  {
    public const string STR_CACHENAME = "something";

    private static object lockobj = new object();

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!this.IsPostBack)
      {
        SomethingDataModel val = new SomethingDataModel()
        {
          ID = 1,
          SomethingName = "Name",
          SomethingDescription = null,
        };

        DataCache.InsertData(STR_CACHENAME, val);
      }
      else if (Request.Form["button1"] != null)
      {
        SomethingDataModel val = DataCache.GetData<SomethingDataModel>(STR_CACHENAME);
        if (val != null)
          lock (Default.lockobj)
          {
            val.ID++;
          }
      }
      else if (Request.Form["button2"] != null)
      {
        SomethingDataModel val = DataCache.GetDeepCopiedData<SomethingDataModel>(STR_CACHENAME);
        if (val != null)
          //lock (Default.lockobj) do not need lock, it is a deep copied object
          {
            val.ID++;
          }
      }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
      this.button1.Enabled = DataCache.GetData<SomethingDataModel>(STR_CACHENAME) != null;
      this.button2.Enabled = this.button1.Enabled;
    }

  }
}