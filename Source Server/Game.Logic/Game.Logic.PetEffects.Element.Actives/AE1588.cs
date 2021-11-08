using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Actives
{
	public class AE1588 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public AE1588(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.AE1588, elementID)
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
			AE1588 aE = living.PetEffectList.GetOfType(ePetEffectType.AE1588) as AE1588;
			if (aE == null)
			{
				return base.Start(living);
			}
			aE.m_probability = ((m_probability > aE.m_probability) ? m_probability : aE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.PlayerSkip += Player_Skip;
		}

		private void Player_Skip(Player player)
		{
			player.Game.SendPetBuff(player, base.ElementInfo, isActive: true);
			new CE1588(2, m_probability, m_type, m_currentId, m_delay, base.ElementInfo.ID.ToString()).Start(player);
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.PlayerSkip -= Player_Skip;
		}
	}
}
