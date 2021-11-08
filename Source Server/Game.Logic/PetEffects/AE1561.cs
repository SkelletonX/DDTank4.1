using Game.Logic.Phy.Object;
using System.Collections.Generic;
using System.Drawing;

namespace Game.Logic.PetEffects.Element.Actives
{
	public class AE1561 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public AE1561(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.AE1561, elementID)
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
			AE1561 aE = living.PetEffectList.GetOfType(ePetEffectType.AE1561) as AE1561;
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
				int x = player.X;
				int y = player.Y;
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
			}
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.PlayerBuffSkillPet -= Player_PlayerBuffSkillPet;
		}
	}
}
