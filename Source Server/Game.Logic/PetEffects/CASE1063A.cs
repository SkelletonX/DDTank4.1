using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1063A : AbstractPetEffect
    {
        private int int_0;
        private int int_1;
        public CASE1063A(int count, int skillId, string elementID) : base(ePetEffectType.CASE1063A, elementID)
        {
            this.int_0 = count;
            if (skillId != 61)
            {
                if (skillId == 62)
                {
                    this.int_1 = 4;
                }
            }
            else
            {
                this.int_1 = 3;
            }
        }
        private void method_0(Living living_0)
        {
            this.int_0--;
            if (this.int_0 < 0)
            {
                living_0.Game.method_43(living_0, base.Info, false);
                this.Stop();
            }
            else
            {
                living_0.SyncAtTime = true;
                living_0.AddBlood(living_0.MaxBlood / 100 * int_1);
                living_0.SyncAtTime = false;
            }
        }
        public override void OnAttached(Living player)
        {
            player.BeginSelfTurn += new LivingEventHandle(this.method_0);
        }
        public override void OnRemoved(Living player)
        {
            player.BeginSelfTurn += new LivingEventHandle(this.method_0);
        }
        public override bool Start(Living living)
        {
            CASE1063A petAddBloodForSelfEquip = living.PetEffectList.GetOfType(ePetEffectType.CASE1063A) as CASE1063A;
            bool result;
            if (petAddBloodForSelfEquip != null)
            {
                petAddBloodForSelfEquip.int_0 = this.int_0;
                result = true;
            }
            else
            {
                result = base.Start(living);
            }
            return result;
        }
    }
}
