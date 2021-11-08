using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Actives
{
	public class PE1614 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public PE1614(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.PE1614, elementID)
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
			PE1614 pE = living.PetEffectList.GetOfType(ePetEffectType.PE1614) as PE1614;
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
			CE1614 cE = living.PetEffectList.GetOfType(ePetEffectType.CE1614) as CE1614;
			m_added = 40;
			double num = (double)living.Blood / (double)living.MaxBlood * 100.0;
			if (cE == null && !(num >= (double)m_added) && num != 0.0)
			{
				living.IsHide = true;
				new CE1614(1, m_probability, m_type, m_currentId, m_delay, base.ElementInfo.ID.ToString()).Start(living);
			}
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			m_added = 0;
			player.BeginSelfTurn -= Player_BeginSelfTurn;
		}
	}
}
