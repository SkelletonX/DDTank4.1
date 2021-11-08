// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.DailyLeagueGetRewardHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
  [PacketHandler(256, "LEAGUE_GETAWARD")]
  public class DailyLeagueGetRewardHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num1 = packet.ReadInt();
      int num2 = 0;
      int Score = 0;
      int num3 = 0;
      if (DateTime.Compare(client.Player.LastOpenCard.AddSeconds(0.5), DateTime.Now) > 0)
        return 0;
      string translateId = "DailyLeagueGetReward.Successfull";
      ProduceBussiness produceBussiness = new ProduceBussiness();
      DailyLeagueAwardList[] dailyLeagueAwardList1 = produceBussiness.GetAllDailyLeagueAwardList();
      DailyLeagueAwardItems[] leagueAwardItems1 = produceBussiness.GetAllDailyLeagueAwardItems();
      List<SqlDataProvider.Data.ItemInfo> items = new List<SqlDataProvider.Data.ItemInfo>();
      foreach (DailyLeagueAwardList dailyLeagueAwardList2 in dailyLeagueAwardList1)
      {
        if (dailyLeagueAwardList2.Class == num1)
        {
          Score = dailyLeagueAwardList2.Score;
          num3 = dailyLeagueAwardList2.Grade;
          switch (Score)
          {
            case 150:
              num2 = 1;
              continue;
            case 200:
              num2 = 3;
              continue;
            case 250:
              num2 = 7;
              continue;
            case 300:
              num2 = 15;
              continue;
            case 400:
              num2 = 31;
              continue;
            case 550:
              num2 = 63;
              continue;
            case 700:
              num2 = (int) sbyte.MaxValue;
              continue;
            case 850:
              num2 = (int) byte.MaxValue;
              continue;
            case 900:
              num2 = 511;
              continue;
            default:
              num2 = 1023;
              translateId = "DailyLeagueGetReward.Error";
              continue;
          }
        }
      }
      int awardGot = (1 << num1) - 1;
      foreach (DailyLeagueAwardItems leagueAwardItems2 in leagueAwardItems1)
      {
        if (leagueAwardItems2.Class == num1)
        {
          SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(leagueAwardItems2.TemplateID), 1, 104);
          fromTemplate.StrengthenLevel = leagueAwardItems2.StrengthLevel;
          fromTemplate.AttackCompose = leagueAwardItems2.AttackCompose;
          fromTemplate.DefendCompose = leagueAwardItems2.DefendCompose;
          fromTemplate.AgilityCompose = leagueAwardItems2.AgilityCompose;
          fromTemplate.LuckCompose = leagueAwardItems2.LuckCompose;
          fromTemplate.IsBinds = leagueAwardItems2.IsBind;
          fromTemplate.ValidDate = leagueAwardItems2.ValidDate;
          fromTemplate.Count = leagueAwardItems2.Count;
          items.Add(fromTemplate);
        }
      }
      if (items != null)
      {
        string str;
        if (client.Player.PlayerCharacter.Grade >= num3)
        {
          if (client.Player.MatchInfo.weeklyScore >= Score)
          {
            client.Player.RefreshLeagueGetReward(awardGot, Score);
            Console.WriteLine(string.Format("awardGot: {0}", (object) awardGot));
            client.Player.SendItemsToMail(items, LanguageMgr.GetTranslation("DailyLeagueGetReward.Content"), LanguageMgr.GetTranslation("Game.Server.LeagueReward.Title"), eMailType.Manage);
            return 1;
          }
          str = "Você não possui pontos semanais suficientes para a compra";
          return 0;
        }
        str = "O nível não é suficiente para a coleta dos itens dessa grade";
        return 0;
      }
      client.Player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation(translateId));
      client.Player.LastOpenCard = DateTime.Now;
      return 1;
    }
  }
}
