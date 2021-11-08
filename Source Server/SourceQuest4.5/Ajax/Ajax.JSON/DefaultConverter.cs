using System;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Ajax.JSON
{
	public sealed class DefaultConverter
	{
		internal static StringBuilder sb = null;

		internal static string ToJSON(object o)
		{
			sb = new StringBuilder();
			ToJSON(ref sb, o);
			return sb.ToString();
		}

		public static object FromString(string s, Type t)
		{
			if (s == null)
			{
				return null;
			}
			if (Utility.AjaxConverters != null)
			{
				foreach (Type key in Utility.AjaxConverters.Keys)
				{
					IAjaxObjectConverter ajaxObjectConverter = (IAjaxObjectConverter)Utility.AjaxConverters[key];
					Type[] supportedTypes = ajaxObjectConverter.SupportedTypes;
					foreach (Type type in supportedTypes)
					{
						if ((object)type == t || (object)type == t.BaseType)
						{
							return ajaxObjectConverter.FromString(s, t);
						}
					}
				}
			}
			if ((object)t == typeof(string))
			{
				return s;
			}
			if ((object)t == typeof(short))
			{
				return Convert.ToInt16(s);
			}
			if ((object)t == typeof(int))
			{
				return Convert.ToInt32(s);
			}
			if ((object)t == typeof(long))
			{
				return Convert.ToInt64(s);
			}
			if ((object)t == typeof(double))
			{
				return Convert.ToDouble(s);
			}
			if ((object)t == typeof(bool))
			{
				return bool.Parse(s.ToLower());
			}
			if ((object)t == typeof(short[]))
			{
				string[] array = s.Split(',');
				short[] array2 = new short[array.Length];
				for (int j = 0; j < array.Length; j++)
				{
					array2[j] = short.Parse(array[j]);
				}
				return array2;
			}
			if ((object)t == typeof(int[]))
			{
				string[] array3 = s.Split(',');
				int[] array4 = new int[array3.Length];
				for (int k = 0; k < array3.Length; k++)
				{
					array4[k] = int.Parse(array3[k]);
				}
				return array4;
			}
			if ((object)t == typeof(long[]))
			{
				string[] array5 = s.Split(',');
				long[] array6 = new long[array5.Length];
				for (int l = 0; l < array5.Length; l++)
				{
					array6[l] = long.Parse(array5[l]);
				}
				return array6;
			}
			if ((object)t == typeof(string[]) && s.StartsWith("[") && s.EndsWith("]"))
			{
				s = s.Substring(1, s.Length - 2);
				Regex regex = new Regex("  \"[^\\\\\"]*\"  |  [^,]+  ", RegexOptions.IgnorePatternWhitespace);
				string[] array7 = new string[regex.Matches(s).Count];
				int num = 0;
				{
					foreach (Match item in regex.Matches(s))
					{
						array7[num++] = item.Value.Substring(1, item.Value.Length - 2);
					}
					return array7;
				}
			}
			throw new NotImplementedException();
		}

		public static void PropsFieldsToJSON(ref StringBuilder sb, object o)
		{
			PropsFieldsToJSON(ref sb, o, writeSeperator: false);
		}

		public static void PropsFieldsToJSON(ref StringBuilder sb, object o, bool writeSeperator)
		{
			bool flag = false;
			PropertyInfo[] properties = o.GetType().GetProperties();
			for (int i = 0; i < properties.Length; i++)
			{
				if (properties[i].CanRead)
				{
					if (i == 0 && writeSeperator)
					{
						sb.Append(",");
					}
					flag = true;
					sb.Append("'" + properties[i].Name + "':");
					try
					{
						ToJSON(ref sb, properties[i].GetValue(o, new object[0]));
					}
					catch (Exception)
					{
						sb.Append("null");
					}
					if (i < properties.Length - 1)
					{
						sb.Append(",");
					}
				}
			}
			FieldInfo[] fields = o.GetType().GetFields();
			for (int j = 0; j < fields.Length; j++)
			{
				if (j == 0 && (flag || writeSeperator))
				{
					sb.Append(",");
				}
				sb.Append("'" + fields[j].Name + "':");
				ToJSON(ref sb, fields[j].GetValue(o));
				if (j < fields.Length - 1)
				{
					sb.Append(",");
				}
			}
		}

		public static void ToJSON(ref StringBuilder sb, object o)
		{
			if (o == null)
			{
				sb.Append("null");
				return;
			}
			Type type = o.GetType();
			if (Utility.AjaxConverters != null)
			{
				foreach (Type key in Utility.AjaxConverters.Keys)
				{
					IAjaxObjectConverter ajaxObjectConverter = (IAjaxObjectConverter)Utility.AjaxConverters[key];
					Type[] supportedTypes = ajaxObjectConverter.SupportedTypes;
					foreach (Type type2 in supportedTypes)
					{
						if (ajaxObjectConverter.IncludeSubclasses && (type2.IsInstanceOfType(o) || (object)type.GetInterface(type2.FullName, ignoreCase: true) != null))
						{
							ajaxObjectConverter.ToJSON(ref sb, o);
							return;
						}
						if ((object)type2 == type || (object)type2 == type.BaseType)
						{
							ajaxObjectConverter.ToJSON(ref sb, o);
							return;
						}
					}
				}
			}
			if (type.IsArray)
			{
				Array array = (Array)o;
				sb.Append("[");
				for (int j = 0; j < array.Length; j++)
				{
					ToJSON(ref sb, array.GetValue(j));
					if (j < array.Length - 1)
					{
						sb.Append(",");
					}
				}
				sb.Append("]");
			}
			else if ((object)type == typeof(byte))
			{
				StringBuilder obj = sb;
				byte b = (byte)o;
				obj.Append(b.ToString());
			}
			else if ((object)type == typeof(sbyte))
			{
				StringBuilder obj2 = sb;
				sbyte b2 = (sbyte)o;
				obj2.Append(b2.ToString());
			}
			else if ((object)type == typeof(short) || (object)type == typeof(int) || (object)type == typeof(long) || (object)type == typeof(ushort) || (object)type == typeof(uint) || (object)type == typeof(ulong))
			{
				sb.Append(o.ToString());
			}
			else if ((object)type == typeof(bool))
			{
				sb.Append(o.ToString().ToLower());
			}
			else if ((object)type == typeof(double) || (object)type == typeof(decimal) || (object)type == typeof(float))
			{
				sb.Append(o.ToString().Replace(",", "."));
			}
			else if ((object)type == typeof(string) || (object)type == typeof(char))
			{
				sb.Append("'" + o.ToString().Replace("\\", "\\\\").Replace("\r", "\\r")
					.Replace("\n", "\\n")
					.Replace("'", "\\'") + "'");
			}
			else if ((object)type == typeof(Guid))
			{
				sb.Append("'" + o.ToString() + "'");
			}
			else if (type.IsEnum)
			{
				sb.Append("'" + o.ToString() + "'");
			}
			else if (type.IsSerializable)
			{
				sb.Append("{");
				PropsFieldsToJSON(ref sb, o);
				sb.Append("}");
			}
			else
			{
				sb.Append("'" + o.ToString().Replace("\\", "\\\\").Replace("\r", "\\r")
					.Replace("\n", "\\n")
					.Replace("'", "\\'") + "'");
			}
		}
	}
}
