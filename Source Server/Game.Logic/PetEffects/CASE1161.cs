using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;
namespace Game.Logic.PetEffects
{
    public class CASE1161 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1161(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1161, elementID)
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
            CASE1161 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1161) as CASE1161;
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
                List<Player> arg_3D_0 = player_0.Game.GetAllEnemyPlayers(player_0);
                this.int_6 = 3000;
                foreach (Player current in arg_3D_0)
                {
                    current.Game.method_7(current, base.Info, true);
                    current.AddBlood(-this.int_6, 1);
                    if (current.Blood <= 0)
                    {
                        current.Die();
                        if (player_0 != null)
                        {
                            player_0.PlayerDetail.OnKillingLiving(player_0.Game, 2, current.Id, current.IsLiving, this.int_6);
                        }
                    }
                    current.AddPetEffect(new CASE1161A(0, this.int_2, this.int_0, this.int_5, this.int_3, base.Info.ID.ToString()), 0);
                    Console.WriteLine("Chan Long Tai Thien 3000 Sat Thuong");
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
    public class CASE1161A : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1161A(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1161A, elementID)
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
            CASE1161A cE = living.PetEffectList.GetOfType(ePetEffectType.CASE1161A) as CASE1161A;
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