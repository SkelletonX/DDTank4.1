using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.ContinueElement
{
	public class CE1184 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public CE1184(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.CE1184, elementID)
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
			CE1184 cE = living.PetEffectList.GetOfType(ePetEffectType.CE1184) as CE1184;
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
			if (m_added == 0)
			{
				m_added = 150;
				player.BaseDamage += m_added;
			}
		}

		private void Player_BeginNextTurn(Living living)
		{
		}

		private void Player_BeginSelfTurn(Living living)
		{
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.BeginNextTurn -= Player_BeginNextTurn;
			player.BeginSelfTurn -= Player_BeginSelfTurn;
			player.Game.SendPetBuff(player, base.ElementInfo, isActive: false);
		}
	}
}
