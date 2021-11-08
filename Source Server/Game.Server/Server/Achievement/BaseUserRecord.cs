// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.BaseUserRecord
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;
using System.Collections;

namespace Game.Server.Achievement
{
  public class BaseUserRecord
  {
    protected GamePlayer m_player;
    protected int m_type;

    public BaseUserRecord(GamePlayer player, int type)
    {
      this.m_player = player;
      this.m_type = type;
    }

    public virtual void AddTrigger(GamePlayer player)
    {
    }

    public virtual void RemoveTrigger(GamePlayer player)
    {
    }

    public static void CreateCondition(Hashtable ht, GamePlayer m_player)
    {
      foreach (DictionaryEntry dictionaryEntry in ht)
      {
        int type = int.Parse(dictionaryEntry.Key.ToString());
        switch (type)
        {
          case 1:
            ChangeAttackCondition changeAttackCondition = new ChangeAttackCondition(m_player, type);
            continue;
          case 2:
            ChangeDefenceCondition defenceCondition = new ChangeDefenceCondition(m_player, type);
            continue;
          case 3:
            ChangeAgilityCondition agilityCondition = new ChangeAgilityCondition(m_player, type);
            continue;
          case 4:
            ChangeLuckyCondition changeLuckyCondition = new ChangeLuckyCondition(m_player, type);
            continue;
          case 5:
            Mission4KillCondition mission4KillCondition = new Mission4KillCondition(m_player, type);
            continue;
          case 6:
            Mission9OverCondition mission9OverCondition = new Mission9OverCondition(m_player, type);
            continue;
          case 7:
            MissionKillChiefCondition killChiefCondition = new MissionKillChiefCondition(m_player, type);
            continue;
          case 8:
            MissionKillMinotaurCondition minotaurCondition = new MissionKillMinotaurCondition(m_player, type);
            continue;
          case 9:
            ChangeFightPowerCondition fightPowerCondition = new ChangeFightPowerCondition(m_player, type);
            continue;
          case 10:
            ChangeGradeCondition changeGradeCondition = new ChangeGradeCondition(m_player, type);
            continue;
          case 11:
            ChangeTotalCondition changeTotalCondition = new ChangeTotalCondition(m_player, type);
            continue;
          case 12:
            ChangeWinCondition changeWinCondition = new ChangeWinCondition(m_player, type);
            continue;
          case 13:
            ChangeOnlineTimeCondition onlineTimeCondition = new ChangeOnlineTimeCondition(m_player, type);
            continue;
          case 14:
            FightByFreeCondition fightByFreeCondition = new FightByFreeCondition(m_player, type);
            continue;
          case 15:
            FightByGuildCondition byGuildCondition = new FightByGuildCondition(m_player, type);
            continue;
          case 17:
            FightByGuildSpanAreaCondition spanAreaCondition1 = new FightByGuildSpanAreaCondition(m_player, type);
            continue;
          case 18:
            MarryApplyReplyCondition applyReplyCondition = new MarryApplyReplyCondition(m_player, type);
            continue;
          case 19:
            GameKillByGameCondition killByGameCondition = new GameKillByGameCondition(m_player, type);
            continue;
          case 20:
            FightDispatchesCondition dispatchesCondition = new FightDispatchesCondition(m_player, type);
            continue;
          case 21:
            QuestBlueCondition questBlueCondition = new QuestBlueCondition(m_player, type);
            continue;
          case 22:
            QuestDailyCondition questDailyCondition = new QuestDailyCondition(m_player, type);
            continue;
          case 23:
            PlayerGoodsPresentCondition presentCondition = new PlayerGoodsPresentCondition(m_player, type);
            continue;
          case 24:
            AddRichesOfferCondition richesOfferCondition = new AddRichesOfferCondition(m_player, type);
            continue;
          case 25:
            AddRichesRobCondition richesRobCondition = new AddRichesRobCondition(m_player, type);
            continue;
          case 26:
            Mission1KillCondition mission1KillCondition = new Mission1KillCondition(m_player, type);
            continue;
          case 27:
            Mission2KillCondition mission2KillCondition = new Mission2KillCondition(m_player, type);
            continue;
          case 28:
            Mission1OverCondition mission1OverCondition = new Mission1OverCondition(m_player, type);
            continue;
          case 29:
            Mission2OverCondition mission2OverCondition = new Mission2OverCondition(m_player, type);
            continue;
          case 30:
            Mission8OverCondition mission8OverCondition = new Mission8OverCondition(m_player, type);
            continue;
          case 31:
            Mission3KillCondition mission3KillCondition = new Mission3KillCondition(m_player, type);
            continue;
          case 32:
            ItemStrengthenCondition strengthenCondition1 = new ItemStrengthenCondition(m_player, type);
            continue;
          case 33:
            HotSpringCondition hotSpringCondition = new HotSpringCondition(m_player, type);
            continue;
          case 34:
            UsingIgnoreArmorCondition ignoreArmorCondition = new UsingIgnoreArmorCondition(m_player, type);
            continue;
          case 35:
            UsingAtomicBombCondition atomicBombCondition = new UsingAtomicBombCondition(m_player, type);
            continue;
          case 36:
            ChangeColorsCondition changeColorsCondition = new ChangeColorsCondition(m_player, type);
            continue;
          case 37:
            PlayerLoginCondition playerLoginCondition = new PlayerLoginCondition(m_player, type);
            continue;
          case 38:
            AddGoldCondition addGoldCondition = new AddGoldCondition(m_player, type);
            continue;
          case 39:
            AddGiftTokenCondition giftTokenCondition = new AddGiftTokenCondition(m_player, type);
            continue;
          case 40:
            AddMedalCondition addMedalCondition = new AddMedalCondition(m_player, type);
            continue;
          case 41:
            FightOneBloodIsWinCondition bloodIsWinCondition = new FightOneBloodIsWinCondition(m_player, type);
            continue;
          case 42:
            UsingSecondWeaponTrueAngelCondition trueAngelCondition = new UsingSecondWeaponTrueAngelCondition(m_player, type);
            continue;
          case 43:
            UsingGEMCondition usingGemCondition = new UsingGEMCondition(m_player, type);
            continue;
          case 44:
            UsingRenameCardCondition renameCardCondition = new UsingRenameCardCondition(m_player, type);
            continue;
          case 45:
            UsingSalutingGunCondition salutingGunCondition = new UsingSalutingGunCondition(m_player, type);
            continue;
          case 46:
            UsingSpanAreaBugleCondition areaBugleCondition = new UsingSpanAreaBugleCondition(m_player, type);
            continue;
          case 47:
            UsingBigBugleCondition bigBugleCondition = new UsingBigBugleCondition(m_player, type);
            continue;
          case 48:
            UsingSmallBugleCondition smallBugleCondition = new UsingSmallBugleCondition(m_player, type);
            continue;
          case 49:
            UsingEngagementRingCondition engagementRingCondition = new UsingEngagementRingCondition(m_player, type);
            continue;
          case 50:
            FightAddOfferCondition addOfferCondition = new FightAddOfferCondition(m_player, type);
            continue;
          case 51:
            FightCoupleCondition fightCoupleCondition = new FightCoupleCondition(m_player, type);
            continue;
          case 52:
            Mission3OverCondition mission3OverCondition = new Mission3OverCondition(m_player, type);
            continue;
          case 53:
            Mission4OverCondition mission4OverCondition = new Mission4OverCondition(m_player, type);
            continue;
          case 54:
            Mission5OverCondition mission5OverCondition = new Mission5OverCondition(m_player, type);
            continue;
          case 55:
            Mission6OverCondition mission6OverCondition = new Mission6OverCondition(m_player, type);
            continue;
          case 56:
            Mission7OverCondition mission7OverCondition = new Mission7OverCondition(m_player, type);
            continue;
          case 57:
            ItemStrengthenCondition strengthenCondition2 = new ItemStrengthenCondition(m_player, type);
            continue;
          case 58:
            UsingSuperWeaponCondition superWeaponCondition = new UsingSuperWeaponCondition(m_player, type);
            continue;
          case 59:
            QuestGoodManCardCondition manCardCondition = new QuestGoodManCardCondition(m_player, type);
            continue;
          case 60:
            MissionKillTerrorKingCondition terrorKingCondition = new MissionKillTerrorKingCondition(m_player, type);
            continue;
          case 61:
            MissionKillTerrorBoguCondition terrorBoguCondition = new MissionKillTerrorBoguCondition(m_player, type);
            continue;
          case 64:
            FightWithWeaponCondition withWeaponCondition1 = new FightWithWeaponCondition(m_player, type);
            continue;
          case 65:
            FightWithWeaponCondition withWeaponCondition2 = new FightWithWeaponCondition(m_player, type);
            continue;
          case 66:
            FightWithWeaponCondition withWeaponCondition3 = new FightWithWeaponCondition(m_player, type);
            continue;
          case 67:
            FightWithWeaponCondition withWeaponCondition4 = new FightWithWeaponCondition(m_player, type);
            continue;
          case 68:
            FightWithWeaponCondition withWeaponCondition5 = new FightWithWeaponCondition(m_player, type);
            continue;
          case 69:
            FightWithWeaponCondition withWeaponCondition6 = new FightWithWeaponCondition(m_player, type);
            continue;
          case 70:
            FightWithWeaponCondition withWeaponCondition7 = new FightWithWeaponCondition(m_player, type);
            continue;
          case 71:
            FightWithWeaponCondition withWeaponCondition8 = new FightWithWeaponCondition(m_player, type);
            continue;
          case 72:
            FightWithWeaponCondition withWeaponCondition9 = new FightWithWeaponCondition(m_player, type);
            continue;
          case 73:
            FightByFreeSpanAreaCondition spanAreaCondition2 = new FightByFreeSpanAreaCondition(m_player, type);
            continue;
          case 74:
            VIPCondition vipCondition = new VIPCondition(m_player, type);
            continue;
          case 75:
            GetApprenticeCondition apprenticeCondition = new GetApprenticeCondition(m_player, type);
            continue;
          case 76:
            ApprenticeCompleteCondition completeCondition1 = new ApprenticeCompleteCondition(m_player, type);
            continue;
          case 77:
            GetMasterCondition getMasterCondition = new GetMasterCondition(m_player, type);
            continue;
          case 78:
            MasterCompleteCondition completeCondition2 = new MasterCompleteCondition(m_player, type);
            continue;
          case 79:
            Mission5KillCondition mission5KillCondition = new Mission5KillCondition(m_player, type);
            continue;
          case 80:
            Mission10OverCondition mission10OverCondition = new Mission10OverCondition(m_player, type);
            continue;
          case 81:
            MissionKillMekaCondition killMekaCondition = new MissionKillMekaCondition(m_player, type);
            continue;
          case 82:
            Mission11OverCondition mission11OverCondition = new Mission11OverCondition(m_player, type);
            continue;
          case 88:
            StartMissionCondition missionCondition = new StartMissionCondition(m_player, type);
            continue;
          case 89:
            MissionKillBatosCondition killBatosCondition = new MissionKillBatosCondition(m_player, type);
            continue;
          case 90:
            Mission12OverCondition mission12OverCondition = new Mission12OverCondition(m_player, type);
            continue;
          case 94:
            FightLabPrimaryCondition primaryCondition = new FightLabPrimaryCondition(m_player, type);
            continue;
          default:
            continue;
        }
      }
    }
  }
}
