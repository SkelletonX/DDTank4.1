// Decompiled with JetBrains decompiler
// Type: Game.Server.HotSpringRooms.TankHotSpringLogicProcessor
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.HotSpringRooms.TankHandle;
using log4net;
using System;
using System.Reflection;

namespace Game.Server.HotSpringRooms
{
  [HotSpringProcessor(9, "礼堂逻辑")]
  public class TankHotSpringLogicProcessor : AbstractHotSpringProcessor
  {
    private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private HotSpringCommandMgr hotSpringCommandMgr_0 = new HotSpringCommandMgr();
    public readonly int TIMEOUT = 60000;
    private ThreadSafeRandom threadSafeRandom_0 = new ThreadSafeRandom();

    public override void OnGameData(HotSpringRoom room, GamePlayer player, GSPacketIn packet)
    {
      HotSpringCmdType hotSpringCmdType = (HotSpringCmdType) packet.ReadByte();
      try
      {
        IHotSpringCommandHandler springCommandHandler = this.hotSpringCommandMgr_0.LoadCommandHandler((int) hotSpringCmdType);
        if (springCommandHandler != null)
          springCommandHandler.HandleCommand(this, player, packet);
        else
          TankHotSpringLogicProcessor.ilog_0.Error((object) string.Format("IP: {0}", (object) player.Client.TcpEndpoint));
      }
      catch (Exception ex)
      {
        TankHotSpringLogicProcessor.ilog_0.Error((object) string.Format("IP:{1}, OnGameData is Error: {0}", (object) ex.ToString(), (object) player.Client.TcpEndpoint));
      }
    }

    public override void OnTick(HotSpringRoom room)
    {
    }
  }
}
