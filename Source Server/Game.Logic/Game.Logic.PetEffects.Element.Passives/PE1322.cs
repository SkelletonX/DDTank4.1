using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Passives
{
	public class PE1322 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public PE1322(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.PE1322, elementID)
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
			PE1322 pE = living.PetEffectList.GetOfType(ePetEffectType.PE1322) as PE1322;
			if (pE == null)
			{
				return base.Start(living);
			}
			pE.m_probability = ((m_probability > pE.m_probability) ? m_probability : pE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.BeforeTakeDamage += Player_BeforeTakeDamage;
		}

		private void Player_BeforeTakeDamage(Living living, Living source, ref int damageAmount, ref int criticalAmount)
		{
			m_added = (damageAmount + criticalAmount) / 3500;
			if (m_added > 0)
			{
				(living as Player).AddPetMP(m_added);
			}
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.BeforeTakeDamage -= Player_BeforeTakeDamage;
		}
	}
}
