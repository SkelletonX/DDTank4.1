using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Actives
{
	public class AE1222 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public AE1222(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.AE1222, elementID)
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
			AE1222 aE = living.PetEffectList.GetOfType(ePetEffectType.AE1222) as AE1222;
			if (aE == null)
			{
				return base.Start(living);
			}
			aE.m_probability = ((m_probability > aE.m_probability) ? m_probability : aE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.PlayerBuffSkillPet += player_AfterBuffSkillPetByLiving;
			player.AfterKillingLiving += Player_AfterKillingLiving;
			player.BeginSelfTurn += Player_BeginSelfTurn;
		}

		private void Player_BeginSelfTurn(Living living)
		{
			IsTrigger = false;
		}

		private void Player_AfterKillingLiving(Living living, Living target, int damageAmount, int criticalAmount)
		{
			if (IsTrigger)
			{
				target.Game.SendPetBuff(target, base.ElementInfo, isActive: true, 0);
				target.AddPetEffect(new CE1222(2, m_probability, m_type, m_currentId, m_delay, base.ElementInfo.ID.ToString()), 0);
				IsTrigger = false;
			}
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.PlayerBuffSkillPet -= player_AfterBuffSkillPetByLiving;
			player.AfterKillingLiving -= Player_AfterKillingLiving;
			player.BeginSelfTurn -= Player_BeginSelfTurn;
		}

		private void player_AfterBuffSkillPetByLiving(Player player)
		{
			if (player.PetEffects.CurrentUseSkill == m_currentId)
			{
				if ((uint)(m_currentId - 154) <= 1u)
				{
					foreach (Player allEnemyPlayer in player.Game.GetAllEnemyPlayers(player))
					{
						allEnemyPlayer.Game.SendPetBuff(allEnemyPlayer, base.ElementInfo, isActive: true, 0);
						allEnemyPlayer.AddPetEffect(new CE1222(2, m_probability, m_type, m_currentId, m_delay, base.ElementInfo.ID.ToString()), 0);
					}
				}
				else
				{
					IsTrigger = true;
				}
			}
		}
	}
}
