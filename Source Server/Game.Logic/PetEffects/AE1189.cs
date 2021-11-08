using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Actives
{
	public class AE1189 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public AE1189(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.AE1189, elementID)
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
			AE1189 aE = living.PetEffectList.GetOfType(ePetEffectType.AE1189) as AE1189;
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
				(player.PetEffectList.GetOfType(ePetEffectType.CE1184) as CE1184)?.Stop();
				(player.PetEffectList.GetOfType(ePetEffectType.CE1185) as CE1185)?.Stop();
				(player.PetEffectList.GetOfType(ePetEffectType.CE1186) as CE1186)?.Stop();
				(player.PetEffectList.GetOfType(ePetEffectType.CE1187) as CE1187)?.Stop();
				IsTrigger = false;
			}
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.PlayerBeginMoving -= Player_PlayerBeginMoving;
		}
	}
}
