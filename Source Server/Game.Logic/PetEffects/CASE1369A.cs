using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1369A : BasePetEffect
    {
        private int int_0;
        private int ecufPjvekk;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        public CASE1369A(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1369A, elementID)
        {
            this.ecufPjvekk = count;
            this.int_3 = count;
            this.int_1 = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.int_2 = delay;
            this.int_4 = skillId;
        }
        public override bool Start(Living living)
        {
            CASE1369A cE = living.PetEffectList.GetOfType(ePetEffectType.CASE1369A) as CASE1369A;
            if (cE != null)
            {
                cE.int_1 = ((this.int_1 > cE.int_1) ? this.int_1 : cE.int_1);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.AfterKilledByLiving += new KillLivingEventHanlde(this.method_0);
        }
        private void method_0(Living living_0, Living living_1, int int_6, int int_7)
        {
            this.int_5 = living_0.MaxBlood * 4 / 10 / 100;
            foreach (Player current in living_0.Game.GetAllTeamPlayers(living_0))
            {
                current.SyncAtTime = true;
                if (current.Blood < current.MaxBlood * 20 / 100)
                {
                    current.AddBlood(this.int_5 * 2);
                }
                else
                {
                    current.AddBlood(this.int_5);
                }
                current.SyncAtTime = false;
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.AfterKilledByLiving -= new KillLivingEventHanlde(this.method_0);
        }
    }
}
