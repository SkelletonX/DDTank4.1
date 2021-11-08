using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class GuideShoots : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int CkPmcucLiG;
        private int int_4;
        private int int_5;
        public GuideShoots(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.GuideShoots, elementID)
        {
            this.int_1 = count;
            this.CkPmcucLiG = count;
            this.int_2 = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.int_3 = delay;
            this.int_4 = skillId;
        }
        public override bool Start(Living living)
        {
            GuideShoots cE = living.PetEffectList.GetOfType(ePetEffectType.GuideShoots) as GuideShoots;
            if (cE != null)
            {
                cE.int_2 = ((this.int_2 > cE.int_2) ? this.int_2 : cE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.BeginSelfTurn += new LivingEventHandle(this.method_1);
            player.ControlBall = true;
            player.PlayerClearBuffSkillPet += new PlayerEventHandle(this.method_0);
        }
        private void method_0(Player player_0)
        {
            this.Stop();
        }
        private void method_1(Living living_0)
        {
            living_0.ControlBall = true;
            this.int_1--;
            if (this.int_1 < 0)
            {
                this.Stop();
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.ControlBall = false;
            player.BeginSelfTurn -= new LivingEventHandle(this.method_1);
        }
    }
}
