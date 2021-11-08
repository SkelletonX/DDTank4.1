using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Actives
{
	public class AE1375 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public AE1375(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.AE1375, elementID)
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
			AE1375 aE = living.PetEffectList.GetOfType(ePetEffectType.AE1375) as AE1375;
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
			player.PlayerCompleteShoot += Player_PlayerCompleteShoot;
		}

		private void Player_PlayerCompleteShoot(Player player)
		{
			player.PetEffects.AddBloodPercent = 0;
		}

		private void Player_PlayerBuffSkillPet(Player player)
		{
			if (player.PetEffects.CurrentUseSkill == m_currentId)
			{
				m_added = 3;
				player.PetEffects.AddBloodPercent = m_added;
			}
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.PlayerBuffSkillPet -= Player_PlayerBuffSkillPet;
			player.PlayerCompleteShoot -= Player_PlayerCompleteShoot;
		}
	}
}
