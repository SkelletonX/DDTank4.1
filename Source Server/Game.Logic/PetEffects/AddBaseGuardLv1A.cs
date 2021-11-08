using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class AddBaseGuardLv1A : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public AddBaseGuardLv1A(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.AddBaseGuardLv1A, elementID)
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
            AddBaseGuardLv1A cE = living.PetEffectList.GetOfType(ePetEffectType.AddBaseGuardLv1A) as AddBaseGuardLv1A;
            if (cE != null)
            {
                cE.int_2 = ((this.int_2 > cE.int_2) ? this.int_2 : cE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.BeginSelfTurn += new LivingEventHandle(this.method_0);
            if (this.int_6 == 0)
            {
                this.int_6 = 250;
                player.BaseGuard += (double)this.int_6;
            }
        }
        private void method_0(Living living_0)
        {
            this.int_1--;
            if (this.int_1 > 0)
            {
                this.int_6 += 250;
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.BeginSelfTurn -= new LivingEventHandle(this.method_0);
            player.Game.method_7(player, base.Info, false);
            player.BaseGuard -= (double)this.int_6;
        }
    }
}
