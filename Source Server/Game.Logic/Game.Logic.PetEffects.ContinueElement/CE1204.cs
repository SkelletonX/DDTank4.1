using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.ContinueElement
{
	public class CE1204 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public CE1204(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.CE1204, elementID)
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
			CE1204 cE = living.PetEffectList.GetOfType(ePetEffectType.CE1204) as CE1204;
			if (cE == null)
			{
				return base.Start(living);
			}
			cE.m_probability = ((m_probability > cE.m_probability) ? m_probability : cE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.BeginNextTurn += Player_BeginNextTurn;
			player.BeginSelfTurn += Player_BeginSelfTurn;
			player.PlayerClearBuffSkillPet += Player_PlayerClearBuffSkillPet;
		}

		private void Player_PlayerClearBuffSkillPet(Player player)
		{
			Stop();
		}

		private void Player_BeginNextTurn(Living living)
		{
			if (m_added == 0)
			{
				m_added = 300;
				if (living.Attack < (double)m_added)
				{
					m_added = (int)living.Attack - 1;
				}
				living.Attack -= m_added;
			}
		}

		private void Player_BeginSelfTurn(Living living)
		{
			m_count--;
			if (m_count < 0)
			{
				Stop();
			}
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.Attack += m_added;
			m_added = 0;
			player.Game.SendPetBuff(player, base.ElementInfo, isActive: false);
			player.BeginNextTurn -= Player_BeginNextTurn;
			player.BeginSelfTurn -= Player_BeginSelfTurn;
		}
	}
}
