using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class PlayerStopMovingEffect : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        public PlayerStopMovingEffect(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.PlayerStopMovingEffect, elementID)
        {
            this.int_1 = count;
            this.int_2 = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.int_3 = delay;
            this.int_4 = skillId;
        }
        private void method_0(Living living_0)
        {
            if (this.rand.Next(10000) < this.int_2 && living_0.PetEffects.CurrentUseSkill == this.int_4)
            {
                foreach (Player current in living_0.Game.GetAllEnemyPlayers(living_0))
                {
                    current.PetEffectTrigger = true;
                    current.Game.method_43(current, base.Info, true);
                    current.AddPetEffect(new PlayerStopMovingEffects(this.int_1, base.Info.ID.ToString()), 0);
                    Console.WriteLine("CANT MOVE");
                }
            }
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.PlayerBuffSkillPet += new PlayerEventHandle(this.method_0);
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(this.method_0);
        }
        public override bool Start(Living living)
        {
            PlayerStopMovingEffect petStopMovingAllEnemyEffect = living.PetEffectList.GetOfType(ePetEffectType.PlayerStopMovingEffect) as PlayerStopMovingEffect;
            bool result;
            if (petStopMovingAllEnemyEffect != null)
            {
                petStopMovingAllEnemyEffect.int_2 = ((this.int_2 > petStopMovingAllEnemyEffect.int_2) ? this.int_2 : petStopMovingAllEnemyEffect.int_2);
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
