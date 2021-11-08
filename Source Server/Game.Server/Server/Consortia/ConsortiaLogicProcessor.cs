// Decompiled with JetBrains decompiler
// Type: Game.Server.Consortia.ConsortiaLogicProcessor
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.Consortia.Handle;
using Game.Server.GameObjects;
using Game.Server.Packets;
using log4net;
using System;
using System.Reflection;

namespace Game.Server.Consortia
{
  [ConsortiaProcessorAtribute(99, "礼堂逻辑")]
  public class ConsortiaLogicProcessor : AbstractConsortiaProcessor
  {
    private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private ConsortiaHandleMgr consortiaHandleMgr_0;

    public ConsortiaLogicProcessor()
    {
      this.consortiaHandleMgr_0 = new ConsortiaHandleMgr();
    }

    public override void OnGameData(GamePlayer player, GSPacketIn packet)
    {
      ConsortiaPackageType consortiaPackageType = (ConsortiaPackageType) packet.ReadInt();
      try
      {
        IConsortiaCommandHadler consortiaCommandHadler = this.consortiaHandleMgr_0.LoadCommandHandler((int) consortiaPackageType);
        if (consortiaCommandHadler != null)
        {
          consortiaCommandHadler.CommandHandler(player, packet);
        }
        else
        {
          Console.WriteLine("______________ERROR______________");
          Console.WriteLine("LoadCommandHandler not found!");
          Console.WriteLine("_______________END_______________");
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("______________ERROR______________");
        Console.WriteLine("ConsortiaLogicProcessor PackageType {0} not found! Log: {1}", (object) consortiaPackageType, (object) ex);
        Console.WriteLine("_______________END_______________");
      }
    }
  }
}
