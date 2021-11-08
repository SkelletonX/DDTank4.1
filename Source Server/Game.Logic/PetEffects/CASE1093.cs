using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1093 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int kAkwkViyAH;
        public CASE1093(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1093, elementID)
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
            CASE1093 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1093) as CASE1093;
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
            if (player_0.PetEffects.CurrentUseSkill == this.int_5)
            {
                foreach (Player current in player_0.Game.GetAllTeamPlayers(player_0))
                {
                    current.Game.method_8(current, base.Info, true, 1);
                    current.AddPetEffect(new CASE1093A(3, this.int_2, this.int_0, this.int_5, this.int_3, base.Info.ID.ToString()), 0);
                    Console.WriteLine("+300 Cong Kich");
                }
            }
        }
    }
}
namespace Game.Logic.PetEffects
{
    public class CASE1093A : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1093A(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1093A, elementID)
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
            CASE1093A cE = living.PetEffectList.GetOfType(ePetEffectType.CASE1093A) as CASE1093A;
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
                this.int_6 = 300;
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