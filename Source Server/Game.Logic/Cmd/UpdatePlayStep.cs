// Decompiled with JetBrains decompiler
// Type: Game.Logic.Cmd.UpdatePlayStep
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Base.Packets;
using Game.Logic.Phy.Object;
using log4net;
using System.Reflection;

namespace Game.Logic.Cmd
{
  [GameCommand(25, "希望成为队长")]
  public class UpdatePlayStep : ICommandHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
    {
      packet.ReadInt();
      packet.ReadString();
    }
  }
}
