using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Actives
{
	public class AE1287 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public AE1287(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.AE1287, elementID)
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
			AE1287 aE = living.PetEffectList.GetOfType(ePetEffectType.AE1287) as AE1287;
			if (aE == null)
			{
				return base.Start(living);
			}
			aE.m_probability = ((m_probability > aE.m_probability) ? m_probability : aE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.PlayerBuffSkillPet += player_afterBuffSkillPetByLiving;
			player.AfterKillingLiving += player_afterKillingLiving;
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.PlayerBuffSkillPet -= player_afterBuffSkillPetByLiving;
			player.AfterKillingLiving -= player_afterKillingLiving;
		}

		private void player_afterBuffSkillPetByLiving(Player player)
		{
			if (player.PetEffects.CurrentUseSkill == m_currentId)
			{
				IsTrigger = true;
			}
		}

		private void player_afterKillingLiving(Living living, Living target, int damageAmount, int criticalAmount)
		{
			if (IsTrigger && target is Player)
			{
				target.Game.SendPetBuff(target, base.ElementInfo, isActive: true);
				target.AddPetEffect(new CE1287(2, m_probability, m_type, m_currentId, m_delay, base.ElementInfo.ID.ToString()), 0);
				IsTrigger = false;
			}
		}
	}
}
