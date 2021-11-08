using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace Game.Logic.PetEffects.Element.Actives
{
	public class AE1183 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public AE1183(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.AE1183, elementID)
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
			AE1183 aE = living.PetEffectList.GetOfType(ePetEffectType.AE1183) as AE1183;
			if (aE == null)
			{
				return base.Start(living);
			}
			aE.m_probability = ((m_probability > aE.m_probability) ? m_probability : aE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.PlayerBuffSkillPet += player_AfterBuffSkillPetByLiving;
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.PlayerBuffSkillPet -= player_AfterBuffSkillPetByLiving;
		}

		private void player_AfterBuffSkillPetByLiving(Player player)
		{
			if (player.PetEffects.CurrentUseSkill == m_currentId)
			{
				List<Player> allTeamPlayers = player.Game.GetAllTeamPlayers(player);
				foreach (Player item in allTeamPlayers)
				{
					int count = 2;
					CE1181 cE = item.PetEffectList.GetOfType(ePetEffectType.CE1181) as CE1181;
					CE1182 cE2 = item.PetEffectList.GetOfType(ePetEffectType.CE1182) as CE1182;
					CE1183 cE3 = item.PetEffectList.GetOfType(ePetEffectType.CE1183) as CE1183;
					if (cE != null)
					{
						count = cE.Count;
						cE.Stop();
					}
					if (cE2 != null)
					{
						count = cE2.Count;
						cE2.Stop();
					}
					if (cE3 != null)
					{
						count = cE3.Count;
						cE3.Stop();
					}
					item.Game.SendPetBuff(player, base.ElementInfo, isActive: true, 1);
					item.AddPetEffect(new CE1183(count, m_probability, m_type, m_currentId, m_delay, base.ElementInfo.ID.ToString()), 0);
				}
			}
		}
	}
}
