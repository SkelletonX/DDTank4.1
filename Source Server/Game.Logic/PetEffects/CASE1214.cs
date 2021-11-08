using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1214 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1214(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1214, elementID)
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
            CASE1214 pE = living.PetEffectList.GetOfType(ePetEffectType.CASE1214) as CASE1214;
            if (pE != null)
            {
                pE.int_2 = ((this.int_2 > pE.int_2) ? this.int_2 : pE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.BeginNextTurn += new LivingEventHandle(this.player_beginNextTurn);
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.BeginNextTurn -= new LivingEventHandle(this.player_beginNextTurn);
        }
        public void player_beginNextTurn(Living living)
        {
            if (this.int_6 == 0)
            {
                this.int_6 = 100;
                living.BaseGuard += (double)this.int_6;
                Console.WriteLine("+100 diem ho giap");
            }
        }
    }
}
