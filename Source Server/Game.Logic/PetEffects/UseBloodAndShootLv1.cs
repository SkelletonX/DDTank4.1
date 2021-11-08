using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class UseBloodAndShootLv1 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public UseBloodAndShootLv1(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.UseBloodAndShootLv1, elementID)
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
            UseBloodAndShootLv1 pE = living.PetEffectList.GetOfType(ePetEffectType.UseBloodAndShootLv1) as UseBloodAndShootLv1;
            if (pE != null)
            {
                pE.int_2 = ((this.int_2 > pE.int_2) ? this.int_2 : pE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.PlayerShoot += new PlayerEventHandle(this.method_1);
            player.AfterPlayerShooted += new PlayerEventHandle(this.method_0);
        }
        private void method_0(Player player_0)
        {
            player_0.Game.method_8(player_0, base.Info, false, 0);
        }
        private void method_1(Player player_0)
        {
            this.int_6 = player_0.Blood * 2 / 100;
            player_0.Game.method_8(player_0, base.Info, true, 0);
            foreach (Living current in player_0.Game.Map.FindAllNearestEnemy(player_0.X, player_0.Y, 250.0, player_0))
            {
                current.SyncAtTime = true;
                current.AddBlood(-this.int_6, 1);
                current.SyncAtTime = false;
                if (current.Blood <= 0)
                {
                    current.Die();
                    if (player_0 != null && player_0 != null)
                    {
                        player_0.PlayerDetail.OnKillingLiving(player_0.Game, 2, current.Id, current.IsLiving, this.int_6);
                        Console.WriteLine("USE 2% BLOOD FOR DAMAGE");
                    }
                }
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerShoot -= new PlayerEventHandle(this.method_1);
            player.AfterPlayerShooted -= new PlayerEventHandle(this.method_0);
        }
    }
}
