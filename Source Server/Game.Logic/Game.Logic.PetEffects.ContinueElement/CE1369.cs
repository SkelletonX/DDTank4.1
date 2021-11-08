using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.ContinueElement
{
	public class CE1369 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public CE1369(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.CE1369, elementID)
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
			CE1369 cE = living.PetEffectList.GetOfType(ePetEffectType.CE1369) as CE1369;
			if (cE == null)
			{
				return base.Start(living);
			}
			cE.m_probability = ((m_probability > cE.m_probability) ? m_probability : cE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.AfterKilledByLiving += Player_AfterKilledByLiving;
		}

		private void Player_AfterKilledByLiving(Living living, Living target, int damageAmount, int criticalAmount)
		{
			m_added = living.MaxBlood * 4 / 10 / 100;
			foreach (Player allTeamPlayer in living.Game.GetAllTeamPlayers(living))
			{
				allTeamPlayer.SyncAtTime = true;
				if (allTeamPlayer.Blood < allTeamPlayer.MaxBlood * 20 / 100)
				{
					allTeamPlayer.AddBlood(m_added * 2);
				}
				else
				{
					allTeamPlayer.AddBlood(m_added);
				}
				allTeamPlayer.SyncAtTime = false;
			}
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.AfterKilledByLiving -= Player_AfterKilledByLiving;
		}
	}
}
