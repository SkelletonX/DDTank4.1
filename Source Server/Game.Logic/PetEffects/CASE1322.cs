using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1322 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1322(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1322, elementID)
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
            CASE1322 pE = living.PetEffectList.GetOfType(ePetEffectType.CASE1322) as CASE1322;
            if (pE != null)
            {
                pE.int_2 = ((this.int_2 > pE.int_2) ? this.int_2 : pE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.BeforeTakeDamage += new LivingTakedDamageEventHandle(this.method_0);
        }
        private void method_0(Living living_0, Living living_1, ref int int_7, ref int int_8)
        {
            this.int_6 = (int_7 + int_8) / 3500;
            if (this.int_6 > 0)
            {
                (living_0 as Player).AddPetMP(this.int_6);
                Console.WriteLine("-3500HP = 1MP");
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.BeforeTakeDamage -= new LivingTakedDamageEventHandle(this.method_0);
        }
    }
}
