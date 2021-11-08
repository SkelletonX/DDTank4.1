using Game.Logic.PetEffects.Element.Passives;
using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.ContinueElement
{
	public class CE1301 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public CE1301(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.CE1301, elementID)
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
			CE1301 cE = living.PetEffectList.GetOfType(ePetEffectType.CE1301) as CE1301;
			if (cE == null)
			{
				return base.Start(living);
			}
			cE.m_probability = ((m_probability > cE.m_probability) ? m_probability : cE.m_probability);
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
			foreach (Player allEnemyPlayer in player.Game.GetAllEnemyPlayers(player))
			{
				if (allEnemyPlayer.PetEffectList.GetOfType(ePetEffectType.PE1301) is PE1301 && player.PetMP > 0)
				{
					m_added = player.PetMP * 50 / 100;
					allEnemyPlayer.AddPetMP((m_added == 0) ? 1 : m_added);
				}
			}
		}
	}
}
