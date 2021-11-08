using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1213A : BasePetEffect
    {
        private int int_0;
        private int GcrzhkosnL;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        public CASE1213A(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1213A, elementID)
        {
            this.GcrzhkosnL = count;
            this.int_3 = count;
            this.int_1 = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.int_2 = delay;
            this.int_4 = skillId;
        }
        public override bool Start(Living living)
        {
            CASE1213A cE = living.PetEffectList.GetOfType(ePetEffectType.CASE1213A) as CASE1213A;
            if (cE != null)
            {
                cE.int_1 = ((this.int_1 > cE.int_1) ? this.int_1 : cE.int_1);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.BeginSelfTurn += new LivingEventHandle(this.method_1);
            player.PlayerClearBuffSkillPet += new PlayerEventHandle(this.method_0);
        }
        private void method_0(Player player_0)
        {
            this.Stop();
        }
        private void method_1(Living living_0)
        {
            this.GcrzhkosnL--;
            if (this.GcrzhkosnL < 0)
            {
                this.Stop();
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PetEffects.CritRate = 0;
            player.Game.method_7(player, base.Info, false);
            player.BeginSelfTurn -= new LivingEventHandle(this.method_1);
        }
    }
}
