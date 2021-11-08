using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Actives
{
	public class AE1349 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public AE1349(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.AE1349, elementID)
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
			AE1349 aE = living.PetEffectList.GetOfType(ePetEffectType.AE1349) as AE1349;
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
				(player.PetEffectList.GetOfType(ePetEffectType.AE1342) as AE1342)?.Pause();
				(player.PetEffectList.GetOfType(ePetEffectType.AE1343) as AE1343)?.Pause();
				(player.PetEffectList.GetOfType(ePetEffectType.AE1344) as AE1344)?.Pause();
				player.Game.SendPlayerPicture(player, 5, state: false);
				player.IsNoHole = false;
				IsTrigger = false;
			}
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.PlayerBeginMoving -= Player_PlayerBeginMoving;
		}
	}
}
