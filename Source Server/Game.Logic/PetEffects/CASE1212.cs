using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1212 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int ikeLoAsniV;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        public CASE1212(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1212, elementID)
        {
            this.int_1 = count;
            this.int_3 = count;
            this.ikeLoAsniV = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.int_2 = delay;
            this.int_4 = skillId;
        }
        public override bool Start(Living living)
        {
            CASE1212 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1212) as CASE1212;
            if (aE != null)
            {
                aE.ikeLoAsniV = ((this.ikeLoAsniV > aE.ikeLoAsniV) ? this.ikeLoAsniV : aE.ikeLoAsniV);
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
                player_0.PetEffects.CritRate = 20;
                player_0.Game.method_7(player_0, base.Info, true);
                player_0.AddPetEffect(new CASE1212A(3, this.ikeLoAsniV, this.int_0, this.int_4, this.int_2, base.Info.ID.ToString()), 0);
                Console.WriteLine("+20% Bao Kich");
            }
        }
    }
}
