using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Actives
{
	public class AE1610 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public AE1610(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.AE1610, elementID)
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
			AE1610 aE = living.PetEffectList.GetOfType(ePetEffectType.AE1610) as AE1610;
			if (aE == null)
			{
				return base.Start(living);
			}
			aE.m_probability = ((m_probability > aE.m_probability) ? m_probability : aE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.PlayerBuffSkillPet += player_AfterBuffSkillPetByLiving;
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.PlayerBuffSkillPet -= player_AfterBuffSkillPetByLiving;
		}

		private void player_AfterBuffSkillPetByLiving(Player player)
		{
			int count = 1;
			int currentId = m_currentId;
			int num = currentId;
			if ((uint)(num - 468) <= 1u || (uint)(num - 476) <= 1u)
			{
				count = 2;
			}
			new CE1610(count, m_probability, m_type, m_currentId, m_delay, base.ElementInfo.ID.ToString()).Start(player);
		}
	}
}
