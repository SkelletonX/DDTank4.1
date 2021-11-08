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
    public class ThirteenNormalAntBoss : ABrain
    {
        private int m_attackTurn = 0;

        private PhysicalObj moive;

        private PhysicalObj k_moive;
		
		private PhysicalObj m_moive;

        private int isSay = 0;

        #region NPC 说话内容
        private static string[] AllAttackChat = new string[] {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg1"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg2"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg3")
        };

        private static string[] ShootChat = new string[]{
            LanguageMgr.GetTranslation("Nén nhẹ mũi tên của ta đây !"),

            LanguageMgr.GetTranslation("Hãy nén thử mũi tên băng này đi !")  
        };

        private static string[] KillPlayerChat = new string[]{
            LanguageMgr.GetTranslation("Nén nhẹ mũi tên của ta đây !"),

            LanguageMgr.GetTranslation("Đón nhận mũi tên thần kì !")
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
                if (player.IsLiving && player.X > 1169 && player.X < Game.Map.Info.ForegroundWidth + 1)
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
                KillAttack(1169, Game.Map.Info.ForegroundWidth + 1);
                return;
            }

			if (m_attackTurn == 0)
            {
				PersonalAttack2();
                m_attackTurn++;
            }
			else if (m_attackTurn == 1)
            {
                Healing();
                Plain();               
                m_attackTurn++;
            }
            else
            {
                PersonalAttack();				
                m_attackTurn = 0;
            }
        }
        private void Healing()
        {
            Body.SyncAtTime = true;
            if (Game.GetDiedBossCount() == 0)
                Body.AddBlood(15000);
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        private void KillAttack(int fx, int tx)
        {
            int index = Game.Random.Next(0, KillAttackChat.Length);
            Body.Say(KillAttackChat[index], 1, 1000);
            Body.CurrentDamagePlus = 10;
            Body.PlayMovie("beatB", 3000, 0);
            Body.RangeAttacking(fx, tx, "cry", 3000, null);
        }

        private void PersonalAttack()
        {
            Player targetplayer = Game.FindRandomPlayer();

            if (targetplayer != null)
            {
                Body.CurrentDamagePlus = 2.8f;
                int index = Game.Random.Next(0, KillPlayerChat.Length);
                Body.Say(KillPlayerChat[index], 1, 0);

                if (Body.ShootPoint(targetplayer.X, targetplayer.Y, 51, 1400, 10000, 1, 3.0f, 2550))
                {
                    Body.PlayMovie("beatA", 1700, 0);
                }
            }
        }

        private void Plain()
        {
            int index = Game.Random.Next(0, ShootChat.Length);
            Body.Say(ShootChat[index], 1, 0);

            Body.CurrentDamagePlus = 1.8f;
            Player[] players = Game.GetAllPlayers();
            int Count = 0;
            foreach (Player target in players)
            {
                if (target != null)
                {
                    if (Body.ShootPoint(target.X, target.Y, 99, 1000, 10000, 1, 2.7f, 3000))
                    {
                        Body.PlayMovie("beatD", 1500, 0);
                    }
                }
                Count++;
                if (Count == 2)
                    break;
            }
        }

        private void PersonalAttack2()
        {
            int index = Game.Random.Next(0, KillPlayerChat.Length);
            Body.Say(KillPlayerChat[index], 1, 0);
            Body.PlayMovie("beatC", 3500, 0);
            Body.CallFuction(new LivingCallBack(GoShoot), 4000);
        }

        private void GoShoot()
        {
            Body.CurrentDamagePlus = 4.8f;
            Player[] players = Game.GetAllPlayers();
            foreach (Player target in players)
            {
                moive = ((PVEGame)Game).Createlayer(target.X, target.Y, "moive", "asset.game.ten.jianyu", "out", 1, 1);
                Body.RangeAttacking(0, Game.Map.Info.ForegroundWidth + 1, "cry", 1000, null);
            }
            Body.CallFuction(GoOut, 2000);
        }

        private void GoOut()
        {
            if (moive != null)
            {
                Game.RemovePhysicalObj(moive, true);
                moive = null;
            }
        }

        public override void OnKillPlayerSay()
        {
            base.OnKillPlayerSay();
            int index = Game.Random.Next(0, KillPlayerChat.Length);
            Body.Say(KillPlayerChat[index], 1, 0, 2000);
        }

        private void CreateChild()
        {

        }

        public override void OnShootedSay()
        {
            int index = Game.Random.Next(0, ShootedChat.Length);
            if (isSay == 0 && Body.IsLiving == true)
            {
                Body.Say(ShootedChat[index], 1, 900, 0);
                isSay = 1;
            }

            if (!Body.IsLiving)
            {
                index = Game.Random.Next(0, DiedChat.Length);
                Body.Say(DiedChat[index], 1, 900 - 800, 2000);
            }
        }
    }
}
