using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class RemoveEffectForAddBaseGuard : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public RemoveEffectForAddBaseGuard(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.RemoveEffectForAddBaseGuard, elementID)
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
            RemoveEffectForAddBaseGuard aE = living.PetEffectList.GetOfType(ePetEffectType.RemoveEffectForAddBaseGuard) as RemoveEffectForAddBaseGuard;
            if (aE != null)
            {
                aE.int_2 = ((this.int_2 > aE.int_2) ? this.int_2 : aE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.PlayerBeginMoving += new PlayerEventHandle(this.method_1);
            player.PlayerBuffSkillPet += new PlayerEventHandle(this.method_0);
        }
        private void method_0(Player player_0)
        {
            if (player_0.PetEffects.CurrentUseSkill == this.int_5)
            {
                this.IsTrigger = true;
                Console.WriteLine("PLAYER RUNNING AND REMOVE EFFECT");
            }
        }
        private void method_1(Player player_0)
        {
            if (this.IsTrigger)
            {
                AddBaseGuardLv1 cE = player_0.PetEffectList.GetOfType(ePetEffectType.AddBaseGuardLv1) as AddBaseGuardLv1;
                if (cE != null)
                {
                    cE.Stop();
                }
                AddBaseGuardLv2 cE2 = player_0.PetEffectList.GetOfType(ePetEffectType.AddBaseGuardLv2) as AddBaseGuardLv2;
                if (cE2 != null)
                {
                    cE2.Stop();
                }
                this.IsTrigger = false;
                Console.WriteLine("PLAYER RUNNING AND REMOVE EFFECT");
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBeginMoving -= new PlayerEventHandle(this.method_1);
        }
    }
}
