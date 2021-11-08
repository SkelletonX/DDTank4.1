using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Passives
{
	public class PE1341 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int m_currentId;

		public PE1341(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.PE1341, elementID)
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
			PE1341 pE = living.PetEffectList.GetOfType(ePetEffectType.PE1341) as PE1341;
			if (pE == null)
			{
				return base.Start(living);
			}
			pE.m_probability = ((m_probability > pE.m_probability) ? m_probability : pE.m_probability);
			return true;
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.PlayerShoot += Player_PlayerShoot;
			player.AfterPlayerShooted += Player_AfterPlayerShooted;
		}

		private void Player_AfterPlayerShooted(Player player)
		{
			player.Game.SendPetBuff(player, base.ElementInfo, isActive: false, 0);
		}

		private void Player_PlayerShoot(Player player)
		{
			m_added = player.Blood * 3 / 100;
			player.Game.SendPetBuff(player, base.ElementInfo, isActive: true, 0);
			foreach (Living item in player.Game.Map.FindAllNearestEnemy(player.X, player.Y, 100.0, player))
			{
				item.SyncAtTime = true;
				item.AddBlood(-m_added, 1);
				item.SyncAtTime = false;
				if (item.Blood <= 0)
				{
					item.Die();
					if (player != null && player != null)
					{
						player.PlayerDetail.OnKillingLiving(player.Game, 2, item.Id, item.IsLiving, m_added);
					}
				}
			}
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.PlayerShoot -= Player_PlayerShoot;
			player.AfterPlayerShooted -= Player_AfterPlayerShooted;
		}
	}
}
