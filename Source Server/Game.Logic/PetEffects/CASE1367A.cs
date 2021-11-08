using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
    public class CASE1367A : BasePetEffect
    {
        private int int_0;

        private int int_1;

        private int int_2;

        private int int_3;

        private int int_4;

        private int int_5;

        private int int_6;

        public CASE1367A(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1367A, elementID)
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
            CASE1367A cE = living.PetEffectList.GetOfType(ePetEffectType.CASE1367A) as CASE1367A;
            if (cE != null)
            {
                cE.int_2 = ((int_2 > cE.int_2) ? int_2 : cE.int_2);
                return true;
            }
            return base.Start(living);
        }

        protected override void OnAttachedToPlayer(Player player)
        {
            player.BeginSelfTurn += method_0;
            player.PlayerClearBuffSkillPet += DiHreefqdgG;
        }

        private void DiHreefqdgG(Player player_0)
        {
            Stop();
        }

        private void method_0(Living living_0)
        {
            int_1--;
            if (int_1 < 0)
            {
                Stop();
                return;
            }
            int_6 = living_0.MaxBlood * 4 / 100;
            living_0.SyncAtTime = true;
            living_0.AddBlood(int_6);
            living_0.SyncAtTime = false;
        }

        protected override void OnRemovedFromPlayer(Player player)
        {
            player.Game.method_11(player, base.Info, bool_1: false, 1);
            player.BeginSelfTurn -= method_0;
        }
    }
}
