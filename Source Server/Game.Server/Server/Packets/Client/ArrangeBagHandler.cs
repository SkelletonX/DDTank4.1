// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.ArrangeBagHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.GameUtils;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Server.Packets.Client
{
  [PacketHandler(124, "Arrange Bag")]
  public class ArrangeBagHandler : IPacketHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      bool flag = packet.ReadBoolean();
      int num1 = packet.ReadInt();
      int num2 = packet.ReadInt();
      PlayerInventory inventory = client.Player.GetInventory((eBageType) num2);
      int capalility = inventory.Capalility;
      List<SqlDataProvider.Data.ItemInfo> items1 = inventory.GetItems(inventory.BeginSlot, capalility);
      int count = items1.Count;
      if (num1 == count)
      {
        inventory.BeginChanges();
        try
        {
          if (inventory.FindFirstEmptySlot(inventory.BeginSlot, capalility) != -1)
          {
            for (int index = 1; inventory.FindFirstEmptySlot(inventory.BeginSlot, capalility) < items1[items1.Count - index].Place; ++index)
              inventory.MoveItem(items1[items1.Count - index].Place, inventory.FindFirstEmptySlot(inventory.BeginSlot, capalility), items1[items1.Count - index].Count);
          }
        }
        finally
        {
          if (flag)
          {
            try
            {
              List<SqlDataProvider.Data.ItemInfo> items2 = inventory.GetItems(inventory.BeginSlot, capalility);
              List<int> intList = new List<int>();
              for (int index1 = 0; index1 < items2.Count; ++index1)
              {
                if (!intList.Contains(index1))
                {
                  for (int index2 = items2.Count - 1; index2 > index1; --index2)
                  {
                    if (!intList.Contains(index2) && items2[index1].TemplateID == items2[index2].TemplateID && items2[index1].CanStackedTo(items2[index2]))
                    {
                      inventory.MoveItem(items2[index2].Place, items2[index1].Place, items2[index2].Count);
                      intList.Add(index2);
                    }
                  }
                }
              }
            }
            finally
            {
              List<SqlDataProvider.Data.ItemInfo> items2 = inventory.GetItems(inventory.BeginSlot, capalility);
              if (inventory.FindFirstEmptySlot(inventory.BeginSlot, capalility) != -1)
              {
                for (int index = 1; inventory.FindFirstEmptySlot(inventory.BeginSlot, capalility) < items2[items2.Count - index].Place; ++index)
                  inventory.MoveItem(items2[items2.Count - index].Place, inventory.FindFirstEmptySlot(inventory.BeginSlot, capalility), items2[items2.Count - index].Count);
              }
            }
          }
          inventory.CommitChanges();
        }
      }
      else
        Console.WriteLine(string.Format("is not count equal  capability: {0}", (object) capalility));
      return 0;
    }
  }
}
