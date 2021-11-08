using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Passives
{
	public class PE1235 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public PE1235(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.PE1235, elementID)
		{
			m_count = count;
			m_coldDown = 3;
			m_probability = ((probability == -1) ? 10000 : probability);
			m_type = type;
			m_delay = delay;
			m_currentId = skillId;
		}

		public override bool Start(Living living)
		{
			PE1235 pE = living.PetEffectList.GetOfType(ePetEffectType.PE1235) as PE1235;
			if (pE == null)
			{
				return base.Start(living);
			}
			pE.m_probability = ((m_probability > pE.m_probability) ? m_probability : pE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.AfterKilledByLiving += player_afterKilledByLiving;
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.AfterKilledByLiving -= player_afterKilledByLiving;
		}

		private void player_afterKilledByLiving(Living living, Living target, int damageAmount, int criticalAmount)
		{
			if (living.PetEffects.ReboundDamage <= 0)
			{
				return;
			}
			target.Game.SendPetBuff(target, base.ElementInfo, isActive: true, 0);
			if (target.Blood < 0)
			{
				target.Die();
				if (living != null && living is Player)
				{
					(living as Player).PlayerDetail.OnKillingLiving(living.Game, 2, living.Id, living.IsLiving, target.PetEffects.ReboundDamage);
				}
			}
			else
			{
				target.AddPetEffect(new CE1235(0, m_probability, m_type, m_currentId, m_delay, base.ElementInfo.ID.ToString()), 0);
			}
			living.PetEffects.ReboundDamage = 0;
		}
	}
}
