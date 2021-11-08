using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Actives
{
	public class AE1056 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public AE1056(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.AE1056, elementID)
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
			AE1056 aE = living.PetEffectList.GetOfType(ePetEffectType.AE1056) as AE1056;
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
				foreach (Player allTeamPlayer in player.Game.GetAllTeamPlayers(player))
				{
					m_added = 1500;
					allTeamPlayer.SyncAtTime = true;
					allTeamPlayer.AddBlood(m_added);
					allTeamPlayer.SyncAtTime = false;
				}
			}
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.PlayerBuffSkillPet -= Player_PlayerBuffSkillPet;
		}
	}
}
