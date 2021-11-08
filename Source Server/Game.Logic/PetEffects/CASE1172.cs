using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    #region Rồng Bảo Vệ
    public class CASE1172 : BasePetEffect
    {
        private int m_type;
        private int m_count;
        private int m_probability;
        private int m_delay;
        private int m_currentId;
        public CASE1172(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1172, elementID)
        {
            this.m_count = count;
            this.m_probability = ((probability == -1) ? 10000 : probability);
            this.m_type = type;
            this.m_delay = delay;
            this.m_currentId = skillId;
        }
        public override bool Start(Living living)
        {
            CASE1172 effect = living.PetEffectList.GetOfType(ePetEffectType.CASE1172) as CASE1172;
            if (effect != null)
            {
                effect.m_probability = ((this.m_probability > effect.m_probability) ? this.m_probability : effect.m_probability);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.PlayerBuffSkillPet += new PlayerEventHandle(this.player_AfterKilledByLiving);
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(this.player_AfterKilledByLiving);
        }
        private void player_AfterKilledByLiving(Living living)
        {
            if (this.rand.Next(10) < this.m_probability && living.PetEffects.CurrentUseSkill == this.m_currentId)
            {
                new CASE1172A(this.m_count, this.m_currentId, base.Info.ID.ToString()).Start(living);
                Console.WriteLine("Skill rong bao ve");
            }
        }
    }
    public class CASE1172A : AbstractPetEffect
    {
        private int m_count;
        private int m_currentId;
        private int m_value;

        public CASE1172A(int count, int skillId, string elementID) : base(ePetEffectType.CASE1172A, elementID)
        {
            this.m_count = count;
            this.m_currentId = skillId;
            switch (skillId)
            {
                case 108:
                    this.m_value = 2;
                    return;
                case 109:
                    this.m_value = 4;
                    return;
                default:
                    return;
            }
        }
        public override bool Start(Living living)
        {
            CASE1172A effect = living.PetEffectList.GetOfType(ePetEffectType.CASE1172A) as CASE1172A;
            if (effect != null)
            {
                effect.m_count = this.m_count;
                return true;
            }
            return base.Start(living);
        }
        public override void OnAttached(Living living)
        {
            living.BeginSelfTurn += new LivingEventHandle(this.player_BeginFitting);
            living.AfterKilledByLiving += new KillLivingEventHanlde(this.player_BeforeTakeDamage);
        }
        public override void OnRemoved(Living living)
        {
            living.BeginSelfTurn -= new LivingEventHandle(this.player_BeginFitting);
            living.AfterKilledByLiving -= new KillLivingEventHanlde(this.player_BeforeTakeDamage);
        }
        private void player_BeforeTakeDamage(Living living, Living source, int damageAmount, int criticalAmount)
        {
            if (this.rand.Next(10) < 3500)
            {
                living.SyncAtTime = true;
                living.AddBlood(living.MaxBlood / 100 * m_value);
                living.SyncAtTime = false;
            }
        }
        private void player_BeginFitting(Living living)
        {
            this.m_count--;
            if (this.m_count < 0)
            {
                this.Stop();
            }
        }
        #endregion
    }
}
