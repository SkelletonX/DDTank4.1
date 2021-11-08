using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1134 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1134(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1134, elementID)
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
            CASE1134 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1134) as CASE1134;
            if (aE != null)
            {
                aE.int_2 = ((this.int_2 > aE.int_2) ? this.int_2 : aE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.PlayerBuffSkillPet += new PlayerEventHandle(this.method_0);
            player.AfterPlayerShooted += new PlayerEventHandle(this.method_1);
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(this.method_0);
            player.AfterPlayerShooted -= new PlayerEventHandle(this.method_1);
        }
        private void method_0(Player player_0)
        {
            if (player_0.PetEffects.CurrentUseSkill == this.int_5)
            {
                this.int_6 = 180;
                player_0.CurrentDamagePlus += (float)(this.int_6 / 100);
                Console.WriteLine("180% Sat Thuong");
            }
        }
        private void method_1(Player player_0)
        {
            if (player_0.PetEffects.CurrentUseSkill == this.int_5)
            {
                player_0.CurrentDamagePlus -= (float)(this.int_6 / 100);
                this.int_6 = 0;
            }
        }
    }
}