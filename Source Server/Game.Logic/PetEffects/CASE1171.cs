using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;
namespace Game.Logic.PetEffects
{
    public class CASE1171 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int ylrhzmNwqZ;
        private int int_3;
        private int int_4;
        private int int_5;
        public CASE1171(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1171, elementID)
        {
            this.int_1 = count;
            this.int_3 = count;
            this.int_2 = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.ylrhzmNwqZ = delay;
            this.int_4 = skillId;
        }
        public override bool Start(Living living)
        {
            CASE1171 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1171) as CASE1171;
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
            if (player_0.PetEffects.CurrentUseSkill == this.int_4 && player_0.Game is PVPGame)
            {
                using (List<Player>.Enumerator enumerator = player_0.Game.GetAllEnemyPlayers(player_0).GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        enumerator.Current.AddPetEffect(new CASE1171A(3, this.int_2, this.int_0, this.int_4, this.ylrhzmNwqZ, base.Info.ID.ToString(), player_0), 0);
                        Console.WriteLine("Turn Dich Se Bi Tru -20% HP");
                    }
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
    public class CASE1171A : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int aaYvprckEL;
        private int int_5;
        private Living living_0;
        public CASE1171A(int count, int probability, int type, int skillId, int delay, string elementID, Living source) : base(ePetEffectType.CASE1171A, elementID)
        {
            this.int_1 = count;
            this.int_4 = count;
            this.int_2 = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.int_3 = delay;
            this.aaYvprckEL = skillId;
            this.living_0 = source;
        }
        public override bool Start(Living living)
        {
            CASE1171A cE = living.PetEffectList.GetOfType(ePetEffectType.CASE1171A) as CASE1171A;
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
            this.int_5 = 2000;
            living_1.AddBlood(-this.int_5, 1);
            if (living_1.Blood <= 0)
            {
                living_1.Die();
                if (this.living_0 != null && this.living_0 is Player)
                {
                    (this.living_0 as Player).PlayerDetail.OnKillingLiving(this.living_0.Game, 2, living_1.Id, living_1.IsLiving, this.int_5);
                }
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.BeginNextTurn -= new LivingEventHandle(this.method_1);
            player.BeginSelfTurn -= new LivingEventHandle(this.method_2);
        }
    }
}
