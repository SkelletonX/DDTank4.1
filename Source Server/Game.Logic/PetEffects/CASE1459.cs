using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1459 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1459(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1459, elementID)
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
            CASE1459 pE = living.PetEffectList.GetOfType(ePetEffectType.CASE1459) as CASE1459;
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
            if (living_0.PetEffects.ActiveEffect)
            {
                this.int_6 = living_0.MaxBlood * 3 / 100;
                if (this.int_6 > 0)
                {
                    living_1.SyncAtTime = true;
                    living_1.AddBlood(-this.int_6, 1);
                    living_1.SyncAtTime = false;
                    living_1.Game.method_8(living_0, base.Info, true, 0);
                    if (living_1.Blood <= 0)
                    {
                        living_1.Die();
                        if (living_0 != null && living_0 is Player)
                        {
                            (living_0 as Player).PlayerDetail.OnKillingLiving(living_0.Game, 2, living_0.Id, living_0.IsLiving, this.int_6);
                        }
                    }
                    else
                    {
                        living_1.AddPetEffect(new CASE1459A(0, this.int_2, this.int_0, this.int_5, this.int_3, base.Info.ID.ToString()), 0);
                    }
                    living_0.PetEffects.ActiveEffect = false;
                    Console.WriteLine("Phan Don Bang 3% HP");
                }
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.AfterKilledByLiving -= new KillLivingEventHanlde(this.method_0);
        }
    }
}
