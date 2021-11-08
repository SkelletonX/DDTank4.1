using Game.Base.Packets;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Game.Logic.PetEffects.Element.Actives
{
	public class AE1437 : BasePetEffect
	{
		private int m_type = 0;

		private int m_count = 0;

		private int m_probability = 0;

		private int m_delay = 0;

		private int m_coldDown = 0;

		private int m_added = 0;

		private int BaseDistant = 500;

		private int m_currentId;

		private int m_fx;

		private int m_tx;

		public AE1437(int count, int probability, int type, int skillId, int delay, string elementID)
			: base(ePetEffectType.AE1437, elementID)
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
			AE1437 aE = living.PetEffectList.GetOfType(ePetEffectType.AE1437) as AE1437;
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
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.PlayerBuffSkillPet -= player_AfterBuffSkillPetByLiving;
		}

		private void player_AfterBuffSkillPetByLiving(Player player)
		{
			if (player.PetEffects.CurrentUseSkill == m_currentId)
			{
				int num = m_living.X - BaseDistant;
				int num2 = m_living.X + BaseDistant;
				int backroundWidht = m_living.Game.Map.Info.BackroundWidht;
				m_fx = ((num > 0) ? num : 0);
				m_tx = ((num2 >= backroundWidht) ? backroundWidht : num2);
				if (m_fx == 0)
				{
					m_tx += Math.Abs(num);
				}
				if (m_tx == backroundWidht)
				{
					m_fx -= Math.Abs(num2 - backroundWidht);
				}
				List<Living> list = new List<Living>();
				List<Living> list2 = (!(m_living.Game is PVPGame)) ? m_living.Game.Map.FindRandomLiving(m_fx, m_tx) : m_living.Game.Map.FindRandomPlayer(m_fx, m_tx, m_living.Game.GetAllEnemyPlayers(m_living));
				int count = list2.Count;
				for (int i = 0; i < 5; i++)
				{
					GSPacketIn gSPacketIn = new GSPacketIn(91, m_living.Id);
					gSPacketIn.Parameter1 = m_living.Id;
					gSPacketIn.WriteByte(61);
					gSPacketIn.WriteInt(count);
					m_living.SyncAtTime = false;
					try
					{
						foreach (Living item in list2)
						{
							int val = 0;
							item.SyncAtTime = false;
							item.IsFrost = false;
							item.IsHide = false;
							player.Game.SendGameUpdateFrozenState(item);
							player.Game.SendGameUpdateHideState(item);
							int num3 = MakeDamage(item);
							int damageAmount = num3 + num3 * 50 / 100;
							int criticalAmount = MakeCriticalDamage(item, damageAmount);
							int val2 = 0;
							if (item is Player)
							{
								item.OnTakedDamage(item, ref damageAmount, ref criticalAmount);
							}
							if (item.TakeDamage(m_living, ref damageAmount, ref criticalAmount, "范围攻击"))
							{
								val2 = damageAmount + criticalAmount;
								if (item is Player)
								{
									val = (item as Player).Dander;
								}
							}
							gSPacketIn.WriteInt(item.Id);
							gSPacketIn.WriteInt(val2);
							gSPacketIn.WriteInt(item.Blood);
							gSPacketIn.WriteInt(val);
							gSPacketIn.WriteInt(1);
						}
						player.Game.SendToAll(gSPacketIn);
					}
					finally
					{
						m_living.SyncAtTime = true;
						foreach (Living item2 in list2)
						{
							item2.SyncAtTime = true;
						}
					}
				}
				foreach (Living item3 in list2)
				{
					if (!item3.IsLiving)
					{
						foreach (Player allTeamPlayer in m_living.Game.GetAllTeamPlayers(m_living))
						{
							allTeamPlayer.SyncAtTime = true;
							allTeamPlayer.AddBlood(allTeamPlayer.MaxBlood * 5 / 100);
							allTeamPlayer.SyncAtTime = false;
						}
					}
				}
			}
		}

		private int MakeDamage(Living p)
		{
			double baseDamage = m_living.BaseDamage;
			double num = p.BaseGuard;
			double num2 = p.Defence + p.MagicDefence;
			double num3 = m_living.Attack + m_living.MagicAttack;
			if (p.AddArmor && (p as Player).DeputyWeapon != null)
			{
				int num4 = (p as Player).DeputyWeapon.Template.Property7 + (int)Math.Pow(1.1, (p as Player).DeputyWeapon.StrengthenLevel);
				num += (double)num4;
				num2 += (double)num4;
			}
			if (m_living.IgnoreArmor)
			{
				num = 0.0;
				num2 = 0.0;
			}
			float currentDamagePlus = m_living.CurrentDamagePlus;
			float currentShootMinus = m_living.CurrentShootMinus;
			double num5 = 0.95 * (num - (double)(3 * m_living.Grade)) / (500.0 + num - (double)(3 * m_living.Grade));
			double num6 = (num2 - m_living.Lucky >= 0.0) ? (0.95 * (num2 - m_living.Lucky) / (600.0 + num2 - m_living.Lucky)) : 0.0;
			double num7 = baseDamage * (1.0 + num3 * 0.001) * (1.0 - (num5 + num6 - num5 * num6)) * (double)currentDamagePlus * (double)currentShootMinus;
			Rectangle directDemageRect = p.GetDirectDemageRect();
			double num8 = Math.Sqrt((directDemageRect.X - m_living.X) * (directDemageRect.X - m_living.X) + (directDemageRect.Y - m_living.Y) * (directDemageRect.Y - m_living.Y));
			double num9 = num7 * (1.0 - num8 / (double)Math.Abs(m_tx - m_fx) / 4.0);
			if (num9 < 0.0)
			{
				return 1;
			}
			return (int)num9;
		}

		private int MakeCriticalDamage(Living p, int baseDamage)
		{
			double lucky = m_living.Lucky;
			Random random = new Random();
			if (75000.0 * lucky / (lucky + 800.0) <= (double)random.Next(100000))
			{
				return 0;
			}
			int num = p.ReduceCritFisrtGem + p.ReduceCritSecondGem;
			return (int)((0.5 + lucky * 0.0003) * (double)baseDamage) * (100 - num) / 100;
		}
	}
}
