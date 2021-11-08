using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1363 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int mqJihhxwms;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        public CASE1363(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1363, elementID)
        {
            this.int_1 = count;
            this.int_3 = count;
            this.mqJihhxwms = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.int_2 = delay;
            this.int_4 = skillId;
        }
        public override bool Start(Living living)
        {
            CASE1363 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1363) as CASE1363;
            if (aE != null)
            {
                aE.mqJihhxwms = ((this.mqJihhxwms > aE.mqJihhxwms) ? this.mqJihhxwms : aE.mqJihhxwms);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.PlayerBuffSkillPet += new PlayerEventHandle(this.method_0);
        }
        private void method_0(Player player_0)
        {
            if (player_0.PetEffects.CurrentUseSkill == this.int_4)
            {
                player_0.Game.method_7(player_0, base.Info, true);
                player_0.AddPetEffect(new CASE1363A(4, this.mqJihhxwms, this.int_0, this.int_4, this.int_2, base.Info.ID.ToString()), 0);
                Console.WriteLine("Hoi Phuc 3% + 1500 HP");
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(this.method_0);
        }
    }
}
