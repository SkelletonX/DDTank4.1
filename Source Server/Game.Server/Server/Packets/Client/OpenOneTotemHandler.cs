using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server;
//using Game.Server.DragonBoat;
using Game.Server.GameObjects;
using Game.Server.GameUtils;
using Game.Server.Packets;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler(136, "场景用户离开")]
    public class OpenOneTotemHandler : IPacketHandler
    {
        public OpenOneTotemHandler()
        {


        }

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            if (client.Player.PlayerCharacter.Grade < 20)
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("OpenOneTotemHandler.Msg1"));
                return 0;
            }
            if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Bag.Locked"));
                return 1;
            }
            int playerCharacter = client.Player.PlayerCharacter.totemId + 1;
            if (playerCharacter <= 10000)
            {
                playerCharacter = 10001;
            }
            if (playerCharacter > TotemMgr.MaxTotem())
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("OpenOneTotemHandler.Maxlevel"));
                client.Player.Out.SendPlayerRefreshTotem(client.Player.PlayerCharacter);
                return 1;
            }
            TotemInfo totemInfo = TotemMgr.FindTotemInfo(playerCharacter);
            if (totemInfo == null)
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("OpenOneTotemHandler.ErrorData"));
                client.Player.Out.SendPlayerRefreshTotem(client.Player.PlayerCharacter);
                return 1;
            }
            int consumeExp = totemInfo.ConsumeExp;
            int consumeHonor = totemInfo.ConsumeHonor;
            if (client.Player.PlayerCharacter.myHonor < consumeHonor)
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("OpenOneTotemHandler.Msg2"));
            }
            else if (client.Player.MoneyDirect(consumeExp))
            {
                client.Player.AddTotem(playerCharacter);
                client.Player.RemovemyHonor(consumeHonor);
                client.Player.Out.SendPlayerRefreshTotem(client.Player.PlayerCharacter);
                client.Player.EquipBag.UpdatePlayerProperties();
                client.Player.OnUserToemGemstoneEvent();
            }
            return 0;
        }
    }
}