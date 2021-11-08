using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.ContinueElement
{
	public class CE1602 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public CE1602(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.CE1602, elementID)
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
			CE1602 cE = living.PetEffectList.GetOfType(ePetEffectType.CE1602) as CE1602;
			if (cE == null)
			{
				return base.Start(living);
			}
			cE.m_probability = ((m_probability > cE.m_probability) ? m_probability : cE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			if (m_added == 0)
			{
				m_added = (int)(player.BaseGuard * 30.0 / 100.0);
				player.BaseGuard -= m_added;
			}
			player.PlayerClearBuffSkillPet += Player_PlayerClearBuffSkillPet;
		}

		private void Player_PlayerClearBuffSkillPet(Player player)
		{
			Stop();
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.BaseGuard += m_added;
			m_added = 0;
		}
	}
}
