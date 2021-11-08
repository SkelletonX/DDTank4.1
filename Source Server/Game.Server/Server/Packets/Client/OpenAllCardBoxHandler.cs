// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.OpenAllCardBoxHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
    [PacketHandler(204, "打开物品")]
    public class OpenAllCardBoxHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            //abrir carta
            Console.WriteLine("Abrir Cartas");
            List<SqlDataProvider.Data.ItemInfo> items = client.Player.CaddyBag.GetItems();
            for(int x=0;x< items.Count;x++)
            {
                SqlDataProvider.Data.ItemInfo itemAt1 = items[x];
                if (itemAt1 != null)
                {
                    if (itemAt1.Count > 0)
                    {
                        int property5 = itemAt1.Template.Property5;
                        ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(property5);
                        if (itemTemplate != null && itemTemplate.CategoryID == 26)
                        {
                           
                            int count2 = 0;
                            Random random = new Random();
                            for (int index = 0; index < itemAt1.Count; ++index)
                                count2 += random.Next(1, 3);
                            if(client.Player.CardBag.AddCard(property5, count2))
                            {
                                client.Player.CaddyBag.RemoveCountFromStack(itemAt1, itemAt1.Count);
                            }
                            else
                            {
                                client.Player.SendMessage(string.Format("Quantidade Maxima de {0} Atingida", itemTemplate.Name));
                            }
                        }
                        else
                        {
                            client.Player.SendMessage("Erro ao adicionar a carta");
                        }
                    }
                }
                else
                {
                    client.Player.SendMessage("Esta caixa de cartão não existe");
                }
            }
            return 1;
        }
    }
}
