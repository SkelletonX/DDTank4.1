using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1203A : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1203A(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1203A, elementID)
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
            CASE1203A cE = living.PetEffectList.GetOfType(ePetEffectType.CASE1203A) as CASE1203A;
            if (cE != null)
            {
                cE.int_2 = ((this.int_2 > cE.int_2) ? this.int_2 : cE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.BeginNextTurn += new LivingEventHandle(this.method_1);
            player.BeginSelfTurn += new LivingEventHandle(this.method_2);
            player.PlayerClearBuffSkillPet += new PlayerEventHandle(this.method_0);
        }
        private void method_0(Player player_0)
        {
            this.Stop();
        }
        private void method_1(Living living_0)
        {
            if (this.int_6 == 0)
            {
                this.int_6 = 300;
                if (living_0.BaseDamage < (double)this.int_6)
                {
                    this.int_6 = (int)living_0.BaseDamage - 1;
                }
                living_0.BaseDamage -= (double)this.int_6;
            }
        }
        private void method_2(Living living_0)
        {
            this.int_1--;
            if (this.int_1 < 0)
            {
                this.Stop();
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.BaseDamage += (double)this.int_6;
            this.int_6 = 0;
            player.BeginNextTurn -= new LivingEventHandle(this.method_1);
            player.BeginSelfTurn -= new LivingEventHandle(this.method_2);
        }
    }
}
