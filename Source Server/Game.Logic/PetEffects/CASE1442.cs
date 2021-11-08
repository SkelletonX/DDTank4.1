using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1442 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1442(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1442, elementID)
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
            CASE1442 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1442) as CASE1442;
            if (aE != null)
            {
                aE.int_2 = ((this.int_2 > aE.int_2) ? this.int_2 : aE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.PlayerBuffSkillPet += new PlayerEventHandle(this.method_1);
            player.BeforeTakeDamage += new LivingTakedDamageEventHandle(this.method_0);
        }
        private void method_0(Living living_0, Living living_1, ref int int_7, ref int int_8)
        {
            if (this.IsTrigger)
            {
                this.int_6 = 500;
                int_7 -= this.int_6;
                this.IsTrigger = false;
                Console.WriteLine("+500 diem giam thuong");
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(this.method_1);
        }
        private void method_1(Player player_0)
        {
            if (player_0.PetEffects.CurrentUseSkill == this.int_5)
            {
                this.IsTrigger = true;
            }
        }
    }
}
