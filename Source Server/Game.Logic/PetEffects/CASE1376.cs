﻿using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1376 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1376(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1376, elementID)
        {
            this.int_1 = count;
            this.int_4 = count;
            this.int_2 = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.int_3 = delay;
            this.int_5 = skillId;
        }
        public override bool Start(Living living)
        {
            CASE1376 pE = living.PetEffectList.GetOfType(ePetEffectType.CASE1376) as CASE1376;
            if (pE != null)
            {
                pE.int_2 = ((this.int_2 > pE.int_2) ? this.int_2 : pE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.BeginNextTurn += new LivingEventHandle(this.player_beginNextTurn);
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.BeginNextTurn -= new LivingEventHandle(this.player_beginNextTurn);
        }
        public void player_beginNextTurn(Living living)
        {
            if (living.Game.CurrentLiving is Player && living.Game.CurrentLiving != living)
            {
                bool arg_5F_0 = living.Game.CurrentLiving.PetEffectList.GetOfType(ePetEffectType.CASE1058) is CASE1058;
                CASE1063 cE = living.Game.CurrentLiving.PetEffectList.GetOfType(ePetEffectType.CASE1063) as CASE1063;
                if (arg_5F_0 || cE != null)
                {
                    (living as Player).AddPetMP(1);
                    Console.WriteLine("+1 MP");
                }
            }
        }
    }
}
