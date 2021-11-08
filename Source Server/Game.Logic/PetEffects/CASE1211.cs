using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1211 : BasePetEffect
    {
        private int int_0;
        private int JxnLviIyKj;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        public CASE1211(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1211, elementID)
        {
            this.JxnLviIyKj = count;
            this.int_3 = count;
            this.int_1 = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.int_2 = delay;
            this.int_4 = skillId;
        }
        public override bool Start(Living living)
        {
            CASE1211 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1211) as CASE1211;
            if (aE != null)
            {
                aE.int_1 = ((this.int_1 > aE.int_1) ? this.int_1 : aE.int_1);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.PlayerBuffSkillPet += new PlayerEventHandle(this.method_0);
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(this.method_0);
        }
        private void method_0(Player player_0)
        {
            if (player_0.PetEffects.CurrentUseSkill == this.int_4)
            {
                player_0.AddPetEffect(new CASE1211A(3, this.int_1, this.int_0, this.int_4, this.int_2, base.Info.ID.ToString()), 0);
                Console.WriteLine("Moi Turn -1000 HP");
            }
        }
    }
}
