// Decompiled with JetBrains decompiler
// Type: Fighting.Server.ServerClient
// Assembly: Fighting.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8F1EB855-F1B7-44B3-B212-6508DAF33CC5
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Fight\Fighting.Server.dll

using Bussiness;
using Bussiness.Managers;
using Fighting.Server.GameObjects;
using Fighting.Server.Games;
using Fighting.Server.Rooms;
using Game.Base;
using Game.Base.Packets;
using Game.Logic;
using Game.Logic.Phy.Object;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Fighting.Server
{
    public class ServerClient : BaseClient
    {
        private static readonly ILog ilog_1 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private RSACryptoServiceProvider rsacryptoServiceProvider_0;
        private FightServer fightServer_0;
        private Dictionary<int, ProxyRoom> dictionary_0;

        protected override void OnConnect()
        {
            base.OnConnect();
            this.rsacryptoServiceProvider_0 = new RSACryptoServiceProvider();
            RSAParameters rsaParameters = this.rsacryptoServiceProvider_0.ExportParameters(false);
            this.method_5(rsaParameters.Modulus, rsaParameters.Exponent);
        }

        protected override void OnDisconnect()
        {
            base.OnDisconnect();
            this.rsacryptoServiceProvider_0 = (RSACryptoServiceProvider)null;
        }

        public override void OnRecvPacket(GSPacketIn pkg)
        {
            Console.WriteLine("Pacote Carregado {0}", (ePackageType)pkg.Code);
            switch (pkg.Code)
            {
                case 1:
                    this.HandleLogin(pkg);
                    break;
                case 2:
                    this.HanleSendToGame(pkg);
                    break;
                case 3:
                    this.method_3(pkg);
                    break;
                case 19:
                    this.method_4(pkg);
                    break;
                case 36:
                    this.method_1(pkg);
                    break;
                case 64:
                    this.HandleGameRoomCreate(pkg);
                    break;
                case 65:
                    this.HandleGameRoomCancel(pkg);
                    break;
                case 69:
                    this.HandleConsortiaAlly(pkg);
                    break;
                case 83:
                    this.method_2(pkg);
                    break;
                default:
                    Console.WriteLine("Pacote inexistente "+ pkg.Code);
                    break;
            }
        }

        private void method_1(GSPacketIn gspacketIn_0)
        {
            BaseGame game = GameMgr.FindGame(gspacketIn_0.ClientID);
            if (game == null)
                return;
            game.Resume();
            if (!gspacketIn_0.ReadBoolean())
                return;
            Player player = game.FindPlayer(gspacketIn_0.Parameter1);
            ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(gspacketIn_0.Parameter2);
            if (player == null || itemTemplate == null)
                return;
            player.UseItem(itemTemplate, 1);
        }

        private void method_2(GSPacketIn gspacketIn_0)
        {
            BaseGame game = GameMgr.FindGame(gspacketIn_0.ClientID);
            if (game == null)
                return;
            Player player = game.FindPlayer(gspacketIn_0.Parameter1);
            if (player == null)
                return;
            GSPacketIn pkg = new GSPacketIn((short)83, player.PlayerDetail.PlayerCharacter.ID);
            game.SendToAll(pkg);
            game.RemovePlayer(player.PlayerDetail, false);
            ProxyRoom roomUnsafe = ProxyRoomMgr.GetRoomUnsafe((game as BattleGame).Red.RoomId);
            if (roomUnsafe == null || roomUnsafe.RemovePlayer(player.PlayerDetail))
                return;
            ProxyRoomMgr.GetRoomUnsafe((game as BattleGame).Blue.RoomId)?.RemovePlayer(player.PlayerDetail);
        }

        public void HandleConsortiaAlly(GSPacketIn pkg)
        {
            BaseGame game = GameMgr.FindGame(pkg.ClientID);
            if (game == null)
                return;
            game.ConsortiaAlly = pkg.ReadInt();
            game.RichesRate = pkg.ReadInt();
        }

        private void method_3(GSPacketIn gspacketIn_0)
        {
            BaseGame game = GameMgr.FindGame(gspacketIn_0.ClientID);
            if (game == null)
                return;
            Player player = game.FindPlayer(gspacketIn_0.Parameter1);
            GSPacketIn pkg = new GSPacketIn((short)3);
            pkg.WriteInt(3);
            pkg.WriteString(LanguageMgr.GetTranslation("AbstractPacketLib.SendGamePlayerLeave.Msg6", (object)(player.PlayerDetail.PlayerCharacter.Grade * 12), (object)15));
            player.PlayerDetail.SendTCP(pkg);
            pkg.ClearContext();
            pkg.WriteInt(3);
            pkg.WriteString(LanguageMgr.GetTranslation("AbstractPacketLib.SendGamePlayerLeave.Msg7", (object)player.PlayerDetail.PlayerCharacter.NickName, (object)(player.PlayerDetail.PlayerCharacter.Grade * 12), (object)15));
            game.SendToAll(pkg, player.PlayerDetail);
        }

        private void method_4(GSPacketIn gspacketIn_0)
        {
            BaseGame game = GameMgr.FindGame(gspacketIn_0.ClientID);
            if (game == null)
                return;
            Player player = game.FindPlayer(gspacketIn_0.ReadInt());
            bool val = gspacketIn_0.ReadBoolean();
            string str = gspacketIn_0.ReadString();
            if (player == null)
                return;
            GSPacketIn pkg = new GSPacketIn((short)19);
            pkg.ClientID = player.PlayerDetail.PlayerCharacter.ID;
            pkg.WriteInt(player.PlayerDetail.ZoneId);
            pkg.WriteByte((byte)5);
            pkg.WriteBoolean(val);
            pkg.WriteString(player.PlayerDetail.PlayerCharacter.NickName);
            pkg.WriteString(str);
            if (val)
                game.SendToTeam(gspacketIn_0, player.Team);
            else
                game.SendToAll(pkg);
        }

        public void HandleLogin(GSPacketIn pkg)
        {
            string[] strArray = Encoding.UTF8.GetString(this.rsacryptoServiceProvider_0.Decrypt(pkg.ReadBytes(), false)).Split(',');
            if (strArray.Length == 2)
            {
                this.rsacryptoServiceProvider_0 = (RSACryptoServiceProvider)null;
                int.Parse(strArray[0]);
                this.Strict = false;
            }
            else
            {
                ServerClient.ilog_1.ErrorFormat("Error Login Packet from {0}", (object)this.TcpEndpoint);
                this.Disconnect();
            }
        }

        public void HandleGameRoomCreate(GSPacketIn pkg) //private function __addPlayerInRoom(param1:CrazyTankSocketEvent) : void
        {
            int num1 = pkg.ReadInt();
            int num2 = pkg.ReadInt();
            int num3 = pkg.ReadInt();
            int num4 = pkg.ReadInt();
            int npcId = pkg.ReadInt();
            bool pickUpWithNPC = pkg.ReadBoolean();
            bool isBot = pkg.ReadBoolean();
            bool flag = pkg.ReadBoolean();
            int length = pkg.ReadInt();
            int num5 = 0;
            int num6 = 0;
            int zoneID = 0;
            int num7 = 0;
            IGamePlayer[] players = new IGamePlayer[length];
            for (int index1 = 0; index1 < length; ++index1)
            {
                PlayerInfo character = new PlayerInfo();
                ProxyPlayerInfo proxyPlayer = new ProxyPlayerInfo();
                character.ID = pkg.ReadInt();
                zoneID = pkg.ReadInt();
                proxyPlayer.ZoneId = zoneID;
                proxyPlayer.ZoneName = pkg.ReadString();
                int num8 = pkg.ReadInt();
                character.NickName = pkg.ReadString();
                character.Sex = pkg.ReadBoolean();
                character.Hide = pkg.ReadInt();
                character.Style = pkg.ReadString();
                character.Colors = pkg.ReadString();
                character.Skin = pkg.ReadString();
                character.Offer = pkg.ReadInt();
                character.GP = pkg.ReadInt();
                character.Grade = pkg.ReadInt();
                character.Repute = pkg.ReadInt();
                character.ConsortiaID = pkg.ReadInt();
                character.ConsortiaName = pkg.ReadString();
                character.ConsortiaLevel = pkg.ReadInt();
                character.ConsortiaRepute = pkg.ReadInt();
                character.IsShowConsortia = pkg.ReadBoolean();
                character.badgeID = pkg.ReadInt();
                character.Honor = pkg.ReadString();
                character.AchievementPoint = pkg.ReadInt();
                character.WeaklessGuildProgressStr = pkg.ReadString();
                character.MoneyPlus = pkg.ReadInt();
                character.FightPower = pkg.ReadInt();
                character.apprenticeshipState = pkg.ReadInt();
                character.masterID = pkg.ReadInt();
                character.masterOrApprentices = pkg.ReadString();
                character.IsAutoBot = isBot;
                num7 += character.FightPower;
                character.Attack = pkg.ReadInt();
                character.Defence = pkg.ReadInt();
                character.Agility = pkg.ReadInt();
                character.Luck = pkg.ReadInt();
                character.hp = pkg.ReadInt();
                proxyPlayer.BaseAttack = pkg.ReadDouble();
                proxyPlayer.BaseDefence = pkg.ReadDouble();
                proxyPlayer.BaseAgility = pkg.ReadDouble();
                proxyPlayer.BaseBlood = pkg.ReadDouble();
                proxyPlayer.TemplateId = pkg.ReadInt();
                proxyPlayer.WeaponStrengthLevel = pkg.ReadInt();
                int num9 = pkg.ReadInt();
                if (num9 != 0)
                {
                    proxyPlayer.GoldTemplateId = num9;
                    proxyPlayer.goldBeginTime = pkg.ReadDateTime();
                    proxyPlayer.goldValidDate = pkg.ReadInt();
                }
                proxyPlayer.CanUserProp = pkg.ReadBoolean();
                proxyPlayer.SecondWeapon = pkg.ReadInt();
                proxyPlayer.StrengthLevel = pkg.ReadInt();
                proxyPlayer.Healstone = pkg.ReadInt();
                proxyPlayer.HealstoneCount = pkg.ReadInt();
                double num10 = pkg.ReadDouble();
                double num11 = pkg.ReadDouble();
                double num12 = pkg.ReadDouble();
                double num13 = pkg.ReadDouble();
                double num14 = pkg.ReadDouble();
                pkg.ReadInt();
                List<BufferInfo> buffers = new List<BufferInfo>();
                int num15 = pkg.ReadInt();
                for (int index2 = 0; index2 < num15; ++index2)
                {
                    BufferInfo bufferInfo = new BufferInfo();
                    bufferInfo.Type = pkg.ReadInt();
                    bufferInfo.IsExist = pkg.ReadBoolean();
                    bufferInfo.BeginDate = pkg.ReadDateTime();
                    bufferInfo.ValidDate = pkg.ReadInt();
                    bufferInfo.Value = pkg.ReadInt();
                    if (character != null)
                        buffers.Add(bufferInfo);
                }
                List<int> equipEffect = new List<int>();
                int num16 = pkg.ReadInt();
                for (int index2 = 0; index2 < num16; ++index2)
                {
                    int num17 = pkg.ReadInt();
                    equipEffect.Add(num17);
                }
                List<BufferInfo> fightBuffer = new List<BufferInfo>();
                int num18 = pkg.ReadInt();
                for (int index2 = 0; index2 < num18; ++index2)
                {
                    int num17 = pkg.ReadInt();
                    int num19 = pkg.ReadInt();
                    fightBuffer.Add(new BufferInfo()
                    {
                        Type = num17,
                        Value = num19
                    });
                }
                UserVIPInfo userVipInfo = new UserVIPInfo()
                {
                    
                    typeVIP = pkg.ReadByte(),
                    VIPLevel = pkg.ReadInt(),
                    VIPExpireDay = pkg.ReadDateTime()

                }; 
               
                UserMatchInfo matchInfo = new UserMatchInfo();
                matchInfo.DailyLeagueFirst = pkg.ReadBoolean();
                matchInfo.DailyLeagueLastScore = pkg.ReadInt();
                int num20 = pkg.ReadBoolean() ? 1 : 0;
                UsersPetInfo pet = (UsersPetInfo)null;
                if (num20 != 0)
                {
                    pet = new UsersPetInfo();
                    pet.Place = pkg.ReadInt();
                    pet.TemplateID = pkg.ReadInt();
                    pet.ID = pkg.ReadInt();
                    pet.Name = pkg.ReadString();
                    pet.UserID = pkg.ReadInt();
                    pet.Level = pkg.ReadInt();
                    pet.Skill = pkg.ReadString();
                    pet.SkillEquip = pkg.ReadString();
                }
                players[index1] = (IGamePlayer)new ProxyPlayer(this, character, pet, buffers, equipEffect, fightBuffer, proxyPlayer, matchInfo);
                players[index1].CurrentEnemyId = num8;
                players[index1].GPApprenticeOnline = num12;
                players[index1].GPAddPlus = num10;
                players[index1].OfferAddPlus = num11;
                players[index1].GPApprenticeTeam = num13;
                players[index1].GPSpouseTeam = num14;
                num6 = character.ID;
                num5 += character.Grade;
            }
            ProxyRoom room = new ProxyRoom(ProxyRoomMgr.NextRoomId(), num1, zoneID, players, this, npcId, pickUpWithNPC, isBot, false);
            room.GuildId = num4;
            room.selfId = num6;
            room.AvgLevel = num5;
            room.startWithNpc = pickUpWithNPC;
            if(players.Length <2 && eGameType.Guild == (eGameType)num3 && (eRoomType)num2 == eRoomType.Match)
            {
                room.RoomType = eRoomType.Match;
                room.GameType = eGameType.Free;
                for(int x=0;x< players.Length;x++)
                {
                    players[x].SendMessage("Não é possível jogar GVG em uma pessoa só, essa partida será considerada PVP automaticamente");
                }

            }
            else
            {
                room.RoomType = (eRoomType)num2;
                room.GameType = (eGameType)num3;
            }
           
            room.IsCrossZone = flag;
            room.FightPower = num7;
            lock (this.dictionary_0)
            {
                if (!this.dictionary_0.ContainsKey(num1))
                    this.dictionary_0.Add(num1, room);
                else
                    room = (ProxyRoom)null;
            }
            if (room != null)
            {
                ProxyRoomMgr.AddRoom(room);
            }
            else
            {
                this.RemoveRoom(num1, room);
                ServerClient.ilog_1.ErrorFormat("Room already exists:{0}.", (object)num1);
            }
        }
        public void HandleGameRoomCancel(GSPacketIn pkg)
        {
            ProxyRoom room = (ProxyRoom)null;
            lock (this.dictionary_0)
            {
                if (this.dictionary_0.ContainsKey(pkg.Parameter1))
                    room = this.dictionary_0[pkg.Parameter1];
            }
            if (room == null)
                return;
            ProxyRoomMgr.RemoveRoom(room);
        }

        public void HanleSendToGame(GSPacketIn pkg)
        {
            BaseGame game = GameMgr.FindGame(pkg.ClientID);
            if (game == null)
                return;
            GSPacketIn pkg1 = pkg.ReadPacket();
            game.ProcessData(pkg1);
        }

        public void method_5(byte[] m, byte[] e)
        {
            GSPacketIn pkg = new GSPacketIn((short)0);
            pkg.Write(m);
            pkg.Write(e);
            this.SendTCP(pkg);
        }

        public void SendPacketToPlayer(int playerId, GSPacketIn pkg)
        {
            GSPacketIn pkg1 = new GSPacketIn((short)32, playerId);
            pkg1.WritePacket(pkg);
            this.SendTCP(pkg1);
        }

        public void SendRemoveRoom(int roomId)
        {
            this.SendTCP(new GSPacketIn((short)65, roomId));
        }

        public void SendToRoom(int roomId, GSPacketIn pkg, IGamePlayer except)
        {
            GSPacketIn pkg1 = new GSPacketIn((short)67, roomId);
            if (except != null)
            {
                pkg1.Parameter1 = except.PlayerCharacter.ID;
                pkg1.Parameter2 = except.GameId;
            }
            else
            {
                pkg1.Parameter1 = 0;
                pkg1.Parameter2 = 0;
            }
            pkg1.WritePacket(pkg);
            this.SendTCP(pkg1);
        }

        public void SendStartGame(int roomId, AbstractGame game)
        {
            GSPacketIn pkg = new GSPacketIn((short)66);
            pkg.Parameter1 = roomId;
            pkg.Parameter2 = game.Id;
            pkg.WriteInt((int)game.RoomType);
            pkg.WriteInt((int)game.GameType);
            pkg.WriteInt(game.TimeType);
            this.SendTCP(pkg);
        }

        public void SendStopGame(int roomId, int gameId)
        {
            this.SendTCP(new GSPacketIn((short)68)
            {
                Parameter1 = roomId,
                Parameter2 = gameId
            });
        }

        public void SendGamePlayerId(IGamePlayer player)
        {
            this.SendTCP(new GSPacketIn((short)33)
            {
                Parameter1 = player.PlayerCharacter.ID,
                Parameter2 = player.GameId
            });
        }

        public void SendAddRobRiches(int playerId, int value)
        {
            GSPacketIn pkg = new GSPacketIn((short)52, playerId);
            pkg.Parameter1 = value;
            pkg.WriteInt(value);
            this.SendTCP(pkg);
        }

        public void SendPlayerAddOffer(int playerId, int value)
        {
            this.SendTCP(new GSPacketIn((short)51, playerId)
            {
                Parameter1 = value
            });
        }

        public void SendDisconnectPlayer(int playerId)
        {
            this.SendTCP(new GSPacketIn((short)34, playerId));
        }

        public void SendPlayerOnGameOver(
          int playerId,
          int gameId,
          bool isWin,
          int gainXp,
          bool isSpanArea,
          bool isCouple,
          int blood,
          int playerCount)
        {
            GSPacketIn pkg = new GSPacketIn((short)35, playerId)
            {
                Parameter1 = gameId
            };
            pkg.WriteBoolean(isWin);
            pkg.WriteInt(gainXp);
            pkg.WriteBoolean(isSpanArea);
            pkg.WriteBoolean(isCouple);
            pkg.WriteInt(blood);
            pkg.WriteInt(playerCount);
            this.SendTCP(pkg);
        }

        public void SendFightAddOffer(int playerid, int offter)
        {
            this.SendTCP(new GSPacketIn((short)201, playerid)
            {
                Parameter1 = offter
            });
        }

        public void SendFightOneBloodIsWin(int playerid, eRoomType roomType)
        {
            this.SendTCP(new GSPacketIn((short)200, playerid)
            {
                Parameter1 = (int)roomType
            });
        }

        public void SendTakeCard(int playerid, int roomType, int place, int templateId, int count)
        {
            GSPacketIn pkg = new GSPacketIn((short)666, playerid);
            pkg.WriteInt(roomType);
            pkg.WriteInt(place);
            pkg.WriteInt(templateId);
            pkg.WriteInt(count);
            this.SendTCP(pkg);
        }

        public void SendPlayerUsePropInGame(
          int playerId,
          int bag,
          int place,
          int templateId,
          bool isLiving)
        {
            GSPacketIn pkg = new GSPacketIn((short)36, playerId);
            pkg.Parameter1 = bag;
            pkg.Parameter2 = place;
            pkg.WriteInt(templateId);
            pkg.WriteBoolean(isLiving);
            this.SendTCP(pkg);
        }

        public void SendPlayerAddGold(int playerId, int value)
        {
            this.SendTCP(new GSPacketIn((short)38, playerId)
            {
                Parameter1 = value
            });
        }

        public void SendPlayerAddMoney(int playerId, int value, bool isAll)
        {
            this.SendTCP(new GSPacketIn((short)70, playerId)
            {
                Parameter1 = value,
                Parameter2 = isAll ? 1 : 0
            });
        }

        public void SendPlayerAddMoneyLock(int playerId, int value)
        {
            this.SendTCP(new GSPacketIn((short)202, playerId)
            {
                Parameter1 = value
            });
        }

        public void SendPlayerAddGiftToken(int playerId, int value)
        {
            this.SendTCP(new GSPacketIn((short)71, playerId)
            {
                Parameter1 = value
            });
        }

        public void SendAddEliteGameScore(int playerId, int value)
        {
            this.SendTCP(new GSPacketIn((short)204, playerId)
            {
                Parameter1 = value
            });
        }

        public void SendRemoveEliteGameScore(int playerId, int value)
        {
            this.SendTCP(new GSPacketIn((short)205, playerId)
            {
                Parameter1 = value
            });
        }

        public void SendEliteGameWinUpdate(int playerId)
        {
            this.SendTCP(new GSPacketIn((short)206, playerId));
        }

        public void SendPlayerAddGP(int playerId, int value)
        {
            this.SendTCP(new GSPacketIn((short)39, playerId)
            {
                Parameter1 = value
            });
        }

        public void SendPlayerRemoveGP(int playerId, int value)
        {
            this.SendTCP(new GSPacketIn((short)49, playerId)
            {
                Parameter1 = value
            });
        }

        public void SendPlayerOnKillingLiving(
          int playerId,
          AbstractGame game,
          int type,
          int id,
          bool isLiving,
          int demage)
        {
            GSPacketIn pkg = new GSPacketIn((short)40, playerId);
            pkg.WriteInt(type);
            pkg.WriteBoolean(isLiving);
            pkg.WriteInt(demage);
            this.SendTCP(pkg);
        }

        public void SendPlayerOnMissionOver(
          int playerId,
          AbstractGame game,
          bool isWin,
          int MissionID,
          int turnNum)
        {
            GSPacketIn pkg = new GSPacketIn((short)41, playerId);
            pkg.WriteBoolean(isWin);
            pkg.WriteInt(MissionID);
            pkg.WriteInt(turnNum);
            this.SendTCP(pkg);
        }

        public void SendPlayerConsortiaFight(
          int playerId,
          int consortiaWin,
          int consortiaLose,
          Dictionary<int, Player> players,
          eRoomType roomType,
          eGameType gameClass,
          int totalKillHealth)
        {
            GSPacketIn pkg = new GSPacketIn((short)42, playerId);
            pkg.WriteInt(consortiaWin);
            pkg.WriteInt(consortiaLose);
            pkg.WriteInt(players.Count);
            foreach (Player player in players.Values)
                pkg.WriteInt(player.PlayerDetail.PlayerCharacter.ID);
            pkg.WriteByte((byte)roomType);
            pkg.WriteByte((byte)gameClass);
            pkg.WriteInt(totalKillHealth);
            this.SendTCP(pkg);
        }

        public void SendPlayerSendConsortiaFight(
          int playerId,
          int consortiaID,
          int riches,
          string msg)
        {
            GSPacketIn pkg = new GSPacketIn((short)43, playerId);
            pkg.WriteInt(consortiaID);
            pkg.WriteInt(riches);
            pkg.WriteString(msg);
            this.SendTCP(pkg);
        }

        public void SendPlayerRemoveGold(int playerId, int value)
        {
            GSPacketIn pkg = new GSPacketIn((short)44, playerId);
            pkg.WriteInt(value);
            this.SendTCP(pkg);
        }

        public void SendPlayerRemoveMoney(int playerId, int value)
        {
            GSPacketIn pkg = new GSPacketIn((short)45, playerId);
            pkg.WriteInt(value);
            this.SendTCP(pkg);
        }

        public void SendPlayerRemoveOffer(int playerId, int value)
        {
            GSPacketIn pkg = new GSPacketIn((short)50, playerId);
            pkg.WriteInt(value);
            this.SendTCP(pkg);
        }

        public void SendPlayerAddTemplate(
          int playerId,
          ItemInfo cloneItem,
          eBageType bagType,
          int count)
        {
            if (cloneItem == null)
                return;
            GSPacketIn pkg = new GSPacketIn((short)48, playerId);
            pkg.WriteInt(cloneItem.TemplateID);
            pkg.WriteByte((byte)bagType);
            pkg.WriteInt(count);
            pkg.WriteInt(cloneItem.ValidDate);
            pkg.WriteBoolean(cloneItem.IsBinds);
            pkg.WriteBoolean(cloneItem.IsUsed);
            this.SendTCP(pkg);
        }

        public void SendConsortiaAlly(int Consortia1, int Consortia2, int GameId)
        {
            GSPacketIn pkg = new GSPacketIn((short)69);
            pkg.WriteInt(Consortia1);
            pkg.WriteInt(Consortia2);
            pkg.WriteInt(GameId);
            this.SendTCP(pkg);
        }

        public void SendBeginFightNpc(int playerId, int RoomType, int GameType, int OrientRoomId)
        {
            GSPacketIn pkg = new GSPacketIn((short)203);
            pkg.Parameter1 = playerId;
            pkg.WriteInt(RoomType);
            pkg.WriteInt(GameType);
            pkg.WriteInt(OrientRoomId);
            this.SendTCP(pkg);
        }

        public void SendPlayerRemoveHealstone(int playerId)
        {
            this.SendTCP(new GSPacketIn((short)73, playerId));
        }

        public ServerClient(FightServer svr)
          : base(new byte[8192], new byte[8192])
        {
            this.dictionary_0 = new Dictionary<int, ProxyRoom>();
            this.fightServer_0 = svr;
        }

        public override string ToString()
        {
            return string.Format("Server Client: {0} IsConnected:{1}  RoomCount:{2}", (object)0, (object)this.IsConnected, (object)this.dictionary_0.Count);
        }

        public void RemoveRoom(int orientId, ProxyRoom room)
        {
            bool flag = false;
            lock (this.dictionary_0)
            {
                if (this.dictionary_0.ContainsKey(orientId))
                {
                    if (this.dictionary_0[orientId] == room)
                        flag = this.dictionary_0.Remove(orientId);
                }
            }
            if (!flag)
                return;
            this.SendRemoveRoom(orientId);
        }
    }
}
