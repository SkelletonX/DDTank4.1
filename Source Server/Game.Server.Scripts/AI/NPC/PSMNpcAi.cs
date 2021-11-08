using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using System.Drawing;
using Bussiness;


namespace GameServerScript.AI.NPC
{
    public class PSMNpcAi : ABrain
    {
        private int m_attackTurn = 0;

        private int IsSay = 0;

        private Point[] brithPoint = { new Point(600, 539), new Point(950, 539) };

        #region NPC 说话内容
        private static string[] AllAttackChat = new string[] {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleCaptainAi.msg1"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleCaptainAi.msg2"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleCaptainAi.msg3")
        };

        private static string[] ShootChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleCaptainAi.msg4"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleCaptainAi.msg5")
        };

        private static string[] KillPlayerChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleCaptainAi.msg6"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleCaptainAi.msg7")
        };

        private static string[] CallChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleCaptainAi.msg8"),
                  
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleCaptainAi.msg9")
        };

        private static string[] JumpChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleCaptainAi.msg10"),

             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleCaptainAi.msg11"),

             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleCaptainAi.msg12")
        };

        private static string[] KillAttackChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleCaptainAi.msg13")
        };

        private static string[] KillChat = new string[]{
           LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleCaptainAi.msg14"),

           LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleCaptainAi.msg15")
        };

        private static string[] ShootedChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleCaptainAi.msg16"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleCaptainAi.msg17")
        };

        private static string[] DiedChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleCaptainAi.msg18")
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

            IsSay = 0;

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
            Body.Direction = Game.FindlivingbyDir(Body);
            bool result = false;
            int mindis = 400;
            foreach (Player player in Game.GetAllFightPlayers())
            {
                int dis = (int)Body.Distance(player.X, player.Y);
                if (dis < mindis)
                {
                    mindis = dis;
                    result = true;
                    break;
                }
            }

            if (result)
            {
                KillAttack(Body.X - 400, Body.X + 400);
            }
            else
            {
                PersonalAttack();
                m_attackTurn++;
            }
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        private void KillAttack(int fx, int tx)
        {
            ChangeDirection(3);
            int index = Game.Random.Next(0, KillAttackChat.Length);
            Body.Say(KillAttackChat[index], 1, 1000);
            Body.CurrentDamagePlus = 10;
            Body.PlayMovie("beat2", 3000, 0);
            Body.RangeAttacking(fx, tx, "cry", 5000, null);
        }


        private void PersonalAttack()
        {
            ChangeDirection(3);
            Player p = Game.FindNearestPlayer(Body.X, Body.Y);
            int index = Game.Random.Next(0, ShootChat.Length);
            Body.Say(ShootChat[index], 1, 0);
            int dis = 0;
            if (p.X > Body.X)
                dis = Game.Random.Next(Body.X + 50, Body.X + 250);
            else
                dis = Game.Random.Next(Body.X - 250, Body.X - 50);
            int direction = Body.Direction;
            dis = dis < 250 ? 250 : dis;
            dis = dis > Game.Map.Info.ForegroundWidth - 250 ? Game.Map.Info.ForegroundWidth - 250 : dis;
            Body.MoveTo(dis, Body.Y, "walk", 1000, new LivingCallBack(NextAttack));

            Body.ChangeDirection(direction, 9000);
        }


        private void NextAttack()
        {
            Player target = Game.FindRandomPlayer();
            if (target != null)
            {
                Body.SetRect(0, 0, 0, 0);
                (Body as SimpleBoss).TowardsToPlayer(target.X, 500);

                Body.CurrentDamagePlus = 1.0f;


                int mtX = Game.Random.Next(target.X - 50, target.X + 50);

                if (Body.ShootPoint(mtX, target.Y, 61, 1000, 10000, 1, 1, 2200))
                {
                    Body.PlayMovie("beat", 1700, 0);
                }
            }
        }

        private void ChangeDirection(int count)
        {
            int direction = Body.Direction;
            for (int i = 0; i < count; i++)
            {
                Body.ChangeDirection(-direction, i * 200 + 100);
                Body.ChangeDirection(direction, (i + 1) * 100 + i * 200);
            }
        }


        public override void OnKillPlayerSay()
        {
            base.OnKillPlayerSay();
            int index = Game.Random.Next(0, KillPlayerChat.Length);
            Body.Say(KillPlayerChat[index], 1, 0, 2000);
        }


        public override void OnShootedSay(int delay)
        {
            int index = Game.Random.Next(0, ShootedChat.Length);
            if (IsSay == 0 && Body.IsLiving == true)
            {
                Body.Say(ShootedChat[index], 1, delay, 0);
                IsSay = 1;
            }

            if (!Body.IsLiving)
            {
                index = Game.Random.Next(0, DiedChat.Length);
                Body.Say(DiedChat[index], 1, delay - 800, 2000);
            }
        }
    }
}
