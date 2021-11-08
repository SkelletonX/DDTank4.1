using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Actives
{
	public class AE1421 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public AE1421(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.AE1421, elementID)
		{
			m_count = -1;
			m_coldDown = count;
			m_probability = ((probability == -1) ? 10000 : probability);
			m_type = type;
			m_delay = delay;
			m_currentId = skillId;
		}

		public override bool Start(Living living)
		{
			AE1421 aE = living.PetEffectList.GetOfType(ePetEffectType.AE1421) as AE1421;
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
			player.PlayerShoot += ChangeProperty;
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.PlayerBuffSkillPet -= player_AfterBuffSkillPetByLiving;
			player.PlayerShoot -= ChangeProperty;
		}

		private void player_AfterBuffSkillPetByLiving(Player player)
		{
			if (player.PetEffects.CurrentUseSkill == m_currentId)
			{
				m_added = 20;
				player.PetEffects.CritRate += m_added;
				m_count = m_coldDown;
				player.Game.SendPetBuff(player, base.ElementInfo, isActive: true);
			}
		}

		private void ChangeProperty(Player player)
		{
			m_count--;
			if (m_count == 0)
			{
				player.PetEffects.CritRate -= m_added;
				m_added = 0;
				player.Game.SendPetBuff(player, base.ElementInfo, isActive: false);
			}
		}
	}
}
