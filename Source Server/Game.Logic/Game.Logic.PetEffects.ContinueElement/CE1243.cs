using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.ContinueElement
{
	public class CE1243 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public CE1243(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.CE1243, elementID)
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
			CE1243 cE = living.PetEffectList.GetOfType(ePetEffectType.CE1243) as CE1243;
			if (cE == null)
			{
				return base.Start(living);
			}
			cE.m_probability = ((m_probability > cE.m_probability) ? m_probability : cE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.BeginSelfTurn += Player_BeginSelfTurn;
			player.AfterKilledByLiving += Player_AfterKilledByLiving;
			player.PlayerClearBuffSkillPet += Player_PlayerClearBuffSkillPet;
		}

		private void Player_PlayerClearBuffSkillPet(Player player)
		{
			Stop();
		}

		private void Player_AfterKilledByLiving(Living living, Living target, int damageAmount, int criticalAmount)
		{
			m_added = 1500;
			if (living.IsLiving)
			{
				living.SyncAtTime = true;
				living.AddBlood(m_added);
				living.SyncAtTime = false;
			}
		}

		private void Player_BeginSelfTurn(Living living)
		{
			m_count--;
			if (m_count < 0)
			{
				m_added = 0;
				Stop();
			}
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.BeginSelfTurn -= Player_BeginSelfTurn;
			player.AfterKilledByLiving -= Player_AfterKilledByLiving;
		}
	}
}
