using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Actives
{
	public class AE1364 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public AE1364(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.AE1364, elementID)
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
			AE1364 aE = living.PetEffectList.GetOfType(ePetEffectType.AE1364) as AE1364;
			if (aE == null)
			{
				return base.Start(living);
			}
			aE.m_probability = ((m_probability > aE.m_probability) ? m_probability : aE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.PlayerBuffSkillPet += Player_PlayerBuffSkillPet;
		}

		private void Player_PlayerBuffSkillPet(Player player)
		{
			if (player.PetEffects.CurrentUseSkill == m_currentId)
			{
				foreach (Living item in player.Game.Map.FindAllNearestSameTeam(player.X, player.Y, 250.0, player))
				{
					m_added = item.MaxBlood * 8 / 100;
					item.SyncAtTime = true;
					item.AddBlood(m_added);
					item.SyncAtTime = false;
				}
			}
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.PlayerBuffSkillPet -= Player_PlayerBuffSkillPet;
		}
	}
}
