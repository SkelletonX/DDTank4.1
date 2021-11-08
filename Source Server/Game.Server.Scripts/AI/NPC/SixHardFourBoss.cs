using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using Bussiness;

namespace GameServerScript.AI.NPC
{
    public class SixHardFourBoss : ABrain
    {
        private int m_attackTurn = 0;
		
		private PhysicalObj m_front;

        private int isSay = 0;

        private string[] OnShootedChat =
        {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SixNormalFourBoss.Msg3"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SixNormalFourBoss.Msg4"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SixNormalFourBoss.Msg5")
        };

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            Body.CurrentDamagePlus = 1f;
            Body.CurrentShootMinus = 1f;
            isSay = 0;
            if (Body.Direction == -1)
            {
                Body.SetRect(((SimpleBoss)Body).NpcInfo.X, ((SimpleBoss)Body).NpcInfo.Y, ((SimpleBoss)Body).NpcInfo.Width, ((SimpleBoss)Body).NpcInfo.Height);
            }
            else
            {
                Body.SetRect(-((SimpleBoss)Body).NpcInfo.X - ((SimpleBoss)Body).NpcInfo.Width, ((SimpleBoss)Body).NpcInfo.Y, ((SimpleBoss)Body).NpcInfo.Width, ((SimpleBoss)Body).NpcInfo.Height);
            }
        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            Body.Direction = Game.FindlivingbyDir(Body);
            bool result = false;
            int maxdis = 0;
            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving && player.X > 0 && player.X < 0)
                {
                    int dis = (int)Body.Distance(player.X, player.Y);
                    if (dis > maxdis)
                    {
                        maxdis = dis;
                    }
                    result = true;
                }
            }

            if (result)
            {
                return;
            }

            if (m_attackTurn == 0)
            {
                BeatB();
                m_attackTurn++;
            }
            else if (m_attackTurn == 1)
            {
                BeatA();
                m_attackTurn++;
            }
			else if (m_attackTurn == 2)
            {
                BeatD();
                m_attackTurn++;
            }
			else if (m_attackTurn == 3)
            {
                InX();
                m_attackTurn++;
            }
			else if (m_attackTurn == 4)
            {
                BeatE();
                m_attackTurn++;
            }
			else if (m_attackTurn == 5)
            {
                AddBlood();
                m_attackTurn++;
            }
            else
            {
                BeatC();
                m_attackTurn = 0;
            }
        }

        private void BeatB()
        {
			Player target = Game.FindRandomPlayer();
			if(Body.X > target.X)
			{
                Body.MoveTo(target.X + 150, Body.Y, "walk", "", 1000, 5, new LivingCallBack(RBeatB), 0); 
			}
			else
			{
                Body.MoveTo(target.X - 150, Body.Y, "walk", "", 1000, 5, new LivingCallBack(RBeatB), 0); 
			}
			if (target.X > Body.Y)
            {
                Body.ChangeDirection(1, 1200);
            }
            else
            {
                Body.ChangeDirection(-1, 1200);
            }
        }
		
		private void RBeatB()
        {
			Body.CurrentDamagePlus = 1.5f;
            Body.PlayMovie("beatB", 1500, 0);
            Body.RangeAttacking(Body.X - 1000, Body.X + 1000, "cry", 3000, null);
        }
		
		private void BeatC()
        {
            Body.CurrentDamagePlus = 1.5f;
            Body.PlayMovie("beatC", 1500, 0);
            Body.RangeAttacking(Body.X - 1000, Body.X + 1000, "cry", 3000, null);
        }
		
		private void BeatD()
        {
            Player target = Game.FindRandomPlayer();
			Body.CurrentDamagePlus = 1.5f;
            Body.PlayMovie("beatD", 1000, 3000);
			Body.JumpTo(target.X, Body.Y - 475, "", 1500, 0, 5, new LivingCallBack(GBeatD), 1);
        }
		
		private void GBeatD()
        {
		    Body.RangeAttacking(Body.X - 300, Body.X + 300, "cry", 4000, null);
		}
		
		private void BeatE()
        {
            Body.PlayMovie("beatE", 3000, 0);
			Body.CallFuction(new LivingCallBack(RBeatE), 3200);
        }
		
		private void RBeatE()
        {
		    Body.PlayMovie("stand", 4000, 4000);
			m_front = ((PVEGame)Game).Createlayerboss(Body.X - 500, 550, "font", "asset.game.six.chang", "in", 1, 0);
			((PVEGame)Game).SendGameFocus(1250, 450, 1, 0, 4000);
			Body.CurrentDamagePlus = 1.5f;
			Body.RangeAttacking(Body.X - 3000, Body.X + 3000, "cry", 4000, null);
        }
		
        private void BeatA()
        {
            int dis = Game.Random.Next(900, 1400);
            Body.MoveTo(dis, Body.Y, "walk", "", 1000, 4, new LivingCallBack(NextAttack), 0);
        }

        private void NextAttack()
        {
            Player target = Game.FindRandomPlayer();

            if (target.X > Body.Y)
            {
                Body.ChangeDirection(1, 800);
            }
            else
            {
                Body.ChangeDirection(-1, 800);
            }

            Body.CurrentDamagePlus = 1.8f;

            if (target != null)
            {
                int mtX = Game.Random.Next(target.X - 50, target.X + 50);

                if (Body.ShootPoint(mtX, target.Y, 61, 1000, 10000, 1, 1, 2300))
                {
                    Body.PlayMovie("beatA", 1500, 0);
                }

                if (Body.ShootPoint(mtX, target.Y, 61, 1000, 10000, 1, 1, 4100))
                {
                    Body.PlayMovie("beatA", 3300, 0);
                }
            }
        }
		
		
		private void AddBlood()
        {
            m_front = ((PVEGame)Game).Createlayerboss(Body.X, Body.Y, "font", "asset.game.six.popcan", "green", 1, 0);
			Body.CallFuction(new LivingCallBack(Blood), 2000);
        }
		
		private void Blood()
        {
			Body.SyncAtTime = true;
			Body.PlayMovie("shieldB", 0, 2500);
			Body.AddBlood(1523);
        }
		
		private void InX()
        {
            Body.SyncAtTime = true;
			Body.PlayMovie("inX", 0, 4500);
			Body.AddBlood(1523);
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        public override void OnShootedSay(int delay)
        {
            base.OnShootedSay(delay);
            int index = Game.Random.Next(0, OnShootedChat.Length);
            if (isSay == 0 && Body.IsLiving == true)
            {
                Body.Say(OnShootedChat[index], 0, 1000 + delay, 0);
                isSay = 1;
            }
        }
    }
}
