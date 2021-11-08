using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using System.Drawing;
using Game.Logic.Actions;
using Bussiness;


namespace GameServerScript.AI.NPC
{
    public class SeventhNormalThirdtBoss : ABrain
    {
        private int m_attackTurn = 0;

        private int isSay = 0;
		
	    private int orchinIndex = 1;

        private int currentCount = 0;
		
		private PhysicalObj moive;

        private int Dander = 0;

        #region NPC 说话内容
        private static string[] AllAttackChat = new string[] {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SeventhSimpleNpc.msg1"),
        };

        private static string[] ShootChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SeventhSimpleNpc.msg2"),
        };

        private static string[] KillPlayerChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SeventhSimpleNpc.msg3")
        };

        private static string[] CallChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SeventhSimpleFirstBoss.msg1"),

        };

        private static string[] JumpChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SeventhSimpleFirstBoss.msg2"),

        };

        private static string[] KillAttackChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg13"),

              LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg14")
        };

        private static string[] ShootedChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg15"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg16")

        };

        private static string[] DiedChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg17")
        };

        #endregion

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();

            Body.CurrentDamagePlus = 1;
            Body.CurrentShootMinus = 1;

            isSay = 0;
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
                if (player.IsLiving && player.X > 1269 && player.X < Game.Map.Info.ForegroundWidth + 1)
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
                KillAttack(1269, Game.Map.Info.ForegroundWidth + 1);

                return;
            }
			if (m_attackTurn == 0)
            {               
				Angger();				
                m_attackTurn++;
            }
            else if (m_attackTurn == 1)
            {
                Summon();
				Body.State = 0;
                m_attackTurn++;
            }
			else if (m_attackTurn == 2)
            {
                Angger();					
                m_attackTurn++;
            }
			else if (m_attackTurn == 3)
            {
                Angger2();
                m_attackTurn++;
            }
			else if (m_attackTurn == 4)
            {
                Summon2();
				Body.State = 0;
                m_attackTurn++;
            }
			else if (m_attackTurn == 5)
            {
                Angger();
                m_attackTurn++;
            }
			else if (m_attackTurn == 6)
            {
                Angger2();
                m_attackTurn++;
            }
			else if (m_attackTurn == 7)
            {
                Summon3();
				Body.State = 0;
                m_attackTurn++;
            }
            else
            {
                Angger();
                m_attackTurn = 0;
            }
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        private void KillAttack(int fx, int tx)
        {
            Body.CurrentDamagePlus = 1;
            Body.PlayMovie("beatB", 2000, 0);
            Body.RangeAttacking(fx, tx, "cry", 3000, null);
        }
		
		public void Summon()
        {
            Body.PlayMovie("Ato", 100, 0);
            Body.CallFuction(new LivingCallBack(PersonalAttack), 2500);
        }
		
		public void Summon2()
        {
            Body.PlayMovie("Ato", 100, 0);
            Body.CallFuction(new LivingCallBack(Seal), 2500);
        }
		
		public void Summon3()
        {
            Body.PlayMovie("Ato", 100, 0);
            Body.CallFuction(new LivingCallBack(PersonalAttackDame), 2500);
        }
		
		private void Seal()
        {
            Body.PlayMovie("beatB", 3000, 4000);
			((SimpleBoss)Body).SetRelateDemagemRect(-21, -87, 72, 59);
			Body.CallFuction(new LivingCallBack(GoMovie), 7000);
        }
		
		private void GoMovie()
        {
		    Player target = Game.FindRandomPlayer();
			moive = ((PVEGame)Game).Createlayer(target.X, target.Y, "moive", "asset.game.seven.cao", "in", 1, 0);
			Player target2 = Game.FindRandomPlayer();
			moive = ((PVEGame)Game).Createlayer(target2.X, target2.Y, "moive", "asset.game.seven.cao", "in", 1, 0);
            Player target3 = Game.FindRandomPlayer();
			moive = ((PVEGame)Game).Createlayer(target3.X, target3.Y, "moive", "asset.game.seven.cao", "in", 1, 0);
			Player target4 = Game.FindRandomPlayer();
			moive = ((PVEGame)Game).Createlayer(target4.X, target4.Y, "moive", "asset.game.seven.cao", "in", 1, 0);
			Body.SyncAtTime = true;
            Body.PlayMovie("", 1000, 1500);
			int blood = Game.Random.Next(225, 774);//số máu	
			target.AddBlood(-blood, 1);
		}

        private void PersonalAttack()
        {
            Player target = Game.FindRandomPlayer();
            ((SimpleBoss)Body).SetRelateDemagemRect(-21, -87, 72, 59);

            if (target != null)
            {
                Body.CurrentDamagePlus = 1.8f;

                int mtX = Game.Random.Next(target.X - 0, target.X + 0);

				if (Body.ShootPoint(target.X, target.Y, 84, 1200, 10000, 1, 3.0f, 2550))
                {
                    Body.PlayMovie("beatA", 1700, 0);
                }
            }
        }
		
		public void Angger()
        {
            Body.State = 1;
            Dander = Dander + 100;
			Body.PlayMovie("toA", 1700, 0);
            ((SimpleBoss)Body).SetDander(Dander);

            if (Body.Direction == -1)
            {
                ((SimpleBoss)Body).SetRelateDemagemRect(8, -252, 74, 50);
            }
            else
            {
                ((SimpleBoss)Body).SetRelateDemagemRect(-82, -252, 74, 50);
            }
        }
		
		public void Angger2()
        {
            Body.State = 1;
            Dander = Dander + 100;
			Body.PlayMovie("standA", 1700, 0);
            ((SimpleBoss)Body).SetDander(Dander);

            if (Body.Direction == -1)
            {
                ((SimpleBoss)Body).SetRelateDemagemRect(8, -252, 74, 50);
            }
            else
            {
                ((SimpleBoss)Body).SetRelateDemagemRect(-82, -252, 74, 50);
            }
        }

		private void PersonalAttackDame()
        {
            Player target = Game.FindRandomPlayer();
            ((SimpleBoss)Body).SetRelateDemagemRect(-21, -87, 72, 59);

            if (target != null)
            {
                Body.CurrentDamagePlus = 1.8f;
                
                int mtX = Game.Random.Next(target.X - 0, target.X + 0);

                if (Body.ShootPoint(target.X, target.Y, 84, 1200, 10000, 1, 3.0f, 3000))
                {
                    Body.PlayMovie("beat", 1700, 0);
                }
            }
        }
    }
}
