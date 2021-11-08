using System;
using System.Data;
using System.Text;

namespace Ajax.JSON
{
	internal sealed class DataTableConverter : IAjaxObjectConverter
	{
		public string ClientScriptIdentifier => null;

		public Type[] SupportedTypes => new Type[1]
		{
			typeof(DataTable)
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
			if ((object)o.GetType() != typeof(DataTable) && (object)o.GetType().BaseType != typeof(DataTable))
			{
				return;
			}
			DataTable dataTable = (DataTable)o;
			sb.Append("{'Name':'" + dataTable.TableName + "',");
			sb.Append("'Rows':[");
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				DefaultConverter.ToJSON(ref sb, dataTable.Rows[i]);
				if (i < dataTable.Rows.Count - 1)
				{
					sb.Append(",");
				}
			}
			sb.Append("]}");
		}
	}
}
