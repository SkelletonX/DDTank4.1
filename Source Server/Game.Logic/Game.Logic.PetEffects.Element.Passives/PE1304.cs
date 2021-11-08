using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Passives
{
	public class PE1304 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public PE1304(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.PE1304, elementID)
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
			PE1304 pE = living.PetEffectList.GetOfType(ePetEffectType.PE1304) as PE1304;
			if (pE == null)
			{
				return base.Start(living);
			}
			pE.m_probability = ((m_probability > pE.m_probability) ? m_probability : pE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.BeginSelfTurn += player_beginSeftTurn;
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.BeginSelfTurn -= player_beginSeftTurn;
		}

		public void player_beginSeftTurn(Living living)
		{
			if (living.Game is PVPGame)
			{
				foreach (Player allEnemyPlayer in living.Game.GetAllEnemyPlayers(living))
				{
					if (allEnemyPlayer.PetMP > 0)
					{
						allEnemyPlayer.Game.SendPetBuff(allEnemyPlayer, base.ElementInfo, isActive: true, 0);
						allEnemyPlayer.RemovePetMP(1);
						allEnemyPlayer.AddPetEffect(new CE1304(0, m_probability, m_type, m_currentId, m_delay, base.ElementInfo.ID.ToString()), 0);
					}
				}
			}
		}
	}
}
