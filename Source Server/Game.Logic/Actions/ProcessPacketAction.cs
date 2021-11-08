// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.ProcessPacketAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Base.Packets;
using Game.Logic.Cmd;
using Game.Logic.Phy.Object;
using log4net;
using System;
using System.Reflection;

namespace Game.Logic.Actions
{
  public class ProcessPacketAction : IAction
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private GSPacketIn m_packet;
    private Player m_player;

    public ProcessPacketAction(Player player, GSPacketIn pkg)
    {
      this.m_player = player;
      this.m_packet = pkg;
    }

    public void Execute(BaseGame game, long tick)
    {
      if (!this.m_player.IsActive)
        return;
      eTankCmdType eTankCmdType = (eTankCmdType) this.m_packet.ReadByte();
      try
      {
        ICommandHandler commandHandler = CommandMgr.LoadCommandHandler((int) eTankCmdType);
        if (commandHandler != null)
          commandHandler.HandleCommand(game, this.m_player, this.m_packet);
        else
          ProcessPacketAction.log.Error((object) string.Format("Player Id: {0}", (object) this.m_player.Id));
      }
      catch (Exception ex)
      {
        ProcessPacketAction.log.Error((object) string.Format("Player Id: {0}  cmd:0x{1:X2}", (object) this.m_player.Id, (object) (byte) eTankCmdType), ex);
      }
    }

    public bool IsFinished(long tick)
    {
      return true;
    }
  }
}
