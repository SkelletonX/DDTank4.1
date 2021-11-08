using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1372 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1372(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1372, elementID)
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
            CASE1372 pE = living.PetEffectList.GetOfType(ePetEffectType.CASE1372) as CASE1372;
            if (pE != null)
            {
                pE.int_2 = ((this.int_2 > pE.int_2) ? this.int_2 : pE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.PlayerCure += new PlayerEventHandle(this.method_0);
        }
        private void method_0(Player player_0)
        {
            this.int_6 = player_0.MaxBlood * 3 / 100;
            player_0.SyncAtTime = true;
            player_0.AddBlood(this.int_6);
            player_0.SyncAtTime = false;
            Console.WriteLine("Thien Su +3% HP");
        }
        private void method_1(Player player_0, int int_7)
        {
            if (int_7 != 31)
            {
                this.int_6 = player_0.MaxBlood * 3 / 100;
                player_0.SyncAtTime = true;
                player_0.AddBlood(this.int_6);
                player_0.SyncAtTime = false;
                Console.WriteLine("Thien Su +3% HP");
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerCure -= new PlayerEventHandle(this.method_0);
        }
    }
}
