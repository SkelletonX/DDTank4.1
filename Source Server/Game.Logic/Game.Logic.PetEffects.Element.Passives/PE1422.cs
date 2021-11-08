using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Passives
{
	public class PE1422 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public PE1422(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.AE1422, elementID)
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
			PE1422 pE = living.PetEffectList.GetOfType(ePetEffectType.AE1422) as PE1422;
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
				foreach (Player allTeamPlayer in living.Game.GetAllTeamPlayers(living))
				{
					if (allTeamPlayer.MagicAttack != 0.0)
					{
						m_added = (int)(allTeamPlayer.MagicAttack * 5.0 / 100.0);
						allTeamPlayer.MagicAttack += m_added;
					}
				}
			}
		}
	}
}
