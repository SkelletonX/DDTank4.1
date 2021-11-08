using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Passives
{
	public class PE1575 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public PE1575(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.PE1575, elementID)
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
			PE1575 pE = living.PetEffectList.GetOfType(ePetEffectType.PE1575) as PE1575;
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
			if (living.PetEffectList.GetOfType(ePetEffectType.CE1574) is CE1574)
			{
				((TurnedLiving)living).AddPetMP(4);
			}
		}
	}
}
