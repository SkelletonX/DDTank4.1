// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserLoginHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Interface;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
using System.Text;

namespace Game.Server.Packets.Client
{
    [PacketHandler(1, "User Login handler")]
    public class UserLoginHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            try
            {
                packet.Clone().ClearContext();
                if (client.Player == null)
                {
                    int num = packet.ReadInt();
                    int version = packet.ReadInt();
                    byte[] data = new byte[8];
                    byte[] rgb = packet.ReadBytes();
                    byte[] bytes;
                    try
                    {
                        bytes = WorldMgr.RsaCryptor.Decrypt(rgb, false);
                    }
                    catch (ExecutionEngineException ex)
                    {
                        client.Out.SendKitoff(LanguageMgr.GetTranslation("UserLoginHandler.RsaCryptorError"));
                        client.Disconnect();
                        GameServer.log.Error((object)"ExecutionEngineException", (Exception)ex);
                        return 0;
                    }
                    catch (Exception ex)
                    {
                        client.Out.SendKitoff(LanguageMgr.GetTranslation("UserLoginHandler.RsaCryptorError"));
                        client.Disconnect();
                        GameServer.log.Error((object)"RsaCryptor", ex);
                        return 0;
                    }
                    for (int index = 0; index < 8; ++index)
                        data[index] = bytes[index + 7];
                    client.setKey(data);
                    string[] strArray = Encoding.UTF8.GetString(bytes, 15, bytes.Length - 15).Split(',');
                    if (strArray.Length == 2)
                    {
                        string str = strArray[0];
                        string pass = strArray[1];
                        if (!LoginMgr.ContainsUser(str))
                        {
                            bool isFirst = false;
                            BaseInterface baseInterface = BaseInterface.CreateInterface();
                            PlayerInfo info = BaseInterface.CreateInterface().LoginGame(str, pass, ref isFirst);
                            if (info != null && (uint)info.ID != 0)
                            {
                                if (info.ID == -2)
                                {
                                    client.Out.SendKitoff(LanguageMgr.GetTranslation("UserLoginHandler.Forbid"));
                                    client.Disconnect();
                                    return 0;
                                }
                                if (!isFirst)
                                {

                                    if (version == 1)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("[Proteção] A Conta: {0} está sendo executada em uma aplicação de ataque, por isso foi desconectada.", str);
                                        Console.ResetColor();
                                        client.Out.SendKitoff("Você não pode usar bot para realizar login !");
                                        client.Disconnect();
                                    }
                                    else
                                    {
                                        client.Player = new GamePlayer(info.ID, str, client, info);
                                        LoginMgr.Add(info.ID, client);
                                        client.Server.LoginServer.SendAllowUserLogin(info.ID);
                                        client.Version = num;
                                        Console.WriteLine("O jogador [{0}] acabou de realizar o login...",str);
                                    }

                                }
                                else
                                {
                                    client.Out.SendKitoff(LanguageMgr.GetTranslation("UserLoginHandler.Register"));
                                    client.Disconnect();
                                }
                            }
                            else
                            {
                                client.Out.SendKitoff(LanguageMgr.GetTranslation("UserLoginHandler.OverTime"));
                                client.Disconnect();
                            }
                        }
                        else
                        {
                            client.Out.SendKitoff(LanguageMgr.GetTranslation("UserLoginHandler.LoginError"));
                            client.Disconnect();
                        }
                    }
                    else
                    {
                        client.Out.SendKitoff(LanguageMgr.GetTranslation("UserLoginHandler.LengthError"));
                        client.Disconnect();
                    }
                }
            }
            catch (Exception ex)
            {
                client.Out.SendKitoff(LanguageMgr.GetTranslation("UserLoginHandler.ServerError"));
                client.Disconnect();
                GameServer.log.Error((object)LanguageMgr.GetTranslation("UserLoginHandler.ServerError"), ex);
            }
            return 1;
        }
    }
}
