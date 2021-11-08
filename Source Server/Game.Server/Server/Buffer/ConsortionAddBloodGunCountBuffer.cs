// Decompiled with JetBrains decompiler
// Type: Game.Server.Buffer.ConsortionAddBloodGunCountBuffer
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Buffer
{
  public class ConsortionAddBloodGunCountBuffer : AbstractBuffer
  {
    public ConsortionAddBloodGunCountBuffer(BufferInfo buffer)
      : base(buffer)
    {
    }

    public override void Start(GamePlayer player)
    {
      ConsortionAddBloodGunCountBuffer ofType = player.BufferList.GetOfType(typeof (ConsortionAddBloodGunCountBuffer)) as ConsortionAddBloodGunCountBuffer;
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
