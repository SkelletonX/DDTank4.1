using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1078 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        public CASE1078(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1078, elementID)
        {
            this.int_1 = count;
            this.int_2 = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.int_3 = delay;
            this.int_4 = skillId;
            if (skillId != 80)
            {
                if (skillId == 81)
                {
                    this.int_5 = 1000;
                }
            }
            else
            {
                this.int_5 = 500;
            }
        }
        private void method_0(Living living_0)
        {
            if (this.rand.Next(10000) < this.int_2)
            {
                living_0.SyncAtTime = true;
                living_0.AddBlood(this.int_5);
                living_0.SyncAtTime = false;
                Console.WriteLine("Su Dung Khien Hoi HP ");
            }
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.PlayerGuard += new PlayerEventHandle(this.method_0);
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerGuard -= new PlayerEventHandle(this.method_0);
        }
        public override bool Start(Living living)
        {
            CASE1078 ce = living.PetEffectList.GetOfType(ePetEffectType.CASE1078) as CASE1078;
            bool result;
            if (ce != null)
            {
                ce.int_2 = ((this.int_2 > ce.int_2) ? this.int_2 : ce.int_2);
                result = true;
            }
            else
            {
                result = base.Start(living);
            }
            return result;
        }
    }
}
