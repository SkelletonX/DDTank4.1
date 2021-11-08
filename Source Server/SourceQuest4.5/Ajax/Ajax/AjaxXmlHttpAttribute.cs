using System;

namespace Ajax
{
	[Obsolete("Please remove this Attribute. There is no restriction for concurrent requests.", true)]
	[AttributeUsage(AttributeTargets.Method)]
	public class AjaxXmlHttpAttribute : Attribute
	{
		private string xmlHttpVariable = null;

		public string XmlHttpVariable => xmlHttpVariable;

		public AjaxXmlHttpAttribute(string xmlHttpVariable)
		{
			this.xmlHttpVariable = xmlHttpVariable;
		}
	}
}
