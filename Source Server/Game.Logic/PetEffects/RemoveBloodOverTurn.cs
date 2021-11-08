using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class RemoveBloodOverTurn : BasePetEffect
    {
        private int JbxZgoHyFT;
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        public RemoveBloodOverTurn(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.RemoveBloodOverTurn, elementID)
        {
            this.int_0 = count;
            this.int_3 = count;
            this.int_1 = ((probability == -1) ? 10000 : probability);
            this.JbxZgoHyFT = type;
            this.int_2 = delay;
            this.int_4 = skillId;
        }
        public override bool Start(Living living)
        {
            RemoveBloodOverTurn pE = living.PetEffectList.GetOfType(ePetEffectType.RemoveBloodOverTurn) as RemoveBloodOverTurn;
            if (pE != null)
            {
                pE.int_1 = ((this.int_1 > pE.int_1) ? this.int_1 : pE.int_1);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.BeginSelfTurn += new LivingEventHandle(this.method_0);
        }
        private void method_0(Living living_0)
        {
            this.int_5 = living_0.MaxBlood * 33 / 10 / 100;
            living_0.AddBlood(-this.int_5, 1);
            if (living_0.Blood <= 0)
            {
                living_0.Die();
                if (living_0.Game.CurrentLiving != null && living_0.Game.CurrentLiving is Player)
                {
                    (living_0.Game.CurrentLiving as Player).PlayerDetail.OnKillingLiving(living_0.Game, 2, living_0.Id, living_0.IsLiving, this.int_5);
                    Console.WriteLine("REMOVE 3,3% HP 1 TURN");
                }
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.BeginSelfTurn -= new LivingEventHandle(this.method_0);
        }
    }
}
