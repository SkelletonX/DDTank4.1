using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Passives
{
	public class PE1430 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public PE1430(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.PE1430, elementID)
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
			PE1430 pE = living.PetEffectList.GetOfType(ePetEffectType.PE1430) as PE1430;
			if (pE == null)
			{
				return base.Start(living);
			}
			pE.m_probability = ((m_probability > pE.m_probability) ? m_probability : pE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.TakePlayerDamage += player_BeforeTakeDamage;
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.TakePlayerDamage -= player_BeforeTakeDamage;
		}

		private void player_BeforeTakeDamage(Living living, Living source, ref int damageAmount, ref int criticalAmount)
		{
			if (rand.Next(100) < 20)
			{
				criticalAmount = 0;
			}
		}
	}
}
