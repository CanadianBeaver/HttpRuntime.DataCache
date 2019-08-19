using System;
using System.Collections.Generic;
using System.Web;

namespace DevBian.DemoWebSite
{
	[Serializable]
	public class SomethingDataModel
	{
		public int ID { get; set; }
		public string SomethingName { get; set; }
		public string SomethingDescription { get; set; }

		public override string ToString()
		{
			return string.Format("id: {0}, name: {1}", this.ID, this.SomethingName);
		}
	}
}