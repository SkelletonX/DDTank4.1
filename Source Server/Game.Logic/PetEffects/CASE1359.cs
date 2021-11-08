using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1359 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int uSbixrlSbn;
        private int int_4;
        private int int_5;
        public CASE1359(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1359, elementID)
        {
            this.int_1 = count;
            this.uSbixrlSbn = count;
            this.int_2 = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.int_3 = delay;
            this.int_4 = skillId;
        }
        public override bool Start(Living living)
        {
            CASE1359 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1359) as CASE1359;
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
        }
        private void method_0(Player player_0)
        {
            if (player_0.PetEffects.CurrentUseSkill == this.int_4)
            {
                player_0.Game.method_7(player_0, base.Info, true);
                player_0.AddPetEffect(new CASE1359A(2, this.int_2, this.int_0, this.int_4, this.int_3, base.Info.ID.ToString()), 0);
                Console.WriteLine("Hoi Phuc 4% HP Moi Turn");
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(this.method_0);
        }
    }
}
