using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1449 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1449(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1449, elementID)
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
            CASE1449 pE = living.PetEffectList.GetOfType(ePetEffectType.CASE1449) as CASE1449;
            if (pE != null)
            {
                pE.int_2 = ((this.int_2 > pE.int_2) ? this.int_2 : pE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.BeginNextTurn += new LivingEventHandle(this.method_0);
        }
        private void method_0(Living living_0)
        {
            if (this.int_6 == 0)
            {
                this.int_6 = (int)(living_0.BaseGuard * 6.0 / 100.0);
                living_0.BaseGuard += (double)this.int_6;
                Console.WriteLine("Tang 6% Ho Giap");
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.BeginNextTurn -= new LivingEventHandle(this.method_0);
        }
    }
}
