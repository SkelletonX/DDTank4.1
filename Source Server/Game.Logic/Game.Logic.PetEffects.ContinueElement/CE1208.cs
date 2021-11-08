using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.ContinueElement
{
	public class CE1208 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public CE1208(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.CE1208, elementID)
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
			CE1208 cE = living.PetEffectList.GetOfType(ePetEffectType.CE1208) as CE1208;
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
			player.PlayerClearBuffSkillPet += Player_PlayerClearBuffSkillPet;
		}

		private void Player_PlayerClearBuffSkillPet(Player player)
		{
			Stop();
		}

		private void Player_BeginSelfTurn(Living living)
		{
			Stop();
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.Game.SendPetBuff(player, base.ElementInfo, isActive: false);
			player.BeginSelfTurn -= Player_BeginSelfTurn;
		}
	}
}
