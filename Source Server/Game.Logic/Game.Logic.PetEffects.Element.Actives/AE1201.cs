using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Actives
{
	public class AE1201 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public AE1201(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.AE1201, elementID)
		{
			m_count = count;
			m_coldDown = count;
			m_probability = ((probability == -1) ? 10000 : probability);
			m_type = type;
			m_delay = delay;
			m_currentId = skillId;
		}

		public override bool Start(Living living)
		{
			AE1201 aE = living.PetEffectList.GetOfType(ePetEffectType.AE1201) as AE1201;
			if (aE == null)
			{
				return base.Start(living);
			}
			aE.m_probability = ((m_probability > aE.m_probability) ? m_probability : aE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.AfterKillingLiving += Player_AfterKillingLiving;
			player.PlayerBuffSkillPet += Player_PlayerBuffSkillPet;
		}

		private void Player_PlayerBuffSkillPet(Player player)
		{
			if (player.PetEffects.CurrentUseSkill == m_currentId)
			{
				IsTrigger = true;
			}
		}

		private void Player_AfterKillingLiving(Living living, Living target, int damageAmount, int criticalAmount)
		{
			if (IsTrigger)
			{
				IsTrigger = false;
				target.AddPetEffect(new CE1201(3, m_probability, m_type, m_currentId, m_delay, base.ElementInfo.ID.ToString()), 0);
			}
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.AfterKillingLiving -= Player_AfterKillingLiving;
			player.PlayerBuffSkillPet -= Player_PlayerBuffSkillPet;
		}
	}
}
