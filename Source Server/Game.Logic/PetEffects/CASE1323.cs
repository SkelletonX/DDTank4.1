using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1323 : BasePetEffect
    {
        private int oujhNlcsrm;
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int qbUhpMnps2;
        private int int_4;
        public CASE1323(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1323, elementID)
        {
            this.int_0 = count;
            this.int_3 = count;
            this.int_1 = ((probability == -1) ? 10000 : probability);
            this.oujhNlcsrm = type;
            this.int_2 = delay;
            this.qbUhpMnps2 = skillId;
        }
        public override bool Start(Living living)
        {
            CASE1323 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1323) as CASE1323;
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
            if (player_0.PetEffects.CurrentUseSkill == this.qbUhpMnps2 && player_0.Game is PVPGame)
            {
                foreach (Player current in player_0.Game.GetAllEnemyPlayers(player_0))
                {
                    this.int_4 = current.MaxBlood * 5 / 100;
                    if (current.Blood < this.int_4)
                    {
                        current.Die();
                        if (player_0 != null)
                        {
                            player_0.PlayerDetail.OnKillingLiving(player_0.Game, 2, current.Id, current.IsLiving, this.int_4);
                            Console.WriteLine("Giet Dich Duoi 5% HP");
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
