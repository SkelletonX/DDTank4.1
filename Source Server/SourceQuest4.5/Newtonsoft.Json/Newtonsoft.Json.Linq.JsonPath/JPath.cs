using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Newtonsoft.Json.Linq.JsonPath
{
	internal class JPath
	{
		private readonly string _expression;

		private int _currentIndex;

		public List<PathFilter> Filters
		{
			get;
			private set;
		}

		public JPath(string expression)
		{
			ValidationUtils.ArgumentNotNull(expression, "expression");
			_expression = expression;
			Filters = new List<PathFilter>();
			ParseMain();
		}

		private void ParseMain()
		{
			int currentIndex = _currentIndex;
			EatWhitespace();
			if (_expression.Length == _currentIndex)
			{
				return;
			}
			if (_expression[_currentIndex] == '$')
			{
				if (_expression.Length == 1)
				{
					return;
				}
				char c = _expression[_currentIndex + 1];
				if (c == '.' || c == '[')
				{
					_currentIndex++;
					currentIndex = _currentIndex;
				}
			}
			if (!ParsePath(Filters, currentIndex, query: false))
			{
				int currentIndex2 = _currentIndex;
				EatWhitespace();
				if (_currentIndex < _expression.Length)
				{
					throw new JsonException("Unexpected character while parsing path: " + _expression[currentIndex2]);
				}
			}
		}

		private bool ParsePath(List<PathFilter> filters, int currentPartStartIndex, bool query)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			while (_currentIndex < _expression.Length && !flag4)
			{
				char c = _expression[_currentIndex];
				switch (c)
				{
				case '(':
				case '[':
					if (_currentIndex > currentPartStartIndex)
					{
						string name = _expression.Substring(currentPartStartIndex, _currentIndex - currentPartStartIndex);
						object obj2;
						if (!flag)
						{
							FieldFilter fieldFilter2 = new FieldFilter();
							fieldFilter2.Name = name;
							obj2 = fieldFilter2;
						}
						else
						{
							ScanFilter scanFilter2 = new ScanFilter();
							scanFilter2.Name = name;
							obj2 = scanFilter2;
						}
						PathFilter item2 = (PathFilter)obj2;
						filters.Add(item2);
						flag = false;
					}
					filters.Add(ParseIndexer(c));
					_currentIndex++;
					currentPartStartIndex = _currentIndex;
					flag2 = true;
					flag3 = false;
					break;
				case ')':
				case ']':
					flag4 = true;
					break;
				case ' ':
					if (_currentIndex < _expression.Length)
					{
						flag4 = true;
					}
					break;
				case '.':
					if (_currentIndex > currentPartStartIndex)
					{
						string text = _expression.Substring(currentPartStartIndex, _currentIndex - currentPartStartIndex);
						if (text == "*")
						{
							text = null;
						}
						object obj;
						if (!flag)
						{
							FieldFilter fieldFilter = new FieldFilter();
							fieldFilter.Name = text;
							obj = fieldFilter;
						}
						else
						{
							ScanFilter scanFilter = new ScanFilter();
							scanFilter.Name = text;
							obj = scanFilter;
						}
						PathFilter item = (PathFilter)obj;
						filters.Add(item);
						flag = false;
					}
					if (_currentIndex + 1 < _expression.Length && _expression[_currentIndex + 1] == '.')
					{
						flag = true;
						_currentIndex++;
					}
					_currentIndex++;
					currentPartStartIndex = _currentIndex;
					flag2 = false;
					flag3 = true;
					break;
				default:
					if (query && (c == '=' || c == '<' || c == '!' || c == '>' || c == '|' || c == '&'))
					{
						flag4 = true;
						break;
					}
					if (flag2)
					{
						throw new JsonException("Unexpected character following indexer: " + c);
					}
					_currentIndex++;
					break;
				}
			}
			bool flag5 = _currentIndex == _expression.Length;
			if (_currentIndex > currentPartStartIndex)
			{
				string text2 = _expression.Substring(currentPartStartIndex, _currentIndex - currentPartStartIndex).TrimEnd();
				if (text2 == "*")
				{
					text2 = null;
				}
				object obj3;
				if (!flag)
				{
					FieldFilter fieldFilter3 = new FieldFilter();
					fieldFilter3.Name = text2;
					obj3 = fieldFilter3;
				}
				else
				{
					ScanFilter scanFilter3 = new ScanFilter();
					scanFilter3.Name = text2;
					obj3 = scanFilter3;
				}
				PathFilter item3 = (PathFilter)obj3;
				filters.Add(item3);
			}
			else if (flag3 && (flag5 || query))
			{
				throw new JsonException("Unexpected end while parsing path.");
			}
			return flag5;
		}

		private PathFilter ParseIndexer(char indexerOpenChar)
		{
			_currentIndex++;
			char indexerCloseChar = (indexerOpenChar == '[') ? ']' : ')';
			EnsureLength("Path ended with open indexer.");
			EatWhitespace();
			if (_expression[_currentIndex] == '\'')
			{
				return ParseQuotedField(indexerCloseChar);
			}
			if (_expression[_currentIndex] == '?')
			{
				return ParseQuery(indexerCloseChar);
			}
			return ParseArrayIndexer(indexerCloseChar);
		}

		private PathFilter ParseArrayIndexer(char indexerCloseChar)
		{
			int currentIndex = _currentIndex;
			int? num = null;
			List<int> list = null;
			int num2 = 0;
			int? start = null;
			int? end = null;
			int? step = null;
			while (_currentIndex < _expression.Length)
			{
				char c = _expression[_currentIndex];
				if (c == ' ')
				{
					num = _currentIndex;
					EatWhitespace();
					continue;
				}
				if (c == indexerCloseChar)
				{
					int num3 = (num ?? _currentIndex) - currentIndex;
					if (list != null)
					{
						if (num3 == 0)
						{
							throw new JsonException("Array index expected.");
						}
						string value = _expression.Substring(currentIndex, num3);
						int item = Convert.ToInt32(value, CultureInfo.InvariantCulture);
						list.Add(item);
						ArrayMultipleIndexFilter arrayMultipleIndexFilter = new ArrayMultipleIndexFilter();
						arrayMultipleIndexFilter.Indexes = list;
						return arrayMultipleIndexFilter;
					}
					if (num2 > 0)
					{
						if (num3 > 0)
						{
							string value2 = _expression.Substring(currentIndex, num3);
							int value3 = Convert.ToInt32(value2, CultureInfo.InvariantCulture);
							if (num2 != 1)
							{
								step = value3;
							}
							else
							{
								end = value3;
							}
						}
						ArraySliceFilter arraySliceFilter = new ArraySliceFilter();
						arraySliceFilter.Start = start;
						arraySliceFilter.End = end;
						arraySliceFilter.Step = step;
						return arraySliceFilter;
					}
					if (num3 == 0)
					{
						throw new JsonException("Array index expected.");
					}
					string value4 = _expression.Substring(currentIndex, num3);
					int value5 = Convert.ToInt32(value4, CultureInfo.InvariantCulture);
					ArrayIndexFilter arrayIndexFilter = new ArrayIndexFilter();
					arrayIndexFilter.Index = value5;
					return arrayIndexFilter;
				}
				switch (c)
				{
				case ',':
				{
					int num5 = (num ?? _currentIndex) - currentIndex;
					if (num5 == 0)
					{
						throw new JsonException("Array index expected.");
					}
					if (list == null)
					{
						list = new List<int>();
					}
					string value8 = _expression.Substring(currentIndex, num5);
					list.Add(Convert.ToInt32(value8, CultureInfo.InvariantCulture));
					_currentIndex++;
					EatWhitespace();
					currentIndex = _currentIndex;
					num = null;
					break;
				}
				case '*':
					_currentIndex++;
					EnsureLength("Path ended with open indexer.");
					EatWhitespace();
					if (_expression[_currentIndex] != indexerCloseChar)
					{
						throw new JsonException("Unexpected character while parsing path indexer: " + c);
					}
					return new ArrayIndexFilter();
				case ':':
				{
					int num4 = (num ?? _currentIndex) - currentIndex;
					if (num4 > 0)
					{
						string value6 = _expression.Substring(currentIndex, num4);
						int value7 = Convert.ToInt32(value6, CultureInfo.InvariantCulture);
						switch (num2)
						{
						case 0:
							start = value7;
							break;
						case 1:
							end = value7;
							break;
						default:
							step = value7;
							break;
						}
					}
					num2++;
					_currentIndex++;
					EatWhitespace();
					currentIndex = _currentIndex;
					num = null;
					break;
				}
				default:
					if (!char.IsDigit(c) && c != '-')
					{
						throw new JsonException("Unexpected character while parsing path indexer: " + c);
					}
					if (num.HasValue)
					{
						throw new JsonException("Unexpected character while parsing path indexer: " + c);
					}
					_currentIndex++;
					break;
				}
			}
			throw new JsonException("Path ended with open indexer.");
		}

		private void EatWhitespace()
		{
			while (_currentIndex < _expression.Length && _expression[_currentIndex] == ' ')
			{
				_currentIndex++;
			}
		}

		private PathFilter ParseQuery(char indexerCloseChar)
		{
			_currentIndex++;
			EnsureLength("Path ended with open indexer.");
			if (_expression[_currentIndex] != '(')
			{
				throw new JsonException("Unexpected character while parsing path indexer: " + _expression[_currentIndex]);
			}
			_currentIndex++;
			QueryExpression expression = ParseExpression();
			_currentIndex++;
			EnsureLength("Path ended with open indexer.");
			EatWhitespace();
			if (_expression[_currentIndex] != indexerCloseChar)
			{
				throw new JsonException("Unexpected character while parsing path indexer: " + _expression[_currentIndex]);
			}
			QueryFilter queryFilter = new QueryFilter();
			queryFilter.Expression = expression;
			return queryFilter;
		}

		private QueryExpression ParseExpression()
		{
			QueryExpression queryExpression = null;
			CompositeExpression compositeExpression = null;
			while (_currentIndex < _expression.Length)
			{
				EatWhitespace();
				if (_expression[_currentIndex] != '@')
				{
					throw new JsonException("Unexpected character while parsing path query: " + _expression[_currentIndex]);
				}
				_currentIndex++;
				List<PathFilter> list = new List<PathFilter>();
				if (ParsePath(list, _currentIndex, query: true))
				{
					throw new JsonException("Path ended with open query.");
				}
				EatWhitespace();
				EnsureLength("Path ended with open query.");
				object value = null;
				QueryOperator queryOperator;
				if (_expression[_currentIndex] == ')' || _expression[_currentIndex] == '|' || _expression[_currentIndex] == '&')
				{
					queryOperator = QueryOperator.Exists;
				}
				else
				{
					queryOperator = ParseOperator();
					EatWhitespace();
					EnsureLength("Path ended with open query.");
					value = ParseValue();
					EatWhitespace();
					EnsureLength("Path ended with open query.");
				}
				BooleanQueryExpression booleanQueryExpression = new BooleanQueryExpression();
				booleanQueryExpression.Path = list;
				booleanQueryExpression.Operator = queryOperator;
				booleanQueryExpression.Value = ((queryOperator != QueryOperator.Exists) ? new JValue(value) : null);
				BooleanQueryExpression booleanQueryExpression2 = booleanQueryExpression;
				if (_expression[_currentIndex] == ')')
				{
					if (compositeExpression != null)
					{
						compositeExpression.Expressions.Add(booleanQueryExpression2);
						return queryExpression;
					}
					return booleanQueryExpression2;
				}
				if (_expression[_currentIndex] == '&' && Match("&&"))
				{
					if (compositeExpression == null || compositeExpression.Operator != QueryOperator.And)
					{
						CompositeExpression compositeExpression2 = new CompositeExpression();
						compositeExpression2.Operator = QueryOperator.And;
						CompositeExpression compositeExpression3 = compositeExpression2;
						compositeExpression?.Expressions.Add(compositeExpression3);
						compositeExpression = compositeExpression3;
						if (queryExpression == null)
						{
							queryExpression = compositeExpression;
						}
					}
					compositeExpression.Expressions.Add(booleanQueryExpression2);
				}
				if (_expression[_currentIndex] != '|' || !Match("||"))
				{
					continue;
				}
				if (compositeExpression == null || compositeExpression.Operator != QueryOperator.Or)
				{
					CompositeExpression compositeExpression4 = new CompositeExpression();
					compositeExpression4.Operator = QueryOperator.Or;
					CompositeExpression compositeExpression5 = compositeExpression4;
					compositeExpression?.Expressions.Add(compositeExpression5);
					compositeExpression = compositeExpression5;
					if (queryExpression == null)
					{
						queryExpression = compositeExpression;
					}
				}
				compositeExpression.Expressions.Add(booleanQueryExpression2);
			}
			throw new JsonException("Path ended with open query.");
		}

		private object ParseValue()
		{
			char c = _expression[_currentIndex];
			if (c == '\'')
			{
				return ReadQuotedString();
			}
			if (char.IsDigit(c) || c == '-')
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(c);
				_currentIndex++;
				while (_currentIndex < _expression.Length)
				{
					c = _expression[_currentIndex];
					if (c == ' ' || c == ')')
					{
						string text = stringBuilder.ToString();
						if (text.IndexOfAny(new char[3]
						{
							'.',
							'E',
							'e'
						}) != -1)
						{
							if (double.TryParse(text, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out double result))
							{
								return result;
							}
							throw new JsonException("Could not read query value.");
						}
						if (long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out long result2))
						{
							return result2;
						}
						throw new JsonException("Could not read query value.");
					}
					stringBuilder.Append(c);
					_currentIndex++;
				}
			}
			else
			{
				switch (c)
				{
				case 't':
					if (Match("true"))
					{
						return true;
					}
					break;
				case 'f':
					if (Match("false"))
					{
						return false;
					}
					break;
				case 'n':
					if (Match("null"))
					{
						return null;
					}
					break;
				}
			}
			throw new JsonException("Could not read query value.");
		}

		private string ReadQuotedString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			_currentIndex++;
			while (_currentIndex < _expression.Length)
			{
				char c = _expression[_currentIndex];
				if (c == '\\' && _currentIndex + 1 < _expression.Length)
				{
					_currentIndex++;
					if (_expression[_currentIndex] == '\'')
					{
						stringBuilder.Append('\'');
					}
					else
					{
						if (_expression[_currentIndex] != '\\')
						{
							throw new JsonException("Unknown escape chracter: \\" + _expression[_currentIndex]);
						}
						stringBuilder.Append('\\');
					}
					_currentIndex++;
				}
				else
				{
					if (c == '\'')
					{
						_currentIndex++;
						return stringBuilder.ToString();
					}
					_currentIndex++;
					stringBuilder.Append(c);
				}
			}
			throw new JsonException("Path ended with an open string.");
		}

		private bool Match(string s)
		{
			int num = _currentIndex;
			foreach (char c in s)
			{
				if (num < _expression.Length && _expression[num] == c)
				{
					num++;
					continue;
				}
				return false;
			}
			_currentIndex = num;
			return true;
		}

		private QueryOperator ParseOperator()
		{
			if (_currentIndex + 1 >= _expression.Length)
			{
				throw new JsonException("Path ended with open query.");
			}
			if (Match("=="))
			{
				return QueryOperator.Equals;
			}
			if (Match("!=") || Match("<>"))
			{
				return QueryOperator.NotEquals;
			}
			if (Match("<="))
			{
				return QueryOperator.LessThanOrEquals;
			}
			if (Match("<"))
			{
				return QueryOperator.LessThan;
			}
			if (Match(">="))
			{
				return QueryOperator.GreaterThanOrEquals;
			}
			if (Match(">"))
			{
				return QueryOperator.GreaterThan;
			}
			throw new JsonException("Could not read query operator.");
		}

		private PathFilter ParseQuotedField(char indexerCloseChar)
		{
			List<string> list = null;
			while (_currentIndex < _expression.Length)
			{
				string text = ReadQuotedString();
				EatWhitespace();
				EnsureLength("Path ended with open indexer.");
				if (_expression[_currentIndex] == indexerCloseChar)
				{
					if (list != null)
					{
						list.Add(text);
						FieldMultipleFilter fieldMultipleFilter = new FieldMultipleFilter();
						fieldMultipleFilter.Names = list;
						return fieldMultipleFilter;
					}
					FieldFilter fieldFilter = new FieldFilter();
					fieldFilter.Name = text;
					return fieldFilter;
				}
				if (_expression[_currentIndex] == ',')
				{
					_currentIndex++;
					EatWhitespace();
					if (list == null)
					{
						list = new List<string>();
					}
					list.Add(text);
					continue;
				}
				throw new JsonException("Unexpected character while parsing path indexer: " + _expression[_currentIndex]);
			}
			throw new JsonException("Path ended with open indexer.");
		}

		private void EnsureLength(string message)
		{
			if (_currentIndex >= _expression.Length)
			{
				throw new JsonException(message);
			}
		}

		internal IEnumerable<JToken> Evaluate(JToken t, bool errorWhenNoMatch)
		{
			return Evaluate(Filters, t, errorWhenNoMatch);
		}

		internal static IEnumerable<JToken> Evaluate(List<PathFilter> filters, JToken t, bool errorWhenNoMatch)
		{
			IEnumerable<JToken> enumerable = new JToken[1]
			{
				t
			};
			foreach (PathFilter filter in filters)
			{
				enumerable = filter.ExecuteFilter(enumerable, errorWhenNoMatch);
			}
			return enumerable;
		}
	}
}
