using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Passives
{
	public class PE1435 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public PE1435(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.PE1435, elementID)
		{
			m_count = count;
			m_coldDown = 3;
			m_probability = ((probability == -1) ? 10000 : probability);
			m_type = type;
			m_delay = delay;
			m_currentId = skillId;
		}

		public override bool Start(Living living)
		{
			PE1435 pE = living.PetEffectList.GetOfType(ePetEffectType.PE1435) as PE1435;
			if (pE == null)
			{
				return base.Start(living);
			}
			pE.m_probability = ((m_probability > pE.m_probability) ? m_probability : pE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.AfterKilledByLiving += player_afterKilledByLiving;
			player.BeginNextTurn += player_beginNextTurn;
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.AfterKilledByLiving -= player_afterKilledByLiving;
			player.BeginNextTurn -= player_beginNextTurn;
		}

		private void player_afterKilledByLiving(Living living, Living target, int damageAmount, int criticalAmount)
		{
			if (living.Game is PVPGame && m_coldDown > 0 && criticalAmount != 0 && living.Blood <= 0)
			{
				m_added = living.MaxBlood * 5 / 100;
				living.SyncAtTime = true;
				living.AddBlood(m_added);
				living.SyncAtTime = false;
				living.Game.SendPetBuff(living, base.ElementInfo, isActive: true);
				m_coldDown--;
			}
		}

		public void player_beginNextTurn(Living living)
		{
			if (m_coldDown > 0 && m_added != 0)
			{
				living.Game.SendPetBuff(living, base.ElementInfo, isActive: false);
				m_added = 0;
			}
		}
	}
}
