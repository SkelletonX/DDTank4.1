using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Passives
{
	public class PE2955 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public PE2955(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.PE2955, elementID)
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
			PE2955 pE = living.PetEffectList.GetOfType(ePetEffectType.PE2955) as PE2955;
			if (pE == null)
			{
				return base.Start(living);
			}
			pE.m_probability = ((m_probability > pE.m_probability) ? m_probability : pE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.PlayerShoot += ChangeProperty;
			player.AfterPlayerShooted += player_AfterPlayerShooted;
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.PlayerShoot -= ChangeProperty;
			player.AfterPlayerShooted -= player_AfterPlayerShooted;
		}

		private void ChangeProperty(Player player)
		{
			m_added = 50;
			player.PetEffects.DamagePercent += m_added;
		}

		private void player_AfterPlayerShooted(Player player)
		{
			player.PetEffects.DamagePercent -= m_added;
			m_added = 0;
		}
	}
}
