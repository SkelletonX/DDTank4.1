// Decompiled with JetBrains decompiler
// Type: Game.Server.Consortia.Handle.SkillSocket
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using Game.Server.Packets;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Consortia.Handle
{
  [global::Consortia(26)]
  public class SkillSocket : IConsortiaCommandHadler
  {
    public int CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      if (Player.PlayerCharacter.ConsortiaID == 0)
        return 0;
      if (DateTime.Compare(Player.LastRequestTime.AddSeconds(2.0), DateTime.Now) > 0)
      {
        Player.SendMessage(LanguageMgr.GetTranslation("GoSlow"));
        return 0;
      }
      Player.LastRequestTime = DateTime.Now;
      packet.ReadBoolean();
      int id = packet.ReadInt();
      int num1 = packet.ReadInt();
      int num2 = packet.ReadInt();
      if (num1 < 0)
        num1 = 1;
      ConsortiaBuffTempInfo consortiaBuffInfo = ConsortiaExtraMgr.FindConsortiaBuffInfo(id);
      if (consortiaBuffInfo == null)
      {
        Player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Consortia.Msg1"));
        return 0;
      }
      bool flag = false;
      int num3;
      if (num2 == 1)
      {
        num3 = num1 * consortiaBuffInfo.riches;
        if (Player.PlayerCharacter.Riches >= num3)
          flag = true;
      }
      else
      {
        num3 = num1 * consortiaBuffInfo.metal;
        if (Player.GetMedalNum() >= num3)
          flag = true;
      }
      int validate = 1440 * num1;
      ConsortiaInfo consortiaInfo = ConsortiaMgr.FindConsortiaInfo(Player.PlayerCharacter.ConsortiaID);
      if (consortiaInfo != null & flag)
      {
        if (consortiaBuffInfo.level <= consortiaInfo.Level)
        {
          ConsortiaMgr.AddBuffConsortia(Player, consortiaBuffInfo, Player.PlayerCharacter.ConsortiaID, id, validate);
          if (num2 == 1)
          {
            if (consortiaBuffInfo.type == 1)
            {
              using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
              {
                int riches = num3;
                consortiaBussiness.ConsortiaRichRemove(Player.PlayerCharacter.ConsortiaID, ref riches);
              }
            }
            else
              Player.RemoveRichesOffer(num3);
          }
          else
            Player.RemoveMedal(num3);
          Player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Consortia.Msg4"));
        }
        else
          Player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Consortia.Msg5"));
      }
      else
        Player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Consortia.Msg6"));
      return 0;
    }
  }
}
