using Game.Logic.Phy.Object;
using System;

namespace Game.Logic.PetEffects
{
    public class CASE1365 : BasePetEffect
    {
        private int int_0;

        private int int_1;

        private int int_2;

        private int int_3;

        private int int_4;

        private int int_5;

        private int int_6;

        public CASE1365(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1365, elementID)
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
            CASE1365 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1365) as CASE1365;
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
                    int_6 = item.MaxBlood * 10 / 100;
                    item.SyncAtTime = true;
                    item.AddBlood(int_6);
                    item.SyncAtTime = false;
                    Console.WriteLine("hoi phuc 10% HP cho team minh");
                }
            }
        }

        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= method_0;
        }
    }
}
