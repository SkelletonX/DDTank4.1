using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1211A : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1211A(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1211A, elementID)
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
            CASE1211A cE = living.PetEffectList.GetOfType(ePetEffectType.CASE1211A) as CASE1211A;
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
        }
        private void method_2(Living living_0)
        {
            this.int_1--;
            if (this.int_1 < 0)
            {
                this.Stop();
            }
            this.int_6 = 1000;
            living_0.AddBlood(-this.int_6, 1);
            if (living_0.Blood <= 0)
            {
                living_0.Die();
                if (living_0.Game.CurrentLiving != null && living_0.Game.CurrentLiving is Player)
                {
                    (living_0.Game.CurrentLiving as Player).PlayerDetail.OnKillingLiving(living_0.Game, 2, living_0.Id, living_0.IsLiving, this.int_6);
                }
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.BeginNextTurn -= new LivingEventHandle(this.method_1);
            player.BeginSelfTurn -= new LivingEventHandle(this.method_2);
        }
    }
}
