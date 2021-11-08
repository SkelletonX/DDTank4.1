using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Passives
{
	public class PE1612 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public PE1612(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.PE1612, elementID)
		{
			m_count = 1;
			m_coldDown = count;
			m_probability = ((probability == -1) ? 10000 : probability);
			m_type = type;
			m_delay = delay;
			m_currentId = skillId;
		}

		public override bool Start(Living living)
		{
			PE1612 pE = living.PetEffectList.GetOfType(ePetEffectType.PE1612) as PE1612;
			if (pE == null)
			{
				return base.Start(living);
			}
			pE.m_probability = ((m_probability > pE.m_probability) ? m_probability : pE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.PlayerAfterBuffSkillPet += Player_PlayerAfterBuffSkillPet;
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.PlayerAfterBuffSkillPet -= Player_PlayerAfterBuffSkillPet;
		}

		private void Player_PlayerAfterBuffSkillPet(Living living)
		{
			if (living.PetEffectList.GetOfType(ePetEffectType.CE1610) is CE1610)
			{
				new CE1612(1, m_probability, m_type, m_currentId, m_delay, base.ElementInfo.ID.ToString()).Start(living);
			}
		}
	}
}
