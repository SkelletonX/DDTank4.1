using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;
namespace Game.Logic.PetEffects
{
    public class CASE1100 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1100(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1100, elementID)
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
            CASE1100 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1100) as CASE1100;
            if (aE != null)
            {
                aE.int_2 = ((this.int_2 > aE.int_2) ? this.int_2 : aE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.PlayerBuffSkillPet += new PlayerEventHandle(this.prkHcjirAc);
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(this.prkHcjirAc);
        }
        private void prkHcjirAc(Player player_0)
        {
            if (player_0.PetEffects.CurrentUseSkill == this.int_5)
            {
                using (List<Player>.Enumerator enumerator = player_0.Game.GetAllTeamPlayers(player_0).GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        enumerator.Current.AddPetEffect(new CASE1100A(3, this.int_2, this.int_0, this.int_5, this.int_3, base.Info.ID.ToString()), 0);
                        Console.WriteLine("+100 HP Toi Da");
                    }
                }
            }
        }
    }
}
namespace Game.Logic.PetEffects
{
    public class CASE1100A : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1100A(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1100A, elementID)
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
            CASE1100A cE = living.PetEffectList.GetOfType(ePetEffectType.CASE1100A) as CASE1100A;
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
                player.MaxBlood += this.int_6;
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
            player.MaxBlood -= this.int_6;
            this.int_6 = 0;
            player.BeginSelfTurn -= new LivingEventHandle(this.method_1);
        }
    }
}