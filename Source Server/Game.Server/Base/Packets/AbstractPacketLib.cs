using Bussiness;
using Bussiness.Managers;
using Game.Server;
using Game.Server.Buffer;
using Game.Server.ConsortiaTask;
using Game.Server.GameObjects;
using Game.Server.GameUtils;
using Game.Server.Managers;
using Game.Server.Packets;
using Game.Server.Quests;
using Game.Server.Rooms;
using Game.Server.SceneMarryRooms;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Base.Packets
{
    [PacketLib(1)]
    public class AbstractPacketLib : IPacketLib
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected readonly GameClient m_gameClient;

        public AbstractPacketLib(GameClient client)
        {
            this.m_gameClient = client;
        }

        public static IPacketLib CreatePacketLibForVersion(int rawVersion, GameClient client)
        {
            foreach (Type derivedClass in ScriptMgr.GetDerivedClasses(typeof(IPacketLib)))
            {
                foreach (PacketLibAttribute customAttribute in derivedClass.GetCustomAttributes(typeof(PacketLibAttribute), false))
                {
                    if (customAttribute.RawVersion == rawVersion)
                    {
                        try
                        {
                            return (IPacketLib)Activator.CreateInstance(derivedClass, (object)client);
                        }
                        catch (Exception ex)
                        {
                            if (AbstractPacketLib.log.IsErrorEnabled)
                                AbstractPacketLib.log.Error((object)("error creating packetlib (" + derivedClass.FullName + ") for raw version " + (object)rawVersion), ex);
                        }
                    }
                }
            }
            return (IPacketLib)null;
        }

        public void SendTCP(GSPacketIn packet)
        {
            this.m_gameClient.SendTCP(packet);
        }

        public void SendAcademyGradute(GamePlayer app, int type)
        {
            GSPacketIn packet = new GSPacketIn((short)141);
            packet.WriteByte((byte)11);
            packet.WriteInt(type);
            packet.WriteInt(app.PlayerId);
            packet.WriteString(app.PlayerCharacter.NickName);
            this.SendTCP(packet);
        }

        public GSPacketIn SendAcademySystemNotice(string text, bool isAlert)
        {
            GSPacketIn packet = new GSPacketIn((short)141);
            packet.WriteByte((byte)17);
            packet.WriteString(text);
            packet.WriteBoolean(isAlert);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendAcademyAppState(PlayerInfo player, int removeUserId)
        {
            GSPacketIn packet = new GSPacketIn((short)141);
            packet.WriteByte((byte)10);
            packet.WriteInt(player.apprenticeshipState);
            packet.WriteInt(player.masterID);
            packet.WriteString(player.masterOrApprentices);
            packet.WriteInt(removeUserId);
            packet.WriteInt(player.graduatesCount);
            packet.WriteString(player.honourOfMaster);
            packet.WriteDateTime(player.freezesDate);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendConsortiaTaskInfo(BaseConsortiaTask baseTask)
        {
            GSPacketIn packet = new GSPacketIn((short)129);
            packet.WriteByte((byte)22);
            packet.WriteByte((byte)3);
            if (baseTask != null)
            {
                packet.WriteInt(baseTask.ConditionList.Count);
                foreach (KeyValuePair<int, ConsortiaTaskInfo> condition in baseTask.ConditionList)
                {
                    packet.WriteInt(condition.Key);
                    packet.WriteInt(3);
                    packet.WriteString(condition.Value.CondictionTitle);
                    packet.WriteInt(baseTask.GetTotalValueByConditionPlace(condition.Key));
                    packet.WriteInt(condition.Value.Para2);
                    packet.WriteInt(baseTask.GetValueByConditionPlace(this.m_gameClient.Player.PlayerCharacter.ID, condition.Key));
                }
                packet.WriteInt(baseTask.Info.TotalExp);
                packet.WriteInt(baseTask.Info.TotalOffer);
                packet.WriteInt(baseTask.Info.TotalRiches);
                packet.WriteInt(baseTask.Info.BuffID);
                packet.WriteDateTime(baseTask.Info.StartTime);
                packet.WriteInt(baseTask.Info.VaildDate);
            }
            else
            {
                packet.WriteInt(0);
                packet.WriteInt(0);
                packet.WriteInt(0);
                packet.WriteInt(0);
                packet.WriteInt(0);
                packet.WriteDateTime(DateTime.Now);
                packet.WriteInt(0);
            }
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendSystemConsortiaChat(string content, bool sendToSelf)
        {
            GSPacketIn packet = new GSPacketIn((short)129);
            packet.WriteByte((byte)20);
            packet.WriteByte((byte)0);
            packet.WriteString("");
            packet.WriteString(content);
            if (sendToSelf)
                this.SendTCP(packet);
            return packet;
        }

        public void SendShopGoodsCountUpdate(List<ShopFreeCountInfo> list)
        {
            GSPacketIn packet = new GSPacketIn((short)168);
            packet.WriteInt(list.Count);
            foreach (ShopFreeCountInfo shopFreeCountInfo in list)
            {
                packet.WriteInt(shopFreeCountInfo.ShopID);
                packet.WriteInt(shopFreeCountInfo.Count);
            }
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            this.SendTCP(packet);
        }

        public void SendEliteGameStartRoom()
        {
            GSPacketIn packet = new GSPacketIn((short)162);
            packet.WriteByte((byte)2);
            this.SendTCP(packet);
        }

        public void SendEliteGameInfo(int type)
        {
            GSPacketIn packet = new GSPacketIn((short)162);
            packet.WriteByte((byte)1);
            packet.WriteInt(type);
            this.SendTCP(packet);
        }

        public GSPacketIn SendLabyrinthUpdataInfo(int ID, UserLabyrinthInfo laby)
        {
            GSPacketIn packet = new GSPacketIn((short)131, ID);
            packet.WriteByte((byte)2);
            packet.WriteInt(laby.myProgress);
            packet.WriteInt(laby.currentFloor);
            packet.WriteBoolean(laby.completeChallenge);
            packet.WriteInt(laby.remainTime);
            packet.WriteInt(laby.accumulateExp);
            packet.WriteInt(laby.cleanOutAllTime);
            packet.WriteInt(laby.cleanOutGold);
            packet.WriteInt(laby.myRanking);
            packet.WriteBoolean(laby.isDoubleAward);
            packet.WriteBoolean(laby.isInGame);
            packet.WriteBoolean(laby.isCleanOut);
            packet.WriteBoolean(laby.serverMultiplyingPower);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendUpdateUserPet(PetInventory bag, int[] slots)//#3 For EatPets
        {
            if (this.m_gameClient.Player == null)
                return (GSPacketIn)null;
            GSPacketIn packet = new GSPacketIn((short)68, this.m_gameClient.Player.PlayerId);
            packet.WriteByte((byte)1);
            packet.WriteInt(this.m_gameClient.Player.PlayerId);
            packet.WriteInt(this.m_gameClient.Player.ZoneId);
            packet.WriteInt(slots.Length);
            for (int index = 0; index < slots.Length; ++index)
            {
                int slot = slots[index];
                packet.WriteInt(slot);
                UsersPetInfo petAt = bag.GetPetAt(slot);
                if (petAt == null)
                {
                    packet.WriteBoolean(false);
                }
                else
                {
                    packet.WriteBoolean(true);
                    packet.WriteInt(petAt.ID);
                    packet.WriteInt(petAt.TemplateID);
                    packet.WriteString(petAt.Name);
                    packet.WriteInt(petAt.UserID);
                    packet.WriteInt(petAt.TotalAttack);
                    packet.WriteInt(petAt.TotalDefence);
                    packet.WriteInt(petAt.TotalLuck);
                    packet.WriteInt(petAt.TotalAgility);
                    packet.WriteInt(petAt.TotalBlood);
                    packet.WriteInt(petAt.TotalDamage);
                    packet.WriteInt(petAt.TotalGuard);
                    packet.WriteInt(petAt.AttackGrow);
                    packet.WriteInt(petAt.DefenceGrow);
                    packet.WriteInt(petAt.LuckGrow);
                    packet.WriteInt(petAt.AgilityGrow);
                    packet.WriteInt(petAt.BloodGrow);
                    packet.WriteInt(petAt.DamageGrow);
                    packet.WriteInt(petAt.GuardGrow);
                    packet.WriteInt(petAt.Level);
                    packet.WriteInt(petAt.GP);
                    packet.WriteInt(petAt.MaxGP);
                    packet.WriteInt(petAt.Hunger);
                    packet.WriteInt(petAt.PetHappyStar);
                    packet.WriteInt(petAt.MP);
                    List<string> skill = petAt.GetSkill();
                    packet.WriteInt(skill.Count);
                    foreach (string str in skill)
                    {
                        packet.WriteInt(int.Parse(str.Split(',')[0]));
                        packet.WriteInt(int.Parse(str.Split(',')[1]));
                    }
                    List<string> skillEquip = petAt.GetSkillEquip();
                    packet.WriteInt(skillEquip.Count);
                    foreach (string str in skillEquip)
                    {
                        packet.WriteInt(int.Parse(str.Split(',')[1]));
                        packet.WriteInt(int.Parse(str.Split(',')[0]));
                    }
                    packet.WriteBoolean(petAt.IsEquip);
                    packet.WriteInt(petAt.PetEquips.Count);
                    foreach (PetEquipInfo petEquip in petAt.PetEquips)
                    {
                        packet.WriteInt(petEquip.eqType);
                        packet.WriteInt(petEquip.eqTemplateID);
                        packet.WriteDateTime(petEquip.startTime);
                        packet.WriteInt(petEquip.ValidDate);
                    }
                    packet.WriteInt(petAt.currentStarExp);
                }
            }
            packet.WriteInt(this.m_gameClient.Player.PetBag.EatPets.weaponLevel);
            packet.WriteInt(this.m_gameClient.Player.PetBag.EatPets.clothesLevel);
            packet.WriteInt(this.m_gameClient.Player.PetBag.EatPets.hatLevel);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendPetInfo(int id, int zoneId, UsersPetInfo[] pets, EatPetsInfo eatpet) //#2 For EatPets
        {
            GSPacketIn packet = new GSPacketIn((short)68, id);
            packet.WriteByte((byte)1);
            packet.WriteInt(id);
            packet.WriteInt(zoneId);
            packet.WriteInt(pets.Length);
            for (int index = 0; index < pets.Length; ++index)
            {
                UsersPetInfo pet = pets[index];
                packet.WriteInt(pet.Place);
                packet.WriteBoolean(true);
                packet.WriteInt(pet.ID);
                packet.WriteInt(pet.TemplateID);
                packet.WriteString(pet.Name);
                packet.WriteInt(pet.UserID);
                packet.WriteInt(pet.TotalAttack);
                packet.WriteInt(pet.TotalDamage);
                packet.WriteInt(pet.TotalLuck);
                packet.WriteInt(pet.TotalAgility);
                packet.WriteInt(pet.TotalBlood);
                packet.WriteInt(pet.TotalDamage);
                packet.WriteInt(pet.TotalGuard);
                packet.WriteInt(pet.AttackGrow);
                packet.WriteInt(pet.DefenceGrow);
                packet.WriteInt(pet.LuckGrow);
                packet.WriteInt(pet.AgilityGrow);
                packet.WriteInt(pet.BloodGrow);
                packet.WriteInt(pet.DamageGrow);
                packet.WriteInt(pet.GuardGrow);
                packet.WriteInt(pet.Level);
                packet.WriteInt(pet.GP);
                packet.WriteInt(pet.MaxGP);
                packet.WriteInt(pet.Hunger);
                packet.WriteInt(pet.PetHappyStar);
                packet.WriteInt(pet.MP);
                List<string> skill = pet.GetSkill();
                List<string> skillEquip = pet.GetSkillEquip();
                packet.WriteInt(skill.Count);
                foreach (string str in skill)
                {
                    packet.WriteInt(int.Parse(str.Split(',')[0]));
                    packet.WriteInt(int.Parse(str.Split(',')[1]));
                }
                packet.WriteInt(skillEquip.Count);
                foreach (string str in skillEquip)
                {
                    packet.WriteInt(int.Parse(str.Split(',')[1]));
                    packet.WriteInt(int.Parse(str.Split(',')[0]));
                }
                packet.WriteBoolean(pet.IsEquip);
                packet.WriteInt(pet.PetEquips.Count);
                foreach (PetEquipInfo petEquip in pet.PetEquips)
                {
                    packet.WriteInt(petEquip.eqType);
                    packet.WriteInt(petEquip.eqTemplateID);
                    packet.WriteDateTime(petEquip.startTime);
                    packet.WriteInt(petEquip.ValidDate);
                }
                packet.WriteInt(pet.currentStarExp);
            }
            packet.WriteInt((eatpet == null ? 0 : eatpet.weaponLevel));
            packet.WriteInt((eatpet == null ? 0 : eatpet.clothesLevel));
            packet.WriteInt((eatpet == null ? 0 : eatpet.hatLevel));
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn sendBuyBadge(
          int consortiaID,
          int BadgeID,
          int ValidDate,
          bool result,
          string BadgeBuyTime,
          int playerid)
        {
            GSPacketIn packet = new GSPacketIn((short)129, playerid);
            packet.WriteByte((byte)28);
            packet.WriteInt(consortiaID);
            packet.WriteInt(BadgeID);
            packet.WriteInt(ValidDate);
            packet.WriteDateTime(Convert.ToDateTime(BadgeBuyTime));
            packet.WriteBoolean(result);
            this.SendTCP(packet);
            return packet;
        }

        public void SendEdictumVersion()
        {
            EdictumInfo[] allEdictumVersion = WorldMgr.GetAllEdictumVersion();
            Random random = new Random();
            if (allEdictumVersion.Length == 0)
                return;
            GSPacketIn packet = new GSPacketIn((short)75);
            packet.WriteInt(allEdictumVersion.Length);
            foreach (EdictumInfo edictumInfo in allEdictumVersion)
                packet.WriteInt(edictumInfo.ID + random.Next(10000));
            this.SendTCP(packet);
        }

        public void SendLeftRouleteOpen(UsersExtraInfo info)
        {
            GSPacketIn gSPacketIn = new GSPacketIn((byte)ePackageType.LEFT_GUN_ROULETTE);
            gSPacketIn.WriteInt(1);
            gSPacketIn.WriteInt(1);
            gSPacketIn.WriteBoolean(true);
            gSPacketIn.WriteInt((!(info.LeftRoutteRate > 0f)) ? info.LeftRoutteCount : 1);
            gSPacketIn.WriteString($"{info.LeftRoutteRate:N1}");
            string leftRouterRateData = GameProperties.LeftRouterRateData;
            for (int i = 0; i < leftRouterRateData.Length; i++)
            {
                char c = leftRouterRateData[i];
                if (c != '.' && c != '|')
                {
                    gSPacketIn.WriteInt(int.Parse(c.ToString()));
                }
                else
                {
                    gSPacketIn.WriteInt(0);
                }
            }
            SendTCP(gSPacketIn);
        }

        public void SendLeftRouleteResult(UsersExtraInfo info)
        {
            GSPacketIn packet = new GSPacketIn((short)163);
            packet.WriteInt((double)info.LeftRoutteRate > 0.0 ? 0 : info.LeftRoutteCount);
            packet.WriteString(string.Format("{0:N1}", (object)info.LeftRoutteRate));
            this.SendTCP(packet);
        }

        public void SendEnthrallLight()
        {
            GSPacketIn packet = new GSPacketIn((short)227);
            packet.WriteBoolean(false);
            packet.WriteInt(0);
            packet.WriteBoolean(false);
            packet.WriteBoolean(false);
            this.SendTCP(packet);
        }

        public void SendLittleGameActived()
        {
            GSPacketIn pkg = new GSPacketIn((short)ePackageType.LITTLEGAME_ACTIVED);
            pkg.WriteBoolean(true);
            this.SendTCP(pkg);
        }

        public void SendLoginFailed(string msg)
        {
            GSPacketIn packet = new GSPacketIn((short)1);
            packet.WriteByte((byte)1);
            packet.WriteString(msg);
            this.SendTCP(packet);
        }

        public void SendOpenNoviceActive(
          int channel,
          int activeId,
          int condition,
          int awardGot,
          DateTime startTime,
          DateTime endTime)
        {
            GSPacketIn packet = new GSPacketIn((short)258);
            packet.WriteInt(channel);
            if ((uint)channel > 0U)
            {
                if (channel == 1)
                    packet.WriteBoolean(false);
            }
            else
            {
                packet.WriteInt(activeId);
                packet.WriteInt(condition);
                packet.WriteInt(awardGot);
                packet.WriteDateTime(startTime);
                packet.WriteDateTime(endTime);
            }
            this.SendTCP(packet);
        }

        public void SendUpdateFirstRecharge(bool isRecharge, bool isGetAward)
        {
            GSPacketIn packet = new GSPacketIn((short)ePackageType.FIRSTRECHARGE);
            packet.WriteBoolean(isRecharge);
            packet.WriteBoolean(isGetAward);
            this.SendTCP(packet);
        }

        public GSPacketIn sendBuyBadge(
          int BadgeID,
          int ValidDate,
          bool result,
          string BadgeBuyTime,
          int playerid)
        {
            GSPacketIn packet = new GSPacketIn((short)164, playerid);
            packet.WriteInt(BadgeID);
            packet.WriteInt(BadgeID);
            packet.WriteInt(ValidDate);
            packet.WriteDateTime(Convert.ToDateTime(BadgeBuyTime));
            packet.WriteBoolean(result);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendOpenTimeBox(int condtion, bool isSuccess)
        {
            GSPacketIn packet = new GSPacketIn((short)53);
            packet.WriteBoolean(isSuccess);
            packet.WriteInt(condtion);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendConsortiaMail(bool result, int playerid)
        {
            GSPacketIn packet = new GSPacketIn((short)129, playerid);
            packet.WriteByte(29);// new add
            packet.WriteBoolean(result);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendAddFriend(PlayerInfo user, int relation, bool state)
        {
            GSPacketIn packet = new GSPacketIn((short)160, user.ID);
            packet.WriteByte((byte)160);
            packet.WriteBoolean(state);
            if (state)
            {
                packet.WriteInt(user.ID);
                packet.WriteString(user.NickName);
                packet.WriteByte(user.typeVIP);
                packet.WriteInt(user.VIPLevel);
                packet.WriteBoolean(user.Sex);
                packet.WriteString(user.Style);
                packet.WriteString(user.Colors);
                packet.WriteString(user.Skin);
                packet.WriteInt(user.State == 1 ? 1 : 0);
                packet.WriteInt(user.Grade);
                packet.WriteInt(user.Hide);
                packet.WriteString(user.ConsortiaName);
                packet.WriteInt(user.Total);
                packet.WriteInt(user.Escape);
                packet.WriteInt(user.Win);
                packet.WriteInt(user.Offer);
                packet.WriteInt(user.Repute);
                packet.WriteInt(relation);
                packet.WriteString(user.UserName);
                packet.WriteInt(user.Nimbus);
                packet.WriteInt(user.FightPower);
                packet.WriteInt(user.apprenticeshipState);
                packet.WriteInt(user.masterID);
                packet.WriteString(user.masterOrApprentices);
                packet.WriteInt(user.graduatesCount);
                packet.WriteString(user.honourOfMaster);
                packet.WriteInt(user.AchievementPoint);
                packet.WriteString(user.Honor);
                packet.WriteBoolean(user.IsMarried);
            }
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendFriendRemove(int FriendID)
        {
            GSPacketIn packet = new GSPacketIn((short)160, FriendID);
            packet.WriteByte((byte)161);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendFriendState(
          int playerID,
          int state,
          byte typeVip,
          int viplevel)
        {
            GSPacketIn packet = new GSPacketIn((short)160, playerID);
            packet.WriteByte((byte)165);
            packet.WriteInt(state);
            packet.WriteInt((int)typeVip);
            packet.WriteInt(viplevel);
            packet.WriteBoolean(true);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn sendOneOnOneTalk(
          int receiverID,
          bool isAutoReply,
          string SenderNickName,
          string msg,
          int playerid)
        {
            GSPacketIn packet = new GSPacketIn((short)160, playerid);
            packet.WriteByte((byte)51);
            packet.WriteInt(receiverID);
            packet.WriteString(SenderNickName);
            packet.WriteDateTime(DateTime.Now);
            packet.WriteString(msg);
            packet.WriteBoolean(isAutoReply);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendUpdateConsotiaBoss(ConsortiaBossInfo bossInfo)
        {
            GSPacketIn packet = new GSPacketIn((short)162);
            packet.WriteByte((byte)bossInfo.typeBoss);
            packet.WriteInt(bossInfo.powerPoint);
            packet.WriteInt(bossInfo.callBossCount);
            packet.WriteDateTime(bossInfo.BossOpenTime);
            packet.WriteInt(bossInfo.BossLevel);
            packet.WriteBoolean(false);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendUpdateConsotiaBuffer(
          GamePlayer player,
          Dictionary<string, BufferInfo> bufflist)
        {
            List<ConsortiaBuffTempInfo> allConsortiaBuff = ConsortiaExtraMgr.GetAllConsortiaBuff();
            GSPacketIn packet = new GSPacketIn((short)129, player.PlayerId);
            packet.WriteByte((byte)26);
            packet.WriteInt(allConsortiaBuff.Count);
            foreach (ConsortiaBuffTempInfo consortiaBuffTempInfo in allConsortiaBuff)
            {
                if (bufflist.ContainsKey(consortiaBuffTempInfo.id.ToString()))
                {
                    BufferInfo bufferInfo = bufflist[consortiaBuffTempInfo.id.ToString()];
                    packet.WriteInt(consortiaBuffTempInfo.id);
                    packet.WriteBoolean(true);
                    packet.WriteDateTime(bufferInfo.BeginDate);
                    packet.WriteInt(bufferInfo.ValidDate / 24 / 60);
                }
                else
                {
                    packet.WriteInt(consortiaBuffTempInfo.id);
                    packet.WriteBoolean(false);
                    packet.WriteDateTime(DateTime.Now);
                    packet.WriteInt(0);
                }
            }
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendPlayerDrill(int ID, Dictionary<int, UserDrillInfo> drills)
        {
            GSPacketIn packet = new GSPacketIn((short)121, ID);
            packet.WriteByte((byte)6);
            packet.WriteInt(ID);
            packet.WriteInt(drills[0].HoleExp);
            packet.WriteInt(drills[1].HoleExp);
            packet.WriteInt(drills[2].HoleExp);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(drills[0].HoleLv);
            packet.WriteInt(drills[1].HoleLv);
            packet.WriteInt(drills[2].HoleLv);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendUpdateAchievementData(List<AchievementDataInfo> infos)
        {
            bool flag;
            if (infos != null)
            {
                int id = this.m_gameClient.Player.PlayerCharacter.ID;
                flag = true;
            }
            else
                flag = false;
            GSPacketIn gsPacketIn1;
            if (!flag)
            {
                gsPacketIn1 = (GSPacketIn)null;
            }
            else
            {
                GSPacketIn packet = new GSPacketIn((short)231, this.m_gameClient.Player.PlayerCharacter.ID);
                packet.WriteInt(infos.Count);
                for (int index = 0; index < infos.Count; ++index)
                {
                    AchievementDataInfo info = infos[index];
                    packet.WriteInt(info.AchievementID);
                    GSPacketIn gsPacketIn2 = packet;
                    DateTime completedDate = info.CompletedDate;
                    int year = completedDate.Year;
                    gsPacketIn2.WriteInt(year);
                    GSPacketIn gsPacketIn3 = packet;
                    completedDate = info.CompletedDate;
                    int month = completedDate.Month;
                    gsPacketIn3.WriteInt(month);
                    GSPacketIn gsPacketIn4 = packet;
                    completedDate = info.CompletedDate;
                    int day = completedDate.Day;
                    gsPacketIn4.WriteInt(day);
                }
                this.SendTCP(packet);
                gsPacketIn1 = packet;
            }
            return gsPacketIn1;
        }

        public GSPacketIn SendAchievementSuccess(AchievementDataInfo d)
        {
            GSPacketIn packet = new GSPacketIn((short)230);
            packet.WriteInt(d.AchievementID);
            packet.WriteInt(d.CompletedDate.Year);
            packet.WriteInt(d.CompletedDate.Month);
            packet.WriteInt(d.CompletedDate.Day);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendUpdateAchievements(List<UsersRecordInfo> infos)
        {
            bool flag;
            if (infos != null && this.m_gameClient != null && this.m_gameClient.Player != null)
            {
                int id = this.m_gameClient.Player.PlayerCharacter.ID;
                flag = true;
            }
            else
                flag = false;
            GSPacketIn gsPacketIn;
            if (!flag)
            {
                gsPacketIn = (GSPacketIn)null;
            }
            else
            {
                GSPacketIn packet = new GSPacketIn((short)229, this.m_gameClient.Player.PlayerCharacter.ID);
                packet.WriteInt(infos.Count);
                for (int index = 0; index < infos.Count; ++index)
                {
                    UsersRecordInfo info = infos[index];
                    packet.WriteInt(info.RecordID);
                    packet.WriteInt(info.Total);
                }
                this.SendTCP(packet);
                gsPacketIn = packet;
            }
            return gsPacketIn;
        }

        public GSPacketIn SendUpdateAchievements(UsersRecordInfo info)
        {
            bool flag;
            if (info != null && this.m_gameClient != null && this.m_gameClient.Player != null)
            {
                int id = this.m_gameClient.Player.PlayerCharacter.ID;
                flag = true;
            }
            else
                flag = false;
            GSPacketIn gsPacketIn;
            if (!flag)
            {
                gsPacketIn = (GSPacketIn)null;
            }
            else
            {
                GSPacketIn packet = new GSPacketIn((short)229, this.m_gameClient.Player.PlayerCharacter.ID);
                packet.WriteInt(1);
                for (int index = 0; index < 1; ++index)
                {
                    packet.WriteInt(info.RecordID);
                    packet.WriteInt(info.Total);
                }
                this.SendTCP(packet);
                gsPacketIn = packet;
            }
            return gsPacketIn;
        }

        public GSPacketIn SendInitAchievements(List<UsersRecordInfo> infos)
        {
            bool flag;
            if (infos != null && this.m_gameClient.Player != null)
            {
                int id = this.m_gameClient.Player.PlayerCharacter.ID;
                flag = true;
            }
            else
                flag = false;
            GSPacketIn gsPacketIn;
            if (!flag)
            {
                gsPacketIn = (GSPacketIn)null;
            }
            else
            {
                GSPacketIn packet = new GSPacketIn((short)228, this.m_gameClient.Player.PlayerCharacter.ID);
                packet.WriteInt(infos.Count);
                for (int index = 0; index < infos.Count; ++index)
                {
                    UsersRecordInfo info = infos[index];
                    packet.WriteInt(info.RecordID);
                    packet.WriteInt(info.Total);
                }
                this.SendTCP(packet);
                this.SendUpdateAchievements(infos);
                gsPacketIn = packet;
            }
            return gsPacketIn;
        }

        public void SendLoginSuccess()
        {
            if (this.m_gameClient.Player == null)
                return;
            GSPacketIn packet = new GSPacketIn((short)1, this.m_gameClient.Player.PlayerCharacter.ID);
            packet.WriteByte((byte)0);
            packet.WriteInt(this.m_gameClient.Player.ZoneId);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Attack);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Defence);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Agility);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Luck);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.GP);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Repute);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Gold);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Money + this.m_gameClient.Player.PlayerCharacter.MoneyLock);
            packet.WriteInt(this.m_gameClient.Player.GetMedalNum());
            packet.WriteInt(0);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Hide);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.FightPower);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.apprenticeshipState);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.masterID);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.masterOrApprentices);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.graduatesCount);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.honourOfMaster);
            packet.WriteDateTime(this.m_gameClient.Player.PlayerCharacter.freezesDate);
            packet.WriteByte(this.m_gameClient.Player.PlayerCharacter.typeVIP);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.VIPLevel);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.VIPExp);
            packet.WriteDateTime(this.m_gameClient.Player.PlayerCharacter.VIPExpireDay);
            packet.WriteDateTime(this.m_gameClient.Player.PlayerCharacter.LastDate);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.VIPNextLevelDaysNeeded);
            packet.WriteDateTime(DateTime.Now);
            packet.WriteBoolean(this.m_gameClient.Player.PlayerCharacter.CanTakeVipReward);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.OptionOnOff);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.AchievementPoint);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.Honor);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.OnlineTime);
            packet.WriteBoolean(this.m_gameClient.Player.PlayerCharacter.Sex);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.Style + "&" + this.m_gameClient.Player.PlayerCharacter.Colors);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.Skin);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.ConsortiaID);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.ConsortiaName);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.badgeID);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.DutyLevel);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.DutyName);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Right);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.ChairmanName);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.ConsortiaHonor);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.ConsortiaRiches);
            packet.WriteBoolean(this.m_gameClient.Player.PlayerCharacter.HasBagPassword);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.PasswordQuest1);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.PasswordQuest2);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.FailedPasswordAttemptCount);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.UserName);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Nimbus);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.PvePermission);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.FightLabPermission);
            packet.WriteInt(99999);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.BoxProgression);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.GetBoxLevel);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.AlreadyGetBox);
            packet.WriteDateTime(this.m_gameClient.Player.Extra.Info.LastTimeHotSpring);
            packet.WriteDateTime(this.m_gameClient.Player.PlayerCharacter.ShopFinallyGottenTime);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Riches);
            packet.WriteInt(this.m_gameClient.Player.MatchInfo.dailyScore);
            packet.WriteInt(this.m_gameClient.Player.MatchInfo.dailyWinCount);
            packet.WriteInt(this.m_gameClient.Player.MatchInfo.dailyGameCount);
            packet.WriteBoolean(this.m_gameClient.Player.MatchInfo.DailyLeagueFirst);
            packet.WriteInt(this.m_gameClient.Player.MatchInfo.DailyLeagueLastScore);
            packet.WriteInt(this.m_gameClient.Player.MatchInfo.weeklyScore);
            packet.WriteInt(this.m_gameClient.Player.MatchInfo.weeklyGameCount);
            packet.WriteInt(this.m_gameClient.Player.MatchInfo.weeklyRanking);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Texp.spdTexpExp);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Texp.attTexpExp);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Texp.defTexpExp);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Texp.hpTexpExp);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Texp.lukTexpExp);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Texp.texpTaskCount);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Texp.texpCount);
            packet.WriteDateTime(this.m_gameClient.Player.PlayerCharacter.Texp.texpTaskDate);
            packet.WriteBoolean(false);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.badLuckNumber);
            packet.WriteInt(0);
            packet.WriteDateTime(DateTime.Now);
            packet.WriteInt(0);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.totemId);
            this.SendTCP(packet);
        }

        public void SendLoginSuccess2()
        {
        }

        public void method_0(byte[] m, byte[] e)
        {
            GSPacketIn packet = new GSPacketIn((short)7);
            packet.Write(m);
            packet.Write(e);
            this.SendTCP(packet);
        }

        public void SendCheckCode()
        {
            if (this.m_gameClient.Player == null || this.m_gameClient.Player.PlayerCharacter.CheckCount < GameProperties.CHECK_MAX_FAILED_COUNT)
                return;
            if (this.m_gameClient.Player.PlayerCharacter.CheckError == 0)
                this.m_gameClient.Player.PlayerCharacter.CheckCount += 10000;
            GSPacketIn packet = new GSPacketIn((short)200, this.m_gameClient.Player.PlayerCharacter.ID, 10240);
            if (this.m_gameClient.Player.PlayerCharacter.CheckError < 1)
                packet.WriteByte((byte)0);
            else
                packet.WriteByte((byte)2);
            packet.WriteBoolean(true);
            this.m_gameClient.Player.PlayerCharacter.CheckCode = CheckCode.GenerateCheckCode();
            packet.Write(CheckCode.CreateImage(this.m_gameClient.Player.PlayerCharacter.CheckCode));
            this.SendTCP(packet);
        }

        public void SendKitoff(string msg)
        {
            GSPacketIn packet = new GSPacketIn((short)2);
            packet.WriteString(msg);
            this.SendTCP(packet);
        }

        public void SendEditionError(string msg)
        {
            GSPacketIn packet = new GSPacketIn((short)12);
            packet.WriteString(msg);
            this.SendTCP(packet);
        }

        public void SendWaitingRoom(bool result)
        {
            GSPacketIn packet = new GSPacketIn((short)16);
            packet.WriteByte(result ? (byte)1 : (byte)0);
            this.SendTCP(packet);
        }

        public GSPacketIn SendPlayerState(int id, byte state)
        {
            GSPacketIn packet = new GSPacketIn((short)32, id);
            packet.WriteByte(state);
            this.SendTCP(packet);
            return packet;
        }

        public virtual GSPacketIn SendMessage(eMessageType type, string message)
        {
            GSPacketIn packet = new GSPacketIn((short)3);
            packet.WriteInt((int)type);
            packet.WriteString(message);
            this.SendTCP(packet);
            return packet;
        }

        public void SendReady()
        {
            this.SendTCP(new GSPacketIn((short)0));
        }

        public void SendUpdatePrivateInfo(PlayerInfo info, int medal)
        {
            //private function __updatePrivateInfo
            GSPacketIn packet = new GSPacketIn((short)38, info.ID);
            packet.WriteInt(info.Money + info.MoneyLock);
            packet.WriteInt(medal);
            packet.WriteInt(19924);
            packet.WriteInt(info.Gold);
            packet.WriteInt(info.GiftToken);
            packet.WriteInt(info.badLuckNumber);
            packet.WriteInt(info.hardCurrency);
            packet.WriteInt(info.myHonor);
            packet.WriteInt(12467);//this._self.damageScores = param1.pkg.readInt();
            this.SendTCP(packet);
        }

        public GSPacketIn SendUpdatePublicPlayer(
          PlayerInfo info,
          UserMatchInfo matchInfo,
          UsersExtraInfo extraInfo)
        {
            GSPacketIn packet = new GSPacketIn((short)67, info.ID);
            packet.WriteInt(info.GP);
            packet.WriteInt(info.Offer);
            packet.WriteInt(info.RichesOffer);
            packet.WriteInt(info.RichesRob);
            packet.WriteInt(info.Win);
            packet.WriteInt(info.Total);
            packet.WriteInt(info.Escape);
            packet.WriteInt(info.Attack);
            packet.WriteInt(info.Defence);
            packet.WriteInt(info.Agility);
            packet.WriteInt(info.Luck);
            packet.WriteInt(info.hp);
            packet.WriteInt(info.Hide);
            packet.WriteString(info.Style);
            packet.WriteString(info.Colors);
            packet.WriteString(info.Skin);
            packet.WriteBoolean(info.IsShowConsortia);
            packet.WriteInt(info.ConsortiaID);
            packet.WriteString(info.ConsortiaName);
            packet.WriteInt(info.badgeID);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(info.Nimbus);
            packet.WriteString(info.PvePermission);
            packet.WriteString(info.FightLabPermission);
            packet.WriteInt(info.FightPower);
            packet.WriteInt(info.apprenticeshipState);
            packet.WriteInt(info.masterID);
            packet.WriteString(info.masterOrApprentices);
            packet.WriteInt(info.graduatesCount);
            packet.WriteString(info.honourOfMaster);
            packet.WriteInt(info.AchievementPoint);
            packet.WriteString(info.Honor);
            packet.WriteDateTime(info.LastSpaDate);
            packet.WriteInt(info.charmGP);
            packet.WriteInt(0);
            packet.WriteDateTime(info.ShopFinallyGottenTime);
            packet.WriteInt(info.Riches);
            packet.WriteInt(matchInfo.dailyScore);
            packet.WriteInt(matchInfo.dailyWinCount);
            packet.WriteInt(matchInfo.dailyGameCount);
            packet.WriteInt(matchInfo.weeklyScore);
            packet.WriteInt(matchInfo.weeklyGameCount);
            packet.WriteInt(info.Texp.spdTexpExp);
            packet.WriteInt(info.Texp.attTexpExp);
            packet.WriteInt(info.Texp.defTexpExp);
            packet.WriteInt(info.Texp.hpTexpExp);
            packet.WriteInt(info.Texp.lukTexpExp);
            packet.WriteInt(info.Texp.texpTaskCount);
            packet.WriteInt(info.Texp.texpCount);
            packet.WriteDateTime(info.Texp.texpTaskDate);
            packet.WriteInt(0);
            packet.WriteInt(info.evolutionGrade);
            packet.WriteInt(info.evolutionExp);
            this.SendTCP(packet);
            return packet;
        }

        public void SendPingTime(GamePlayer player)
        {
            GSPacketIn packet = new GSPacketIn((short)4);
            player.PingStart = DateTime.Now.Ticks;
            packet.WriteInt(player.PlayerCharacter.AntiAddiction);
            this.SendTCP(packet);
        }

        public GSPacketIn SendNetWork(int id, long delay)
        {
            GSPacketIn packet = new GSPacketIn((short)6, id);
            packet.WriteInt((int)delay / 1000 / 10);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendUserEquip(PlayerInfo player, List<SqlDataProvider.Data.ItemInfo> items, List<UserGemStone> userGemStone)
        {
            // private function __itemEquip(param1:CrazyTankSocketEvent)
            GSPacketIn packet = new GSPacketIn((short)0, player.ID);
            packet.WriteInt(player.ID);
            packet.WriteString(player.NickName);
            packet.WriteInt(player.Agility);
            packet.WriteInt(player.Attack);
            packet.WriteString(player.Colors);
            packet.WriteString(player.Skin);
            packet.WriteInt(player.Defence);
            packet.WriteInt(player.GP);
            packet.WriteInt(player.Grade);
            packet.WriteInt(player.Luck);
            packet.WriteInt(player.hp);
            packet.WriteInt(player.Hide);
            packet.WriteInt(player.Repute);
            packet.WriteBoolean(player.Sex);
            packet.WriteString(player.Style);
            packet.WriteInt(player.Offer);
            packet.WriteByte(player.typeVIP);
            packet.WriteInt(player.VIPLevel);
            packet.WriteInt(player.Win);
            packet.WriteInt(player.Total);
            packet.WriteInt(player.Escape);
            packet.WriteInt(player.ConsortiaID);
            packet.WriteString(player.ConsortiaName);
            packet.WriteInt(player.badgeID);
            packet.WriteInt(player.RichesOffer);
            packet.WriteInt(player.RichesRob);
            packet.WriteBoolean(player.IsMarried);
            packet.WriteInt(player.SpouseID);
            packet.WriteString(player.SpouseName);
            packet.WriteString(player.DutyName);
            packet.WriteInt(player.Nimbus);
            packet.WriteInt(player.FightPower);
            packet.WriteInt(player.apprenticeshipState);
            packet.WriteInt(player.masterID);
            packet.WriteString(player.masterOrApprentices);
            packet.WriteInt(player.graduatesCount);
            packet.WriteString(player.honourOfMaster);
            packet.WriteInt(player.AchievementPoint);
            packet.WriteString(player.Honor);
            packet.WriteDateTime(DateTime.Now.AddDays(-2.0));
            packet.WriteInt(player.Texp.spdTexpExp);
            packet.WriteInt(player.Texp.attTexpExp);
            packet.WriteInt(player.Texp.defTexpExp);
            packet.WriteInt(player.Texp.hpTexpExp);
            packet.WriteInt(player.Texp.lukTexpExp);
            packet.WriteBoolean(false);
            packet.WriteInt(0);
            packet.WriteInt(player.totemId);
            packet.WriteInt(items.Count);
            foreach (SqlDataProvider.Data.ItemInfo itemInfo in items)
            {
                packet.WriteByte((byte)itemInfo.BagType);
                packet.WriteInt(itemInfo.UserID);
                packet.WriteInt(itemInfo.ItemID);
                packet.WriteInt(itemInfo.Count);
                packet.WriteInt(itemInfo.Place);
                packet.WriteInt(itemInfo.TemplateID);
                packet.WriteInt(itemInfo.AttackCompose);
                packet.WriteInt(itemInfo.DefendCompose);
                packet.WriteInt(itemInfo.AgilityCompose);
                packet.WriteInt(itemInfo.LuckCompose);
                packet.WriteInt(itemInfo.StrengthenLevel);
                packet.WriteBoolean(itemInfo.IsBinds);
                packet.WriteBoolean(itemInfo.IsJudge);
                packet.WriteDateTime(itemInfo.BeginDate);
                packet.WriteInt(itemInfo.ValidDate);
                packet.WriteString(itemInfo.Color);
                packet.WriteString(itemInfo.Skin);
                packet.WriteBoolean(itemInfo.IsUsed);
                packet.WriteInt(itemInfo.Hole1);
                packet.WriteInt(itemInfo.Hole2);
                packet.WriteInt(itemInfo.Hole3);
                packet.WriteInt(itemInfo.Hole4);
                packet.WriteInt(itemInfo.Hole5);
                packet.WriteInt(itemInfo.Hole6);
                packet.WriteString(itemInfo.Pic);
                packet.WriteInt(itemInfo.RefineryLevel);
                packet.WriteDateTime(DateTime.Now);
                packet.WriteByte((byte)itemInfo.Hole5Level);
                packet.WriteInt(itemInfo.Hole5Exp);
                packet.WriteByte((byte)itemInfo.Hole6Level);
                packet.WriteInt(itemInfo.Hole6Exp);
                packet.WriteBoolean(itemInfo.isGold);
                if (itemInfo.isGold)
                {
                    packet.WriteInt(itemInfo.goldValidDate);
                    packet.WriteDateTime(itemInfo.goldBeginTime);
                }
            }
            packet.WriteInt(userGemStone.Count);
            for (int i = 0; i < userGemStone.Count; i++)
            {
                packet.WriteInt(userGemStone[i].FigSpiritId);
                packet.WriteString(userGemStone[i].FigSpiritIdValue);
                packet.WriteInt(userGemStone[i].EquipPlace);
            }
            packet.Compress();
            this.SendTCP(packet);
            return packet;
        }

        public void SendDateTime()
        {
            GSPacketIn packet = new GSPacketIn((short)5);
            packet.WriteDateTime(DateTime.Now);
            this.SendTCP(packet);
        }

        public GSPacketIn SendDailyAward(GamePlayer player)
        {
            bool val = false;
            if (DateTime.Now.Date != player.PlayerCharacter.LastAward.Date)
                val = true;
            GSPacketIn packet = new GSPacketIn((short)13);
            packet.WriteBoolean(val);
            packet.WriteInt(0);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendEatPetsInfo(EatPetsInfo info) //#1 For EatPets
        {
            if (info == null)
            {
                return null;
            }
            GSPacketIn packet = new GSPacketIn((short)68, this.m_gameClient.Player.PlayerId);
            packet.WriteByte((byte)33);
            packet.WriteInt(info.weaponExp);
            packet.WriteInt(info.weaponLevel);
            packet.WriteInt(info.clothesExp);
            packet.WriteInt(info.clothesLevel);
            packet.WriteInt(info.hatExp);
            packet.WriteInt(info.hatLevel);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendUpdateRoomList(List<BaseRoom> roomlist)
        {
            GSPacketIn packet = new GSPacketIn((short)94);
            packet.WriteByte((byte)9);
            packet.WriteInt(roomlist.Count);
            int val = roomlist.Count < 8 ? roomlist.Count : 8;
            packet.WriteInt(val);
            for (int index = 0; index < val; ++index)
            {
                BaseRoom baseRoom = roomlist[index];
                packet.WriteInt(baseRoom.RoomId);
                packet.WriteByte((byte)baseRoom.RoomType);
                packet.WriteByte(baseRoom.TimeMode);
                packet.WriteByte((byte)baseRoom.PlayerCount);
                packet.WriteByte((byte)baseRoom.viewerCnt);
                packet.WriteByte((byte)baseRoom.maxViewerCnt);
                packet.WriteByte((byte)baseRoom.PlacesCount);
                packet.WriteBoolean(!string.IsNullOrEmpty(baseRoom.Password));
                packet.WriteInt(baseRoom.MapId);
                packet.WriteBoolean(baseRoom.IsPlaying);
                packet.WriteString(baseRoom.Name);
                packet.WriteByte((byte)baseRoom.GameType);
                packet.WriteByte((byte)baseRoom.HardLevel);
                packet.WriteInt(baseRoom.LevelLimits);
            }
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendSceneAddPlayer(GamePlayer player)
        {
            GSPacketIn packet = new GSPacketIn((short)18, player.PlayerCharacter.ID);
            packet.WriteInt(player.PlayerCharacter.Grade);
            packet.WriteBoolean(player.PlayerCharacter.Sex);
            packet.WriteString(player.PlayerCharacter.NickName);
            packet.WriteByte(player.PlayerCharacter.typeVIP);
            packet.WriteInt(player.PlayerCharacter.VIPLevel);
            packet.WriteString(player.PlayerCharacter.ConsortiaName);
            packet.WriteInt(player.PlayerCharacter.Offer);
            packet.WriteInt(player.PlayerCharacter.Win);
            packet.WriteInt(player.PlayerCharacter.Total);
            packet.WriteInt(player.PlayerCharacter.Escape);
            packet.WriteInt(player.PlayerCharacter.ConsortiaID);
            packet.WriteInt(player.PlayerCharacter.Repute);
            packet.WriteBoolean(player.PlayerCharacter.IsMarried);
            if (player.PlayerCharacter.IsMarried)
            {
                packet.WriteInt(player.PlayerCharacter.SpouseID);
                packet.WriteString(player.PlayerCharacter.SpouseName);
            }
            packet.WriteString(player.PlayerCharacter.UserName);
            packet.WriteInt(player.PlayerCharacter.FightPower);
            packet.WriteInt(player.PlayerCharacter.apprenticeshipState);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendSceneRemovePlayer(GamePlayer player)
        {
            GSPacketIn packet = new GSPacketIn((short)21, player.PlayerCharacter.ID);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendRoomPlayerAdd(GamePlayer player)
        {
            GSPacketIn packet = new GSPacketIn((short)94, player.PlayerId);
            packet.WriteByte((byte)4);
            bool val = false;
            if (player.CurrentRoom.Game != null)
                val = true;
            packet.WriteBoolean(val);
            packet.WriteByte((byte)player.CurrentRoomIndex);
            packet.WriteByte((byte)player.CurrentRoomTeam);
            packet.WriteBoolean(false);
            packet.WriteInt(player.PlayerCharacter.Grade);
            packet.WriteInt(player.PlayerCharacter.Offer);
            packet.WriteInt(player.PlayerCharacter.Hide);
            packet.WriteInt(player.PlayerCharacter.Repute);
            packet.WriteInt((int)player.PingTime / 1000 / 10);
            packet.WriteInt(player.ZoneId);
            packet.WriteInt(player.PlayerCharacter.ID);
            packet.WriteString(player.PlayerCharacter.NickName);
            packet.WriteByte(player.PlayerCharacter.typeVIP);
            packet.WriteInt(player.PlayerCharacter.VIPLevel);
            packet.WriteBoolean(player.PlayerCharacter.Sex);
            packet.WriteString(player.PlayerCharacter.Style);
            packet.WriteString(player.PlayerCharacter.Colors);
            packet.WriteString(player.PlayerCharacter.Skin);
            SqlDataProvider.Data.ItemInfo itemAt = player.EquipBag.GetItemAt(6);
            packet.WriteInt(itemAt == null ? -1 : itemAt.TemplateID);
            if (player.SecondWeapon == null)
                packet.WriteInt(0);
            else
                packet.WriteInt(player.SecondWeapon.TemplateID);
            packet.WriteInt(player.PlayerCharacter.ConsortiaID);
            packet.WriteString(player.PlayerCharacter.ConsortiaName);
            packet.WriteInt(player.PlayerCharacter.badgeID);
            packet.WriteInt(player.PlayerCharacter.Win);
            packet.WriteInt(player.PlayerCharacter.Total);
            packet.WriteInt(player.PlayerCharacter.Escape);
            packet.WriteInt(player.PlayerCharacter.ConsortiaLevel);
            packet.WriteInt(player.PlayerCharacter.ConsortiaRepute);
            packet.WriteBoolean(player.PlayerCharacter.IsMarried);
            if (player.PlayerCharacter.IsMarried)
            {
                packet.WriteInt(player.PlayerCharacter.SpouseID);
                packet.WriteString(player.PlayerCharacter.SpouseName);
            }
            packet.WriteString(player.PlayerCharacter.UserName);
            packet.WriteInt(player.PlayerCharacter.Nimbus);
            packet.WriteInt(player.PlayerCharacter.FightPower);
            packet.WriteInt(player.PlayerCharacter.apprenticeshipState);
            packet.WriteInt(player.PlayerCharacter.masterID);
            packet.WriteString(player.PlayerCharacter.masterOrApprentices);
            packet.WriteInt(player.PlayerCharacter.graduatesCount);
            packet.WriteString(player.PlayerCharacter.honourOfMaster);
            packet.WriteBoolean(player.MatchInfo.DailyLeagueFirst);
            packet.WriteInt(player.MatchInfo.DailyLeagueLastScore);
            if (player.Pet == null)
            {
                packet.WriteInt(0);
            }
            else
            {
                packet.WriteInt(1);
                packet.WriteInt(player.Pet.Place);
                packet.WriteInt(player.Pet.TemplateID);
                packet.WriteInt(player.Pet.ID);
                packet.WriteString(player.Pet.Name);
                packet.WriteInt(player.PlayerCharacter.ID);
                packet.WriteInt(player.Pet.Level);
                List<string> skillEquip = player.Pet.GetSkillEquip();
                packet.WriteInt(skillEquip.Count);
                foreach (string str in skillEquip)
                {
                    packet.WriteInt(int.Parse(str.Split(',')[1]));
                    packet.WriteInt(int.Parse(str.Split(',')[0]));
                }
            }
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendRoomPlayerRemove(GamePlayer player)
        {
            GSPacketIn packet = new GSPacketIn((short)94, player.PlayerId);
            packet.WriteByte((byte)5);
            packet.Parameter1 = player.PlayerId;
            packet.ClientID = player.PlayerId;
            packet.WriteInt(player.ZoneId);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendRoomUpdatePlayerStates(byte[] states)
        {
            GSPacketIn packet = new GSPacketIn((short)94);
            packet.WriteByte((byte)15);
            for (int index = 0; index < states.Length; ++index)
                packet.WriteByte(states[index]);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendRoomUpdatePlacesStates(int[] states)
        {
            GSPacketIn packet = new GSPacketIn((short)94);
            packet.WriteByte((byte)10);
            for (int index = 0; index < states.Length; ++index)
                packet.WriteInt(states[index]);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendRoomPlayerChangedTeam(GamePlayer player)
        {
            GSPacketIn packet = new GSPacketIn((short)94, player.PlayerId);
            packet.WriteByte((byte)6);
            packet.WriteByte((byte)player.CurrentRoomTeam);
            packet.WriteByte((byte)player.CurrentRoomIndex);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendRoomCreate(BaseRoom room)
        {
            GSPacketIn packet = new GSPacketIn((short)94);
            packet.WriteByte((byte)0);
            packet.WriteInt(room.RoomId);
            packet.WriteByte((byte)room.RoomType);
            packet.WriteByte((byte)room.HardLevel);
            packet.WriteByte(room.TimeMode);
            packet.WriteByte((byte)room.PlayerCount);
            packet.WriteByte((byte)room.viewerCnt);
            packet.WriteByte((byte)room.PlacesCount);
            packet.WriteBoolean(!string.IsNullOrEmpty(room.Password));
            packet.WriteInt(room.MapId);
            packet.WriteBoolean(room.IsPlaying);
            packet.WriteString(room.Name);
            packet.WriteByte((byte)room.GameType);
            packet.WriteInt(room.LevelLimits);
            packet.WriteBoolean(false);
            packet.WriteBoolean(false);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendSingleRoomCreate(BaseRoom room, int zoneId)
        {
            GSPacketIn packet = new GSPacketIn((short)76);
            packet.WriteInt(room.RoomId);
            packet.WriteByte((byte)room.RoomType);
            packet.WriteBoolean(room.IsPlaying);
            packet.WriteByte((byte)room.GameType);
            packet.WriteInt(room.MapId);
            packet.WriteBoolean(room.isCrosszone);
            packet.WriteInt(room.ZoneId);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendRoomLoginResult(bool result)
        {
            GSPacketIn packet = new GSPacketIn((short)94);
            packet.WriteByte((byte)1);
            packet.WriteBoolean(result);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendRoomPairUpStart(BaseRoom room)
        {
            GSPacketIn packet = new GSPacketIn((short)94);
            packet.WriteByte((byte)13);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendGameRoomInfo(GamePlayer player, BaseRoom game)
        {
            return new GSPacketIn((short)94, player.PlayerCharacter.ID);
        }

        public GSPacketIn SendRoomType(GamePlayer player, BaseRoom game)
        {
            GSPacketIn packet = new GSPacketIn((short)94);
            packet.WriteByte((byte)12);
            packet.WriteByte((byte)game.GameType);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendRoomPairUpCancel(BaseRoom room)
        {
            GSPacketIn packet = new GSPacketIn((short)94);
            packet.WriteByte((byte)11);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendRoomClear(GamePlayer player, BaseRoom game)
        {
            GSPacketIn packet = new GSPacketIn((short)96, player.PlayerCharacter.ID);
            packet.WriteInt(game.RoomId);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendEquipChange(
          GamePlayer player,
          int place,
          int goodsID,
          string style)
        {
            GSPacketIn packet = new GSPacketIn((short)66, player.PlayerCharacter.ID);
            packet.WriteByte((byte)place);
            packet.WriteInt(goodsID);
            packet.WriteString(style);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendRoomChange(BaseRoom room)
        {
            GSPacketIn packet = new GSPacketIn((short)94);
            packet.WriteByte((byte)2);
            packet.WriteInt(room.MapId);
            packet.WriteByte((byte)room.RoomType);
            packet.WriteString(room.Password);
            packet.WriteString(room.Name);
            packet.WriteByte(room.TimeMode);
            packet.WriteByte((byte)room.HardLevel);
            packet.WriteInt(room.LevelLimits);
            packet.WriteBoolean(room.isCrosszone);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendGameRoomSetupChange(BaseRoom room)
        {
            GSPacketIn packet = new GSPacketIn((short)94);
            packet.WriteByte((byte)2);
            packet.WriteInt(room.MapId);
            packet.WriteByte((byte)room.RoomType);
            packet.WriteString(room.Password == null ? "" : room.Password);
            packet.WriteString(room.Name == null ? "Gunny1" : room.Name);
            packet.WriteByte(room.TimeMode);
            packet.WriteByte((byte)room.HardLevel);
            packet.WriteInt(room.LevelLimits);
            packet.WriteBoolean(room.isCrosszone);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendFusionPreview(
          GamePlayer player,
          Dictionary<int, double> previewItemList,
          bool isbind,
          int MinValid)
        {
            GSPacketIn packet = new GSPacketIn((short)76, player.PlayerCharacter.ID);
            packet.WriteInt(previewItemList.Count);
            foreach (KeyValuePair<int, double> previewItem in previewItemList)
            {
                packet.WriteInt(previewItem.Key);
                packet.WriteInt(MinValid);
                int int32 = Convert.ToInt32(previewItem.Value);
                packet.WriteInt(int32 > 100 ? 100 : (int32 < 0 ? 0 : int32));
            }
            packet.WriteBoolean(isbind);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendFusionResult(GamePlayer player, bool result)
        {
            GSPacketIn packet = new GSPacketIn((short)78, player.PlayerCharacter.ID);
            packet.WriteInt(2);
            packet.WriteBoolean(result);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendRefineryPreview(
          GamePlayer player,
          int templateid,
          bool isbind,
          SqlDataProvider.Data.ItemInfo item)
        {
            GSPacketIn packet = new GSPacketIn((short)111, player.PlayerCharacter.ID);
            packet.WriteInt(templateid);
            packet.WriteInt(item.ValidDate);
            packet.WriteBoolean(isbind);
            packet.WriteInt(item.AgilityCompose);
            packet.WriteInt(item.AttackCompose);
            packet.WriteInt(item.DefendCompose);
            packet.WriteInt(item.LuckCompose);
            this.SendTCP(packet);
            return packet;
        }

        public void SendUpdateInventorySlot(PlayerInventory bag, int[] updatedSlots)
        {
            if (this.m_gameClient.Player == null)
                return;
            GSPacketIn packet = new GSPacketIn((short)64, this.m_gameClient.Player.PlayerCharacter.ID, 10240);
            packet.WriteInt(bag.BagType);
            packet.WriteInt(updatedSlots.Length);
            for (int index = 0; index < updatedSlots.Length; ++index)
            {
                int updatedSlot = updatedSlots[index];
                packet.WriteInt(updatedSlot);
                SqlDataProvider.Data.ItemInfo itemAt = bag.GetItemAt(updatedSlot);
                if (itemAt == null)
                {
                    packet.WriteBoolean(false);
                }
                else
                {
                    packet.WriteBoolean(true);
                    packet.WriteInt(itemAt.UserID);
                    packet.WriteInt(itemAt.ItemID);
                    packet.WriteInt(itemAt.Count);
                    packet.WriteInt(itemAt.Place);
                    packet.WriteInt(itemAt.TemplateID);
                    packet.WriteInt(itemAt.AttackCompose);
                    packet.WriteInt(itemAt.DefendCompose);
                    packet.WriteInt(itemAt.AgilityCompose);
                    packet.WriteInt(itemAt.LuckCompose);
                    packet.WriteInt(itemAt.StrengthenLevel);
                    packet.WriteInt(itemAt.StrengthenExp);
                    packet.WriteBoolean(itemAt.IsBinds);
                    packet.WriteBoolean(itemAt.IsJudge);
                    packet.WriteDateTime(itemAt.BeginDate);
                    packet.WriteInt(itemAt.ValidDate);
                    packet.WriteString(itemAt.Color == null ? "" : itemAt.Color);
                    packet.WriteString(itemAt.Skin == null ? "" : itemAt.Skin);
                    packet.WriteBoolean(itemAt.IsUsed);
                    packet.WriteInt(itemAt.Hole1);
                    packet.WriteInt(itemAt.Hole2);
                    packet.WriteInt(itemAt.Hole3);
                    packet.WriteInt(itemAt.Hole4);
                    packet.WriteInt(itemAt.Hole5);
                    packet.WriteInt(itemAt.Hole6);
                    packet.WriteString(itemAt.Pic);
                    packet.WriteInt(itemAt.RefineryLevel);
                    packet.WriteDateTime(DateTime.Now.AddDays(5.0));
                    packet.WriteInt(itemAt.StrengthenTimes);
                    packet.WriteByte((byte)itemAt.Hole5Level);
                    packet.WriteInt(itemAt.Hole5Exp);
                    packet.WriteByte((byte)itemAt.Hole6Level);
                    packet.WriteInt(itemAt.Hole6Exp);
                    packet.WriteBoolean(itemAt.isGold);
                    if (itemAt.isGold)
                    {
                        packet.WriteInt(itemAt.goldValidDate);
                        packet.WriteDateTime(itemAt.goldBeginTime);
                    }
                }
            }
            this.SendTCP(packet);
        }



        public void SendUpdateCardData(CardInventory bag, int[] updatedSlots)
        {
            if (m_gameClient.Player == null)
            {
                return;
            }
            GSPacketIn gSPacketIn = new GSPacketIn(216, m_gameClient.Player.PlayerCharacter.ID);
            gSPacketIn.WriteInt(m_gameClient.Player.PlayerCharacter.ID);
            gSPacketIn.WriteInt(updatedSlots.Length);
            foreach (int num in updatedSlots)
            {
                gSPacketIn.WriteInt(num);
                UsersCardInfo itemAt = bag.GetItemAt(num);
                if (itemAt != null && itemAt.TemplateID != 0)
                {
                    gSPacketIn.WriteBoolean(val: true);
                    gSPacketIn.WriteInt(itemAt.CardID);
                    gSPacketIn.WriteInt(itemAt.UserID);
                    gSPacketIn.WriteInt(itemAt.Count);
                    gSPacketIn.WriteInt(itemAt.Place);
                    gSPacketIn.WriteInt(itemAt.TemplateID);
                    gSPacketIn.WriteInt(itemAt.TotalAttack);
                    gSPacketIn.WriteInt(itemAt.TotalDefence);
                    gSPacketIn.WriteInt(itemAt.TotalAgility);
                    gSPacketIn.WriteInt(itemAt.TotalLuck);
                    gSPacketIn.WriteInt(itemAt.Damage);
                    gSPacketIn.WriteInt(itemAt.Guard);
                    gSPacketIn.WriteInt(itemAt.Level);
                    gSPacketIn.WriteInt(itemAt.CardGP);
                    gSPacketIn.WriteBoolean(itemAt.isFirstGet);
                }
                else
                {
                    gSPacketIn.WriteBoolean(val: false);
                }
            }
            SendTCP(gSPacketIn);
        }

        public void SendUpdateCardData(PlayerInfo player, List<UsersCardInfo> userCard)
        {
            if (m_gameClient.Player != null)
            {
                GSPacketIn gSPacketIn = new GSPacketIn(216, player.ID);
                gSPacketIn.WriteInt(player.ID);
                gSPacketIn.WriteInt(userCard.Count);
                foreach (UsersCardInfo item in userCard)
                {
                    gSPacketIn.WriteInt(item.Place);
                    gSPacketIn.WriteBoolean(val: true);
                    gSPacketIn.WriteInt(item.CardID);
                    gSPacketIn.WriteInt(item.UserID);
                    gSPacketIn.WriteInt(item.Count);
                    gSPacketIn.WriteInt(item.Place);
                    gSPacketIn.WriteInt(item.TemplateID);
                    gSPacketIn.WriteInt(item.TotalAttack);
                    gSPacketIn.WriteInt(item.TotalDefence);
                    gSPacketIn.WriteInt(item.TotalAgility);
                    gSPacketIn.WriteInt(item.TotalLuck);
                    gSPacketIn.WriteInt(item.Damage);
                    gSPacketIn.WriteInt(item.Guard);
                    gSPacketIn.WriteInt(item.Level);
                    gSPacketIn.WriteInt(item.CardGP);
                    gSPacketIn.WriteBoolean(item.isFirstGet);
                }
                SendTCP(gSPacketIn);
            }
        }

        public GSPacketIn SendUpdateUpCount(PlayerInfo player)
        {
            //GSPacketIn gSPacketIn = new GSPacketIn(96, player.ID);
            GSPacketIn packet = new GSPacketIn((short)96, player.ID);
            packet.WriteInt(player.MaxBuyHonor);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendUpdateQuests(
          GamePlayer player,
          byte[] states,
          BaseQuest[] infos)
        {
            if (player == null || states == null || infos == null)
                return (GSPacketIn)null;
            GSPacketIn packet = new GSPacketIn((short)178, player.PlayerCharacter.ID);
            packet.WriteInt(infos.Length);
            for (int index = 0; index < infos.Length; ++index)
            {
                BaseQuest info = infos[index];
                packet.WriteInt(info.Data.QuestID);
                packet.WriteBoolean(info.Data.IsComplete);
                packet.WriteInt(info.Data.Condition1);
                packet.WriteInt(info.Data.Condition2);
                packet.WriteInt(info.Data.Condition3);
                packet.WriteInt(info.Data.Condition4);
                packet.WriteDateTime(info.Data.CompletedDate.Date);
                packet.WriteInt(info.Data.RepeatFinish);
                packet.WriteInt(info.Data.RandDobule);
                packet.WriteBoolean(info.Data.IsExist);
            }
            packet.Write(states);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendUpdateBuffer(GamePlayer player, AbstractBuffer[] infos)
        {
            GSPacketIn packet = new GSPacketIn((short)185, player.PlayerId);
            packet.WriteInt(infos.Length);
            for (int index = 0; index < infos.Length; ++index)
            {
                AbstractBuffer info = infos[index];
                packet.WriteInt(info.Info.Type);
                packet.WriteBoolean(info.Info.IsExist);
                packet.WriteDateTime(info.Info.BeginDate);
                if (info.IsPayBuff())
                    packet.WriteInt(info.Info.ValidDate / 60 / 24);
                else
                    packet.WriteInt(info.Info.ValidDate);
                packet.WriteInt(info.Info.Value);
                packet.WriteInt(info.Info.ValidCount);
                packet.WriteInt(info.Info.TemplateID);
            }
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendBufferList(GamePlayer player, List<AbstractBuffer> infos)
        {
            GSPacketIn packet = new GSPacketIn((short)186, player.PlayerId);
            packet.WriteInt(infos.Count);
            foreach (AbstractBuffer info1 in infos)
            {
                BufferInfo info2 = info1.Info;
                packet.WriteInt(info2.Type);
                packet.WriteBoolean(info2.IsExist);
                packet.WriteDateTime(info2.BeginDate);
                if (info1.IsPayBuff())
                    packet.WriteInt(info2.ValidDate / 60 / 24);
                else
                    packet.WriteInt(info2.ValidDate);
                packet.WriteInt(info2.Value);
                packet.WriteInt(info2.ValidCount);
                packet.WriteInt(info2.TemplateID);
            }
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendMailResponse(int playerID, eMailRespose type)
        {
            GSPacketIn packet = new GSPacketIn((short)117);
            packet.WriteInt(playerID);
            packet.WriteInt((int)type);
            GameServer.Instance.LoginServer.SendPacket(packet);
            return packet;//Full
        }

        public GSPacketIn SendConsortiaLevelUp(
          byte type,
          byte level,
          bool result,
          string msg,
          int playerid)
        {
            GSPacketIn packet = new GSPacketIn((short)159, playerid);
            packet.WriteByte(type);
            packet.WriteByte(level);
            packet.WriteBoolean(result);
            packet.WriteString(msg);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendAuctionRefresh(
          AuctionInfo info,
          int auctionID,
          bool isExist,
          SqlDataProvider.Data.ItemInfo item)
        {
            GSPacketIn packet = new GSPacketIn((short)195);
            packet.WriteInt(auctionID);
            packet.WriteBoolean(isExist);
            if (isExist)
            {
                packet.WriteInt(info.AuctioneerID);
                packet.WriteString(info.AuctioneerName);
                packet.WriteDateTime(info.BeginDate);
                packet.WriteInt(info.BuyerID);
                packet.WriteString(info.BuyerName);
                packet.WriteInt(info.ItemID);
                packet.WriteInt(info.Mouthful);
                packet.WriteInt(info.PayType);
                packet.WriteInt(info.Price);
                packet.WriteInt(info.Rise);
                packet.WriteInt(info.ValidDate);
                packet.WriteBoolean(item != null);
                if (item != null)
                {
                    packet.WriteInt(item.Count);
                    packet.WriteInt(item.TemplateID);
                    packet.WriteInt(item.AttackCompose);
                    packet.WriteInt(item.DefendCompose);
                    packet.WriteInt(item.AgilityCompose);
                    packet.WriteInt(item.LuckCompose);
                    packet.WriteInt(item.StrengthenLevel);
                    packet.WriteBoolean(item.IsBinds);
                    packet.WriteBoolean(item.IsJudge);
                    packet.WriteDateTime(item.BeginDate);
                    packet.WriteInt(item.ValidDate);
                    packet.WriteString(item.Color);
                    packet.WriteString(item.Skin);
                    packet.WriteBoolean(item.IsUsed);
                    packet.WriteInt(item.Hole1);
                    packet.WriteInt(item.Hole2);
                    packet.WriteInt(item.Hole3);
                    packet.WriteInt(item.Hole4);
                    packet.WriteInt(item.Hole5);
                    packet.WriteInt(item.Hole6);
                    packet.WriteString(item.Pic);
                    packet.WriteInt(item.RefineryLevel);
                    packet.WriteDateTime(DateTime.Now);
                    packet.WriteByte((byte)item.Hole5Level);
                    packet.WriteInt(item.Hole5Exp);
                    packet.WriteByte((byte)item.Hole6Level);
                    packet.WriteInt(item.Hole6Exp);
                }
            }
            packet.Compress();
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendAASState(bool result)
        {
            GSPacketIn packet = new GSPacketIn((short)224);
            packet.WriteBoolean(result);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendIDNumberCheck(bool result)
        {
            GSPacketIn packet = new GSPacketIn((short)226);
            packet.WriteBoolean(result);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendAASInfoSet(bool result)
        {
            GSPacketIn packet = new GSPacketIn((short)224);
            packet.WriteBoolean(result);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendAASControl(bool result, bool bool_0, bool IsMinor)
        {
            GSPacketIn packet = new GSPacketIn((short)227);
            packet.WriteBoolean(true);
            packet.WriteInt(1);
            packet.WriteBoolean(true);
            packet.WriteBoolean(IsMinor);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendMarryRoomInfo(GamePlayer player, MarryRoom room)
        {
            GSPacketIn packet = new GSPacketIn((short)241, player.PlayerCharacter.ID);
            bool val = room != null;
            packet.WriteBoolean(val);
            if (val)
            {
                packet.WriteInt(room.Info.ID);
                packet.WriteBoolean(room.Info.IsHymeneal);
                packet.WriteString(room.Info.Name);
                packet.WriteBoolean(!(room.Info.Pwd == ""));
                packet.WriteInt(room.Info.MapIndex);
                packet.WriteInt(room.Info.AvailTime);
                packet.WriteInt(room.Count);
                packet.WriteInt(room.Info.PlayerID);
                packet.WriteString(room.Info.PlayerName);
                packet.WriteInt(room.Info.GroomID);
                packet.WriteString(room.Info.GroomName);
                packet.WriteInt(room.Info.BrideID);
                packet.WriteString(room.Info.BrideName);
                packet.WriteDateTime(room.Info.BeginTime);
                packet.WriteByte((byte)room.RoomState);
                packet.WriteString(room.Info.RoomIntroduction);
            }
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendMarryRoomLogin(GamePlayer player, bool result)
        {
            GSPacketIn packet = new GSPacketIn((short)242, player.PlayerCharacter.ID);
            packet.WriteBoolean(result);
            if (result)
            {
                packet.WriteInt(player.CurrentMarryRoom.Info.ID);
                packet.WriteString(player.CurrentMarryRoom.Info.Name);
                packet.WriteInt(player.CurrentMarryRoom.Info.MapIndex);
                packet.WriteInt(player.CurrentMarryRoom.Info.AvailTime);
                packet.WriteInt(player.CurrentMarryRoom.Count);
                packet.WriteInt(player.CurrentMarryRoom.Info.PlayerID);
                packet.WriteString(player.CurrentMarryRoom.Info.PlayerName);
                packet.WriteInt(player.CurrentMarryRoom.Info.GroomID);
                packet.WriteString(player.CurrentMarryRoom.Info.GroomName);
                packet.WriteInt(player.CurrentMarryRoom.Info.BrideID);
                packet.WriteString(player.CurrentMarryRoom.Info.BrideName);
                packet.WriteDateTime(player.CurrentMarryRoom.Info.BeginTime);
                packet.WriteBoolean(player.CurrentMarryRoom.Info.IsHymeneal);
                packet.WriteByte((byte)player.CurrentMarryRoom.RoomState);
                packet.WriteString(player.CurrentMarryRoom.Info.RoomIntroduction);
                packet.WriteBoolean(player.CurrentMarryRoom.Info.GuestInvite);
                packet.WriteInt(player.MarryMap);
                packet.WriteBoolean(player.CurrentMarryRoom.Info.IsGunsaluteUsed);
            }
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendPlayerEnterMarryRoom(GamePlayer player)
        {
            GSPacketIn packet = new GSPacketIn((short)243, player.PlayerCharacter.ID);
            packet.WriteInt(player.PlayerCharacter.Grade);
            packet.WriteInt(player.PlayerCharacter.Hide);
            packet.WriteInt(player.PlayerCharacter.Repute);
            packet.WriteInt(player.PlayerCharacter.ID);
            packet.WriteString(player.PlayerCharacter.NickName);
            packet.WriteByte(player.PlayerCharacter.typeVIP);
            packet.WriteInt(player.PlayerCharacter.VIPLevel);
            packet.WriteBoolean(player.PlayerCharacter.Sex);
            packet.WriteString(player.PlayerCharacter.Style);
            packet.WriteString(player.PlayerCharacter.Colors);
            packet.WriteString(player.PlayerCharacter.Skin);
            packet.WriteInt(player.X);
            packet.WriteInt(player.Y);
            packet.WriteInt(player.PlayerCharacter.FightPower);
            packet.WriteInt(player.PlayerCharacter.Win);
            packet.WriteInt(player.PlayerCharacter.Total);
            packet.WriteInt(player.PlayerCharacter.Offer);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendPlayerFigSpiritinit(int ID, List<UserGemStone> gems)
        {
            GSPacketIn packet = new GSPacketIn((short)209, ID);
            packet.WriteByte((byte)1);
            packet.WriteBoolean(true);
            packet.WriteInt(gems.Count);
            foreach (UserGemStone gem in gems)
            {
                packet.WriteInt(gem.UserID);
                packet.WriteInt(gem.FigSpiritId);
                packet.WriteString(gem.FigSpiritIdValue);
                packet.WriteInt(gem.EquipPlace);
            }
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendPlayerFigSpiritUp(int ID, UserGemStone gem, bool isUp, bool isMaxLevel, bool isFall, int num, int dir)
        {
            //GSPacketIn gSPacketIn = new GSPacketIn(209, ID);
            GSPacketIn packet = new GSPacketIn((short)209, ID);
            packet.WriteByte((byte)2);
            string[] strArrays = gem.FigSpiritIdValue.Split(new char[] { '|' });
            packet.WriteBoolean(isUp);
            packet.WriteBoolean(isMaxLevel);
            packet.WriteBoolean(isFall);
            packet.WriteInt(num);
            packet.WriteInt((int)strArrays.Length);
            for (int i = 0; i < (int)strArrays.Length; i++)
            {
                string str = strArrays[i];
                packet.WriteInt(gem.FigSpiritId);
                packet.WriteInt(Convert.ToInt32(str.Split(new char[] { ',' })[0]));
                packet.WriteInt(Convert.ToInt32(str.Split(new char[] { ',' })[1]));
                packet.WriteInt(Convert.ToInt32(str.Split(new char[] { ',' })[2]));
            }
            packet.WriteInt(gem.EquipPlace);
            packet.WriteInt(dir);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendMarryInfoRefresh(MarryInfo info, int ID, bool isExist)
        {
            GSPacketIn packet = new GSPacketIn((short)239);
            packet.WriteInt(ID);
            packet.WriteBoolean(isExist);
            if (isExist)
            {
                packet.WriteInt(info.UserID);
                packet.WriteBoolean(info.IsPublishEquip);
                packet.WriteString(info.Introduction);
            }
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendPlayerMarryStatus(
          GamePlayer player,
          int userID,
          bool isMarried)
        {
            GSPacketIn packet = new GSPacketIn((short)246, player.PlayerCharacter.ID);
            packet.WriteInt(userID);
            packet.WriteBoolean(isMarried);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendPlayerRefreshTotem(PlayerInfo player)
        {
            //GSPacketIn gSPacketIn = new GSPacketIn(136, player.ID);
            GSPacketIn packet = new GSPacketIn((short)136, player.ID);
            packet.WriteInt((byte)1);
            packet.WriteInt(player.myHonor);
            packet.WriteInt(player.totemId);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendPlayerMarryApply(
          GamePlayer player,
          int userID,
          string userName,
          string loveProclamation,
          int id)
        {
            GSPacketIn packet = new GSPacketIn((short)247, player.PlayerCharacter.ID);
            packet.WriteInt(userID);
            packet.WriteString(userName);
            packet.WriteString(loveProclamation);
            packet.WriteInt(id);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendPlayerDivorceApply(
          GamePlayer player,
          bool result,
          bool isProposer)
        {
            GSPacketIn packet = new GSPacketIn((short)248, player.PlayerCharacter.ID);
            packet.WriteBoolean(result);
            packet.WriteBoolean(isProposer);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendMarryApplyReply(
          GamePlayer player,
          int UserID,
          string UserName,
          bool result,
          bool isApplicant,
          int id)
        {
            GSPacketIn packet = new GSPacketIn((short)250, player.PlayerCharacter.ID);
            packet.WriteInt(UserID);
            packet.WriteBoolean(result);
            packet.WriteString(UserName);
            packet.WriteBoolean(isApplicant);
            packet.WriteInt(id);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendBigSpeakerMsg(GamePlayer player, string msg)
        {
            GSPacketIn packet = new GSPacketIn((short)72, player.PlayerCharacter.ID);
            packet.WriteInt(player.PlayerCharacter.ID);
            packet.WriteString(player.PlayerCharacter.NickName);
            packet.WriteString(msg);
            GameServer.Instance.LoginServer.SendPacket(packet);
            foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
                allPlayer.Out.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendPlayerLeaveMarryRoom(GamePlayer player)
        {
            GSPacketIn packet = new GSPacketIn((short)244, player.PlayerCharacter.ID);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendMarryRoomInfoToPlayer(
          GamePlayer player,
          bool state,
          MarryRoomInfo info)
        {
            GSPacketIn packet = new GSPacketIn((short)252, player.PlayerCharacter.ID);
            packet.WriteBoolean(state);
            if (state)
            {
                packet.WriteInt(info.ID);
                packet.WriteString(info.Name);
                packet.WriteInt(info.MapIndex);
                packet.WriteInt(info.AvailTime);
                packet.WriteInt(info.PlayerID);
                packet.WriteInt(info.GroomID);
                packet.WriteInt(info.BrideID);
                packet.WriteDateTime(info.BeginTime);
                packet.WriteBoolean(info.IsGunsaluteUsed);
            }
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendMarryInfo(GamePlayer player, MarryInfo info)
        {
            GSPacketIn packet = new GSPacketIn((short)235, player.PlayerCharacter.ID);
            packet.WriteString(info.Introduction);
            packet.WriteBoolean(info.IsPublishEquip);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendContinuation(GamePlayer player, MarryRoomInfo info)
        {
            GSPacketIn packet = new GSPacketIn((short)249, player.PlayerCharacter.ID);
            packet.WriteByte((byte)3);
            packet.WriteInt(info.AvailTime);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendMarryProp(GamePlayer player, MarryProp info)
        {
            GSPacketIn packet = new GSPacketIn((short)234, player.PlayerCharacter.ID);
            packet.WriteBoolean(info.IsMarried);
            packet.WriteInt(info.SpouseID);
            packet.WriteString(info.SpouseName);
            packet.WriteBoolean(info.IsCreatedMarryRoom);
            packet.WriteInt(info.SelfMarryRoomID);
            packet.WriteBoolean(info.IsGotRing);
            this.SendTCP(packet);
            return packet;
        }

        public void SendWeaklessGuildProgress(PlayerInfo player)
        {
            GSPacketIn packet = new GSPacketIn((short)15, player.ID);
            packet.WriteInt(player.weaklessGuildProgress.Length);
            for (int index = 0; index < player.weaklessGuildProgress.Length; ++index)
                packet.WriteByte(player.weaklessGuildProgress[index]);
            this.SendTCP(packet);
        }

        public void SendUserLuckyNum()
        {
            GSPacketIn packet = new GSPacketIn((short)161);
            packet.WriteInt(1);
            packet.WriteString("");
            this.SendTCP(packet);
        }

        public GSPacketIn SendUserRanks(List<UserRankInfo> rankList)
        {
            GSPacketIn packet = new GSPacketIn((short)34);
            packet.WriteInt(rankList.Count);
            foreach (UserRankInfo rank in rankList)
                packet.WriteString(rank.UserRank);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendUpdatePlayerProperty(PlayerInfo info, PlayerProperty PlayerProp)
        {
            string[] strArray = new string[4] { "Attack", "Defence", "Agility", "Luck" };
            GSPacketIn packet = new GSPacketIn((short)167, info.ID);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.ID);
            foreach (string index in strArray)
            {
                packet.WriteInt(0);
                packet.WriteInt(PlayerProp.Current["Texp"][index]);
                packet.WriteInt(PlayerProp.Current["Card"][index]);
                packet.WriteInt(0);//chắc là pet
                packet.WriteInt(0);//chắc là gemstone
                packet.WriteInt(0);//suit
            }
            packet.WriteInt(0);//original
            packet.WriteInt(0);//texp
            packet.WriteInt(0);//pet
            packet.WriteInt(0);//gem
            packet.WriteInt(0);//suit
            packet.WriteInt(PlayerProp.Current["Damage"]["Bead"]);
            packet.WriteInt(PlayerProp.Current["Armor"]["Bead"]);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendOpenVIP(GamePlayer Player)
        {
            GSPacketIn packet = new GSPacketIn((short)92, Player.PlayerCharacter.ID);
            packet.WriteByte(Player.PlayerCharacter.typeVIP);
            packet.WriteInt(Player.PlayerCharacter.VIPLevel);
            packet.WriteInt(Player.PlayerCharacter.VIPExp);
            packet.WriteDateTime(Player.PlayerCharacter.VIPExpireDay);
            packet.WriteDateTime(Player.PlayerCharacter.LastDate);
            packet.WriteInt(Player.PlayerCharacter.VIPNextLevelDaysNeeded);
            packet.WriteBoolean(Player.PlayerCharacter.CanTakeVipReward);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendEnterHotSpringRoom(GamePlayer player)
        {
            if (player.CurrentHotSpringRoom == null)
                return (GSPacketIn)null;
            GSPacketIn packet = new GSPacketIn((short)202, player.PlayerCharacter.ID);
            packet.WriteInt(player.CurrentHotSpringRoom.Info.roomID);
            packet.WriteInt(player.CurrentHotSpringRoom.Info.roomNumber);
            packet.WriteString(player.CurrentHotSpringRoom.Info.roomName);
            packet.WriteString(player.CurrentHotSpringRoom.Info.roomPassword);
            packet.WriteInt(player.CurrentHotSpringRoom.Info.effectiveTime);
            packet.WriteInt(player.CurrentHotSpringRoom.Count);
            packet.WriteInt(player.CurrentHotSpringRoom.Info.playerID);
            packet.WriteString(player.CurrentHotSpringRoom.Info.playerName);
            packet.WriteDateTime(player.CurrentHotSpringRoom.Info.startTime);
            packet.WriteString(player.CurrentHotSpringRoom.Info.roomIntroduction);
            packet.WriteInt(player.CurrentHotSpringRoom.Info.roomType);
            packet.WriteInt(player.CurrentHotSpringRoom.Info.maxCount);
            packet.WriteDateTime(player.Extra.Info.LastTimeHotSpring);
            packet.WriteInt(player.Extra.Info.MinHotSpring);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendHotSpringUpdateTime(GamePlayer player, int expAdd)
        {
            if (player.CurrentHotSpringRoom == null)
                return (GSPacketIn)null;
            GSPacketIn packet = new GSPacketIn((short)191, player.PlayerCharacter.ID);
            packet.WriteByte((byte)7);
            packet.WriteInt(player.Extra.Info.MinHotSpring);
            packet.WriteInt(expAdd);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendGetUserGift(PlayerInfo player, UserGiftInfo[] allGifts)
        {
            GSPacketIn packet = new GSPacketIn((short)218);
            packet.WriteInt(player.ID);
            packet.WriteInt(player.charmGP);
            packet.WriteInt(allGifts.Length);
            for (int index = 0; index < allGifts.Length; ++index)
            {
                UserGiftInfo allGift = allGifts[index];
                packet.WriteInt(allGift.TemplateID);
                packet.WriteInt(allGift.Count);
            }
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendUserEquip(PlayerInfo player, List<ItemInfo> items)
        {

            GSPacketIn gSPacketIn = new GSPacketIn(74, player.ID);
            gSPacketIn.WriteInt(player.ID);
            gSPacketIn.WriteString(player.NickName);
            gSPacketIn.WriteInt(player.Agility);
            gSPacketIn.WriteInt(player.Attack);
            gSPacketIn.WriteString(player.Colors);
            gSPacketIn.WriteString(player.Skin);
            gSPacketIn.WriteInt(player.Defence);
            gSPacketIn.WriteInt(player.GP);
            gSPacketIn.WriteInt(player.Grade);
            gSPacketIn.WriteInt(player.Luck);
            gSPacketIn.WriteInt(player.hp);
            gSPacketIn.WriteInt(player.Hide);
            gSPacketIn.WriteInt(player.Repute);
            gSPacketIn.WriteBoolean(player.Sex);
            gSPacketIn.WriteString(player.Style);
            gSPacketIn.WriteInt(player.Offer);
            gSPacketIn.WriteByte(player.typeVIP);
            gSPacketIn.WriteInt(player.VIPLevel);
            gSPacketIn.WriteInt(player.Win);
            gSPacketIn.WriteInt(player.Total);
            gSPacketIn.WriteInt(player.Escape);
            gSPacketIn.WriteInt(player.ConsortiaID);
            gSPacketIn.WriteString(player.ConsortiaName);
            gSPacketIn.WriteInt(player.badgeID);
            gSPacketIn.WriteInt(player.RichesOffer);
            gSPacketIn.WriteInt(player.RichesRob);
            gSPacketIn.WriteBoolean(player.IsMarried);
            gSPacketIn.WriteInt(player.SpouseID);
            gSPacketIn.WriteString(player.SpouseName);
            gSPacketIn.WriteString(player.DutyName);
            gSPacketIn.WriteInt(player.Nimbus);
            gSPacketIn.WriteInt(player.FightPower);
            gSPacketIn.WriteInt(player.apprenticeshipState);
            gSPacketIn.WriteInt(player.masterID);
            gSPacketIn.WriteString(player.masterOrApprentices);
            gSPacketIn.WriteInt(player.graduatesCount);
            gSPacketIn.WriteString(player.honourOfMaster);
            gSPacketIn.WriteInt(player.AchievementPoint);
            gSPacketIn.WriteString(player.Honor);
            gSPacketIn.WriteDateTime(DateTime.Now.AddDays(-2.0));
            gSPacketIn.WriteInt(player.Texp.spdTexpExp);
            gSPacketIn.WriteInt(player.Texp.attTexpExp);
            gSPacketIn.WriteInt(player.Texp.defTexpExp);
            gSPacketIn.WriteInt(player.Texp.hpTexpExp);
            gSPacketIn.WriteInt(player.Texp.lukTexpExp);
            gSPacketIn.WriteBoolean(val: false);
            gSPacketIn.WriteInt(0);
            gSPacketIn.WriteInt(items.Count);
            foreach (ItemInfo item in items)
            {
                gSPacketIn.WriteByte((byte)item.BagType);
                gSPacketIn.WriteInt(item.UserID);
                gSPacketIn.WriteInt(item.ItemID);
                gSPacketIn.WriteInt(item.Count);
                gSPacketIn.WriteInt(item.Place);
                gSPacketIn.WriteInt(item.TemplateID);
                gSPacketIn.WriteInt(item.AttackCompose);
                gSPacketIn.WriteInt(item.DefendCompose);
                gSPacketIn.WriteInt(item.AgilityCompose);
                gSPacketIn.WriteInt(item.LuckCompose);
                gSPacketIn.WriteInt(item.StrengthenLevel);
                gSPacketIn.WriteBoolean(item.IsBinds);
                gSPacketIn.WriteBoolean(item.IsJudge);
                gSPacketIn.WriteDateTime(item.BeginDate);
                gSPacketIn.WriteInt(item.ValidDate);
                gSPacketIn.WriteString(item.Color);
                gSPacketIn.WriteString(item.Skin);
                gSPacketIn.WriteBoolean(item.IsUsed);
                gSPacketIn.WriteInt(item.Hole1);
                gSPacketIn.WriteInt(item.Hole2);
                gSPacketIn.WriteInt(item.Hole3);
                gSPacketIn.WriteInt(item.Hole4);
                gSPacketIn.WriteInt(item.Hole5);
                gSPacketIn.WriteInt(item.Hole6);
                gSPacketIn.WriteString(item.Pic);
                gSPacketIn.WriteInt(item.RefineryLevel);
                gSPacketIn.WriteDateTime(DateTime.Now);
                gSPacketIn.WriteByte((byte)item.Hole5Level);
                gSPacketIn.WriteInt(item.Hole5Exp);
                gSPacketIn.WriteByte((byte)item.Hole6Level);
                gSPacketIn.WriteInt(item.Hole6Exp);
                gSPacketIn.WriteBoolean(item.isGold);
                if (item.isGold)
                {
                    gSPacketIn.WriteInt(item.goldValidDate);
                    gSPacketIn.WriteDateTime(item.goldBeginTime);
                }
            }
            gSPacketIn.Compress();
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }
    }
}
