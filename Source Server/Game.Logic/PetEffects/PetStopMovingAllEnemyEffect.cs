// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetStopMovingAllEnemyEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll
using System;
using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
    public class PetStopMovingAllEnemyEffect : BasePetEffect
    {
        private int m_count;
        private int m_currentId;
        private int m_delay;
        private int m_probability;
        private int m_type;

        public PetStopMovingAllEnemyEffect(
          int count,
          int probability,
          int type,
          int skillId,
          int delay,
          string elementID)
          : base(ePetEffectType.PetStopMovingAllEnemyEffect, elementID)
        {
            this.m_count = count;
            this.m_probability = probability == -1 ? 10000 : probability;
            this.m_type = type;
            this.m_delay = delay;
            this.m_currentId = skillId;
        }

        protected override void OnAttachedToPlayer(Player player)
        {
            player.PlayerBuffSkillPet += new PlayerEventHandle(this.player_AfterKilledByLiving);
        }

        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(this.player_AfterKilledByLiving);
        }

        private void player_AfterKilledByLiving(Living living)
        {
            if (this.rand.Next(10000) >= this.m_probability || living.PetEffects.CurrentUseSkill != this.m_currentId)
                return;
            foreach (Living allEnemyPlayer in living.Game.GetAllEnemyPlayers(living))
                allEnemyPlayer.AddPetEffect((AbstractPetEffect)new PetStopMovingEquip(this.m_count, this.Info.ID.ToString()), 0);
            Console.WriteLine("PET STOP MOVING ALL ENEMY");
        }

        public override bool Start(Living living)
        {
            PetStopMovingAllEnemyEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetStopMovingAllEnemyEffect) as PetStopMovingAllEnemyEffect;
            if (ofType == null)
                return base.Start(living);
            ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
            return true;
        }
    }
}
