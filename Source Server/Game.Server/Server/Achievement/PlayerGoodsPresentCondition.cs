// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.PlayerGoodsPresentCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;

namespace Game.Server.Achievement
{
  internal class PlayerGoodsPresentCondition : BaseUserRecord
  {
    public PlayerGoodsPresentCondition(GamePlayer player, int type)
      : base(player, type)
    {
    }

    public override void AddTrigger(GamePlayer player)
    {
    }

    private void player_PlayerGoodsPresent(int count)
    {
    }

    public override void RemoveTrigger(GamePlayer player)
    {
    }
  }
}
