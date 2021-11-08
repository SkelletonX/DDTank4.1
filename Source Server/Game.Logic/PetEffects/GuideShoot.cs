using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class GuideShoot : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public GuideShoot(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.GuideShoot, elementID)
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
            GuideShoot aE = living.PetEffectList.GetOfType(ePetEffectType.GuideShoot) as GuideShoot;
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
                if (this.int_5 >= 25 && this.int_5 <= 27 && player_0.Game is PVEGame)
                {
                    return;
                }
                player_0.Game.method_7(player_0, base.Info, true);
                player_0.AddPetEffect(new GuideShoots(1, this.int_2, this.int_0, this.int_5, this.int_3, base.Info.ID.ToString()), 0);
                Console.WriteLine("GUIDE SHOOT");
            }
        }
    }
}
