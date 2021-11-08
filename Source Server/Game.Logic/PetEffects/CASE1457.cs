using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1457 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1457(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1457, elementID)
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
            CASE1457 pE = living.PetEffectList.GetOfType(ePetEffectType.CASE1457) as CASE1457;
            if (pE != null)
            {
                pE.int_2 = ((this.int_2 > pE.int_2) ? this.int_2 : pE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.AfterKilledByLiving += new KillLivingEventHanlde(this.method_0);
        }
        private void method_0(Living living_0, Living living_1, int int_7, int int_8)
        {
            ((Player)living_0).AddPetMP(1);
            Console.WriteLine("Hoi 1 Ma Phap");
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.AfterKilledByLiving -= new KillLivingEventHanlde(this.method_0);
        }
    }
}
