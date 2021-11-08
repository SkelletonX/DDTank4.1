using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Passives
{
	public class PE1078 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public PE1078(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.PE1078, elementID)
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
			PE1078 pE = living.PetEffectList.GetOfType(ePetEffectType.PE1078) as PE1078;
			if (pE == null)
			{
				return base.Start(living);
			}
			pE.m_probability = ((m_probability > pE.m_probability) ? m_probability : pE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.PlayerUseSecondWeapon += Player_PlayerUseSecondWeapon;
		}

		private void Player_PlayerUseSecondWeapon(Player player, int value)
		{
			if (value == 31)
			{
				m_added = 500;
				player.SyncAtTime = true;
				player.AddBlood(m_added);
				player.SyncAtTime = false;
			}
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.PlayerUseSecondWeapon -= Player_PlayerUseSecondWeapon;
		}
	}
}
