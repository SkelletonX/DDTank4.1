using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class ImmunityEffects : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int CqmtiLajYR;
        private int int_3;
        private int int_4;
        private int JlUtIkIbGG;
        public ImmunityEffects(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.ImmunityEffects, elementID)
        {
            this.int_1 = count;
            this.int_3 = count;
            this.int_2 = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.CqmtiLajYR = delay;
            this.int_4 = skillId;
        }
        public override bool Start(Living living)
        {
            ImmunityEffects cE = living.PetEffectList.GetOfType(ePetEffectType.ImmunityEffects) as ImmunityEffects;
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
            player.IsNoHole = true;
            player.Game.method_68(player, 5, true);
            player.PlayerClearBuffSkillPet += new PlayerEventHandle(this.method_0);
        }
        private void method_0(Player player_0)
        {
            this.Stop();
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.BeginSelfTurn -= new LivingEventHandle(this.method_1);
            player.IsNoHole = false;
            player.Game.method_68(player, 5, false);
        }
        private void method_1(Living living_0)
        {
            this.int_1--;
            if (this.int_1 < 0)
            {
                this.Stop();
            }
        }
    }
}
