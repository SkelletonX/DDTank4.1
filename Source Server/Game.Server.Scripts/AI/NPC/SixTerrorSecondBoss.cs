using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;

namespace GameServerScript.AI.NPC
{
    public class SixTerrorSecondBoss : ABrain
    {
        private int m_attackTurn = 0;

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();

            Body.CurrentDamagePlus = 1;
            Body.CurrentShootMinus = 1;
        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
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
                BeatC();
                m_attackTurn++;
            }
            else if (m_attackTurn == 1)
            {
                BeatA();
                m_attackTurn++;
            }
            else
            {
                BeatB();
                m_attackTurn = 0;
            }
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        private void BeatC()
        {
            Player target = Game.FindRandomPlayer();

            if (target != null)
            {
                int mtX = Game.Random.Next(target.X - 50, target.X + 50);

                if (Body.ShootPoint(target.X, target.Y, 86, 1000, 10000, 1, 2, 2900))
                {
                    Body.PlayMovie("beatC", 1700, 0);
                }
            }
        }
		
		private void BeatA()
        {
			Body.PlayMovie("beatA", 1100, 3000, new LivingCallBack(GoBeatA));
        }
		
		private PhysicalObj m_movie;
		private void GoBeatA()
        {
            Player target = Game.FindRandomPlayer();
			m_movie = ((PVEGame)Game).Createlayerboss(target.X, target.Y, "font", "asset.game.six.danti", "in", 1, 0);
			int blood = Game.Random.Next(451, 757);//số máu	
			target.AddBlood(-blood, 1);//dame máu
			Body.RangeAttacking(target.X - 10, target.X + 10, "cry", 0, null);
        }
		
		private void BeatB()
        {
			Body.PlayMovie("beatB", 1100, 3200, new LivingCallBack(GoBeatB));
        }
		
		private void GoBeatB()
        {
            Player target = Game.FindRandomPlayer();
			m_movie = ((PVEGame)Game).Createlayerboss(target.X, target.Y, "font", "asset.game.six.qunti", "in", 1, 0);
			int blood = Game.Random.Next(451, 757);//số máu	
			target.AddBlood(-blood, 1);//dame máu
			Body.RangeAttacking(target.X - 10, target.X + 10, "cry", 500, null);
        }
    }
}
