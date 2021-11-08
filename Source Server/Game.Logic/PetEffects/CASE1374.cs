using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1374 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1374(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1374, elementID)
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
            CASE1374 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1374) as CASE1374;
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
            player.PlayerCompleteShoot += new PlayerEventHandle(this.method_0);
        }
        private void method_0(Player player_0)
        {
            player_0.PetEffects.AddBloodPercent = 0;
        }
        private void method_1(Player player_0)
        {
            if (player_0.PetEffects.CurrentUseSkill == this.int_5)
            {
                this.int_6 = 3;
                player_0.PetEffects.MaxBlood = this.int_6;
                Console.WriteLine("+3% HP");
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(this.method_1);
            player.PlayerCompleteShoot -= new PlayerEventHandle(this.method_0);
        }
    }
}
