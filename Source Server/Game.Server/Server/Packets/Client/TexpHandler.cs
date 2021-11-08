using Bussiness;
using Game.Base.Packets;
using Game.Server.Buffer;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler(99, "场景用户离开")]
    public class TexpHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int num = packet.ReadInt();
            int num2 = packet.ReadInt();
            int slot = packet.ReadInt();
            ItemInfo itemAt = client.Player.StoreBag.GetItemAt(slot);
            TexpInfo texp = client.Player.PlayerCharacter.Texp;
            if (itemAt != null && texp != null && itemAt.TemplateID == num2)
            {
                if (!itemAt.isTexp())
                {
                    client.Out.SendMessage(eMessageType.Normal, "Ocorreu um erro. Altere os canal e tente novamente.");
                    return 0;
                }
                int num3 = client.Player.PlayerCharacter.Grade;
                if (client.Player.UsePayBuff(BuffType.Train_Good))
                {
                    AbstractBuffer ofType = client.Player.BufferList.GetOfType(BuffType.Train_Good);
                    num3 += ofType.Info.Value;
                }
                if (texp.texpCount < num3)
                {
                    switch (num)
                    {
                        case 0:
                            texp.hpTexpExp += itemAt.Template.Property2;
                            client.Player.OnUsingItem(45005, 1);
                            break;
                        case 1:
                            texp.attTexpExp += itemAt.Template.Property2;
                            client.Player.OnUsingItem(45001, 1);
                            break;
                        case 2:
                            texp.defTexpExp += itemAt.Template.Property2;
                            client.Player.OnUsingItem(45002, 1);
                            break;
                        case 3:
                            texp.spdTexpExp += itemAt.Template.Property2;
                            client.Player.OnUsingItem(45003, 1);
                            break;
                        case 4:
                            texp.lukTexpExp += itemAt.Template.Property2;
                            client.Player.OnUsingItem(45004, 1);
                            break;
                    }
                    client.Player.StoreBag.RemoveCountFromStack(itemAt, 1);
                    client.Player.OnUsingItem(itemAt.TemplateID, 1);
                    texp.texpCount++;
                    texp.texpTaskCount++;
                    texp.texpTaskDate = DateTime.Now;
                    using (PlayerBussiness playerBussiness = new PlayerBussiness())
                    {
                        playerBussiness.UpdateUserTexpInfo(texp);
                    }
                    client.Player.PlayerCharacter.Texp = texp;
                    client.Player.EquipBag.UpdatePlayerProperties();
                }
                else
                {
                    client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Hoje, o limite de prática ​​atingiu o máximo."));
                }
                return 0;
            }
            client.Out.SendMessage(eMessageType.Normal, "Ocorreu um erro. Altere os canal e tente novamente.");
            return 0;
        }

        public TexpHandler()
        {


        }
    }
}
