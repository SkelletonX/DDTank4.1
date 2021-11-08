using System;
using System.Text;

namespace Ajax.JSON
{
	internal sealed class GuidConverter : IAjaxObjectConverter
	{
		public string ClientScriptIdentifier => null;

		public Type[] SupportedTypes => new Type[1]
		{
			typeof(Guid)
		};

		public bool IncludeSubclasses => false;

		public void RenderClientScript(ref StringBuilder sb)
		{
		}

		public object FromString(string s, Type t)
		{
			throw new NotImplementedException();
		}

		public void ToJSON(ref StringBuilder sb, object o)
		{
			Guid guid = (Guid)o;
			DefaultConverter.ToJSON(ref sb, guid.ToString());
		}
	}
}
