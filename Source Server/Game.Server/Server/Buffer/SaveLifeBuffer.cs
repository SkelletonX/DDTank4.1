// Decompiled with JetBrains decompiler
// Type: Game.Server.Buffer.SaveLifeBuffer
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Buffer
{
  public class SaveLifeBuffer : AbstractBuffer
  {
    public SaveLifeBuffer(BufferInfo info)
      : base(info)
    {
    }

    public override void Start(GamePlayer player)
    {
      SaveLifeBuffer ofType = player.BufferList.GetOfType(typeof (SaveLifeBuffer)) as SaveLifeBuffer;
      if (ofType != null)
      {
        ofType.Info.ValidCount = this.m_info.ValidCount;
        player.BufferList.UpdateBuffer((AbstractBuffer) ofType);
      }
      else
        base.Start(player);
    }

    public override void Stop()
    {
      base.Stop();
    }
  }
}
