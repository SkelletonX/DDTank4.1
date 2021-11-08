using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.ContinueElement
{
	public class CE1562 : BasePetEffect
	{
		private int m_count;

		public CE1562(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.CE1562, elementID)
		{
			m_count = count;
		}

		public override bool Start(Living living)
		{
			CE1562 cE = living.PetEffectList.GetOfType(ePetEffectType.CE1562) as CE1562;
			if (cE == null)
			{
				return base.Start(living);
			}
			cE.m_count = m_count;
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.BeginSelfTurn += player_BeginFitting;
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.BeginSelfTurn -= player_BeginFitting;
			player.Game.SendPlayerPicture(player, 3, state: false);
			player.Game.SendPetBuff(player, base.ElementInfo, isActive: false);
		}

		private void player_BeginFitting(Living living)
		{
			m_count--;
			if (m_count < 0)
			{
				Stop();
			}
		}
	}
}
