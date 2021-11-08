using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Passives
{
	public class PE1351 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public PE1351(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.PE1351, elementID)
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
			PE1351 pE = living.PetEffectList.GetOfType(ePetEffectType.PE1351) as PE1351;
			if (pE == null)
			{
				return base.Start(living);
			}
			pE.m_probability = ((m_probability > pE.m_probability) ? m_probability : pE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.BeginSelfTurn += Player_BeginSelfTurn;
		}

		private void Player_BeginSelfTurn(Living living)
		{
			(living as Player).AddPetMP(4);
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.BeginSelfTurn -= Player_BeginSelfTurn;
		}
	}
}
