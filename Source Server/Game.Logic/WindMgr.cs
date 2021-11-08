// Decompiled with JetBrains decompiler
// Type: Game.Logic.WindMgr
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Game.Logic
{
  public class WindMgr
  {
    private static readonly Color[] c = new Color[8]
    {
      Color.Yellow,
      Color.Red,
      Color.Blue,
      Color.Green,
      Color.Orange,
      Color.Aqua,
      Color.DarkCyan,
      Color.Purple
    };
    private static readonly int[] CategoryID = new int[9]
    {
      1001,
      1002,
      1003,
      1004,
      1005,
      1005,
      1007,
      1008,
      1009
    };
    private static readonly string[] font = new string[1]
    {
      "Arial"
    };
    private static readonly string[] fontWind = new string[11]
    {
      "•",
      "1",
      "2",
      "3",
      "4",
      "5",
      "6",
      "7",
      "8",
      "9",
      "0"
    };
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static readonly int[] WindID = new int[11]
    {
      0,
      1,
      2,
      3,
      4,
      5,
      6,
      7,
      8,
      9,
      10
    };
    private static Dictionary<int, WindInfo> _winds;
    private static ReaderWriterLock m_lock;
    private static ThreadSafeRandom rand;

    public static byte[] CreateVane(string wind)
    {
      int maxValue = 1;
      int width = 18;
      if (WindMgr.isSmall(wind))
        width = 10;
      Bitmap bitmap = new Bitmap(width, 32);
      Graphics graphics = Graphics.FromImage((Image) bitmap);
      graphics.SmoothingMode = SmoothingMode.HighQuality;
      try
      {
        graphics.Clear(Color.Transparent);
        WindMgr.rand.Next(7);
        Brush brush = (Brush) new SolidBrush(Color.Red);
        StringFormat format = new StringFormat(StringFormatFlags.NoClip)
        {
          Alignment = StringAlignment.Center,
          LineAlignment = StringAlignment.Center
        };
        int index = WindMgr.rand.Next(WindMgr.font.Length);
        Font font = new Font(WindMgr.font[index], 17f, FontStyle.Italic);
        Point point = new Point(8, 12);
        if (WindMgr.isSmall(wind))
        {
          if (wind == WindMgr.fontWind[0])
            font = new Font(WindMgr.font[index], 10f, FontStyle.Regular);
          point = new Point(4, 16);
        }
        float angle = (float) ThreadSafeRandom.NextStatic(-maxValue, maxValue);
        graphics.TranslateTransform((float) point.X, (float) point.Y);
        graphics.RotateTransform(angle);
        graphics.DrawString(wind.ToString(), font, brush, 1f, 1f, format);
        graphics.RotateTransform(-angle);
        graphics.TranslateTransform(2f, -(float) point.Y);
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

    public static WindInfo FindWind(int ID)
    {
      WindMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        if (WindMgr._winds.ContainsKey(ID))
          return WindMgr._winds[ID];
      }
      catch
      {
      }
      finally
      {
        WindMgr.m_lock.ReleaseReaderLock();
      }
      return (WindInfo) null;
    }

    public static List<WindInfo> GetWind()
    {
      WindMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        List<WindInfo> windInfoList = new List<WindInfo>();
        for (int index = 0; index < WindMgr._winds.Values.Count; ++index)
          windInfoList.Add(WindMgr._winds[index]);
        if (windInfoList.Count > 0)
          return windInfoList;
      }
      catch
      {
      }
      finally
      {
        WindMgr.m_lock.ReleaseReaderLock();
      }
      return (List<WindInfo>) null;
    }

    public static byte GetWindID(int wind, int pos)
    {
      if (wind < 10)
      {
        switch (pos)
        {
          case 1:
            return 10;
          case 3:
            if ((uint) wind > 0U)
              return (byte) wind;
            return 10;
        }
      }
      if (wind >= 10 && wind < 20)
      {
        switch (pos)
        {
          case 1:
            return 1;
          case 3:
            if ((uint) (wind - 10) > 0U)
              return (byte) (wind - 10);
            return 10;
        }
      }
      if (wind >= 20 && wind < 30)
      {
        switch (pos)
        {
          case 1:
            return 2;
          case 3:
            if ((uint) (wind - 20) > 0U)
              return (byte) (wind - 20);
            return 10;
        }
      }
      if (wind >= 30 && wind < 40)
      {
        switch (pos)
        {
          case 1:
            return 3;
          case 3:
            if ((uint) (wind - 30) > 0U)
              return (byte) (wind - 30);
            return 10;
        }
      }
      if (wind >= 40 && wind < 50)
      {
        switch (pos)
        {
          case 1:
            return 4;
          case 3:
            if ((uint) (wind - 40) > 0U)
              return (byte) (wind - 40);
            return 10;
        }
      }
      return 0;
    }

    public static bool Init()
    {
      try
      {
        WindMgr.m_lock = new ReaderWriterLock();
        WindMgr._winds = new Dictionary<int, WindInfo>();
        WindMgr.rand = new ThreadSafeRandom();
        return WindMgr.LoadWinds(WindMgr._winds);
      }
      catch (Exception ex)
      {
        if (WindMgr.log.IsErrorEnabled)
          WindMgr.log.Error((object) "WindInfoMgr", ex);
        return false;
      }
    }

    public static bool isSmall(string wind)
    {
      if (!(wind == WindMgr.fontWind[0]))
        return wind == WindMgr.fontWind[1];
      return true;
    }

    private static bool LoadWinds(Dictionary<int, WindInfo> Winds)
    {
      foreach (int key in WindMgr.WindID)
      {
        WindInfo windInfo = new WindInfo();
        byte[] vane = WindMgr.CreateVane(WindMgr.fontWind[key]);
        if (vane == null || vane.Length == 0)
        {
          if (WindMgr.log.IsErrorEnabled)
            WindMgr.log.Error((object) "Load Wind Error!");
          return false;
        }
        windInfo.WindID = key;
        windInfo.WindPic = vane;
        if (!Winds.ContainsKey(key))
          Winds.Add(key, windInfo);
      }
      return true;
    }

    public static byte[] ReadImageFile(string imageLocation)
    {
      long length = new FileInfo(imageLocation).Length;
      return new BinaryReader((Stream) new FileStream(imageLocation, FileMode.Open, FileAccess.Read)).ReadBytes((int) length);
    }

    public static bool ReLoad()
    {
      try
      {
        Dictionary<int, WindInfo> Winds = new Dictionary<int, WindInfo>();
        if (WindMgr.LoadWinds(Winds))
        {
          WindMgr.m_lock.AcquireWriterLock(-1);
          try
          {
            WindMgr._winds = Winds;
            return true;
          }
          catch
          {
          }
          finally
          {
            WindMgr.m_lock.ReleaseWriterLock();
          }
        }
      }
      catch (Exception ex)
      {
        if (WindMgr.log.IsErrorEnabled)
          WindMgr.log.Error((object) nameof (WindMgr), ex);
      }
      return false;
    }
  }
}
