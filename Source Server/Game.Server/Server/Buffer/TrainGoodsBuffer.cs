﻿// Decompiled with JetBrains decompiler
// Type: Game.Server.Buffer.TrainGoodsBuffer
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Buffer
{
  public class TrainGoodsBuffer : AbstractBuffer
  {
    public TrainGoodsBuffer(BufferInfo buffer)
      : base(buffer)
    {
    }

    public override void Start(GamePlayer player)
    {
      TrainGoodsBuffer ofType = player.BufferList.GetOfType(typeof (TrainGoodsBuffer)) as TrainGoodsBuffer;
      if (ofType != null)
      {
        ofType.Info.ValidDate += this.Info.ValidDate;
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
