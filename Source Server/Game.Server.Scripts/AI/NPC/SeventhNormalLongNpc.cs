using Bussiness;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class SeventhNormalLongNpc : ABrain
	{
		private int m_attackTurn = 0;

		private PhysicalObj moive;

		private static string[] AllAttackChat = new string[]
		{
			LanguageMgr.GetTranslation("Ddtank super là số 1", new object[0])
		};

		private static string[] ShootChat = new string[]
		{
			LanguageMgr.GetTranslation("Anh em tiến lên !", new object[0])
		};

		private static string[] KillPlayerChat = new string[]
		{
			LanguageMgr.GetTranslation("Anh em tiến lên !", new object[0])
		};

		private static string[] CallChat = new string[]
		{
			LanguageMgr.GetTranslation("Ai giết được chúng sẻ được ban thưởng !", new object[0])
		};

		private static string[] JumpChat = new string[]
		{
			LanguageMgr.GetTranslation("Ai giết được chúng sẻ được ban thưởng !", new object[0])
		};

		private static string[] KillAttackChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg13", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg14", new object[0])
		};

		private static string[] ShootedChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg15", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg16", new object[0])
		};

		private static string[] DiedChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg17", new object[0])
		};

		public override void OnBeginSelfTurn()
		{
			base.OnBeginSelfTurn();
		}

		public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
			base.Body.CurrentDamagePlus = 1f;
			base.Body.CurrentShootMinus = 1f;
		}

		public override void OnCreated()
		{
			base.OnCreated();
		}

		public override void OnStartAttacking()
		{
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			bool flag = false;
			int num = 0;
			foreach (Player current in base.Game.GetAllFightPlayers())
			{
				if (current.IsLiving && current.X < 870)
				{
					int num2 = (int)base.Body.Distance(current.X, current.Y);
					if (num2 > num)
					{
						num = num2;
					}
					flag = true;
				}
			}
			if (flag)
			{
				this.KillAttack(0, base.Body.X + 100);
			}
			else if (this.m_attackTurn == 0)
			{
				this.AllAttack();
				this.m_attackTurn++;
			}
			else
			{
				this.Move();
				this.m_attackTurn = 0;
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		public void Move()
		{
			int x = base.Game.Random.Next(600, 750);
			base.Body.MoveTo(x, base.Body.Y, "walk", 1200, "", 3);
		}

		private void KillAttack(int fx, int tx)
		{
			base.Body.CurrentDamagePlus = 1f;
			base.Body.PlayMovie("beatB", 2000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 3000, null);
		}

		private void AllAttack()
		{
			base.Body.PlayMovie("beatB", 3000, 0);
			base.Body.RangeAttacking(base.Body.X - 10000, base.Body.X + 10000, "cry", 6000, null);
			base.Body.CallFuction(new LivingCallBack(this.GoMovie), 5000);
		}

		private void GoMovie()
		{
			List<Player> allFightPlayers = base.Game.GetAllFightPlayers();
			foreach (Player current in allFightPlayers)
			{
				this.moive = ((PVEGame)base.Game).Createlayer(current.X, current.Y, "moive", "asset.game.seven.cao", "out", 1, 0);
				this.moive.PlayMovie("in", 1000, 0);
			}
		}
	}
}
