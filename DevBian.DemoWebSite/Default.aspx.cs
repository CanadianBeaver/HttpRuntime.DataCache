using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevBian.Caching;

namespace DevBian.DemoWebSite
{
	public partial class Default : System.Web.UI.Page
	{
		private const string STR_CACHENAME = "something";

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
				this.label1.Text = DataCache.GetData<SomethingDataModel>(STR_CACHENAME).ToString();
			}
			else if (Request.Form["button1"] != null)
			{
				SomethingDataModel val = DataCache.GetData<SomethingDataModel>(STR_CACHENAME);
				val.ID++;
				this.label1.Text = DataCache.GetData<SomethingDataModel>(STR_CACHENAME).ToString();
			}
			else if (Request.Form["button2"] != null)
			{
				SomethingDataModel val = DataCache.GetDeepCopiedData<SomethingDataModel>(STR_CACHENAME);
				val.ID++;
				this.label1.Text = DataCache.GetData<SomethingDataModel>(STR_CACHENAME).ToString();
			}
		}

	}
}