using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CriticalChance : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CriticalChance(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CriticalChance, elementID)
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
            CriticalChance aE = living.PetEffectList.GetOfType(ePetEffectType.CriticalChance) as CriticalChance;
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
                this.int_6 = 100;
                player_0.PetEffects.CritRate += this.int_6;
                Console.WriteLine("{0} CritRate", player_0.PetEffects.CritRate);
            }
        }
        private void method_1(Player player_0)
        {
            if (player_0.PetEffects.CurrentUseSkill == this.int_5)
            {
                player_0.PetEffects.CritRate -= this.int_6;
                this.int_6 = 0;
            }
        }
    }
}
