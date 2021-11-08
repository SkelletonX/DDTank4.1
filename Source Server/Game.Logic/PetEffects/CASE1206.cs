using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;
namespace Game.Logic.PetEffects
{
    public class CASE1206 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1206(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1206, elementID)
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
            CASE1206 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1206) as CASE1206;
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
            if (player_0.PetEffects.CurrentUseSkill == this.int_5 && player_0.Game is PVPGame)
            {
                using (List<Player>.Enumerator enumerator = player_0.Game.GetAllEnemyPlayers(player_0).GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        enumerator.Current.AddPetEffect(new CASE1206A(2, this.int_2, this.int_0, this.int_5, this.int_3, base.Info.ID.ToString()), 0);
                        Console.WriteLine("-300 diem tan cong cua ke dich");
                    }
                }
            }
        }
    }
}
