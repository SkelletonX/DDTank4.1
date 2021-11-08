using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class AdditionalDamagePointLv1 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public AdditionalDamagePointLv1(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.AdditionalDamagePointLv1, elementID)
        {
            this.int_1 = count;
            this.int_4 = 0;
            this.int_2 = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.int_3 = delay;
            this.int_5 = skillId;
        }
        public override bool Start(Living living)
        {
            AdditionalDamagePointLv1 pE = living.PetEffectList.GetOfType(ePetEffectType.AdditionalDamagePointLv1) as AdditionalDamagePointLv1;
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
            player.AfterKilledByLiving += new KillLivingEventHanlde(this.method_2);
            player.BeginSelfTurn += new LivingEventHandle(this.method_1);
        }
        private void method_0(Living living_0, Living living_1, ref int int_7, ref int int_8)
        {
            if (this.int_4 < 4)
            {
                if (this.int_6 == 0)
                {
                    this.int_6 = 40;
                }
                living_0.BaseDamage += (double)this.int_6;
                this.int_1 += this.int_6;
                this.IsTrigger = true;
                this.int_4++;
                Console.WriteLine("BEING ATTACKED {0}", int_4);
            }
        }
        private void method_1(Living living_0)
        {
            this.int_6 = 20;
            Console.WriteLine("FINISH TURN EFFECTIVELY HALVED {0} ", int_6);
        }
        private void method_2(Living living_0, Living living_1, int int_7, int int_8)
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
