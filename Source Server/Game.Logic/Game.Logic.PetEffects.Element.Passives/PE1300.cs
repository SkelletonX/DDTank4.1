using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Passives
{
	public class PE1300 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public PE1300(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.PE1300, elementID)
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
			PE1300 pE = living.PetEffectList.GetOfType(ePetEffectType.PE1300) as PE1300;
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
			if (!IsTrigger)
			{
				foreach (Player allEnemyPlayer in living.Game.GetAllEnemyPlayers(living))
				{
					if (!(allEnemyPlayer.PetEffectList.GetOfType(ePetEffectType.CE1300) is CE1300))
					{
						allEnemyPlayer.AddPetEffect(new CE1300(0, m_probability, m_type, m_currentId, m_delay, base.ElementInfo.ID.ToString()), 0);
					}
				}
				IsTrigger = true;
			}
		}
	}
}
