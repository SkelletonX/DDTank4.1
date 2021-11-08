using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1451 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1451(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1451, elementID)
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
            CASE1451 pE = living.PetEffectList.GetOfType(ePetEffectType.CASE1451) as CASE1451;
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
                this.int_6 = (int)(living_0.Defence * 6.0 / 100.0);
                living_0.Defence += (double)this.int_6;
                Console.WriteLine("Tang 6% Phong Thu");
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.BeginNextTurn -= new LivingEventHandle(this.method_0);
        }
    }
}
