// Decompiled with JetBrains decompiler
// Type: Game.Base.Marshal
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2C15C00-C3DB-415D-8006-692895AE7555
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Base.dll

using ProtoBuf;
using System;
using System.IO;
using System.Text;
using zlib;

namespace Game.Base
{
  public class Marshal
  {
    public static byte[] Compress(byte[] src)
    {
      return Marshal.Compress(src, 0, src.Length);
    }

    public static byte[] Compress(byte[] src, int offset, int length)
    {
      MemoryStream memoryStream = new MemoryStream();
      ZOutputStream zoutputStream = new ZOutputStream((Stream) memoryStream, 9);
      zoutputStream.Write(src, offset, length);
      zoutputStream.Close();
      return memoryStream.ToArray();
    }

    public static short ConvertToInt16(byte[] val)
    {
      return Marshal.ConvertToInt16(val, 0);
    }

    public static short ConvertToInt16(byte v1, byte v2)
    {
      return (short) ((int) v1 << 8 | (int) v2);
    }

    public static short ConvertToInt16(byte[] val, int startIndex)
    {
      return Marshal.ConvertToInt16(val[startIndex], val[startIndex + 1]);
    }

    public static int ConvertToInt32(byte[] val)
    {
      return Marshal.ConvertToInt32(val, 0);
    }

    public static int ConvertToInt32(byte[] val, int startIndex)
    {
      return Marshal.ConvertToInt32(val[startIndex], val[startIndex + 1], val[startIndex + 2], val[startIndex + 3]);
    }

    public static int ConvertToInt32(byte v1, byte v2, byte v3, byte v4)
    {
      return (int) v1 << 24 | (int) v2 << 16 | (int) v3 << 8 | (int) v4;
    }

    public static long ConvertToInt64(int v1, uint v2)
    {
      int num = 1;
      if (v1 < 0)
        num = -1;
      return (long) ((double) num * (Math.Abs((double) v1 * Math.Pow(2.0, 32.0)) + (double) v2));
    }

    public static string ConvertToString(byte[] cstyle)
    {
      if (cstyle == null)
        return (string) null;
      for (int count = 0; count < cstyle.Length; ++count)
      {
        if (cstyle[count] == (byte) 0)
          return Encoding.Default.GetString(cstyle, 0, count);
      }
      return Encoding.Default.GetString(cstyle);
    }

    public static ushort ConvertToUInt16(byte[] val)
    {
      return Marshal.ConvertToUInt16(val, 0);
    }

    public static ushort ConvertToUInt16(byte v1, byte v2)
    {
      return (ushort) ((uint) v2 | (uint) v1 << 8);
    }

    public static ushort ConvertToUInt16(byte[] val, int startIndex)
    {
      return Marshal.ConvertToUInt16(val[startIndex], val[startIndex + 1]);
    }

    public static uint ConvertToUInt32(byte[] val)
    {
      return Marshal.ConvertToUInt32(val, 0);
    }

    public static uint ConvertToUInt32(byte[] val, int startIndex)
    {
      return Marshal.ConvertToUInt32(val[startIndex], val[startIndex + 1], val[startIndex + 2], val[startIndex + 3]);
    }

    public static uint ConvertToUInt32(byte v1, byte v2, byte v3, byte v4)
    {
      return (uint) ((int) v1 << 24 | (int) v2 << 16 | (int) v3 << 8) | (uint) v4;
    }

    public static string ToHexDump(string description, byte[] dump)
    {
      return Marshal.ToHexDump(description, dump, 0, dump.Length);
    }

    public static string ToHexDump(string description, byte[] dump, int start, int count)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      if (description != null)
        stringBuilder1.Append(description).Append("\n");
      int num1 = start + count;
      for (int index1 = start; index1 < num1; index1 += 16)
      {
        StringBuilder stringBuilder2 = new StringBuilder();
        StringBuilder stringBuilder3 = new StringBuilder();
        stringBuilder3.Append(index1.ToString("X4"));
        stringBuilder3.Append(": ");
        for (int index2 = 0; index2 < 16; ++index2)
        {
          if (index2 + index1 < num1)
          {
            byte num2 = dump[index2 + index1];
            stringBuilder3.Append(dump[index2 + index1].ToString("X2"));
            stringBuilder3.Append(" ");
            if (num2 >= (byte) 32 && num2 <= (byte) 127)
              stringBuilder2.Append((char) num2);
            else
              stringBuilder2.Append(".");
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
        stringBuilder1.Append(stringBuilder3.ToString());
      }
      return stringBuilder1.ToString();
    }

    public static byte[] Uncompress(byte[] src)
    {
      MemoryStream memoryStream = new MemoryStream();
      ZOutputStream zoutputStream = new ZOutputStream((Stream) memoryStream);
      zoutputStream.Write(src, 0, src.Length);
      zoutputStream.Close();
      return memoryStream.ToArray();
    }

    public static T LoadDataFile<T>(string filename, bool isEncrypt)
    {
      if (!File.Exists("datas/" + filename + ".data"))
        return default (T);
      try
      {
        byte[] numArray = File.ReadAllBytes("datas/" + filename + ".data");
        if (isEncrypt)
          numArray = Marshal.Uncompress(numArray);
        MemoryStream memoryStream = new MemoryStream(numArray);
        memoryStream.Position = 0L;
        T obj = Serializer.Deserialize<T>((Stream) memoryStream);
        memoryStream.Dispose();
        return obj;
      }
      catch (Exception ex)
      {
        Console.WriteLine("Data " + filename + " is error!" + (object) ex);
      }
      return default (T);
    }

    public static bool SaveDataFile<T>(T instance, string filename, bool isEncrypt)
    {
      try
      {
        byte[] numArray;
        if ((object) instance == null)
        {
          numArray = new byte[0];
        }
        else
        {
          MemoryStream memoryStream = new MemoryStream();
          Serializer.Serialize<T>((Stream) memoryStream, instance);
          numArray = new byte[memoryStream.Length];
          memoryStream.Position = 0L;
          memoryStream.Read(numArray, 0, numArray.Length);
          memoryStream.Dispose();
        }
        if (isEncrypt)
          numArray = Marshal.Compress(numArray);
        File.WriteAllBytes("datas/" + filename + ".data", numArray);
        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine((object) ex);
      }
      return false;
    }
  }
}
