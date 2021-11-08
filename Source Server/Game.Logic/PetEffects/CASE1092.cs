using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1092 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int iaewsDuFw1;
        private int int_5;
        public CASE1092(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1092, elementID)
        {
            this.int_1 = count;
            this.int_4 = count;
            this.int_2 = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.int_3 = delay;
            this.iaewsDuFw1 = skillId;
        }
        public override bool Start(Living living)
        {
            CASE1092 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1092) as CASE1092;
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
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(this.method_0);
        }
        private void method_0(Player player_0)
        {
            if (player_0.PetEffects.CurrentUseSkill == this.iaewsDuFw1)
            {
                foreach (Player current in player_0.Game.GetAllTeamPlayers(player_0))
                {
                    current.Game.method_8(current, base.Info, true, 1);
                    current.AddPetEffect(new CASE1092A(3, this.int_2, this.int_0, this.iaewsDuFw1, this.int_3, base.Info.ID.ToString()), 0);
                    Console.WriteLine("+100 Cong Kich");
                }
            }
        }
    }
}
namespace Game.Logic.PetEffects
{
    public class CASE1092A : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1092A(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1092A, elementID)
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
            CASE1092A cE = living.PetEffectList.GetOfType(ePetEffectType.CASE1092A) as CASE1092A;
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
            if (this.int_6 == 0)
            {
                this.int_6 = 100;
                player.Attack += (double)this.int_6;
            }
            player.PlayerClearBuffSkillPet += new PlayerEventHandle(this.method_0);
        }
        private void method_0(Player player_0)
        {
            this.Stop();
        }
        private void method_1(Living living_0)
        {
            this.int_1--;
            if (this.int_1 < 0)
            {
                this.Stop();
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.Game.method_8(player, base.Info, false, 1);
            player.Attack -= (double)this.int_6;
            this.int_6 = 0;
            player.BeginSelfTurn -= new LivingEventHandle(this.method_1);
        }
    }
}