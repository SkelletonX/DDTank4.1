using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.ContinueElement
{
	public class CE1228 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public CE1228(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.CE1228, elementID)
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
			CE1228 cE = living.PetEffectList.GetOfType(ePetEffectType.CE1228) as CE1228;
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
			m_added = 600;
			target.SyncAtTime = true;
			target.AddBlood(-m_added, 1);
			target.SyncAtTime = false;
			if (target.Blood <= 0)
			{
				target.Die();
				if (living != null && living is Player)
				{
					(living as Player).PlayerDetail.OnKillingLiving(living.Game, 2, target.Id, target.IsLiving, m_added);
				}
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
