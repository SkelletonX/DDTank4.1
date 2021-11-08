// Decompiled with JetBrains decompiler
// Type: Game.Server.Buffer.PropsBuffer
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Buffer
{
  public class PropsBuffer : AbstractBuffer
  {
    public PropsBuffer(BufferInfo buffer)
      : base(buffer)
    {
    }

    public override void Start(GamePlayer player)
    {
      PropsBuffer ofType = player.BufferList.GetOfType(typeof (PropsBuffer)) as PropsBuffer;
      if (ofType != null)
      {
        ofType.Info.ValidDate += this.Info.ValidDate;
        player.BufferList.UpdateBuffer((AbstractBuffer) ofType);
      }
      else
      {
        base.Start(player);
        player.CanUseProp = true;
      }
    }

    public override void Stop()
    {
      this.m_player.CanUseProp = false;
      base.Stop();
    }
  }
}
