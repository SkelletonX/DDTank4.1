using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1455A : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1455A(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1455A, elementID)
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
            CASE1455A cE = living.PetEffectList.GetOfType(ePetEffectType.CASE1455A) as CASE1455A;
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
            if (living_0 != living_1 && this.IsTrigger)
            {
                this.int_6 = living_0.MaxBlood * 5 / 100;
                if (living_0.IsLiving)
                {
                    living_0.SyncAtTime = true;
                    living_0.AddBlood(this.int_6);
                    living_0.SyncAtTime = false;
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
