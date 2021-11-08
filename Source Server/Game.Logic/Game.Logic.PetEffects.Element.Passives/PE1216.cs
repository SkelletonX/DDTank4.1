using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Passives
{
	public class PE1216 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public PE1216(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.PE1216, elementID)
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
			PE1216 pE = living.PetEffectList.GetOfType(ePetEffectType.PE1216) as PE1216;
			if (pE == null)
			{
				return base.Start(living);
			}
			pE.m_probability = ((m_probability > pE.m_probability) ? m_probability : pE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			if (m_added == 0)
			{
				m_added = 1500;
				player.PetEffects.AddMaxBloodValue = m_added;
			}
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
		}
	}
}
