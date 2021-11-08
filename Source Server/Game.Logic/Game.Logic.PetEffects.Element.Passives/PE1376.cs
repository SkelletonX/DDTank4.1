using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Passives
{
	public class PE1376 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public PE1376(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.PE1376, elementID)
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
			PE1376 pE = living.PetEffectList.GetOfType(ePetEffectType.PE1376) as PE1376;
			if (pE == null)
			{
				return base.Start(living);
			}
			pE.m_probability = ((m_probability > pE.m_probability) ? m_probability : pE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.BeginNextTurn += player_beginNextTurn;
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.BeginNextTurn -= player_beginNextTurn;
		}

		public void player_beginNextTurn(Living living)
		{
			if (living.Game.CurrentLiving is Player && living.Game.CurrentLiving != living)
			{
				CE1366 cE = living.Game.CurrentLiving.PetEffectList.GetOfType(ePetEffectType.CE1366) as CE1366;
				CE1367 cE2 = living.Game.CurrentLiving.PetEffectList.GetOfType(ePetEffectType.CE1367) as CE1367;
				if (cE != null || cE2 != null)
				{
					(living as Player).AddPetMP(1);
				}
			}
		}
	}
}
