using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class RemoveEffectSkill02 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public RemoveEffectSkill02(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.RemoveEffectSkill02, elementID)
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
            RemoveEffectSkill02 aE = living.PetEffectList.GetOfType(ePetEffectType.RemoveEffectSkill02) as RemoveEffectSkill02;
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
            }
        }
        private void method_1(Player player_0)
        {
            if (this.IsTrigger)
            {
                AddLuckyEffectLv2 aE = player_0.PetEffectList.GetOfType(ePetEffectType.AddLuckyEffectLv2) as AddLuckyEffectLv2;
                if (aE != null)
                {
                    aE.Pause();
                }
                RemoveBaseGuardEffectLv1 aE2 = player_0.PetEffectList.GetOfType(ePetEffectType.RemoveBaseGuardEffectLv1) as RemoveBaseGuardEffectLv1;
                if (aE2 != null)
                {
                    aE2.Pause();
                }
                RemoveBaseGuardEffectLv2 aE3 = player_0.PetEffectList.GetOfType(ePetEffectType.RemoveBaseGuardEffectLv2) as RemoveBaseGuardEffectLv2;
                if (aE3 != null)
                {
                    aE3.Pause();
                }
                this.IsTrigger = false;
                Console.WriteLine("REMOVE EFFECT SKILL 02");
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBeginMoving -= new PlayerEventHandle(this.method_1);
        }
    }
}
