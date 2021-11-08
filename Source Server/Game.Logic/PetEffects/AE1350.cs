using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Actives
{
	public class AE1350 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public AE1350(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.AE1350, elementID)
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
			AE1350 aE = living.PetEffectList.GetOfType(ePetEffectType.AE1350) as AE1350;
			if (aE == null)
			{
				return base.Start(living);
			}
			aE.m_probability = ((m_probability > aE.m_probability) ? m_probability : aE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.PlayerBeginMoving += Player_PlayerBeginMoving;
			player.PlayerBuffSkillPet += Player_PlayerBuffSkillPet;
		}

		private void Player_PlayerBuffSkillPet(Player player)
		{
			if (player.PetEffects.CurrentUseSkill == m_currentId)
			{
				IsTrigger = true;
			}
		}

		private void Player_PlayerBeginMoving(Player player)
		{
			if (IsTrigger)
			{
				(player.PetEffectList.GetOfType(ePetEffectType.AE1345) as AE1345)?.Pause();
				(player.PetEffectList.GetOfType(ePetEffectType.AE1346) as AE1346)?.Pause();
				(player.PetEffectList.GetOfType(ePetEffectType.AE1347) as AE1347)?.Pause();
				IsTrigger = false;
			}
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.PlayerBeginMoving -= Player_PlayerBeginMoving;
		}
	}
}
