using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Newtonsoft.Json
{
	internal struct JsonPosition
	{
		private static readonly char[] SpecialCharacters = new char[6]
		{
			'.',
			' ',
			'[',
			']',
			'(',
			')'
		};

		internal JsonContainerType Type;

		internal int Position;

		internal string PropertyName;

		internal bool HasIndex;

		public JsonPosition(JsonContainerType type)
		{
			Type = type;
			HasIndex = TypeHasIndex(type);
			Position = -1;
			PropertyName = null;
		}

		internal void WriteTo(StringBuilder sb)
		{
			switch (Type)
			{
			case JsonContainerType.Object:
			{
				if (sb.Length > 0)
				{
					sb.Append('.');
				}
				string propertyName = PropertyName;
				if (propertyName.IndexOfAny(SpecialCharacters) != -1)
				{
					sb.Append("['");
					sb.Append(propertyName);
					sb.Append("']");
				}
				else
				{
					sb.Append(propertyName);
				}
				break;
			}
			case JsonContainerType.Array:
			case JsonContainerType.Constructor:
				sb.Append('[');
				sb.Append(Position);
				sb.Append(']');
				break;
			}
		}

		internal static bool TypeHasIndex(JsonContainerType type)
		{
			if (type != JsonContainerType.Array)
			{
				return type == JsonContainerType.Constructor;
			}
			return true;
		}

		internal static string BuildPath(IEnumerable<JsonPosition> positions)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (JsonPosition position in positions)
			{
				position.WriteTo(stringBuilder);
			}
			return stringBuilder.ToString();
		}

		internal static string FormatMessage(IJsonLineInfo lineInfo, string path, string message)
		{
			if (!message.EndsWith(Environment.NewLine, StringComparison.Ordinal))
			{
				message = message.Trim();
				if (!message.EndsWith('.'))
				{
					message += ".";
				}
				message += " ";
			}
			message += "Path '{0}'".FormatWith(CultureInfo.InvariantCulture, path);
			if (lineInfo != null && lineInfo.HasLineInfo())
			{
				message += ", line {0}, position {1}".FormatWith(CultureInfo.InvariantCulture, lineInfo.LineNumber, lineInfo.LinePosition);
			}
			message += ".";
			return message;
		}
	}
}
