using Bussiness;
using Bussiness.Managers;
using Game.Base;
using Game.Base.Packets;
using Game.Server;
using Game.Server.GameObjects;
using Game.Server.Packets;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler(96, "场景用户离开")]
    public class HonorUpHandler : IPacketHandler
    {
        public HonorUpHandler()
        {


        }

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int num = packet.ReadByte();
            packet.ReadBoolean();
            if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Bag.Locked"));
                return 0;
            }
            if (num != 1 && num == 2)
            {
                TotemHonorTemplateInfo totemHonorTemplateInfo = TotemHonorMgr.FindTotemHonorTemplateInfo(client.Player.PlayerCharacter.MaxBuyHonor + 1);
                if (totemHonorTemplateInfo == null)
                {
                    return 0;
                }
                int needMoney = totemHonorTemplateInfo.NeedMoney;
                int addHonor = totemHonorTemplateInfo.AddHonor;
                if (client.Player.MoneyDirect(needMoney))
                {
                    client.Player.AddHonor(addHonor);
                    client.Player.AddMaxHonor(1);
                }
            }
            client.Player.Out.SendUpdateUpCount(client.Player.PlayerCharacter);
            return 0;
        }
    }
}