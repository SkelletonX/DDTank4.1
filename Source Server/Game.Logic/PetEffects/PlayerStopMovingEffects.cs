using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class PlayerStopMovingEffects : AbstractPetEffect
    {
        private int int_0;
        public PlayerStopMovingEffects(int count, string elementID) : base(ePetEffectType.PlayerStopMovingEffects, elementID)
        {
            this.int_0 = count;
        }
        private void method_0(Living living_0)
        {
            this.int_0--;
            if (this.int_0 < 0)
            {
                living_0.Game.method_43(living_0, base.Info, false);
                this.Stop();
            }
        }
        public override void OnAttached(Living living)
        {
            living.SpeedMultX(0);
            living.PetEffects.StopMoving = true;
            living.BeginSelfTurn += new LivingEventHandle(this.method_0);
        }
        public override void OnRemoved(Living living)
        {
            living.SpeedMultX(3);
            living.PetEffects.StopMoving = false;
            living.BeginSelfTurn -= new LivingEventHandle(this.method_0);
        }
        public override bool Start(Living living)
        {
            PlayerStopMovingEffects petStopMovingEquip = living.PetEffectList.GetOfType(ePetEffectType.PlayerStopMovingEffects) as PlayerStopMovingEffects;
            bool result;
            if (petStopMovingEquip != null)
            {
                petStopMovingEquip.int_0 = this.int_0;
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
