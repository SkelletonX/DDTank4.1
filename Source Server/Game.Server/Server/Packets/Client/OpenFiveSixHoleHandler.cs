using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler(217, "游戏创建")]
    public class OpenFiveSixHoleHandler : IPacketHandler
    {
        public static Random random;

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int slot = packet.ReadInt();
            int num = packet.ReadInt();
            int templateId = packet.ReadInt();
            if (DateTime.Compare(client.Player.LastOpenHole.AddMilliseconds(100.0), DateTime.Now) > 0)
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("GameServer.OpenHole.TooQuickly"));
                return 0;
            }
            client.Player.LastOpenHole = DateTime.Now;
            ItemInfo itemAt = client.Player.StoreBag.GetItemAt(slot);
            if (itemAt != null && (itemAt.Template.CategoryID == 7 || itemAt.Template.CategoryID == 1 || itemAt.Template.CategoryID == 5))
            {
                ItemInfo itemByTemplateID = client.Player.PropBag.GetItemByTemplateID(0, templateId);
                if (itemByTemplateID == null || itemByTemplateID.Count <= 0)
                {
                    return 0;
                }
                bool val = false;
                switch (num)
                {
                    default:
                        Console.WriteLine("no have hole " + num);
                        break;
                    case 6:
                        if (itemByTemplateID.isDrill(itemAt.Hole6Level))
                        {
                            client.Player.PropBag.RemoveCountFromStack(itemByTemplateID, 1);
                            int num3 = random.Next(itemByTemplateID.Template.Property7, itemByTemplateID.Template.Property8);
                            itemAt.Hole6Exp += num3;
                            switch (itemAt.Hole6Level)
                            {
                                case 0:
                                    if (itemAt.Hole6Exp >= int.Parse(GameProperties.HoleLevelUpExpList.Split('|')[0]))
                                    {
                                        itemAt.Hole6Level++;
                                        itemAt.Hole6Exp = 0;
                                        val = true;
                                    }
                                    break;
                                case 1:
                                    if (itemAt.Hole6Exp >= int.Parse(GameProperties.HoleLevelUpExpList.Split('|')[1]))
                                    {
                                        itemAt.Hole6Level++;
                                        itemAt.Hole6Exp = 0;
                                        val = true;
                                    }
                                    break;
                                case 2:
                                    if (itemAt.Hole6Exp >= int.Parse(GameProperties.HoleLevelUpExpList.Split('|')[2]))
                                    {
                                        itemAt.Hole6Level++;
                                        itemAt.Hole6Exp = 0;
                                        val = true;
                                    }
                                    break;
                                case 3:
                                    if (itemAt.Hole6Exp >= int.Parse(GameProperties.HoleLevelUpExpList.Split('|')[3]))
                                    {
                                        itemAt.Hole6Level++;
                                        itemAt.Hole6Exp = 0;
                                        val = true;
                                    }
                                    break;
                                case 4:
                                    if (itemAt.Hole6Exp >= int.Parse(GameProperties.HoleLevelUpExpList.Split('|')[4]))
                                    {
                                        itemAt.Hole6Level++;
                                        itemAt.Hole6Exp = 0;
                                        val = true;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            client.Player.SendMessage("Cấp mũi khoan không phù hợp.");
                        }
                        break;
                    case 5:
                        if (itemByTemplateID.isDrill(itemAt.Hole5Level))
                        {
                            client.Player.PropBag.RemoveCountFromStack(itemByTemplateID, 1);
                            int num2 = random.Next(itemByTemplateID.Template.Property7, itemByTemplateID.Template.Property8);
                            itemAt.Hole5Exp += num2;
                            switch (itemAt.Hole5Level)
                            {
                                case 0:
                                    if (itemAt.Hole5Exp >= int.Parse(GameProperties.HoleLevelUpExpList.Split('|')[0]))
                                    {
                                        itemAt.Hole5Level++;
                                        itemAt.Hole5Exp = 0;
                                        val = true;
                                    }
                                    break;
                                case 1:
                                    if (itemAt.Hole5Exp >= int.Parse(GameProperties.HoleLevelUpExpList.Split('|')[1]))
                                    {
                                        itemAt.Hole5Level++;
                                        itemAt.Hole5Exp = 0;
                                        val = true;
                                    }
                                    break;
                                case 2:
                                    if (itemAt.Hole5Exp >= int.Parse(GameProperties.HoleLevelUpExpList.Split('|')[2]))
                                    {
                                        itemAt.Hole5Level++;
                                        itemAt.Hole5Exp = 0;
                                        val = true;
                                    }
                                    break;
                                case 3:
                                    if (itemAt.Hole5Exp >= int.Parse(GameProperties.HoleLevelUpExpList.Split('|')[3]))
                                    {
                                        itemAt.Hole5Level++;
                                        itemAt.Hole5Exp = 0;
                                        val = true;
                                    }
                                    break;
                                case 4:
                                    if (itemAt.Hole5Exp >= int.Parse(GameProperties.HoleLevelUpExpList.Split('|')[4]))
                                    {
                                        itemAt.Hole5Level++;
                                        itemAt.Hole5Exp = 0;
                                        val = true;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            client.Player.SendMessage("Cấp mũi khoan không phù hợp.");
                        }
                        break;
                }
                client.Player.StoreBag.UpdateItem(itemAt);
                GSPacketIn gSPacketIn = new GSPacketIn(217);
                gSPacketIn.WriteByte(0);
                gSPacketIn.WriteBoolean(val);
                gSPacketIn.WriteInt(num);
                client.Player.SendTCP(gSPacketIn);
            }
            else
            {
                client.Player.SendMessage("Không thể đục lỗ");
            }
            return 0;
        }

        public OpenFiveSixHoleHandler()
        {


        }

        static OpenFiveSixHoleHandler()
        {

            random = new Random();
        }
    }
}
