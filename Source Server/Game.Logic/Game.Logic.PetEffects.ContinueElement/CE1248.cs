using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.ContinueElement
{
	public class CE1248 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public CE1248(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.CE1248, elementID)
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
			CE1248 cE = living.PetEffectList.GetOfType(ePetEffectType.CE1248) as CE1248;
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
			player.BeforeTakeDamage += Player_BeforeTakeDamage;
			player.PlayerClearBuffSkillPet += Player_PlayerClearBuffSkillPet;
		}

		private void Player_PlayerClearBuffSkillPet(Player player)
		{
			Stop();
		}

		private void Player_BeforeTakeDamage(Living living, Living source, ref int damageAmount, ref int criticalAmount)
		{
			living.Game.SendPetBuff(living, base.ElementInfo, isActive: true);
			m_added = 200;
			damageAmount -= m_added;
			if (damageAmount < 0)
			{
				damageAmount = 1;
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
			player.Game.SendPetBuff(player, base.ElementInfo, isActive: false);
			player.BeginSelfTurn -= Player_BeginSelfTurn;
			player.BeforeTakeDamage -= Player_BeforeTakeDamage;
		}
	}
}
