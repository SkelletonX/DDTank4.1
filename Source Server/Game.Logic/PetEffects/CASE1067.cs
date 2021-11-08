using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1067 : BasePetEffect
    {
        private int woswNoAeAP;
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        public CASE1067(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1067, elementID)
        {
            this.int_0 = count;
            this.int_3 = count;
            this.int_1 = ((probability == -1) ? 10000 : probability);
            this.woswNoAeAP = type;
            this.int_2 = delay;
            this.int_4 = skillId;
        }
        public override bool Start(Living living)
        {
            CASE1067 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1067) as CASE1067;
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
            if (player_0.PetEffects.CurrentUseSkill == this.int_4 && player_0.Game is PVPGame)
            {
                player_0.AddPetEffect(new CASE1067A(2, this.int_1, this.woswNoAeAP, this.int_4, this.int_2, base.Info.ID.ToString()), 0);
                Console.WriteLine("Phan 30%/Tong So Sat Thuong");
            }
        }
    }
}