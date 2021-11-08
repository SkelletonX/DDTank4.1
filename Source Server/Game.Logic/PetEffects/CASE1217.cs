using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1217 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1217(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1217, elementID)
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
            CASE1217 pE = living.PetEffectList.GetOfType(ePetEffectType.CASE1217) as CASE1217;
            if (pE != null)
            {
                pE.int_2 = ((this.int_2 > pE.int_2) ? this.int_2 : pE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            if (this.int_6 == 0)
            {
                this.int_6 = 3000;
                player.PetEffects.MaxBlood = this.int_6;
                Console.WriteLine("+3000 HP");
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
        }
    }
}
