// Decompiled with JetBrains decompiler
// Type: Game.Server.Buffer.AttackBuffer
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Buffer
{
  public class AttackBuffer : AbstractBuffer
  {
    public AttackBuffer(BufferInfo buffer)
      : base(buffer)
    {
    }

    public override void Start(GamePlayer player)
    {
      AttackBuffer ofType = player.BufferList.GetOfType(typeof (AttackBuffer)) as AttackBuffer;
      if (ofType != null)
      {
        ofType.Info.ValidDate += this.Info.ValidDate;
        if (ofType.Info.ValidDate > 30)
          ofType.Info.ValidDate = 30;
        player.BufferList.UpdateBuffer((AbstractBuffer) ofType);
      }
      else
      {
        base.Start(player);
        player.PlayerCharacter.AttackAddPlus += this.Info.Value;
      }
    }

    public override void Stop()
    {
      this.m_player.PlayerCharacter.AttackAddPlus -= this.m_info.Value;
      base.Stop();
    }
  }
}
