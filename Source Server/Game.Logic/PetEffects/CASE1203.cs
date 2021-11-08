using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1203 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1203(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1203, elementID)
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
            CASE1203 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1203) as CASE1203;
            if (aE != null)
            {
                aE.int_2 = ((this.int_2 > aE.int_2) ? this.int_2 : aE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.AfterKillingLiving += new KillLivingEventHanlde(this.method_1);
            player.PlayerBuffSkillPet += new PlayerEventHandle(this.method_0);
        }
        private void method_0(Player player_0)
        {
            if (player_0.PetEffects.CurrentUseSkill == this.int_5)
            {
                this.IsTrigger = true;
            }
        }
        private void method_1(Living living_0, Living living_1, int int_7, int int_8)
        {
            if (this.IsTrigger)
            {
                this.IsTrigger = false;
                living_1.AddPetEffect(new CASE1203A(3, this.int_2, this.int_0, this.int_5, this.int_3, base.Info.ID.ToString()), 0);
                Console.WriteLine("-300 sat thuong cua ke dich khi trung phai");
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.AfterKillingLiving -= new KillLivingEventHanlde(this.method_1);
            player.PlayerBuffSkillPet -= new PlayerEventHandle(this.method_0);
        }
    }
}
