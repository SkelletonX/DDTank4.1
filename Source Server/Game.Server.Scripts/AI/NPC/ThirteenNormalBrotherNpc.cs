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
    public class ThirteenNormalBrotherNpc : ABrain
    {
        private int m_attackTurn = 0;

        private int isSay = 0;
		
		private int IsEixt = 0;
		
	    private PhysicalObj m_moive;

        private PhysicalObj m_front;
		
		private PhysicalObj wallLeft = null;

        #region NPC 说话内容
        private static string[] AllAttackChat = new string[] {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg1"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg2"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg3")
        };

        private static string[] ShootChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg4"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg5")  
        };

        private static string[] KillPlayerChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg6"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg7")
        };

        private static string[] CallChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg8"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg9")

        };

        private static string[] JumpChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg10"),

             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg11"),

             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg12")
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
                KillAttack(0, 0);

                return;
            }
			if (m_attackTurn == 0)
            {
                Jump();			
                m_attackTurn++;
            }
            else if (m_attackTurn == 1)
            {
                JumpPersonalAttack();
                m_attackTurn++;
            }
			else if (m_attackTurn == 2)
            {
                FallSummon();
                m_attackTurn++;
            }
            else
            {
                Healing();
                m_attackTurn = 0;
            }
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }
		
		public void JumpPersonalAttack()
        {
            Body.PlayMovie("walk", 0, 500);
			Body.JumpTo(Body.X, Body.Y - 150, "", 0, 1, new LivingCallBack(PersonalAttack));
			((SimpleBoss)Body).SetRelateDemagemRect(-41, -107, 83, 100);
        }

        private void PersonalAttack()
        {
            Player target = Game.FindRandomPlayer();

            if (target != null)
            {
				((SimpleBoss)Body).SetRelateDemagemRect(-41, -107, 83, 100);

                int mtX = Game.Random.Next(target.X - 10, target.Y + 20);

                if (Body.ShootPoint(target.X, target.Y, 54, 1000, 10000, 1, 3.0f, 2550))
                {
                    Body.PlayMovie("beatA", 1700, 0);
                }
            }
        }

        public void Jump()
        {
            Body.PlayMovie("walk", 700, 0);
			Body.JumpTo(Body.X, Body.Y - 150, "", 1000, 1, new LivingCallBack(CreateChild));
			((SimpleBoss)Body).SetRelateDemagemRect(-41, -107, 83, 100);
        }
		
		
		public void FallSummon()
        {
            Body.PlayMovie("walk", 700, 0);
			Body.FallFrom(Body.X, Body.Y + 150, "", 1000, 0, 50, new LivingCallBack(Summon));
			((SimpleBoss)Body).SetRelateDemagemRect(-41, -107, 83, 100);
        }

        public void Summon()
        {
            Body.PlayMovie("call", 100, 0);
			wallLeft = ((PVEGame)Game).CreatePhysicalObj(1146, 566, "moive", "asset.game.ten.jitan", "beatA", 1, 0);
		    Body.CallFuction(new LivingCallBack(Remove), 1000);
        }
		
		public void Remove()
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
		
        public void KillAttack(int fx, int tx)
        {
            Body.CurrentDamagePlus = 10;
            int index = Game.Random.Next(0, KillAttackChat.Length);
            ((SimpleBoss)Body).Say(KillAttackChat[index], 1, 500);
            Body.PlayMovie("beatB", 2500, 0);
            Body.RangeAttacking(fx, tx, "cry", 3300, null);
        }

        public void Healing()
        {
            Body.SyncAtTime = true;
            Body.AddBlood(5000);
			Body.PlayMovie("castA", 100, 0);
            Body.Say("Hồi phục sức mạnh", 1, 0);
        }
		
		public void CreateChild()
        {
            Body.PlayMovie("call", 100, 0);
			m_moive = ((PVEGame)Game).Createlayer(1146, 566, "moive", "asset.game.ten.jitan", "out", 1, 0);
            m_front = ((PVEGame)Game).Createlayer(1146, 566, "font", "asset.game.ten.jitan", "out", 1, 0);	
        }

    }
}