using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1215 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1215(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1215, elementID)
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
            CASE1215 pE = living.PetEffectList.GetOfType(ePetEffectType.CASE1215) as CASE1215;
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
                this.int_6 = 200;
                living.BaseGuard += (double)this.int_6;
                Console.WriteLine("+200 diem ho giap");
            }
        }
    }
}
