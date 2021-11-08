using Bussiness;
using Game.Logic.Phy.Object;
using System.Collections.Generic;
using System.Drawing;

namespace Game.Logic.PetEffects.Element.Actives
{
	public class AE1560 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public AE1560(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.AE1560, elementID)
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
			AE1560 aE = living.PetEffectList.GetOfType(ePetEffectType.AE1560) as AE1560;
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
				Point xY = new Point(player.X, player.Y);
				List<Point> list = new List<Point>();
				foreach (Player allEnemyPlayer in player.Game.GetAllEnemyPlayers(player))
				{
					list.Add(new Point(allEnemyPlayer.X, allEnemyPlayer.Y));
				}
				if (list.Count > 0)
				{
					int index = rand.Next(list.Count);
					player.SetXY(list[index]);
					player.StartMoving();
				}
				if (GameProperties.VERSION >= 8900)
				{
					Player player2 = new Player(player.PlayerDetail, ((PVPGame)player.Game).PhysicalId++, player.Game as PVPGame, player.Team, player.PlayerDetail.PlayerCharacter.hp);
					player2.Reset();
					player2.Direction = player.Direction;
					player2.IsShadown = true;
					player2.SetXY(xY);
					player2.Delay = player.Delay + m_delay;
					((PVPGame)player.Game).AddShadow(player2);
				}
			}
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.PlayerBuffSkillPet -= Player_PlayerBuffSkillPet;
		}
	}
}
