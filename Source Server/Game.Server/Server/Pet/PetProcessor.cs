// Decompiled with JetBrains decompiler
// Type: Game.Server.Pet.PetProcessor
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.GameObjects;

namespace Game.Server.Pet
{
  public class PetProcessor
  {
    private static object object_0 = new object();
    private IPetProcessor ipetProcessor_0;

    public PetProcessor(IPetProcessor processor)
    {
      this.ipetProcessor_0 = processor;
    }

    public void ProcessData(GamePlayer player, GSPacketIn data)
    {
      lock (PetProcessor.object_0)
        this.ipetProcessor_0.OnGameData(player, data);
    }
  }
}
