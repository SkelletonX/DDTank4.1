using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1058 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        public CASE1058(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1058, elementID)
        {
            this.int_1 = count;
            this.int_5 = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.int_2 = delay;
            this.int_3 = skillId;
            if (skillId != 61)
            {
                if (skillId == 62)
                {
                    this.int_4 = 10;
                }
            }
            else
            {
                this.int_4 = 8;
            }
        }
        private void method_0(Living living_0)
        {
            if (this.rand.Next(10000) < this.int_5 && living_0.PetEffects.CurrentUseSkill == this.int_3)
            {
                living_0.PetEffectTrigger = true;
                living_0.SyncAtTime = true;
                living_0.AddBlood(living_0.MaxBlood / 100 * int_4);
                living_0.SyncAtTime = false;
                foreach (Living current in living_0.Game.Map.FindAllNearestSameTeam(living_0.X, living_0.Y, 150.0, living_0))
                {
                    if (current != living_0)
                    {
                        current.SyncAtTime = true;
                        current.AddBlood(current.MaxBlood / 100 * int_4);
                        current.SyncAtTime = false;
                        Console.WriteLine("Hoi Phuc % HP", int_4);
                    }
                }
            }
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.PlayerBuffSkillPet += new PlayerEventHandle(this.method_0);
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(this.method_0);
        }
        public override bool Start(Living living)
        {
            CASE1058 petRevertBloodAllPlayerAroundEquipEffect = living.PetEffectList.GetOfType(ePetEffectType.CASE1058) as CASE1058;
            bool result;
            if (petRevertBloodAllPlayerAroundEquipEffect != null)
            {
                petRevertBloodAllPlayerAroundEquipEffect.int_5 = ((this.int_5 > petRevertBloodAllPlayerAroundEquipEffect.int_5) ? this.int_5 : petRevertBloodAllPlayerAroundEquipEffect.int_5);
                result = true;
            }
            else
            {
                result = base.Start(living);
            }
            return result;
        }
    }
}
