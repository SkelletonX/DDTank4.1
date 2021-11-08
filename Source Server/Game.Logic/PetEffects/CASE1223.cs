using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1223 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1223(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1223, elementID)
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
            CASE1223 pE = living.PetEffectList.GetOfType(ePetEffectType.CASE1223) as CASE1223;
            if (pE != null)
            {
                pE.int_2 = ((this.int_2 > pE.int_2) ? this.int_2 : pE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.AfterKilledByLiving += new KillLivingEventHanlde(this.eenWjZtApP);
        }
        private void eenWjZtApP(Living living_0, Living living_1, int int_7, int int_8)
        {
            ((Player)living_0).AddPetMP(2);
            Console.WriteLine("Bi Dame +2 MP");
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.AfterKilledByLiving -= new KillLivingEventHanlde(this.eenWjZtApP);
        }
    }
}
