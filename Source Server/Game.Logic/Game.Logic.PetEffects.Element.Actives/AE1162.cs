using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace Game.Logic.PetEffects.Element.Actives
{
	public class AE1162 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public AE1162(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.AE1162, elementID)
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
			AE1162 aE = living.PetEffectList.GetOfType(ePetEffectType.AE1162) as AE1162;
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
				List<Player> allEnemyPlayers = player.Game.GetAllEnemyPlayers(player);
				m_added = 5000;
				foreach (Player item in allEnemyPlayers)
				{
					item.Game.SendPetBuff(item, base.ElementInfo, isActive: true);
					item.AddBlood(-m_added, 1);
					if (item.Blood <= 0)
					{
						item.Die();
						player?.PlayerDetail.OnKillingLiving(player.Game, 2, item.Id, item.IsLiving, m_added);
					}
					item.AddPetEffect(new CE1162(0, m_probability, m_type, m_currentId, m_delay, base.ElementInfo.ID.ToString()), 0);
				}
			}
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.PlayerBuffSkillPet -= Player_PlayerBuffSkillPet;
		}
	}
}
