using System;
using System.Collections;
using System.Text;

namespace Ajax.JSON
{
	internal sealed class ICollectionConverter : IAjaxObjectConverter
	{
		public string ClientScriptIdentifier => null;

		public Type[] SupportedTypes => new Type[1]
		{
			typeof(ICollection)
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
			sb.Append("[");
			ICollection collection = (ICollection)o;
			object[] array = new object[collection.Count];
			collection.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				DefaultConverter.ToJSON(ref sb, array[i]);
				if (i < array.Length - 1)
				{
					sb.Append(",");
				}
			}
			sb.Append("]");
		}
	}
}
