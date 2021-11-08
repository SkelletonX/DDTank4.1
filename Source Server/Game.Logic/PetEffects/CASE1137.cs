using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1137 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1137(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1137, elementID)
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
            CASE1137 pE = living.PetEffectList.GetOfType(ePetEffectType.CASE1137) as CASE1137;
            if (pE != null)
            {
                pE.int_2 = ((this.int_2 > pE.int_2) ? this.int_2 : pE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.BeforeTakeDamage += new LivingTakedDamageEventHandle(this.method_1);
            player.AfterKilledByLiving += new KillLivingEventHanlde(this.method_0);
        }
        private void method_0(Living living_0, Living living_1, int int_7, int int_8)
        {
            living_0.Game.method_8(living_0, base.Info, false, 0);
        }
        private void method_1(Living living_0, Living living_1, ref int int_7, ref int int_8)
        {
            if (this.rand.Next(100) <= 20)
            {
                living_0.PetEffects.ActiveEffect = true;
                living_0.Game.method_8(living_0, base.Info, true, 0);
                Console.WriteLine("Xac Suat Phan Don");
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.BeforeTakeDamage -= new LivingTakedDamageEventHandle(this.method_1);
            player.AfterKilledByLiving -= new KillLivingEventHanlde(this.method_0);
        }
    }
}
