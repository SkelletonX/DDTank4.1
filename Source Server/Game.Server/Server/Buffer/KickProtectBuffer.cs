// Decompiled with JetBrains decompiler
// Type: Game.Server.Buffer.KickProtectBuffer
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Buffer
{
  public class KickProtectBuffer : AbstractBuffer
  {
    public KickProtectBuffer(BufferInfo info)
      : base(info)
    {
    }

    public override void Start(GamePlayer player)
    {
      KickProtectBuffer ofType = player.BufferList.GetOfType(typeof (KickProtectBuffer)) as KickProtectBuffer;
      if (ofType != null)
      {
        ofType.Info.ValidDate += this.Info.ValidDate;
        player.BufferList.UpdateBuffer((AbstractBuffer) ofType);
      }
      else
      {
        base.Start(player);
        player.KickProtect = true;
      }
    }

    public override void Stop()
    {
      this.m_player.KickProtect = false;
      base.Stop();
    }
  }
}
