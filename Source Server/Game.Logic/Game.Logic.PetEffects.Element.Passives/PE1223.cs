using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Passives
{
	public class PE1223 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public PE1223(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.PE1223, elementID)
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
			PE1223 pE = living.PetEffectList.GetOfType(ePetEffectType.PE1223) as PE1223;
			if (pE == null)
			{
				return base.Start(living);
			}
			pE.m_probability = ((m_probability > pE.m_probability) ? m_probability : pE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.AfterKilledByLiving += Player_AfterKilledByLiving;
		}

		private void Player_AfterKilledByLiving(Living living, Living target, int damageAmount, int criticalAmount)
		{
			((TurnedLiving)living).AddPetMP(2);
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.AfterKilledByLiving -= Player_AfterKilledByLiving;
		}
	}
}
