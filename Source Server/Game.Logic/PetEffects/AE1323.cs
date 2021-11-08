using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Actives
{
	public class AE1323 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public AE1323(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.AE1323, elementID)
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
			AE1323 aE = living.PetEffectList.GetOfType(ePetEffectType.AE1323) as AE1323;
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
			if (player.PetEffects.CurrentUseSkill == m_currentId && player.Game is PVPGame)
			{
				foreach (Player allEnemyPlayer in player.Game.GetAllEnemyPlayers(player))
				{
					m_added = allEnemyPlayer.MaxBlood * 5 / 100;
					if (allEnemyPlayer.Blood < m_added)
					{
						allEnemyPlayer.Die();
						player?.PlayerDetail.OnKillingLiving(player.Game, 2, allEnemyPlayer.Id, allEnemyPlayer.IsLiving, m_added);
					}
				}
			}
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.PlayerBuffSkillPet -= Player_PlayerBuffSkillPet;
		}
	}
}
