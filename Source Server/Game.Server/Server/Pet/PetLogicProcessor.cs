// Decompiled with JetBrains decompiler
// Type: Game.Server.Pet.PetLogicProcessor
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Packets;
using Game.Server.Pet.Handle;
using log4net;
using System;
using System.Reflection;

namespace Game.Server.Pet
{
  [PetProcessorAtribute(40, "礼堂逻辑")]
  public class PetLogicProcessor : AbstractPetProcessor
  {
    private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private PetHandleMgr petHandleMgr_0;

    public PetLogicProcessor()
    {
      this.petHandleMgr_0 = new PetHandleMgr();
    }

    public override void OnGameData(GamePlayer player, GSPacketIn packet)
    {
      PetPackageType petPackageType = (PetPackageType) packet.ReadByte();
      try
      {
        IPetCommandHadler petCommandHadler = this.petHandleMgr_0.LoadCommandHandler((int) petPackageType);
        if (petCommandHadler != null)
        {
          petCommandHadler.CommandHandler(player, packet);
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
        PetLogicProcessor.ilog_0.Error((object) string.Format("PetLogicProcessor PackageType {2} :{1}, OnGameData is Error: {0}", (object) ex.ToString(), (object) player.Client.TcpEndpoint, (object) petPackageType));
      }
    }
  }
}
