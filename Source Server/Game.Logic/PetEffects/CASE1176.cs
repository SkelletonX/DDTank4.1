using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1176 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1176(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1176, elementID)
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
            CASE1176 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1176) as CASE1176;
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
        private void method_0(Player player_0)
        {
            if (player_0.PetEffects.CurrentUseSkill == this.int_5 && player_0.Game is PVPGame)
            {
                foreach (Player current in player_0.Game.GetAllEnemyPlayers(player_0))
                {
                    current.Game.method_7(current, base.Info, true);
                    current.AddPetEffect(new CASE1176A(3, this.int_2, this.int_0, this.int_5, this.int_3, base.Info.ID.ToString(), player_0), 0);
                    Console.WriteLine("Moi Turn Mat Them 2% HP");
                }
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(this.method_0);
        }
    }
}
namespace Game.Logic.PetEffects
{
    public class CASE1176A : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        private Living living_0;
        public CASE1176A(int count, int probability, int type, int skillId, int delay, string elementID, Living source) : base(ePetEffectType.CASE1176A, elementID)
        {
            this.int_1 = count;
            this.int_4 = count;
            this.int_2 = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.int_3 = delay;
            this.int_5 = skillId;
            this.living_0 = source;
        }
        public override bool Start(Living living)
        {
            CASE1176A cE = living.PetEffectList.GetOfType(ePetEffectType.CASE1176A) as CASE1176A;
            if (cE != null)
            {
                cE.int_2 = ((this.int_2 > cE.int_2) ? this.int_2 : cE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.BeginNextTurn += new LivingEventHandle(this.method_1);
            player.BeginSelfTurn += new LivingEventHandle(this.method_2);
            player.PlayerClearBuffSkillPet += new PlayerEventHandle(this.method_0);
        }
        private void method_0(Player player_0)
        {
            this.Stop();
        }
        private void method_1(Living living_1)
        {
        }
        private void method_2(Living living_1)
        {
            this.int_1--;
            if (this.int_1 < 0)
            {
                this.Stop();
                return;
            }
            this.int_6 = living_1.Blood * 2 / 100;
            living_1.AddBlood(-this.int_6, 1);
            if (living_1.Blood <= 0)
            {
                living_1.Die();
                if (this.living_0 != null && this.living_0 is Player)
                {
                    (this.living_0 as Player).PlayerDetail.OnKillingLiving(this.living_0.Game, 2, living_1.Id, living_1.IsLiving, this.int_6);
                }
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.Game.method_7(player, base.Info, false);
            player.BeginNextTurn -= new LivingEventHandle(this.method_1);
            player.BeginSelfTurn -= new LivingEventHandle(this.method_2);
        }
    }
}