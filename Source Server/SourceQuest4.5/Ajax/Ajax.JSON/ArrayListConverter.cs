using System;
using System.Collections;
using System.Text;

namespace Ajax.JSON
{
	internal sealed class ArrayListConverter : IAjaxObjectConverter
	{
		public string ClientScriptIdentifier => null;

		public Type[] SupportedTypes => new Type[1]
		{
			typeof(ArrayList)
		};

		public bool IncludeSubclasses => true;

		public void RenderClientScript(ref StringBuilder sb)
		{
		}

		public object FromString(string s, Type t)
		{
			throw new NotImplementedException();
		}

		public void ToJSON(ref StringBuilder sb, object o)
		{
			if ((object)o.GetType() != typeof(ArrayList) && (object)o.GetType().BaseType != typeof(ArrayList))
			{
				return;
			}
			ArrayList arrayList = (ArrayList)o;
			sb.Append("[");
			for (int i = 0; i < arrayList.Count; i++)
			{
				DefaultConverter.ToJSON(ref sb, arrayList[i]);
				if (i < arrayList.Count - 1)
				{
					sb.Append(",");
				}
			}
			sb.Append("]");
		}
	}
}
