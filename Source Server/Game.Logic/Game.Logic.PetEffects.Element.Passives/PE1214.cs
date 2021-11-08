using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Passives
{
	public class PE1214 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public PE1214(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.PE1214, elementID)
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
			PE1214 pE = living.PetEffectList.GetOfType(ePetEffectType.PE1214) as PE1214;
			if (pE == null)
			{
				return base.Start(living);
			}
			pE.m_probability = ((m_probability > pE.m_probability) ? m_probability : pE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.BeginNextTurn += player_beginNextTurn;
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.BeginNextTurn -= player_beginNextTurn;
		}

		public void player_beginNextTurn(Living living)
		{
			if (m_added == 0)
			{
				m_added = 100;
				living.BaseGuard += m_added;
			}
		}
	}
}
