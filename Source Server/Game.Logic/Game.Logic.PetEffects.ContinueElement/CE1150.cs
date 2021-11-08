using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.ContinueElement
{
	public class CE1150 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		private Living m_source;

		public CE1150(int count, int probability, int type, int skillId, int delay, string elementID, Living source)
			: base(ePetEffectType.CE1150, elementID)
		{
			m_count = count;
			m_coldDown = count;
			m_probability = ((probability == -1) ? 10000 : probability);
			m_type = type;
			m_delay = delay;
			m_currentId = skillId;
			m_source = source;
		}

		public override bool Start(Living living)
		{
			CE1150 cE = living.PetEffectList.GetOfType(ePetEffectType.CE1150) as CE1150;
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
		}

		private void Player_BeginSelfTurn(Living living)
		{
			m_count--;
			if (m_count < 0)
			{
				Stop();
				return;
			}
			m_added = living.MaxBlood * 10 / 10 / 100;
			living.AddBlood(-m_added, 1);
			if (living.Blood <= 0)
			{
				living.Die();
				if (m_source != null && m_source is Player)
				{
					(m_source as Player).PlayerDetail.OnKillingLiving(m_source.Game, 2, living.Id, living.IsLiving, m_added);
				}
			}
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.Game.SendPetBuff(player, base.ElementInfo, isActive: false);
			player.BeginNextTurn -= Player_BeginNextTurn;
			player.BeginSelfTurn -= Player_BeginSelfTurn;
		}
	}
}
