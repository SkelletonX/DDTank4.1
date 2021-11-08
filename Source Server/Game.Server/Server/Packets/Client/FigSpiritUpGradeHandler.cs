using Bussiness;
using Bussiness.Managers;
using Game.Base;
using Game.Base.Packets;
using Game.Server;
using Game.Server.GameObjects;
using Game.Server.GameUtils;
using Game.Server.Packets;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Game.Server.Packets.Client
{
    [PacketHandler(209, "场景用户离开")]
    public class FigSpiritUpGradeHandler : IPacketHandler
    {
        private static int[] int_0;

        private static int int_1;

        static FigSpiritUpGradeHandler()
        {

            FigSpiritUpGradeHandler.int_0 = FightSpiritTemplateMgr.Exps();
            FigSpiritUpGradeHandler.int_1 = GameProperties.FightSpiritMaxLevel;
        }

        public FigSpiritUpGradeHandler()
        {


        }

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            if (client.Player.PlayerCharacter.Grade < 30)
            {
                client.Out.SendMessage(eMessageType.Normal, "Level 30 Mới Có Thể Sử Dụng Chiến Hồn");
                return 0;
            }
            int d = client.Player.PlayerCharacter.ID;
            packet.ReadByte();
            int num = packet.ReadInt();
            packet.ReadInt();
            packet.ReadInt();
            int num1 = packet.ReadInt();
            int num2 = packet.ReadInt();
            int num3 = packet.ReadInt();
            int num4 = packet.ReadInt();
            packet.ReadInt();
            ItemInfo itemByTemplateID = client.Player.PropBag.GetItemByTemplateID(0, num1);
            int itemCount = client.Player.PropBag.GetItemCount(num1);
            UserGemStone gemStone = client.Player.GetGemStone(num3);
            if (gemStone == null)
            {
                client.Player.Out.SendPlayerFigSpiritUp(d, gemStone, false, true, true, 0, 0);
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Thành công!", new object[0]));
                return 0;
            }
            string[] strArrays = gemStone.FigSpiritIdValue.Split(new char[] { '|' });
            bool flag = false;
            bool flag1 = this.method_1(strArrays);
            bool flag2 = true;
            int num5 = 1;
            int num6 = 0;
            FigSpiritUpInfo[] figSpiritUpInfoArray = new FigSpiritUpInfo[(int)strArrays.Length];
            for (int i = 0; i < (int)strArrays.Length; i++)
            {
                FigSpiritUpInfo figSpiritUpInfo = new FigSpiritUpInfo()
                {
                    level = Convert.ToInt32(strArrays[i].Split(new char[] { ',' })[0]),
                    exp = Convert.ToInt32(strArrays[i].Split(new char[] { ',' })[1]),
                    place = Convert.ToInt32(strArrays[i].Split(new char[] { ',' })[2])
                };
                figSpiritUpInfoArray[i] = figSpiritUpInfo;
            }
            if (itemCount <= 0 || itemByTemplateID == null)
            {
                client.Player.Out.SendPlayerFigSpiritUp(d, gemStone, flag, flag1, flag2, 0, num6);
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Thành công!", new object[0]));
                return 0;
            }
            if (!itemByTemplateID.isGemStone())
            {
                client.Player.Out.SendPlayerFigSpiritUp(d, gemStone, flag, flag1, flag2, 0, num6);
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Thành công!!", new object[0]));
                return 0;
            }
            if (flag1)
            {
                client.Player.Out.SendPlayerFigSpiritUp(d, gemStone, flag, flag1, flag2, 0, num6);
                return 0;
            }
            FigSpiritUpInfo figSpiritUpInfo1 = this.method_3(figSpiritUpInfoArray, num4);
            if (figSpiritUpInfo1 == null)
            {
                client.Player.Out.SendPlayerFigSpiritUp(d, gemStone, flag, flag1, flag2, 0, num6);
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Phát hiện sai vị trí", new object[0]));
                return 0;
            }
            int property2 = 0;
            if (num != 1)
            {
                property2 = itemByTemplateID.Template.Property2;
                client.Player.PropBag.RemoveCountFromStack(itemByTemplateID, 1);
            }
            else
            {
                int num7 = this.method_0(figSpiritUpInfo1.exp, figSpiritUpInfo1.level) / itemByTemplateID.Template.Property2;
                if (itemCount < num7)
                {
                    num7 = itemCount;
                }
                property2 = itemByTemplateID.Template.Property2 * num7;
                client.Player.PropBag.RemoveTemplate(num1, num7);
            }
            if (figSpiritUpInfo1.level < FigSpiritUpGradeHandler.int_1)
            {
                FigSpiritUpInfo figSpiritUpInfo2 = figSpiritUpInfo1;
                figSpiritUpInfo2.exp = figSpiritUpInfo2.exp + property2;
                bool flag3 = this.method_2(figSpiritUpInfo1.exp, figSpiritUpInfo1.level);
                flag = flag3;
                if (flag3)
                {
                    FigSpiritUpInfo figSpiritUpInfo3 = figSpiritUpInfo1;
                    figSpiritUpInfo3.level = figSpiritUpInfo3.level + 1;
                    figSpiritUpInfo1.exp = 0;
                }
            }
            figSpiritUpInfoArray = this.method_4(figSpiritUpInfoArray, figSpiritUpInfo1, flag);
            if (flag)
            {
                flag2 = false;
                client.Player.EquipBag.UpdatePlayerProperties();
                num6 = 1;
            }
            string str = string.Concat(new object[] { figSpiritUpInfoArray[0].level, ",", figSpiritUpInfoArray[0].exp, ",", figSpiritUpInfoArray[0].place });
            for (int j = 1; j < (int)figSpiritUpInfoArray.Length; j++)
            {
                str = string.Concat(new object[] { str, "|", figSpiritUpInfoArray[j].level, ",", figSpiritUpInfoArray[j].exp, ",", figSpiritUpInfoArray[j].place });
            }
            gemStone.FigSpiritId = num2;
            gemStone.FigSpiritIdValue = str;
            client.Player.UpdateGemStone(num3, gemStone);
            client.Player.OnUserToemGemstoneEvent();
            client.Player.Out.SendPlayerFigSpiritUp(d, gemStone, flag, flag1, flag2, num5, num6);
            return 0;
        }

        private int method_0(int int_2, int int_3)
        {
            return FigSpiritUpGradeHandler.int_0[int_3 + 1] - (int_2 + FigSpiritUpGradeHandler.int_0[int_3]);
        }

        private bool method_1(string[] string_0)
        {
            if (string_0[0].Split(new char[] { ',' })[0] == FigSpiritUpGradeHandler.int_1.ToString())
            {
                if (string_0[1].Split(new char[] { ',' })[0] == FigSpiritUpGradeHandler.int_1.ToString())
                {
                    return string_0[2].Split(new char[] { ',' })[0] == FigSpiritUpGradeHandler.int_1.ToString();
                }
            }
            return false;
        }

        private bool method_2(int int_2, int int_3)
        {
            for (int i = 1; i < (int)FigSpiritUpGradeHandler.int_0.Length; i++)
            {
                if (int_2 >= FigSpiritUpGradeHandler.int_0[i] - FigSpiritUpGradeHandler.int_0[i - 1] && int_3 == i - 1)
                {
                    return true;
                }
            }
            return false;
        }

        private FigSpiritUpInfo method_3(FigSpiritUpInfo[] figSpiritUpInfo_0, int int_2)
        {
            FigSpiritUpInfo[] figSpiritUpInfo0 = figSpiritUpInfo_0;
            for (int i = 0; i < (int)figSpiritUpInfo0.Length; i++)
            {
                FigSpiritUpInfo figSpiritUpInfo = figSpiritUpInfo0[i];
                if (figSpiritUpInfo.place == int_2)
                {
                    return figSpiritUpInfo;
                }
            }
            return null;
        }

        private FigSpiritUpInfo[] method_4(FigSpiritUpInfo[] figSpiritUpInfo_0, FigSpiritUpInfo figSpiritUpInfo_1, bool bool_0)
        {
            for (int i = 0; i < (int)figSpiritUpInfo_0.Length; i++)
            {
                if (figSpiritUpInfo_0[i].place == figSpiritUpInfo_1.place)
                {
                    figSpiritUpInfo_0[i] = figSpiritUpInfo_1;
                }
            }
            if (!bool_0)
            {
                return figSpiritUpInfo_0;
            }
            IOrderedEnumerable<FigSpiritUpInfo> figSpiritUpInfo0 =
                from p in (IEnumerable<FigSpiritUpInfo>)figSpiritUpInfo_0
                orderby p.level, p.exp
                select p;
            FigSpiritUpInfo[] figSpiritUpInfoArray = new FigSpiritUpInfo[(int)figSpiritUpInfo_0.Length];
            int num = 0;
            foreach (FigSpiritUpInfo figSpiritUpInfo in figSpiritUpInfo0)
            {
                switch (num)
                {
                    case 0:
                        {
                            figSpiritUpInfo.place = 2;
                            break;
                        }
                    case 1:
                        {
                            figSpiritUpInfo.place = 1;
                            break;
                        }
                    case 2:
                        {
                            figSpiritUpInfo.place = 0;
                            break;
                        }
                }
                figSpiritUpInfoArray[num] = figSpiritUpInfo;
                num++;
            }
            return figSpiritUpInfoArray;
        }
    }
}