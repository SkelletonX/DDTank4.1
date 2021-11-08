using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Actives
{
	public class PE1615 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public PE1615(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.PE1615, elementID)
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
			PE1615 pE = living.PetEffectList.GetOfType(ePetEffectType.PE1615) as PE1615;
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
			CE1615 cE = living.PetEffectList.GetOfType(ePetEffectType.CE1615) as CE1615;
			m_added = 30;
			double num = (double)living.Blood / (double)living.MaxBlood * 100.0;
			if (cE == null && !(num >= (double)m_added) && num != 0.0)
			{
				living.IsHide = true;
				living.AddPetEffect(new CE1615(1, m_probability, m_type, m_currentId, m_delay, base.ElementInfo.ID.ToString()), 0);
			}
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.BeginSelfTurn -= Player_BeginSelfTurn;
		}
	}
}
