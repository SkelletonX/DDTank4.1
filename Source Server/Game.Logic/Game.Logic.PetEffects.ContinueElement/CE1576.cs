using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.ContinueElement
{
	public class CE1576 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public CE1576(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.CE1576, elementID)
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
			CE1576 cE = living.PetEffectList.GetOfType(ePetEffectType.CE1576) as CE1576;
			if (cE == null)
			{
				return base.Start(living);
			}
			cE.m_probability = ((m_probability > cE.m_probability) ? m_probability : cE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.BeginNextTurn += Player_BeginNextTurn;
			player.BeginSelfTurn += Player_BeginSelfTurn;
		}

		private void Player_BeginNextTurn(Living living)
		{
			m_added = (int)((double)living.Blood / (double)living.MaxBlood * 100.0);
			if (m_added > 80)
			{
				m_added = 80;
				living.BaseDamage += living.BaseDamage * (double)m_added / 100.0;
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
			player.BaseDamage -= m_added;
			m_added = 0;
			player.BeginNextTurn -= Player_BeginNextTurn;
			player.BeginSelfTurn -= Player_BeginSelfTurn;
		}
	}
}
