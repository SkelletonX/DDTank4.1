using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1150 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int iqbYnSvJeI;
        private int int_4;
        private int int_5;
        public CASE1150(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1150, elementID)
        {
            this.int_1 = count;
            this.iqbYnSvJeI = count;
            this.int_2 = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.int_3 = delay;
            this.int_4 = skillId;
        }
        public override bool Start(Living living)
        {
            CASE1150 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1150) as CASE1150;
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
            if (player_0.PetEffects.CurrentUseSkill == this.int_4 && player_0.Game is PVPGame)
            {
                this.IsTrigger = true;
            }
        }
        private void method_1(Living living_0, Living living_1, int int_6, int int_7)
        {
            if (this.IsTrigger)
            {
                this.IsTrigger = false;
                living_1.Game.method_7(living_1, base.Info, true);
                living_1.AddPetEffect(new CASE1150A(3, this.int_2, this.int_0, this.int_4, this.int_3, base.Info.ID.ToString(), living_0), 0);
                Console.WriteLine("Bi Danh Trung Mat 1% HP");
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.AfterKillingLiving -= new KillLivingEventHanlde(this.method_1);
            player.PlayerBuffSkillPet -= new PlayerEventHandle(this.method_0);
        }
    }
}
