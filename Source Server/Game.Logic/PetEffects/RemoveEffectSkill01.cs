using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class RemoveEffectSkill01 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public RemoveEffectSkill01(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.RemoveEffectSkill01, elementID)
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
            RemoveEffectSkill01 aE = living.PetEffectList.GetOfType(ePetEffectType.RemoveEffectSkill01) as RemoveEffectSkill01;
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
                AddBaseDamageEffectLv1 aE = player_0.PetEffectList.GetOfType(ePetEffectType.AddBaseDamageEffectLv1) as AddBaseDamageEffectLv1;
                if (aE != null)
                {
                    aE.Pause();
                }
                AddBaseDamageEffectLv2 aE2 = player_0.PetEffectList.GetOfType(ePetEffectType.AddBaseDamageEffectLv2) as AddBaseDamageEffectLv2;
                if (aE2 != null)
                {
                    aE2.Pause();
                }
                AddLuckyEffectLv1 aE3 = player_0.PetEffectList.GetOfType(ePetEffectType.AddLuckyEffectLv1) as AddLuckyEffectLv1;
                if (aE3 != null)
                {
                    aE3.Pause();
                }
                player_0.Game.method_68(player_0, 5, false);
                player_0.IsNoHole = false;
                this.IsTrigger = false;
                Console.WriteLine("REMOVE EFFECT SKILL 01");
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBeginMoving -= new PlayerEventHandle(this.method_1);
        }
    }
}
