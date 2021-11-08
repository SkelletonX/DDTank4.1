using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1068A : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1068A(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1068A, elementID)
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
            CASE1068A cE = living.PetEffectList.GetOfType(ePetEffectType.CASE1068A) as CASE1068A;
            if (cE != null)
            {
                cE.int_2 = ((this.int_2 > cE.int_2) ? this.int_2 : cE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.BeginSelfTurn += new LivingEventHandle(this.method_2);
            player.AfterKilledByLiving += new KillLivingEventHanlde(this.method_1);
            player.PlayerClearBuffSkillPet += new PlayerEventHandle(this.method_0);
            this.IsTrigger = true;
        }
        private void method_0(Player player_0)
        {
            this.Stop();
        }
        private void method_1(Living living_0, Living living_1, int int_7, int int_8)
        {
            if (this.IsTrigger)
            {
                this.int_6 = int_7 * 50 / 100;
                living_1.SyncAtTime = true;
                living_1.AddBlood(-this.int_6, 1);
                living_1.SyncAtTime = false;
                if (living_1.Blood <= 0)
                {
                    living_1.Die();
                    if (living_0 != null && living_0 is Player)
                    {
                        (living_0 as Player).PlayerDetail.OnKillingLiving(living_0.Game, 2, living_1.Id, living_1.IsLiving, this.int_6);
                    }
                }
                this.IsTrigger = false;
            }
        }
        private void method_2(Living living_0)
        {
            this.int_1--;
            this.IsTrigger = true;
            if (this.int_1 < 0)
            {
                this.int_6 = 0;
                this.Stop();
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.BeginSelfTurn -= new LivingEventHandle(this.method_2);
            player.AfterKilledByLiving -= new KillLivingEventHanlde(this.method_1);
        }
    }
}
