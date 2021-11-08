using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;
namespace Game.Logic.PetEffects
{
    public class CASE1368 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1368(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1368, elementID)
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
            CASE1368 pE = living.PetEffectList.GetOfType(ePetEffectType.CASE1368) as CASE1368;
            if (pE != null)
            {
                pE.int_2 = ((this.int_2 > pE.int_2) ? this.int_2 : pE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.BeginNextTurn += new LivingEventHandle(this.method_0);
        }
        private void method_0(Living living_0)
        {
            if (!this.IsTrigger)
            {
                using (List<Player>.Enumerator enumerator = living_0.Game.GetAllTeamPlayers(living_0).GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        enumerator.Current.AddPetEffect(new CASE1368A(0, this.int_2, this.int_0, this.int_5, this.int_3, base.Info.ID.ToString()), 0);
                        Console.WriteLine("+0.3% HP");
                    }
                }
                this.IsTrigger = true;
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.BeginNextTurn -= new LivingEventHandle(this.method_0);
        }
    }
}
