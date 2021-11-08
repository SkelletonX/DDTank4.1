using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.ContinueElement
{
	public class CE1555 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public CE1555(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.CE1555, elementID)
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
			CE1555 cE = living.PetEffectList.GetOfType(ePetEffectType.CE1555) as CE1555;
			if (cE == null)
			{
				return base.Start(living);
			}
			cE.m_probability = ((m_probability > cE.m_probability) ? m_probability : cE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.BeforeTakeDamage += Player_BeforeTakeDamage;
			player.BeginSelfTurn += Player_BeginSelfTurn;
		}

		private void Player_BeforeTakeDamage(Living living, Living source, ref int damageAmount, ref int criticalAmount)
		{
			if (m_added == 0)
			{
				m_added = 30;
				damageAmount -= damageAmount * m_added / 100;
			}
		}

		private void Player_BeginSelfTurn(Living living)
		{
			m_count--;
			if (m_count < 0)
			{
				Stop();
			}
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			m_added = 0;
			player.BeforeTakeDamage -= Player_BeforeTakeDamage;
			player.BeginSelfTurn -= Player_BeginSelfTurn;
		}
	}
}
