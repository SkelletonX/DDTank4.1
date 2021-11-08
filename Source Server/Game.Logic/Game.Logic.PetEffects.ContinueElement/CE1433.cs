using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.ContinueElement
{
	public class CE1433 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public CE1433(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.CE1433, elementID)
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
			CE1433 cE = living.PetEffectList.GetOfType(ePetEffectType.CE1433) as CE1433;
			if (cE == null)
			{
				return base.Start(living);
			}
			cE.m_probability = ((m_probability > cE.m_probability) ? m_probability : cE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.BeginSelfTurn += player_beginSelfTurn;
			player.BeforeTakeDamage += player_BeforeTakeDamage;
			player.PlayerClearBuffSkillPet += Player_PlayerClearBuffSkillPet;
		}

		private void Player_PlayerClearBuffSkillPet(Player player)
		{
			Stop();
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.BeginSelfTurn -= player_beginSelfTurn;
			player.BeforeTakeDamage -= player_BeforeTakeDamage;
		}

		public void player_beginSelfTurn(Living living)
		{
			m_count--;
			if (m_count < 0)
			{
				Stop();
			}
		}

		private void player_BeforeTakeDamage(Living living, Living source, ref int damageAmount, ref int criticalAmount)
		{
			int num = damageAmount * 20 / 100;
			damageAmount += num;
		}
	}
}
