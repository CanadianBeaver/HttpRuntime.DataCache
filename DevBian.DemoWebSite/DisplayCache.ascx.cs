using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace DevBian.DemoWebSite
{
  public partial class DisplayCache : System.Web.UI.UserControl
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      Cache cache = HttpRuntime.Cache;
      this.panelCache.Visible = cache != null;
      if (this.panelCache.Visible)
      {
        this.panelCache.GroupingText = string.Format("Cache contains {0} item(s)", cache.Count.ToString());
        IList<object> result = new List<object>();
        IDictionaryEnumerator eCache = cache.GetEnumerator();
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