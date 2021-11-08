using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using Game.Logic.Actions;
using Bussiness;

namespace GameServerScript.AI.Messions
{
    public class KD1121 : AMissionControl
    {
        private SimpleBoss boss = null;

        private int npcID = 2004;

        private int bossID = 2003;
		
		private int IsSay = 0;

        private int kill = 0;

        private PhysicalObj m_moive;

        private PhysicalObj m_front;
		
		private static string[] KillChat = new string[]{
           "Ah, mặt tôi .....",  
      
            "Ặc, giáp trụ xinh đẹp của mình đã bị trầy rồi.....",  	  
 
            "Ui za, đau quá !"
        };

        private static string[] ShootedChat = new string[]{
            "Ah, của tôi hết đừng lấy！"
        };

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 1750)
            {
                return 3;
            }
            else if (score > 1675)
            {
                return 2;
            }
            else if (score > 1600)
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
            int[] resources = { bossID, npcID };
            int[] gameOverResource = { bossID };
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(gameOverResource);;
            Game.AddLoadingFile(1, "bombs/51.swf", "tank.resource.bombs.Bomb51");
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.AntQueenAsset");
            Game.SetMap(1121);
        }
       
        public override void OnStartGame()
        {
            base.OnStartGame();
            m_moive = Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 1);
            m_front = Game.Createlayer(1131, 150, "font", "game.asset.living.AntQueenAsset", "out", 1, 1);
            boss = Game.CreateBoss(bossID, 1316, 444, -1, 1, "");
            boss.SetRelateDemagemRect(-42, -200, 84, 194);
            boss.Say(LanguageMgr.GetTranslation("Hộp là của tôi, con tôi, miễn là tôi có thể thấy là của tôi!"), 0, 200, 0);

            m_moive.PlayMovie("in", 6000, 0);
            m_front.PlayMovie("in", 6100, 0);
            m_moive.PlayMovie("out", 10000, 1000);
            m_front.PlayMovie("out", 9900, 0);
        }

        public override void OnNewTurnStarted()
        {
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            if (Game.TurnIndex > 1)
            {
                if (m_moive != null)
                {
                    Game.RemovePhysicalObj(m_moive, true);
                    m_moive = null;
                }
                if (m_front != null)
                {
                    Game.RemovePhysicalObj(m_front, true);
                    m_front = null;
                }
            }
        }

        public override bool CanGameOver()
        {
            if (boss != null && boss.IsLiving == false)
            {
                kill++;
                return true;
            }
            return false;
        }

        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return kill;
        }
       
        public override void OnGameOver()
        {
            base.OnGameOver();
            if (boss.IsLiving == false)
            {
                
                Game.IsWin = true;
            }
            else
            {
                Game.IsWin = false;
            }
        }
		
		public override void DoOther()
        {
            base.DoOther();
            int index = Game.Random.Next(0, KillChat.Length);
            boss.Say(KillChat[index], 0, 0);
        }

        public override void OnShooted()
        {
            if (boss.IsLiving && IsSay == 0)
            {
                int index = Game.Random.Next(0, ShootedChat.Length);
                boss.Say(ShootedChat[index], 0, 1500);
                IsSay = 1;
            }
        }
    }
}
