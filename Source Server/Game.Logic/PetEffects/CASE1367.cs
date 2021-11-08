using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1367 : BasePetEffect
    {
        private int int_0;

        private int int_1;

        private int int_2;

        private int int_3;

        private int int_4;

        private int int_5;

        private int int_6;

        public CASE1367(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1367, elementID)
        {


            int_1 = count;
            int_4 = count;
            int_2 = ((probability == -1) ? 10000 : probability);
            int_0 = type;
            int_3 = delay;
            int_5 = skillId;
        }

        public override bool Start(Living living)
        {
            CASE1367 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1367) as CASE1367;
            if (aE != null)
            {
                aE.int_2 = ((int_2 > aE.int_2) ? int_2 : aE.int_2);
                return true;
            }
            return base.Start(living);
        }

        protected override void OnAttachedToPlayer(Player player)
        {
            player.PlayerBuffSkillPet += method_0;
        }

        private void method_0(Player player_0)
        {
            if (player_0.PetEffects.CurrentUseSkill == int_5)
            {
                foreach (Living item in player_0.Game.Map.FindAllNearestSameTeam(player_0.X, player_0.Y, 250.0, player_0))
                {
                    item.Game.method_9(item, base.Info, bool_1: true, 2, player_0.PlayerDetail);
                    item.AddPetEffect(new CASE1367A(4, int_2, int_0, int_5, int_3, base.Info.ID.ToString()), 0);
                    Console.WriteLine("moi turn hoi phuc 4% HP");

                }
            }
        }

        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= method_0;
        }
    }
}
