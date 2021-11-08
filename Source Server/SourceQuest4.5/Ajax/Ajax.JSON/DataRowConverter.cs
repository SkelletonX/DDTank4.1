using System;
using System.Data;
using System.Text;

namespace Ajax.JSON
{
	internal sealed class DataRowConverter : IAjaxObjectConverter
	{
		public string ClientScriptIdentifier => null;

		public Type[] SupportedTypes => new Type[1]
		{
			typeof(DataRow)
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
			if ((object)o.GetType() != typeof(DataRow) && (object)o.GetType().BaseType != typeof(DataRow))
			{
				return;
			}
			DataRow dataRow = (DataRow)o;
			sb.Append("{");
			for (int i = 0; i < dataRow.Table.Columns.Count; i++)
			{
				sb.Append("'" + dataRow.Table.Columns[i].ColumnName + "':");
				if (dataRow[i] == DBNull.Value)
				{
					sb.Append("null");
				}
				else
				{
					DefaultConverter.ToJSON(ref sb, dataRow[i]);
				}
				if (i < dataRow.Table.Columns.Count - 1)
				{
					sb.Append(",");
				}
			}
			sb.Append("}");
		}
	}
}
