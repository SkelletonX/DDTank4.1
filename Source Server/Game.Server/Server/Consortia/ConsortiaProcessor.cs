// Decompiled with JetBrains decompiler
// Type: Game.Server.Consortia.ConsortiaProcessor
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.GameObjects;

namespace Game.Server.Consortia
{
  public class ConsortiaProcessor
  {
    private static object object_0 = new object();
    private GInterface3 ginterface3_0;

    public ConsortiaProcessor(GInterface3 processor)
    {
      this.ginterface3_0 = processor;
    }

    public void ProcessData(GamePlayer player, GSPacketIn data)
    {
      lock (ConsortiaProcessor.object_0)
        this.ginterface3_0.OnGameData(player, data);
    }
  }
}
