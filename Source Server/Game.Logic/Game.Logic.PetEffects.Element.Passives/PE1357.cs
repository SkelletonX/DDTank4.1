using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Passives
{
	public class PE1357 : BasePetEffect
	{
		private int m_type = 0;

		private double m_count = 0.0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private double m_added = 0.0;

		private int m_currentId;

		public PE1357(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.PE1357, elementID)
		{
			m_count = count;
			m_coldDown = 0;
			m_probability = ((probability == -1) ? 10000 : probability);
			m_type = type;
			m_delay = delay;
			m_currentId = skillId;
		}

		public override bool Start(Living living)
		{
			PE1357 pE = living.PetEffectList.GetOfType(ePetEffectType.PE1357) as PE1357;
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
			player.AfterKilledByLiving += Player_AfterKilledByLiving;
			player.BeginSelfTurn += Player_BeginSelfTurn;
		}

		private void Player_BeforeTakeDamage(Living living, Living source, ref int damageAmount, ref int criticalAmount)
		{
			if (m_coldDown < 6)
			{
				if (m_added == 0.0)
				{
					m_added = 55.0;
				}
				living.BaseDamage += m_added;
				m_count += m_added;
				IsTrigger = true;
				m_coldDown++;
			}
		}

		private void Player_BeginSelfTurn(Living living)
		{
			m_added = 25.5;
		}

		private void Player_AfterKilledByLiving(Living living, Living target, int damageAmount, int criticalAmount)
		{
			if (IsTrigger)
			{
				living.Game.SendPetBuff(living, base.ElementInfo, isActive: true);
				IsTrigger = false;
			}
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.AfterKilledByLiving -= Player_AfterKilledByLiving;
		}
	}
}
