using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Actives
{
	public class AE1342 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public AE1342(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.AE1342, elementID)
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
			AE1342 aE = living.PetEffectList.GetOfType(ePetEffectType.AE1342) as AE1342;
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

		protected override void OnPausedOnPlayer(Player player)
		{
			player.PlayerBuffSkillPet -= player_AfterBuffSkillPetByLiving;
			player.Game.SendPetBuff(player, base.ElementInfo, isActive: false);
			player.BaseDamage -= m_added;
			IsTrigger = false;
		}

		private void player_AfterBuffSkillPetByLiving(Player player)
		{
			if (player.PetEffects.CurrentUseSkill == m_currentId && !IsTrigger)
			{
				IsTrigger = true;
				m_added = (int)(player.BaseDamage * 15.0 / 100.0);
				player.BaseDamage += m_added;
				player.Game.SendPetBuff(player, base.ElementInfo, isActive: true);
			}
		}
	}
}
