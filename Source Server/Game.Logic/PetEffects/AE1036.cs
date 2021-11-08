using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Actives
{
	public class AE1036 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public AE1036(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.AE1036, elementID)
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
			AE1036 aE = living.PetEffectList.GetOfType(ePetEffectType.AE1036) as AE1036;
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
			player.AfterPlayerShooted += player_AfterPlayerShooted;
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.PlayerBuffSkillPet -= player_AfterBuffSkillPetByLiving;
			player.AfterPlayerShooted -= player_AfterPlayerShooted;
		}

		private void player_AfterBuffSkillPetByLiving(Player player)
		{
			if (player.PetEffects.CurrentUseSkill == m_currentId)
			{
				m_added = 120;
				player.CurrentDamagePlus += m_added / 100;
			}
		}

		private void player_AfterPlayerShooted(Player player)
		{
			if (player.PetEffects.CurrentUseSkill == m_currentId)
			{
				player.CurrentDamagePlus -= m_added / 100;
				m_added = 0;
			}
		}
	}
}
