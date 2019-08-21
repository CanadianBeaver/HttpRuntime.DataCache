using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace DevBian.DemoWebSite
{
  public partial class DisplayCache : System.Web.UI.UserControl
  {
    protected void Page_PreRender(object sender, EventArgs e)
    {
      int cacheCount = HttpRuntime.Cache.Count;
      this.panelCache.GroupingText = string.Format("Cache contains {0} item(s)", cacheCount);
      this.repItems.Visible = cacheCount > 0;
      if (this.repItems.Visible)
      {
        IList<object> result = new List<object>();
        IDictionaryEnumerator eCache = HttpRuntime.Cache.GetEnumerator();
        int index = 0;
        while (eCache.MoveNext())
        {
          index++;
          string key = eCache.Key as string;
          string type = eCache.Value.GetType().ToString();
          string value = eCache.Value.ToString();
          result.Add(new { Index = index, Key = key, Type = type, Value = value });
        }
        this.repItems.DataSource = result;
        this.repItems.DataBind();
      }
    }
  }
}