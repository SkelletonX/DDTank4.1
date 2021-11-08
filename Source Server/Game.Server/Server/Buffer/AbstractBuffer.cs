// Decompiled with JetBrains decompiler
// Type: Game.Server.Buffer.AbstractBuffer
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Buffer
{
  public class AbstractBuffer
  {
    protected BufferInfo m_info;
    protected GamePlayer m_player;

    public AbstractBuffer(BufferInfo info)
    {
      this.m_info = info;
    }

    public bool Check()
    {
      return DateTime.Compare(this.m_info.BeginDate.AddMinutes((double) this.m_info.ValidDate), DateTime.Now) >= 0;
    }

    public virtual void Restore(GamePlayer player)
    {
      this.Start(player);
    }

    public virtual void Start(GamePlayer player)
    {
      this.m_info.UserID = player.PlayerId;
      this.m_info.IsExist = true;
      this.m_player = player;
      this.m_player.BufferList.AddBuffer(this);
    }

    public virtual void Stop()
    {
      this.m_info.IsExist = false;
      this.m_player.BufferList.RemoveBuffer(this);
      this.m_player = (GamePlayer) null;
    }

    public bool IsPayBuff()
    {
      return this.IsPayBuff(this.m_info.Type);
    }

    public bool IsPayBuff(int type)
    {
      return (uint) (type - 50) <= 2U || (uint) (type - 70) <= 3U;
    }

    public BufferInfo Info
    {
      get
      {
        return this.m_info;
      }
    }
  }
}
