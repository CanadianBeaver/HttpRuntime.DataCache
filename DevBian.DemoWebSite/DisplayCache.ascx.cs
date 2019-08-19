using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;

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
				this.labelCount.Text = string.Format("Cache contains {0} item(s)", cache.Count.ToString());
				IList<object> result = new List<object>();
				IDictionaryEnumerator eCache = cache.GetEnumerator();
				while (eCache.MoveNext())
				{
					string key = eCache.Key as string;
					string value = eCache.Value.ToString();
					result.Add(new { Key = key, Value = value });
				}
				this.repItems.DataSource = result;
				this.repItems.DataBind();
			}
		}
	}
}