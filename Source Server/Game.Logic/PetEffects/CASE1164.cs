using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1164 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1164(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1164, elementID)
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
            CASE1164 pE = living.PetEffectList.GetOfType(ePetEffectType.CASE1164) as CASE1164;
            if (pE != null)
            {
                pE.int_2 = ((this.int_2 > pE.int_2) ? this.int_2 : pE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.BeginNextTurn += new LivingEventHandle(this.TyvypPxnqb);
        }
        private void TyvypPxnqb(Living living_0)
        {
            if (this.int_6 == 0)
            {
                this.int_6 = 300;
                living_0.Game.method_7(living_0, base.Info, true);
                living_0.Attack += (double)this.int_6;
                Console.WriteLine("Tan Cong Tang +300");
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.BeginNextTurn -= new LivingEventHandle(this.TyvypPxnqb);
        }
    }
}
