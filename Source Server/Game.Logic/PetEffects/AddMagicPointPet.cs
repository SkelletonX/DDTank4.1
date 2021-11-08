using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class AddMagicPointPet : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public AddMagicPointPet(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.AddMagicPointPet, elementID)
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
            AddMagicPointPet pE = living.PetEffectList.GetOfType(ePetEffectType.AddMagicPointPet) as AddMagicPointPet;
            if (pE != null)
            {
                pE.int_2 = ((this.int_2 > pE.int_2) ? this.int_2 : pE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.BeginSelfTurn += new LivingEventHandle(this.method_0);
        }
        private void method_0(Living living_0)
        {
            (living_0 as Player).AddPetMP(4);
            Console.WriteLine("ADD 4 POINT");
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.BeginSelfTurn -= new LivingEventHandle(this.method_0);
        }
    }
}
