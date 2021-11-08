// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.OpenVipHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.VIP_RENEWAL, "VIP")]
    public class OpenVipHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            string nickName = packet.ReadString();
            int num1 = packet.ReadInt();           
            string message1 = LanguageMgr.GetTranslation("OpenVipHandler.Msg1");
            int days = num1;
            int num5;
            switch (days)
            {
                case 31:
                    num5 = 799;
                    break;
                case 93:
                    num5 = 2397;
                    break;
                case 186:
                    num5 = 4794;
                    break;
                case 365:
                    num5 = 8150;
                    break;
                default:
                    num5 = num1 / 31 * 799;
                    break;
            }
            GamePlayer byPlayerNickName = WorldMgr.GetClientByPlayerNickName(nickName);           
            if (client.Player.MoneyDirect(num5))
            {
                DateTime now = DateTime.Now;
                using (PlayerBussiness playerBussiness = new PlayerBussiness())
                {

                    playerBussiness.VIPRenewal(nickName, num1, days, ref now);

                    if (byPlayerNickName == null)
                        message1 = "O jogador " + nickName + " não foi encontrado ou não está online!";
                    else if (client.Player.PlayerCharacter.NickName == nickName)
                    {
                        if (client.Player.PlayerCharacter.typeVIP == 0)
                        {
                            client.Player.OpenVIP(days, now);
                        }
                        else
                        {
                            client.Player.ContinuousVIP(days, now);
                            message1 = LanguageMgr.GetTranslation("OpenVipHandler.Msg3");
                        }
                        client.Out.SendOpenVIP(client.Player);
                    }
                    else
                    {
                        string message2;
                        if (byPlayerNickName.PlayerCharacter.typeVIP == 0)
                        {
                            byPlayerNickName.OpenVIP(days, now);
                            message1 = "O VIP do jogador " + nickName + " foi aberto com sucesso!";
                            message2 = client.Player.PlayerCharacter.NickName + " abriu o VIP para você!";
                        }
                        else
                        {
                            byPlayerNickName.ContinuousVIP(days, now);
                            message1 = "O VIP do jogador " + nickName + " foi renovado com sucesso!";
                            message2 = client.Player.PlayerCharacter.NickName + " renovou o seu VIP!";
                        }
                        byPlayerNickName.Out.SendOpenVIP(byPlayerNickName);
                        byPlayerNickName.Out.SendMessage(eMessageType.GM_NOTICE, message2);
                    }
                    client.Out.SendMessage(eMessageType.GM_NOTICE, message1);
                    if (client.Player.PlayerCharacter.typeVIP > 0)
                        client.Player.PlayerCharacter.VIPNextLevelDaysNeeded = client.Player.GetVIPNextLevelDaysNeeded(client.Player.PlayerCharacter.VIPLevel, client.Player.PlayerCharacter.VIPExp);
                }
            }
            return 0;
        }
    }
}
