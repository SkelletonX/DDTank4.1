using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1443 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int ojnSlTnjsh;
        private int int_3;
        private int int_4;
        private int int_5;
        public CASE1443(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1443, elementID)
        {
            this.int_1 = count;
            this.int_3 = count;
            this.int_2 = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.ojnSlTnjsh = delay;
            this.int_4 = skillId;
        }
        public override bool Start(Living living)
        {
            CASE1443 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1443) as CASE1443;
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
        private void method_0(Living living_0, Living living_1, ref int int_6, ref int int_7)
        {
            if (this.IsTrigger)
            {
                this.int_5 = 500;
                this.int_5 += int_6 * 10 / 100;
                int_6 -= this.int_5;
                this.IsTrigger = false;
                Console.WriteLine("+500 diem giam thuong & 10% diem giam thuong");
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(this.method_1);
        }
        private void method_1(Player player_0)
        {
            if (player_0.PetEffects.CurrentUseSkill == this.int_4)
            {
                this.IsTrigger = true;
            }
        }
    }
}
