// Decompiled with JetBrains decompiler
// Type: Bussiness.Base64
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

namespace Bussiness
{
  public class Base64
  {
    private static readonly string BASE64_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

    public static byte[] decodeToByteArray(string param1)
    {
      byte[] numArray1 = new byte[param1.Length];
      byte[] numArray2 = new byte[4];
      byte[] numArray3 = new byte[3];
      for (int index1 = 0; index1 < param1.Length; index1 += 4)
      {
        int index2 = 0;
        int startIndex;
        do
        {
          startIndex = index1 + index2;
          if (index2 < 4)
            numArray2[index2] = (byte) Base64.BASE64_CHARS.IndexOf(param1.Substring(startIndex, 1));
          ++index2;
        }
        while (startIndex < param1.Length);
        numArray3[0] = (byte) (((int) numArray2[0] << 2) + (((int) numArray2[1] & 48) >> 4));
        numArray3[1] = (byte) ((((int) numArray2[1] & 15) << 4) + (((int) numArray2[2] & 60) >> 2));
        numArray3[2] = (byte) ((uint) (((int) numArray2[2] & 3) << 6) + (uint) numArray2[3]);
        for (int index3 = 0; index3 < numArray3.Length && numArray2[index3 + 1] != (byte) 64; ++index3)
          numArray1[index1 + index3] = numArray3[index3];
      }
      return numArray1;
    }

    public static byte[] decodeToByteArray2(string param1)
    {
      byte[] numArray1 = new byte[param1.Length];
      byte[] numArray2 = new byte[4];
      for (int index1 = 0; index1 < param1.Length; index1 += 4)
      {
        int index2 = 0;
        int startIndex;
        do
        {
          startIndex = index1 + index2;
          if (index2 < 4)
            numArray2[index2] = (byte) Base64.BASE64_CHARS.IndexOf(param1.Substring(startIndex, 1));
          ++index2;
        }
        while (startIndex < param1.Length);
        for (int index3 = 0; index3 < numArray2.Length && numArray2[index3] != (byte) 64; ++index3)
          numArray1[index1 + index3] = numArray2[index3];
      }
      return numArray1;
    }

    public static string encodeByteArray(byte[] param1)
    {
      string str = "";
      byte[] numArray1 = new byte[4];
      for (int index1 = 0; index1 < param1.Length; index1 += 4)
      {
        byte[] numArray2 = new byte[3];
        for (int index2 = 0; index2 < param1.Length; ++index2)
        {
          if (index2 < 3)
          {
            if (index2 + index1 <= param1.Length)
              numArray2[index2] = param1[index2 + index1];
            else
              break;
          }
        }
        numArray1[0] = (byte) (((int) numArray2[0] & 252) >> 2);
        numArray1[1] = (byte) (((int) numArray2[0] & 3) << 4 | (int) numArray2[1] >> 4);
        numArray1[2] = (byte) (((int) numArray2[1] & 15) << 2 | (int) numArray2[2] >> 6);
        numArray1[3] = (byte) ((uint) numArray2[2] & 63U);
        for (int length = numArray2.Length; length < 3; ++length)
          numArray1[length + 1] = (byte) 64;
        for (int index2 = 0; index2 < numArray1.Length; ++index2)
          str += Base64.BASE64_CHARS.Substring((int) numArray1[index2], 1);
      }
      return str.Substring(0, param1.Length - 1) + "=";
    }
  }
}
