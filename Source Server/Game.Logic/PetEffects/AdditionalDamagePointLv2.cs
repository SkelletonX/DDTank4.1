using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class AdditionalDamagePointLv2 : BasePetEffect
    {
        private int int_0;
        private double double_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private double double_1;
        public AdditionalDamagePointLv2(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.AdditionalDamagePointLv2, elementID)
        {
            this.double_0 = (double)count;
            this.int_3 = 0;
            this.int_1 = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.int_2 = delay;
            this.int_4 = skillId;
        }
        public override bool Start(Living living)
        {
            AdditionalDamagePointLv2 pE = living.PetEffectList.GetOfType(ePetEffectType.AdditionalDamagePointLv2) as AdditionalDamagePointLv2;
            if (pE != null)
            {
                pE.int_1 = ((this.int_1 > pE.int_1) ? this.int_1 : pE.int_1);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.BeforeTakeDamage += new LivingTakedDamageEventHandle(this.method_0);
            player.AfterKilledByLiving += new KillLivingEventHanlde(this.method_2);
            player.BeginSelfTurn += new LivingEventHandle(this.method_1);
        }
        private void method_0(Living living_0, Living living_1, ref int int_5, ref int int_6)
        {
            if (this.int_3 < 6)
            {
                if (this.double_1 == 0)
                {
                    this.double_1 = 55;
                }
                living_0.BaseDamage += this.double_1;
                this.double_0 += this.double_1;
                this.IsTrigger = true;
                this.int_3++;
                Console.WriteLine("BEING ATTACKED {0}", int_3);
            }
        }
        private void method_1(Living living_0)
        {
            this.double_1 = 25.5;
            Console.WriteLine("FINISH TURN EFFECTIVELY HALVED {0} ", double_1);
        }
        private void method_2(Living living_0, Living living_1, int int_5, int int_6)
        {
            if (this.IsTrigger)
            {
                living_0.Game.method_7(living_0, base.Info, true);
                this.IsTrigger = false;
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.AfterKilledByLiving -= new KillLivingEventHanlde(this.method_2);
        }
    }
}
