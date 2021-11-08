using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CantDropPlayer : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CantDropPlayer(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CantDropPlayer, elementID)
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
            CantDropPlayer aE = living.PetEffectList.GetOfType(ePetEffectType.CantDropPlayer) as CantDropPlayer;
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
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(this.method_0);
        }
        private void method_0(Player player_0)
        {
            if (player_0.PetEffects.CurrentUseSkill == this.int_5)
            {
                player_0.Game.method_68(player_0, 5, true);
                player_0.IsNoHole = true;
                Console.WriteLine("AUTO NO DROP");
            }
        }
    }
}
