using System;
using System.Data;
using System.Text;

namespace Ajax.JSON
{
	internal sealed class DataSetConverter : IAjaxObjectConverter
	{
		public string ClientScriptIdentifier => "AjaxDataSet";

		public Type[] SupportedTypes => new Type[1]
		{
			typeof(DataSet)
		};

		public bool IncludeSubclasses => true;

		public void RenderClientScript(ref StringBuilder sb)
		{
			sb.Append("function _getTable(n,e){for(var i=0; i<e.Tables.length; i++){if(e.Tables[i].Name == n)return e.Tables[i];}return null;}\r\n");
		}

		public object FromString(string s, Type t)
		{
			throw new NotImplementedException();
		}

		public void ToJSON(ref StringBuilder sb, object o)
		{
			if ((object)o.GetType() != typeof(DataSet) && (object)o.GetType().BaseType != typeof(DataSet))
			{
				return;
			}
			DataSet dataSet = (DataSet)o;
			sb.Append("{'Tables':[");
			for (int i = 0; i < dataSet.Tables.Count; i++)
			{
				DefaultConverter.ToJSON(ref sb, dataSet.Tables[i]);
				if (i < dataSet.Tables.Count - 1)
				{
					sb.Append(",");
				}
			}
			sb.Append("]");
			sb.Append(",'getTable':function(n){return _getTable(n,this);}");
			sb.Append("}");
		}
	}
}
