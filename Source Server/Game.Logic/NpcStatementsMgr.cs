// Decompiled with JetBrains decompiler
// Type: Game.Logic.NpcStatementsMgr
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Game.Logic
{
  public class NpcStatementsMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static List<string> m_npcstatement = new List<string>();
    private static string filePath;
    private static Random random;

    public static string GetRandomStatement()
    {
      int index = NpcStatementsMgr.random.Next(0, NpcStatementsMgr.m_npcstatement.Count);
      return NpcStatementsMgr.m_npcstatement[index];
    }

    public static string GetStatement(int index)
    {
      if (index < 0 || index > NpcStatementsMgr.m_npcstatement.Count)
        return (string) null;
      return NpcStatementsMgr.m_npcstatement[index];
    }

    public static bool Init()
    {
      NpcStatementsMgr.filePath = Directory.GetCurrentDirectory() + "\\ai\\npc\\npc_statements.txt";
      NpcStatementsMgr.random = new Random();
      return NpcStatementsMgr.ReLoad();
    }

    public static string[] RandomStatement(int count)
    {
      string[] strArray = new string[count];
      int[] numArray = NpcStatementsMgr.RandomStatementIndexs(count);
      for (int index1 = 0; index1 < count; ++index1)
      {
        int index2 = numArray[index1];
        strArray[index1] = NpcStatementsMgr.m_npcstatement[index2];
      }
      return strArray;
    }

    public static int[] RandomStatementIndexs(int count)
    {
      int[] numArray = new int[count];
      int index = 0;
      while (index < count)
      {
        int num = NpcStatementsMgr.random.Next(0, NpcStatementsMgr.m_npcstatement.Count);
        if (!((IEnumerable<int>) numArray).Contains<int>(num))
        {
          numArray[index] = num;
          ++index;
        }
      }
      return numArray;
    }

    public static bool ReLoad()
    {
      try
      {
        string empty = string.Empty;
        StreamReader streamReader = new StreamReader(NpcStatementsMgr.filePath, Encoding.Default);
        string str;
        while (!string.IsNullOrEmpty(str = streamReader.ReadLine()))
          NpcStatementsMgr.m_npcstatement.Add(str);
        return true;
      }
      catch (Exception ex)
      {
        NpcStatementsMgr.log.Error((object) "NpcStatementsMgr.Reload()", ex);
        return false;
      }
    }
  }
}
