using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Actives
{
	public class AE1208 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public AE1208(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.AE1208, elementID)
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
			AE1208 aE = living.PetEffectList.GetOfType(ePetEffectType.AE1208) as AE1208;
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
			if (player.PetEffects.CurrentUseSkill == m_currentId && player.Game is PVPGame)
			{
				foreach (Player allEnemyPlayer in player.Game.GetAllEnemyPlayers(player))
				{
					allEnemyPlayer.Game.SendPetBuff(allEnemyPlayer, base.ElementInfo, isActive: true);
					allEnemyPlayer.RemovePetMP(10);
					allEnemyPlayer.AddPetEffect(new CE1208(0, m_probability, m_type, m_currentId, m_delay, base.ElementInfo.ID.ToString()), 0);
				}
			}
		}
	}
}
