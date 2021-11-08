using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Passives
{
	public class PE1237 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public PE1237(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.PE1237, elementID)
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
			PE1237 pE = living.PetEffectList.GetOfType(ePetEffectType.PE1237) as PE1237;
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
			if (rand.Next(100) >= 45)
			{
				return;
			}
			m_added = 800;
			source.SyncAtTime = true;
			source.AddBlood(-m_added, 1);
			source.SyncAtTime = false;
			if (source.Blood < 0)
			{
				source.Die();
				if (living != null && living is Player)
				{
					(living as Player).PlayerDetail.OnKillingLiving(living.Game, 2, source.Id, source.IsLiving, m_added);
				}
			}
			living.PetEffects.ReboundDamage = m_added;
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.BeforeTakeDamage -= Player_BeforeTakeDamage;
		}
	}
}
