// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.SceneChatHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.GameObjects;
using Game.Server.Managers;
using System;
using System.IO;

namespace Game.Server.Packets.Client
{
    [PacketHandler(19, "用户场景聊天")]
    public class SceneChatHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            packet.ClientID = client.Player.PlayerCharacter.ID;
            byte val = packet.ReadByte();
            bool flag = packet.ReadBoolean();
            packet.ReadString();
            string str = packet.ReadString();
            string[] strArray = str.Split('$');
            if (strArray.Length > 1 && strArray.Length <= 5)
            {
                if (strArray[1].Equals("ban") && this.CheckAdmin(client.Player.PlayerCharacter.ID, strArray[1]))
                {
                    DateTime date = DateTime.Now.AddYears(20);
                    if (strArray.Length >= 4 && strArray[3].Length >= 8)
                    {
                        if (strArray.Length == 5)
                        {
                            int result = 0;
                            if (int.TryParse(strArray[4], out result))
                            {
                                date = DateTime.Now.AddDays((double)result);
                            }
                            else
                            {
                                client.Player.SendMessage("Failed converting to days");
                                return 0;
                            }
                        }
                        using (ManageBussiness manageBussiness = new ManageBussiness())
                        {
                            if (manageBussiness.ForbidPlayerByNickName(strArray[2], date, false, strArray[3]))
                            {
                                foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
                                    allPlayer.SendMessage("O usuário " + strArray[2] + " foi banido do jogo.");
                                using (StreamWriter streamWriter = File.AppendText("BanLog.txt"))
                                    SceneChatHandler.Log(client.Player.PlayerCharacter.NickName + " banned the user " + strArray[2] + " for " + (object)(date - DateTime.Now).TotalDays + " days, reason: " + strArray[3], (TextWriter)streamWriter);
                            }
                            else
                                client.Player.SendMessage("Failed to ban the user " + strArray[2]);
                        }
                    }
                    else
                        client.Player.SendMessage("Comando em formato inválido. Use $ban$nick$motivo$dias ou $ban$nick$motivo para ban permanente. Motivo não pode ser vazio ou muito curto!");
                }
                if (strArray[1].Equals("bugbot") && this.CheckAdmin(client.Player.PlayerCharacter.ID, strArray[1]))
                {
                    if (client.Player.CurrentRoom != null)
                    {
                        Console.WriteLine(client.Player.CurrentRoom.RoomType);
                        if (client.Player.CurrentRoom.RoomType == eRoomType.Match && client.Player.CurrentRoom.IsPlaying)
                        {
                          bool IsBot = false;
                            System.Collections.Generic.List<Game.Logic.Phy.Object.Player> Jogadores = client.Player.game.GetAllEnemyPlayers((Game.Logic.Phy.Object.Living)(object)client.Player);
                          if(Jogadores == null || Jogadores.Count <=0)
                          {
                                client.Player.SendMessage("Sua Partida Não Foi Encontrada ou foi encerrada");
                          }
                          else
                            {
                                for (int x = 0; x < Jogadores.Count; x++)
                                {
                                    if (Jogadores[x].AutoBoot)
                                    {
                                        IsBot = true;
                                    }
                                    Console.WriteLine(Jogadores[x].Name);
                                }
                            }

                          if(!IsBot)
                          {
                            client.Player.SendMessage("Sua Partida Não tem Bots!");
                          }
                          else
                          {
                                client.Player.CurrentRoom.Stop();
                          }
                        }
                        else
                        {
                            client.Player.SendMessage("Você Não Esta Em Uma Partida PVP");
                        }
                    }
                    else
                        client.Player.SendMessage("Você Não Esta Em jogo!");
                    
                 
                }
                if (strArray[1].Equals("mute") && this.CheckAdmin(client.Player.PlayerCharacter.ID, strArray[1]))
                {
                    if (strArray.Length == 5 && strArray[4].Length >= 8)
                    {
                        DateTime dateTime = DateTime.Now.AddMinutes((double)Convert.ToInt32(strArray[3]));
                        GamePlayer byPlayerNickName = WorldMgr.GetClientByPlayerNickName(strArray[2]);
                        if (byPlayerNickName == null || dateTime <= DateTime.Now)
                        {
                            client.Player.SendMessage("Failed to mute the user" + strArray[2]);
                            return 0;
                        }
                        byPlayerNickName.PlayerCharacter.IsBanChat = true;
                        byPlayerNickName.PlayerCharacter.BanChatEndDate = dateTime;
                        using (StreamWriter streamWriter = File.AppendText("MuteLog.txt"))
                            SceneChatHandler.Log(client.Player.PlayerCharacter.NickName + " muted the user " + strArray[2] + " for " + strArray[3] + " minutes, reason: " + strArray[4], (TextWriter)streamWriter);
                    }
                    else
                        client.Player.SendMessage("Comando em formato inválido. Use $mute$nick$minutos$motivo. Motivo não pode ser vazio ou muito curto!");
                }
                if (strArray[1].Equals("unban") && this.CheckAdmin(client.Player.PlayerCharacter.ID, strArray[1]))
                {
                    if (strArray.Length == 4 && strArray[3].Length >= 8)
                    {
                        DateTime now = DateTime.Now;
                        using (ManageBussiness manageBussiness = new ManageBussiness())
                        {
                            if (manageBussiness.ForbidPlayerByNickName(strArray[2], now, true))
                            {
                                using (StreamWriter streamWriter = File.AppendText("BanLog.txt"))
                                    SceneChatHandler.Log(client.Player.PlayerCharacter.NickName + " unban the user " + strArray[2] + ", reason: " + strArray[3], (TextWriter)streamWriter);
                            }
                            else
                                client.Player.SendMessage("Failed to unban the user " + strArray[2]);
                        }
                    }
                    else
                        client.Player.SendMessage("Comando em formato inválido. Use $desban$nick$motivo. Motivo não pode ser vazio ou muito curto!");
                }
                if (strArray[1].Equals("kick") && this.CheckAdmin(client.Player.PlayerCharacter.ID, strArray[1]))
                {
                    if (strArray.Length == 4 && strArray[3].Length >= 8)
                    {
                        using (ManageBussiness manageBussiness = new ManageBussiness())
                        {
                            if (manageBussiness.KitoffUserByNickName(strArray[2], "Kick") == 0)
                            {
                                using (StreamWriter streamWriter = File.AppendText("KickLog.txt"))
                                    SceneChatHandler.Log(client.Player.PlayerCharacter.NickName + " kicked the user " + strArray[2] + ", reason: " + strArray[3], (TextWriter)streamWriter);
                            }
                            else
                                client.Player.SendMessage("Failed to kick the user " + strArray[2]);
                        }
                    }
                    else
                        client.Player.SendMessage("Comando em formato inválido. Use $kick$nick$motivo. Motivo não pode ser vazio ou muito curto!");
                }
            }
            else
            {
                GSPacketIn gsPacketIn = new GSPacketIn((short)19, client.Player.PlayerCharacter.ID);
                gsPacketIn.WriteInt(client.Player.ZoneId);
                gsPacketIn.WriteByte(val);
                gsPacketIn.WriteBoolean(flag);
                gsPacketIn.WriteString(client.Player.PlayerCharacter.NickName);
                gsPacketIn.WriteString(str);
                if (client.Player.CurrentRoom != null && client.Player.CurrentRoom.RoomType == eRoomType.Match && client.Player.CurrentRoom.Game != null)
                {
                    if (val != (byte)3)
                    {
                        client.Player.CurrentRoom.BattleServer.Server.SendChatMessage(str, client.Player, flag);
                    }
                    else
                    {
                        if (client.Player.PlayerCharacter.ConsortiaID == 0)
                            return 0;
                        if (client.Player.PlayerCharacter.IsBanChat)
                        {
                            client.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("ConsortiaChatHandler.IsBanChat"));
                            return 1;
                        }
                        gsPacketIn.WriteInt(client.Player.PlayerCharacter.ConsortiaID);
                        foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
                        {
                            if (allPlayer.PlayerCharacter.ConsortiaID == client.Player.PlayerCharacter.ConsortiaID && !allPlayer.IsBlackFriend(client.Player.PlayerCharacter.ID))
                                allPlayer.Out.SendTCP(gsPacketIn);
                        }
                    }
                    return 1;
                }
                switch (val)
                {
                    case 3:
                        if (client.Player.PlayerCharacter.ConsortiaID == 0)
                            return 0;
                        if (client.Player.PlayerCharacter.IsBanChat)
                        {
                            client.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("ConsortiaChatHandler.IsBanChat"));
                            return 1;
                        }
                        gsPacketIn.WriteInt(client.Player.PlayerCharacter.ConsortiaID);
                        foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
                        {
                            if (allPlayer.PlayerCharacter.ConsortiaID == client.Player.PlayerCharacter.ConsortiaID && !allPlayer.IsBlackFriend(client.Player.PlayerCharacter.ID))
                                allPlayer.Out.SendTCP(gsPacketIn);
                        }
                        break;
                    case 9:
                        if (client.Player.CurrentMarryRoom == null)
                            return 1;
                        client.Player.CurrentMarryRoom.SendToAllForScene(gsPacketIn, client.Player.MarryMap);
                        break;
                    case 13:
                        if (client.Player.CurrentHotSpringRoom == null)
                            return 1;
                        client.Player.CurrentHotSpringRoom.SendToRoomPlayer(gsPacketIn);
                        break;
                    default:
                        if (client.Player.CurrentRoom != null)
                        {
                            if (flag)
                            {
                                client.Player.CurrentRoom.SendToTeam(gsPacketIn, client.Player.CurrentRoomTeam, client.Player);
                                break;
                            }
                            client.Player.CurrentRoom.SendToAll(gsPacketIn);
                            break;
                        }
                        if ((uint)((DateTime.Compare(client.Player.LastChatTime.AddSeconds(1.0), DateTime.Now) <= 0 ? 0 : (val == (byte)5 ? 1 : 0)) | (flag ? 1 : 0)) > 0U)
                            return 1;
                        if (DateTime.Compare(client.Player.LastChatTime.AddSeconds(30.0), DateTime.Now) > 0)
                        {
                            client.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("SceneChatHandler.Fast"));
                            return 1;
                        }
                        client.Player.LastChatTime = DateTime.Now;
                        foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
                        {
                            if (allPlayer.CurrentRoom == null && allPlayer.CurrentMarryRoom == null && (allPlayer.CurrentHotSpringRoom == null && !allPlayer.IsBlackFriend(client.Player.PlayerCharacter.ID)))
                                allPlayer.Out.SendTCP(gsPacketIn);
                        }
                        break;
                }
            }
            return 1;
        }

        public bool CheckAdmin(int UserID, string Command)
        {
            return CommandsMgr.CheckAdmin(UserID, Command);
        }

        public static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", (object)DateTime.Now.ToLongTimeString(), (object)DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", (object)logMessage);
            w.WriteLine("-------------------------------");
        }
    }
}
