using ProtoBuf;
using System;
using System.IO;
using System.Text;
using System.Xml;
using zlib;

namespace Game.Base
{
	public class Marshal
	{
		public static string ConvertToString(byte[] cstyle)
		{
			if (cstyle == null)
			{
				return null;
			}
			for (int i = 0; i < cstyle.Length; i++)
			{
				if (cstyle[i] == 0)
				{
					return Encoding.Default.GetString(cstyle, 0, i);
				}
			}
			return Encoding.Default.GetString(cstyle);
		}

		public static int ConvertToInt32(byte[] val)
		{
			return ConvertToInt32(val, 0);
		}

		public static long ConvertToInt64(int v1, uint v2)
		{
			int num = 1;
			if (v1 < 0)
			{
				num = -1;
			}
			return (long)((double)num * (Math.Abs((double)v1 * Math.Pow(2.0, 32.0)) + (double)v2));
		}

		public static int ConvertToInt32(byte[] val, int startIndex)
		{
			return ConvertToInt32(val[startIndex], val[startIndex + 1], val[startIndex + 2], val[startIndex + 3]);
		}

		public static int ConvertToInt32(byte v1, byte v2, byte v3, byte v4)
		{
			return (v1 << 24) | (v2 << 16) | (v3 << 8) | v4;
		}

		public static uint ConvertToUInt32(byte v1, byte v2, byte v3, byte v4)
		{
			return (uint)((v1 << 24) | (v2 << 16) | (v3 << 8) | v4);
		}

		public static uint ConvertToUInt32(byte[] val)
		{
			return ConvertToUInt32(val, 0);
		}

		public static uint ConvertToUInt32(byte[] val, int startIndex)
		{
			return ConvertToUInt32(val[startIndex], val[startIndex + 1], val[startIndex + 2], val[startIndex + 3]);
		}

		public static short ConvertToInt16(byte[] val)
		{
			return ConvertToInt16(val, 0);
		}

		public static short ConvertToInt16(byte[] val, int startIndex)
		{
			return ConvertToInt16(val[startIndex], val[startIndex + 1]);
		}

		public static short ConvertToInt16(byte v1, byte v2)
		{
			return (short)((v1 << 8) | v2);
		}

		public static ushort ConvertToUInt16(byte[] val)
		{
			return ConvertToUInt16(val, 0);
		}

		public static ushort ConvertToUInt16(byte[] val, int startIndex)
		{
			return ConvertToUInt16(val[startIndex], val[startIndex + 1]);
		}

		public static ushort ConvertToUInt16(byte v1, byte v2)
		{
			return (ushort)(v2 | (v1 << 8));
		}

		public static string ToHexDump(string description, byte[] dump)
		{
			return ToHexDump(description, dump, 0, dump.Length);
		}

		public static string ToHexDump(string description, byte[] dump, int start, int count)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (description != null)
			{
				stringBuilder.Append(description).Append("\n");
			}
			int num = start + count;
			for (int i = start; i < num; i += 16)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				StringBuilder stringBuilder3 = new StringBuilder();
				stringBuilder3.Append(i.ToString("X4"));
				stringBuilder3.Append(": ");
				for (int j = 0; j < 16; j++)
				{
					if (j + i < num)
					{
						byte b = dump[j + i];
						stringBuilder3.Append(dump[j + i].ToString("X2"));
						stringBuilder3.Append(" ");
						if (b >= 32 && b <= 127)
						{
							stringBuilder2.Append((char)b);
						}
						else
						{
							stringBuilder2.Append(".");
						}
					}
					else
					{
						stringBuilder3.Append("   ");
						stringBuilder2.Append(" ");
					}
				}
				stringBuilder3.Append("  ");
				stringBuilder3.Append(stringBuilder2.ToString());
				stringBuilder3.Append('\n');
				stringBuilder.Append(stringBuilder3.ToString());
			}
			return stringBuilder.ToString();
		}

		public static byte[] Compress(byte[] src)
		{
			return Compress(src, 0, src.Length);
		}

		public static byte[] Compress(byte[] src, int offset, int length)
		{
			MemoryStream memoryStream = new MemoryStream();
			ZOutputStream zOutputStream = new ZOutputStream(memoryStream, 9);
			zOutputStream.Write(src, offset, length);
			zOutputStream.Close();
			return memoryStream.ToArray();
		}

		public static byte[] Uncompress(byte[] src)
		{
			return Uncompress(src, 0);
		}

		public static byte[] Uncompress(byte[] src, int offset)
		{
			MemoryStream memoryStream = new MemoryStream();
			ZOutputStream zOutputStream = new ZOutputStream(memoryStream);
			zOutputStream.Write(src, offset, src.Length);
			zOutputStream.Close();
			return memoryStream.ToArray();
		}

		public static XmlDocument LoadXMLFile(string filename, bool isEncrypt)
		{
			if (!File.Exists("xmls/" + filename + ".xml"))
			{
				return null;
			}
			try
			{
				byte[] array = File.ReadAllBytes("xmls/" + filename + ".xml");
				if (isEncrypt)
				{
					array = Uncompress(array);
				}
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(new MemoryStream(array));
				return xmlDocument;
			}
			catch (Exception ex)
			{
				Console.WriteLine("DataXML " + filename + " is error!" + ex);
			}
			return null;
		}

		public static T LoadDataFile<T>(string filename, bool isEncrypt)
		{
			if (!File.Exists("datas/" + filename + ".snapedata"))
			{
				return default(T);
			}
			try
			{
				byte[] array = File.ReadAllBytes("datas/" + filename + ".snapedata");
				if (isEncrypt)
				{
					array = Uncompress(array);
				}
				MemoryStream obj = new MemoryStream(array)
				{
					Position = 0L
				};
				T result = Serializer.Deserialize<T>(obj);
				obj.Dispose();
				return result;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Data " + filename + " is error!" + ex);
			}
			return default(T);
		}

		public static bool SaveDataFile<T>(T instance, string filename, bool isEncrypt)
		{
			try
			{
				byte[] array = null;
				if (instance == null)
				{
					array = new byte[0];
				}
				else
				{
					MemoryStream memoryStream = new MemoryStream();
					Serializer.Serialize(memoryStream, instance);
					array = new byte[memoryStream.Length];
					memoryStream.Position = 0L;
					memoryStream.Read(array, 0, array.Length);
					memoryStream.Dispose();
				}
				if (isEncrypt)
				{
					array = Compress(array);
				}
				File.WriteAllBytes("datas/" + filename + ".snapedata", array);
				return true;
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
			return false;
		}

		public static DateTime StartOfWeek(DateTime dt)
		{
			return StartOfWeek(dt, DayOfWeek.Monday);
		}

		public static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
		{
			int num = dt.DayOfWeek - startOfWeek;
			if (num < 0)
			{
				num += 7;
			}
			return dt.AddDays(-1 * num).Date;
		}
	}
}
