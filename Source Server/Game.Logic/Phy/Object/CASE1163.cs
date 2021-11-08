using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1163 : BasePetEffect
    {
        private int int_0;
        private int rsJyiwIlGl;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        public CASE1163(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1163, elementID)
        {
            this.rsJyiwIlGl = count;
            this.int_3 = count;
            this.int_1 = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.int_2 = delay;
            this.int_4 = skillId;
        }
        public override bool Start(Living living)
        {
            CASE1163 pE = living.PetEffectList.GetOfType(ePetEffectType.CASE1163) as CASE1163;
            if (pE != null)
            {
                pE.int_1 = ((this.int_1 > pE.int_1) ? this.int_1 : pE.int_1);
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
            if (this.int_5 == 0)
            {
                this.int_5 = 150;
                living_0.Game.method_7(living_0, base.Info, true);
                living_0.Attack += (double)this.int_5;
                Console.WriteLine("Tan Cong Tang +150");
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.BeginNextTurn -= new LivingEventHandle(this.method_0);
        }
    }
}
