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
    public class TerrorCaptainAi : ABrain
    {
        private int m_attackTurn = 0;

        private int npcID = 1309;

        private int isSay = 0;

        private Point[] brithPoint = { new Point(600, 539), new Point(950, 539) };

        #region NPC 说话内容
        private static string[] AllAttackChat = new string[] {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorCaptainAi.msg1"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorCaptainAi.msg2"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorCaptainAi.msg3")
        };

        private static string[] ShootChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorCaptainAi.msg4"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorCaptainAi.msg5")
        };

        private static string[] KillPlayerChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorCaptainAi.msg6"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorCaptainAi.msg7")
        };

        private static string[] CallChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorCaptainAi.msg8"),
                  
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorCaptainAi.msg9")
        };

        private static string[] JumpChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorCaptainAi.msg10"),

             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorCaptainAi.msg11"),

             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorCaptainAi.msg12")
        };

        private static string[] KillAttackChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorCaptainAi.msg13")
        };

        private static string[] KillChat = new string[]{
           LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorCaptainAi.msg14"),

           LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorCaptainAi.msg15")
        };

        private static string[] ShootedChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorCaptainAi.msg16"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorCaptainAi.msg17")
        };

        private static string[] DiedChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorCaptainAi.msg18")
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
            int maxdis = 0;
            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving && player.X > 480 && player.X < 1000)
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
                KillAttack(480, 1000);

                return;
            }

            if (m_attackTurn == 0)
            {
                AllAttack();
                m_attackTurn++;
            }
            else if (m_attackTurn == 1)
            {
                PersonalAttack();
                m_attackTurn++;
            }
            else
            {
                Summon();
                m_attackTurn = 0;
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

        private void AllAttack()
        {
            ChangeDirection(3);
            Body.CurrentDamagePlus = 0.7f;
            int index = Game.Random.Next(0, AllAttackChat.Length);
            Body.Say(AllAttackChat[index], 1, 0);
            Body.FallFrom(Body.X, 509, null, 1000, 1, 12);
            Body.PlayMovie("beat2", 1000, 0);
            Body.RangeAttacking(-1, Game.Map.Info.ForegroundWidth + 1, "cry", 4000, null);
        }

        private void PersonalAttack()
        {
            ChangeDirection(3);
            int index = Game.Random.Next(0, ShootChat.Length);
            Body.Say(ShootChat[index], 1, 0);
            int dis = Game.Random.Next(670, 880);
            int direction = Body.Direction;
            Body.MoveTo(dis, Body.Y, "walk", 1000, new LivingCallBack(NextAttack));
            Body.ChangeDirection(direction, 9000);
        }

        private void Summon()
        {
            ChangeDirection(3);
            Body.JumpTo(Body.X, Body.Y - 300, "Jump", 1000, 1);
            int index = Game.Random.Next(0, CallChat.Length);
            Body.Say(CallChat[index], 1, 3300);
            Body.PlayMovie("call", 3500, 0);

            Body.CallFuction(new LivingCallBack(CreateChild), 4000);

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

                if (Body.ShootPoint(mtX, target.Y, 61, 1000, 10000, 1, 1, 3200))
                {
                    Body.PlayMovie("beat", 2700, 0);
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

        public void CreateChild()
        {
            ((SimpleBoss)Body).CreateChild(npcID, brithPoint, 8, 2, 1);
        }

        public override void OnKillPlayerSay()
        {
            base.OnKillPlayerSay();
            int index = Game.Random.Next(0, KillPlayerChat.Length);
            Body.Say(KillPlayerChat[index], 1, 0, 2000);
        }

        public override void OnDiedSay()
        {
            int index = Game.Random.Next(0, DiedChat.Length);
            Body.Say(DiedChat[index], 1, 0, 1500);
        }

        public override void OnShootedSay(int delay)
        {
            int index = Game.Random.Next(0, ShootedChat.Length);
            if (isSay == 0 && Body.IsLiving == true)
            {                
                Body.Say(ShootedChat[index], 1, delay, 0);
                isSay = 1;
            }

            if (!Body.IsLiving)
            {
                index = Game.Random.Next(0, DiedChat.Length);
                Body.Say(DiedChat[index], 1, delay - 800, 2000);
            }
        }
    }
}
