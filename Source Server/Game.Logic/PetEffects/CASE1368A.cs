using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1368A : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1368A(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1368A, elementID)
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
            CASE1368A cE = living.PetEffectList.GetOfType(ePetEffectType.CASE1368A) as CASE1368A;
            if (cE != null)
            {
                cE.int_2 = ((this.int_2 > cE.int_2) ? this.int_2 : cE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.AfterKilledByLiving += new KillLivingEventHanlde(this.method_0);
        }
        private void method_0(Living living_0, Living living_1, int int_7, int int_8)
        {
            this.int_6 = living_0.MaxBlood * 3 / 10 / 100;
            foreach (Player expr_30 in living_0.Game.GetAllTeamPlayers(living_0))
            {
                expr_30.SyncAtTime = true;
                expr_30.AddBlood(this.int_6);
                expr_30.SyncAtTime = false;
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.AfterKilledByLiving -= new KillLivingEventHanlde(this.method_0);
        }
    }
}
