// Decompiled with JetBrains decompiler
// Type: Bussiness.CheckCode
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Bussiness
{
  public class CheckCode
  {
    private static Color[] c = new Color[2]
    {
      Color.Gray,
      Color.DimGray
    };
    private static char[] digitals = new char[9]
    {
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9'
    };
    private static string[] font = new string[5]
    {
      "Verdana",
      "Terminal",
      "Comic Sans MS",
      "Arial",
      "Tekton Pro"
    };
    private static char[] letters = new char[50]
    {
      'a',
      'b',
      'c',
      'd',
      'e',
      'f',
      'g',
      'h',
      'i',
      'j',
      'k',
      'l',
      'm',
      'n',
      'p',
      'q',
      'r',
      's',
      't',
      'u',
      'v',
      'w',
      'x',
      'y',
      'z',
      'A',
      'B',
      'C',
      'D',
      'E',
      'F',
      'G',
      'H',
      'I',
      'J',
      'K',
      'L',
      'M',
      'N',
      'P',
      'Q',
      'R',
      'S',
      'T',
      'U',
      'V',
      'W',
      'X',
      'Y',
      'Z'
    };
    private static char[] lowerLetters = new char[21]
    {
      'a',
      'b',
      'c',
      'd',
      'e',
      'f',
      'h',
      'k',
      'm',
      'n',
      'p',
      'q',
      'r',
      's',
      't',
      'u',
      'v',
      'w',
      'x',
      'y',
      'z'
    };
    private static char[] mix = new char[51]
    {
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9',
      'a',
      'b',
      'c',
      'd',
      'e',
      'f',
      'h',
      'k',
      'm',
      'n',
      'p',
      'q',
      'r',
      's',
      't',
      'u',
      'v',
      'w',
      'x',
      'y',
      'z',
      'A',
      'B',
      'C',
      'D',
      'E',
      'F',
      'G',
      'H',
      'K',
      'M',
      'N',
      'P',
      'Q',
      'R',
      'S',
      'T',
      'U',
      'V',
      'W',
      'X',
      'Y',
      'Z'
    };
    public static ThreadSafeRandom rand = new ThreadSafeRandom();
    private static char[] upperLetters = new char[22]
    {
      'A',
      'B',
      'C',
      'D',
      'E',
      'F',
      'G',
      'H',
      'K',
      'M',
      'N',
      'P',
      'Q',
      'R',
      'S',
      'T',
      'U',
      'V',
      'W',
      'X',
      'Y',
      'Z'
    };

    public static byte[] CreateImage(string randomcode)
    {
      int maxValue = 30;
      Bitmap bitmap = new Bitmap(randomcode.Length * 30, 32);
      Graphics graphics = Graphics.FromImage((Image) bitmap);
      graphics.SmoothingMode = SmoothingMode.HighQuality;
      try
      {
        graphics.Clear(Color.Transparent);
        int index1 = CheckCode.rand.Next(2);
        Brush brush = (Brush) new SolidBrush(CheckCode.c[index1]);
        for (int index2 = 0; index2 < 1; ++index2)
        {
          int num1 = CheckCode.rand.Next(bitmap.Width / 2);
          int num2 = CheckCode.rand.Next(bitmap.Width * 3 / 4, bitmap.Width);
          int num3 = CheckCode.rand.Next(bitmap.Height);
          int num4 = CheckCode.rand.Next(bitmap.Height);
          graphics.DrawBezier(new Pen(CheckCode.c[index1], 2f), (float) num1, (float) num3, (float) ((num1 + num2) / 4), 0.0f, (float) ((num1 + num2) * 3 / 4), (float) bitmap.Height, (float) num2, (float) num4);
        }
        char[] charArray = randomcode.ToCharArray();
        StringFormat format = new StringFormat(StringFormatFlags.NoClip)
        {
          Alignment = StringAlignment.Center,
          LineAlignment = StringAlignment.Center
        };
        for (int index2 = 0; index2 < charArray.Length; ++index2)
        {
          int index3 = CheckCode.rand.Next(5);
          Font font = new Font(CheckCode.font[index3], 22f, FontStyle.Bold);
          Point point = new Point(16, 16);
          float angle = (float) ThreadSafeRandom.NextStatic(-maxValue, maxValue);
          graphics.TranslateTransform((float) point.X, (float) point.Y);
          graphics.RotateTransform(angle);
          graphics.DrawString(charArray[index2].ToString(), font, brush, 1f, 1f, format);
          graphics.RotateTransform(-angle);
          graphics.TranslateTransform(2f, -(float) point.Y);
        }
        MemoryStream memoryStream = new MemoryStream();
        bitmap.Save((Stream) memoryStream, ImageFormat.Png);
        return memoryStream.ToArray();
      }
      finally
      {
        graphics.Dispose();
        bitmap.Dispose();
      }
    }

    public static string GenerateCheckCode()
    {
      return CheckCode.GenerateRandomString(4, CheckCode.RandomStringMode.Mix);
    }

    private static string GenerateRandomString(int length, CheckCode.RandomStringMode mode)
    {
      string empty = string.Empty;
      if ((uint) length > 0U)
      {
        switch (mode)
        {
          case CheckCode.RandomStringMode.LowerLetter:
            for (int index = 0; index < length; ++index)
              empty += CheckCode.lowerLetters[CheckCode.rand.Next(0, CheckCode.lowerLetters.Length)].ToString();
            return empty;
          case CheckCode.RandomStringMode.UpperLetter:
            for (int index = 0; index < length; ++index)
              empty += CheckCode.upperLetters[CheckCode.rand.Next(0, CheckCode.upperLetters.Length)].ToString();
            return empty;
          case CheckCode.RandomStringMode.Letter:
            for (int index = 0; index < length; ++index)
              empty += CheckCode.letters[CheckCode.rand.Next(0, CheckCode.letters.Length)].ToString();
            return empty;
          case CheckCode.RandomStringMode.Digital:
            for (int index = 0; index < length; ++index)
              empty += CheckCode.digitals[CheckCode.rand.Next(0, CheckCode.digitals.Length)].ToString();
            return empty;
          default:
            for (int index = 0; index < length; ++index)
              empty += CheckCode.mix[CheckCode.rand.Next(0, CheckCode.mix.Length)].ToString();
            break;
        }
      }
      return empty;
    }

    private enum RandomStringMode
    {
      LowerLetter,
      UpperLetter,
      Letter,
      Digital,
      Mix,
    }
  }
}
