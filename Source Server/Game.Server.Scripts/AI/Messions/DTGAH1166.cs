using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;

namespace GameServerScript.AI.Messions
{
    public class DTGAH1166 : AMissionControl
    {
        private SimpleBoss m_boss;
		private SimpleBoss m_king;
		private SimpleNpc boss;
        private PhysicalObj m_front;
        private int IsSay = 0;
        private int bossID = 6323;
		private int npcID = 6321;
		private int npcID2 = 6322;
        private static string[] KillChat = new string[]{
           "Gửi cho bạn trở về nhà!",

           "Một mình, bạn có ảo tưởng có thể đánh bại tôi?"
        };

        private static string[] ShootedChat = new string[]{
            " Đau ah! Đau ...",

            "Quốc vương vạn tuế ..."
        };
        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 900)
            {
                return 3;
            }
            else if (score > 825)
            {
                return 2;
            }
            else if (score > 725)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            Game.AddLoadingFile(1, "bombs/86.swf", "tank.resource.bombs.Bomb86");
			Game.AddLoadingFile(2, "image/game/living/Living190.swf", "game.living.Living190");
			Game.AddLoadingFile(2, "image/game/effect/6/danti.swf", "asset.game.six.danti");
			Game.AddLoadingFile(2, "image/game/effect/6/cpdian.swf", "asset.game.six.cpdian");
			Game.AddLoadingFile(2, "image/game/effect/6/qunti.swf", "asset.game.six.qunti");
            int[] resources = { bossID, npcID, npcID2 };
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(resources);
            Game.SetMap(1166);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            m_boss = Game.CreateBoss(bossID, 1950, 700, -1, 1, "");
            m_front = Game.Createlayer(1100, 1080, "font", "game.living.Living190", "stand", 1, 0);
            m_boss.FallFrom(m_boss.X, m_boss.Y, "", 0, 0, 1000);
            m_boss.SetRelateDemagemRect(-34, -35, 100, 70);
			
			
			/*boss = Game.CreateNormal(npcID, 10, 750, 1, 10);
			boss.FallFrom(boss.X, boss.Y, "", 0, 0, 1000);
			boss.CallFuction(new LivingCallBack(Go), 1200);
			
			m_king = Game.CreateBoss(npcID2, 10, 750, 1, 10);
			m_king.FallFrom(m_king.X, m_king.Y, "", 0, 0, 1000);
			m_king.CallFuction(new LivingCallBack(Run), 2200);*/
        }
		/*
		private void Go()
        {
            boss.MoveTo(605, boss.Y, "walk", 0, "", 7, new LivingCallBack(FlyUp));
        }
		
		private void FlyUp()
        {
		    boss.MoveTo(boss.X, boss.Y - 110, "flyup", 0, "", 3, new LivingCallBack(FlyLR), 2500);
		}
		
		private void FlyLR()
        {
		    boss.MoveTo(boss.X + 170, boss.Y, "flyLR", 0, "", 3, null);
		}
		
		private void Run()
        {
            m_king.MoveTo(605, m_king.Y, "walk", 0, "", 7, new LivingCallBack(WalkUp));
        }
		
		private void WalkUp()
        {
		    m_king.MoveTo(m_king.X, m_king.Y - 110, "flyup", 0, "", 3, new LivingCallBack(WalkLR), 2500);
		}
		
		private void WalkLR()
        {
		    m_king.MoveTo(m_king.X + 90, m_king.Y, "flyLR", 0, "", 3, null);
		}*/

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            IsSay = 0;
        }

        public override bool CanGameOver()
        {
            base.CanGameOver();

            if (Game.TurnIndex > Game.MissionInfo.TotalTurn - 1)
            {
                return true;
            }

            if (m_boss.IsLiving == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
		
        public override int UpdateUIData()
        {

            if (m_boss == null)
                return 0;

            if (m_boss.IsLiving == false)
            {
                return 1;
            }
            return base.UpdateUIData();
        }

        public override void OnGameOver()
        {
            base.OnGameOver();

            if (m_boss.IsLiving == false)
            {
                Game.IsWin = true;
            }
            else
            {
                Game.IsWin = false;
            }
        }
    }
}
