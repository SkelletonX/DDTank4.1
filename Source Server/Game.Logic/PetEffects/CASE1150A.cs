using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1150A : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        private Living living_0;
        public CASE1150A(int count, int probability, int type, int skillId, int delay, string elementID, Living source) : base(ePetEffectType.CASE1150A, elementID)
        {
            this.int_1 = count;
            this.int_4 = count;
            this.int_2 = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.int_3 = delay;
            this.int_5 = skillId;
            this.living_0 = source;
        }
        public override bool Start(Living living)
        {
            CASE1150A cE = living.PetEffectList.GetOfType(ePetEffectType.CASE1150A) as CASE1150A;
            if (cE != null)
            {
                cE.int_2 = ((this.int_2 > cE.int_2) ? this.int_2 : cE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.BeginNextTurn += new LivingEventHandle(this.FcyzjPuPi5);
            player.BeginSelfTurn += new LivingEventHandle(this.method_1);
            player.PlayerClearBuffSkillPet += new PlayerEventHandle(this.method_0);
        }
        private void method_0(Player player_0)
        {
            this.Stop();
        }
        private void FcyzjPuPi5(Living living_1)
        {
        }
        private void method_1(Living living_1)
        {
            this.int_1--;
            if (this.int_1 < 0)
            {
                this.Stop();
                return;
            }
            this.int_6 = living_1.MaxBlood * 10 / 10 / 100;
            living_1.AddBlood(-this.int_6, 1);
            if (living_1.Blood <= 0)
            {
                living_1.Die();
                if (this.living_0 != null && this.living_0 is Player)
                {
                    (this.living_0 as Player).PlayerDetail.OnKillingLiving(this.living_0.Game, 2, living_1.Id, living_1.IsLiving, this.int_6);
                }
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.Game.method_7(player, base.Info, false);
            player.BeginNextTurn -= new LivingEventHandle(this.FcyzjPuPi5);
            player.BeginSelfTurn -= new LivingEventHandle(this.method_1);
        }
    }
}
