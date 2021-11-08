// Decompiled with JetBrains decompiler
// Type: Game.Server.Buffer.ConsortionAddDamageBuffer
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Buffer
{
  public class ConsortionAddDamageBuffer : AbstractBuffer
  {
    public ConsortionAddDamageBuffer(BufferInfo buffer)
      : base(buffer)
    {
    }

    public override void Start(GamePlayer player)
    {
      ConsortionAddDamageBuffer ofType = player.BufferList.GetOfType(typeof (ConsortionAddDamageBuffer)) as ConsortionAddDamageBuffer;
      if (ofType != null)
      {
        ofType.Info.ValidDate += this.m_info.ValidDate;
        ofType.Info.TemplateID = this.m_info.TemplateID;
        player.BufferList.UpdateBuffer((AbstractBuffer) ofType);
        player.UpdateFightBuff(this.Info);
      }
      else
      {
        base.Start(player);
        player.FightBuffs.Add(this.Info);
      }
    }

    public override void Stop()
    {
      this.m_player.FightBuffs.Remove(this.Info);
      base.Stop();
    }
  }
}
