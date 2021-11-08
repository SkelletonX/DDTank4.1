// Decompiled with JetBrains decompiler
// Type: Game.Logic.MapMgr
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using Game.Logic.Phy.Maps;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Game.Logic
{
  public class MapMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static Dictionary<int, Map> _mapInfos;
    private static Dictionary<int, MapPoint> _maps;
    private static Dictionary<int, List<int>> _serverMap;
    private static ReaderWriterLock m_lock;
    private static ThreadSafeRandom random;

    public static Map CloneMap(int index)
    {
      if (MapMgr._mapInfos.ContainsKey(index))
        return MapMgr._mapInfos[index].Clone();
      return (Map) null;
    }

    public static MapInfo FindMapInfo(int index)
    {
      if (MapMgr._mapInfos.ContainsKey(index))
        return MapMgr._mapInfos[index].Info;
      return (MapInfo) null;
    }

    public static int GetMapIndex(int index, byte type, int serverId)
    {
      if (index != 0 && !MapMgr._maps.Keys.Contains<int>(index))
        index = 0;
      if ((uint) index > 0U)
        return index;
      List<int> intList = new List<int>();
      foreach (int index1 in MapMgr._serverMap[serverId])
      {
        MapInfo mapInfo = MapMgr.FindMapInfo(index1);
        if (((uint) type & (uint) mapInfo.Type) > 0U)
          intList.Add(index1);
      }
      if (intList.Count == 0)
      {
        int count = MapMgr._serverMap[serverId].Count;
        return MapMgr._serverMap[serverId][MapMgr.random.Next(count)];
      }
      int count1 = intList.Count;
      return intList[MapMgr.random.Next(count1)];
    }

    public static MapPoint GetMapRandomPos(int index)
    {
      MapPoint mapPoint = new MapPoint();
      if (index != 0 && !MapMgr._maps.Keys.Contains<int>(index))
        index = 0;
      MapPoint map;
      if (index == 0)
      {
        int[] array = MapMgr._maps.Keys.ToArray<int>();
        map = MapMgr._maps[array[MapMgr.random.Next(array.Length)]];
      }
      else
        map = MapMgr._maps[index];
      if (MapMgr.random.Next(2) == 1)
      {
        mapPoint.PosX.AddRange((IEnumerable<Point>) map.PosX);
        mapPoint.PosX1.AddRange((IEnumerable<Point>) map.PosX1);
        return mapPoint;
      }
      mapPoint.PosX.AddRange((IEnumerable<Point>) map.PosX1);
      mapPoint.PosX1.AddRange((IEnumerable<Point>) map.PosX);
      return mapPoint;
    }

    public static MapPoint GetPVEMapRandomPos(int index)
    {
      MapPoint mapPoint = new MapPoint();
      if (index != 0 && !MapMgr._maps.Keys.Contains<int>(index))
        index = 0;
      MapPoint map;
      if (index == 0)
      {
        int[] array = MapMgr._maps.Keys.ToArray<int>();
        map = MapMgr._maps[array[MapMgr.random.Next(array.Length)]];
      }
      else
        map = MapMgr._maps[index];
      mapPoint.PosX.AddRange((IEnumerable<Point>) map.PosX);
      mapPoint.PosX1.AddRange((IEnumerable<Point>) map.PosX1);
      return mapPoint;
    }

    public static bool Init()
    {
      try
      {
        MapMgr.random = new ThreadSafeRandom();
        MapMgr.m_lock = new ReaderWriterLock();
        MapMgr._maps = new Dictionary<int, MapPoint>();
        MapMgr._mapInfos = new Dictionary<int, Map>();
        if (!MapMgr.LoadMap(MapMgr._maps, MapMgr._mapInfos))
          return false;
        MapMgr._serverMap = new Dictionary<int, List<int>>();
        if (!MapMgr.InitServerMap(MapMgr._serverMap))
          return false;
      }
      catch (Exception ex)
      {
        if (MapMgr.log.IsErrorEnabled)
          MapMgr.log.Error((object) nameof (MapMgr), ex);
        return false;
      }
      return true;
    }

    public static bool InitServerMap(Dictionary<int, List<int>> servermap)
    {
      ServerMapInfo[] allServerMap = new MapBussiness().GetAllServerMap();
      try
      {
        int result = 0;
        foreach (ServerMapInfo serverMapInfo in allServerMap)
        {
          if (!servermap.Keys.Contains<int>(serverMapInfo.ServerID))
          {
            string[] strArray = serverMapInfo.OpenMap.Split(',');
            List<int> intList = new List<int>();
            foreach (string s in strArray)
            {
              if (!string.IsNullOrEmpty(s) && int.TryParse(s, out result))
                intList.Add(result);
            }
            servermap.Add(serverMapInfo.ServerID, intList);
          }
        }
      }
      catch (Exception ex)
      {
        MapMgr.log.Error((object) ex.ToString());
      }
      return true;
    }

    public static bool LoadMap(Dictionary<int, MapPoint> maps, Dictionary<int, Map> mapInfos)
    {
      try
      {
        foreach (MapInfo all in new MapBussiness().GetAllMap())
        {
          if (!string.IsNullOrEmpty(all.PosX))
          {
            if (!maps.Keys.Contains<int>(all.ID))
            {
              string[] strArray1 = all.PosX.Split('|');
              string[] strArray2 = all.PosX1.Split('|');
              MapPoint mapPoint = new MapPoint();
              foreach (string str in strArray1)
              {
                if (!string.IsNullOrEmpty(str.Trim()))
                {
                  string[] strArray3 = str.Split(',');
                  mapPoint.PosX.Add(new Point(int.Parse(strArray3[0]), int.Parse(strArray3[1])));
                }
              }
              foreach (string str in strArray2)
              {
                if (!string.IsNullOrEmpty(str.Trim()))
                {
                  string[] strArray3 = str.Split(',');
                  mapPoint.PosX1.Add(new Point(int.Parse(strArray3[0]), int.Parse(strArray3[1])));
                }
              }
              maps.Add(all.ID, mapPoint);
            }
            if (!mapInfos.ContainsKey(all.ID))
            {
              Tile layer1 = (Tile) null;
              string str1 = string.Format("map\\{0}\\fore.map", (object) all.ID);
              if (File.Exists(str1))
                layer1 = new Tile(str1, true);
              Tile layer2 = (Tile) null;
              string str2 = string.Format("map\\{0}\\dead.map", (object) all.ID);
              if (File.Exists(str2))
                layer2 = new Tile(str2, false);
              if (layer1 != null || layer2 != null)
                mapInfos.Add(all.ID, new Map(all, layer1, layer2));
              else if (MapMgr.log.IsErrorEnabled)
                MapMgr.log.Error((object) ("Map's file" + (object) all.ID + " is not exist!"));
            }
          }
        }
        if ((uint) maps.Count > 0U)
        {
          if ((uint) mapInfos.Count > 0U)
            goto label_36;
        }
        if (MapMgr.log.IsErrorEnabled)
          MapMgr.log.Error((object) ("maps:" + (object) maps.Count + ",mapInfos:" + (object) mapInfos.Count));
        return false;
      }
      catch (Exception ex)
      {
        if (MapMgr.log.IsErrorEnabled)
          MapMgr.log.Error((object) nameof (MapMgr), ex);
        return false;
      }
label_36:
      return true;
    }

    public static bool ReLoadMap()
    {
      try
      {
        Dictionary<int, MapPoint> maps = new Dictionary<int, MapPoint>();
        Dictionary<int, Map> mapInfos = new Dictionary<int, Map>();
        if (MapMgr.LoadMap(maps, mapInfos))
        {
          MapMgr.m_lock.AcquireWriterLock(-1);
          try
          {
            MapMgr._maps = maps;
            MapMgr._mapInfos = mapInfos;
            return true;
          }
          catch
          {
          }
          finally
          {
            MapMgr.m_lock.ReleaseWriterLock();
          }
        }
      }
      catch (Exception ex)
      {
        if (MapMgr.log.IsErrorEnabled)
          MapMgr.log.Error((object) nameof (ReLoadMap), ex);
      }
      return false;
    }

    public static bool ReLoadMapServer()
    {
      try
      {
        Dictionary<int, List<int>> servermap = new Dictionary<int, List<int>>();
        if (MapMgr.InitServerMap(servermap))
        {
          MapMgr.m_lock.AcquireWriterLock(-1);
          try
          {
            MapMgr._serverMap = servermap;
            return true;
          }
          catch
          {
          }
          finally
          {
            MapMgr.m_lock.ReleaseWriterLock();
          }
        }
      }
      catch (Exception ex)
      {
        if (MapMgr.log.IsErrorEnabled)
          MapMgr.log.Error((object) "ReLoadMapWeek", ex);
      }
      return false;
    }

    public static int GetWeekDay
    {
      get
      {
        int int32 = Convert.ToInt32((object) DateTime.Now.DayOfWeek);
        if ((uint) int32 > 0U)
          return int32;
        return 7;
      }
    }
  }
}
