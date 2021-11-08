// Decompiled with JetBrains decompiler
// Type: Game.Server.Consortia.Handle.CConsortiaTask
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.ConsortiaTask;
using Game.Server.GameObjects;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.Consortia.Handle
{
  [global::Consortia(22)]
  public class CConsortiaTask : IConsortiaCommandHadler
  {
    public int CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      if (Player.PlayerCharacter.ConsortiaID == 0)
        return 0;
      switch (packet.ReadInt())
      {
        case 0:
          using (ConsortiaBussiness consortiaBussiness1 = new ConsortiaBussiness())
          {
            ConsortiaInfo consortiaSingle = consortiaBussiness1.GetConsortiaSingle(Player.PlayerCharacter.ConsortiaID);
            if (consortiaSingle != null && consortiaSingle.ChairmanID == Player.PlayerCharacter.ID)
            {
              int riches = GameProperties.MissionRichesArr()[consortiaSingle.Level - 1];
              DateTime dateTime = consortiaSingle.DateOpenTask;
              DateTime date1 = dateTime.Date;
              dateTime = DateTime.Now;
              DateTime date2 = dateTime.Date;
              if (date1 != date2)
              {
                if (consortiaBussiness1.ConsortiaRichRemove(consortiaSingle.ConsortiaID, ref riches))
                {
                  ConsortiaBussiness consortiaBussiness2 = consortiaBussiness1;
                  int consortiaId = consortiaSingle.ConsortiaID;
                  dateTime = DateTime.Now;
                  DateTime date3 = dateTime.Date;
                  int consortiID = consortiaId;
                  DateTime dateTask = date3;
                  consortiaBussiness2.ConsortiaTaskUpdateDate(consortiID, dateTask);
                  if (ConsortiaTaskMgr.AddConsortiaTask(consortiaSingle.ConsortiaID, consortiaSingle.Level))
                  {
                    BaseConsortiaTask singleConsortiaTask = ConsortiaTaskMgr.GetSingleConsortiaTask(Player.PlayerCharacter.ConsortiaID);
                    GSPacketIn pkg = new GSPacketIn((short) 129);
                    pkg.WriteByte((byte) 22);
                    pkg.WriteByte((byte) 0);
                    pkg.WriteInt(singleConsortiaTask.ConditionList.Count);
                    foreach (KeyValuePair<int, ConsortiaTaskInfo> condition in singleConsortiaTask.ConditionList)
                    {
                      pkg.WriteInt(condition.Key);
                      pkg.WriteString(condition.Value.CondictionTitle);
                    }
                    Player.SendTCP(pkg);
                    break;
                  }
                  Player.SendMessage(LanguageMgr.GetTranslation("GameServer.ConsortiaTask.msg6"));
                  break;
                }
                Player.SendMessage(LanguageMgr.GetTranslation("GameServer.ConsortiaTask.msg8"));
                break;
              }
              Player.SendMessage(LanguageMgr.GetTranslation("GameServer.ConsortiaTask.msg5"));
              break;
            }
            Player.SendMessage(LanguageMgr.GetTranslation("GameServer.ConsortiaTask.msg7"));
            break;
          }
        case 2:
          BaseConsortiaTask singleConsortiaTask1 = ConsortiaTaskMgr.GetSingleConsortiaTask(Player.PlayerCharacter.ConsortiaID);
          bool val = false;
          if (singleConsortiaTask1 != null && !singleConsortiaTask1.Info.IsActive && ConsortiaTaskMgr.ActiveTask(Player.PlayerCharacter.ConsortiaID))
          {
            val = true;
            Player.Out.SendConsortiaTaskInfo(singleConsortiaTask1);
          }
          GSPacketIn pkg1 = new GSPacketIn((short) 129);
          pkg1.WriteByte((byte) 22);
          pkg1.WriteByte((byte) 2);
          pkg1.WriteBoolean(val);
          Player.SendTCP(pkg1);
          break;
        case 3:
          BaseConsortiaTask singleConsortiaTask2 = ConsortiaTaskMgr.GetSingleConsortiaTask(Player.PlayerCharacter.ConsortiaID);
          Player.Out.SendConsortiaTaskInfo(singleConsortiaTask2);
          break;
      }
      return 0;
    }
  }
}
