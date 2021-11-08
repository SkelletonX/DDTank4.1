using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1324 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1324(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1324, elementID)
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
            CASE1324 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1324) as CASE1324;
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
                    this.int_6 = current.MaxBlood * 10 / 100;
                    if (current.Blood < this.int_6)
                    {
                        current.Die();
                        if (player_0 != null)
                        {
                            player_0.PlayerDetail.OnKillingLiving(player_0.Game, 2, current.Id, current.IsLiving, this.int_6);
                            Console.WriteLine("Giet Dich Duoi 10% HP");
                        }
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
