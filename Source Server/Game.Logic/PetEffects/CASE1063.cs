using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1063 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        public CASE1063(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1063, elementID)
        {
            this.int_1 = count;
            this.int_2 = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.int_3 = delay;
            this.int_4 = skillId;
        }
        private void method_0(Living living_0)
        {
            if (this.rand.Next(10000) < this.int_2 && living_0.PetEffects.CurrentUseSkill == this.int_4)
            {
                living_0.PetEffectTrigger = true;
                living_0.Game.method_43(living_0, base.Info, true);
                new CASE1063A(this.int_1, this.int_4, base.Info.ID.ToString()).Start(living_0);
                Console.WriteLine("+3% & +4% HP moi turn");
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
            CASE1063 petAddBloodForSelfEquipEffect = living.PetEffectList.GetOfType(ePetEffectType.CASE1063) as CASE1063;
            bool result;
            if (petAddBloodForSelfEquipEffect != null)
            {
                petAddBloodForSelfEquipEffect.int_2 = ((this.int_2 > petAddBloodForSelfEquipEffect.int_2) ? this.int_2 : petAddBloodForSelfEquipEffect.int_2);
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
