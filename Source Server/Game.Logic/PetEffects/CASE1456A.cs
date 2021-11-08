using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1456A : BasePetEffect
    {
        private int vmZliqNxiu;
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        public CASE1456A(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1456A, elementID)
        {
            this.int_0 = count;
            this.int_3 = count;
            this.int_1 = ((probability == -1) ? 10000 : probability);
            this.vmZliqNxiu = type;
            this.int_2 = delay;
            this.int_4 = skillId;
        }
        public override bool Start(Living living)
        {
            CASE1456A cE = living.PetEffectList.GetOfType(ePetEffectType.CASE1456A) as CASE1456A;
            if (cE != null)
            {
                cE.int_1 = ((this.int_1 > cE.int_1) ? this.int_1 : cE.int_1);
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
        private void method_1(Living living_0, Living living_1, int int_6, int int_7)
        {
            if (living_0 != living_1 && this.IsTrigger)
            {
                this.int_5 = living_0.MaxBlood * 15 / 100;
                if (living_0.IsLiving)
                {
                    living_0.SyncAtTime = true;
                    living_0.AddBlood(this.int_5);
                    living_0.SyncAtTime = false;
                }
                this.IsTrigger = false;
            }
        }
        private void method_2(Living living_0)
        {
            this.int_0--;
            this.IsTrigger = true;
            if (this.int_0 < 0)
            {
                this.int_5 = 0;
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
