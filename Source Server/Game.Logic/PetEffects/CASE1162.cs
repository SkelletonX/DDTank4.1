using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;
namespace Game.Logic.PetEffects
{
    public class CASE1162 : BasePetEffect
    {
        private int int_0;
        private int aBdhAjRolk;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        public CASE1162(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1162, elementID)
        {
            this.aBdhAjRolk = count;
            this.int_3 = count;
            this.int_1 = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.int_2 = delay;
            this.int_4 = skillId;
        }
        public override bool Start(Living living)
        {
            CASE1162 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1162) as CASE1162;
            if (aE != null)
            {
                aE.int_1 = ((this.int_1 > aE.int_1) ? this.int_1 : aE.int_1);
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
            if (player_0.PetEffects.CurrentUseSkill == this.int_4 && player_0.Game is PVPGame)
            {
                List<Player> arg_3D_0 = player_0.Game.GetAllEnemyPlayers(player_0);
                this.int_5 = 5000;
                foreach (Player current in arg_3D_0)
                {
                    current.Game.method_7(current, base.Info, true);
                    current.AddBlood(-this.int_5, 1);
                    if (current.Blood <= 0)
                    {
                        current.Die();
                        if (player_0 != null)
                        {
                            player_0.PlayerDetail.OnKillingLiving(player_0.Game, 2, current.Id, current.IsLiving, this.int_5);
                        }
                    }
                    current.AddPetEffect(new CASE1162A(0, this.int_1, this.int_0, this.int_4, this.int_2, base.Info.ID.ToString()), 0);
                    Console.WriteLine("Chan Long Tai Thien 1500% Sat Thuong");
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
    public class CASE1162A : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1162A(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1162A, elementID)
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
            CASE1162A cE = living.PetEffectList.GetOfType(ePetEffectType.CASE1162A) as CASE1162A;
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
            player.PlayerClearBuffSkillPet += new PlayerEventHandle(this.method_0);
        }
        private void method_0(Player player_0)
        {
            this.Stop();
        }
        private void method_1(Living living_0)
        {
            this.Stop();
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.Game.method_7(player, base.Info, false);
            player.BeginSelfTurn -= new LivingEventHandle(this.method_1);
        }
    }
}