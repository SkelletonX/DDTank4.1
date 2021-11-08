using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.ContinueElement
{
	public class CE1310 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public CE1310(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.CE1310, elementID)
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
			CE1310 cE = living.PetEffectList.GetOfType(ePetEffectType.CE1310) as CE1310;
			if (cE == null)
			{
				return base.Start(living);
			}
			cE.m_probability = ((m_probability > cE.m_probability) ? m_probability : cE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.TakePlayerDamage += player_takedDamage;
			player.BeginSelfTurn += player_beginSelfTurn;
			player.PlayerClearBuffSkillPet += Player_PlayerClearBuffSkillPet;
		}

		private void Player_PlayerClearBuffSkillPet(Player player)
		{
			Stop();
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.TakePlayerDamage -= player_takedDamage;
			player.BeginSelfTurn -= player_beginSelfTurn;
		}

		private void player_takedDamage(Living living, Living source, ref int damageAmount, ref int criticalAmount)
		{
			m_added = damageAmount * 30 / 100;
			living.SyncAtTime = true;
			living.AddBlood(m_added);
			living.SyncAtTime = false;
		}

		public void player_beginSelfTurn(Living living)
		{
			m_count--;
			if (m_count < 0)
			{
				Stop();
			}
		}
	}
}
