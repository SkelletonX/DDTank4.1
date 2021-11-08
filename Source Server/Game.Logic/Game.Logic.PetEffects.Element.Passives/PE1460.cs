using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Passives
{
	public class PE1460 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public PE1460(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.PE1460, elementID)
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
			PE1460 pE = living.PetEffectList.GetOfType(ePetEffectType.PE1460) as PE1460;
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
			if (!living.PetEffects.ActiveEffect)
			{
				return;
			}
			m_added = living.MaxBlood * 5 / 100;
			if (m_added <= 0)
			{
				return;
			}
			target.SyncAtTime = true;
			target.AddBlood(-m_added, 1);
			target.SyncAtTime = false;
			target.Game.SendPetBuff(living, base.ElementInfo, isActive: true, 0);
			if (target.Blood <= 0)
			{
				target.Die();
				if (living != null && living is Player)
				{
					(living as Player).PlayerDetail.OnKillingLiving(living.Game, 2, target.Id, target.IsLiving, m_added);
				}
			}
			else
			{
				target.AddPetEffect(new CE1460(0, m_probability, m_type, m_currentId, m_delay, base.ElementInfo.ID.ToString()), 0);
			}
			living.PetEffects.ActiveEffect = false;
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.AfterKilledByLiving -= Player_AfterKilledByLiving;
		}
	}
}
