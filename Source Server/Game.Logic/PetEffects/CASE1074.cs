using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1074 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        public CASE1074(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1074, elementID)
        {
            this.int_1 = count;
            this.int_2 = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.int_3 = delay;
            this.int_4 = skillId;
            if (skillId != 72)
            {
                if (skillId == 73)
                {
                    this.int_5 = 600;
                }
            }
            else
            {
                this.int_5 = 300;
            }
        }
        private void method_0(Living living_0)
        {
            if (this.rand.Next(10000) < this.int_2 && (72 == this.int_4 || 73 == this.int_4) && living_0.PetEffects.BonusPoint < this.int_5)
            {
                living_0.PetEffects.BonusPoint = this.int_5;
                Console.WriteLine("Tang Them Diem Khi Dung Thien Su");
            }
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.BeginSelfTurn += new LivingEventHandle(this.method_0);
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.BeginSelfTurn -= new LivingEventHandle(this.method_0);
        }
        public override bool Start(Living living)
        {
            CASE1074 petSecondWeaponBonusPointEquipEffect = living.PetEffectList.GetOfType(ePetEffectType.CASE1074) as CASE1074;
            bool result;
            if (petSecondWeaponBonusPointEquipEffect != null)
            {
                petSecondWeaponBonusPointEquipEffect.int_2 = ((this.int_2 > petSecondWeaponBonusPointEquipEffect.int_2) ? this.int_2 : petSecondWeaponBonusPointEquipEffect.int_2);
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
