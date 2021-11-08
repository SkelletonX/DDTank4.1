using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class AddBaseDamageEffectLv2 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public AddBaseDamageEffectLv2(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.AddBaseDamageEffectLv2, elementID)
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
            AddBaseDamageEffectLv2 aE = living.PetEffectList.GetOfType(ePetEffectType.AddBaseDamageEffectLv2) as AddBaseDamageEffectLv2;
            if (aE != null)
            {
                aE.int_2 = ((this.int_2 > aE.int_2) ? this.int_2 : aE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.PlayerBuffSkillPet += new PlayerEventHandle(this.method_0);
        }
        protected override void OnPausedOnPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(this.method_0);
            player.Game.method_7(player, base.Info, false);
            player.BaseDamage -= (double)this.int_6;
        }
        private void method_0(Player player_0)
        {
            if (player_0.PetEffects.CurrentUseSkill == this.int_5)
            {
                this.int_6 = (int)(player_0.BaseDamage * 45.0 / 100.0);
                player_0.BaseDamage += (double)this.int_6;
                player_0.Game.method_7(player_0, base.Info, true);
                Console.WriteLine("+45% BASE DAMAGE");
            }
        }
    }
}
