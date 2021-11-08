using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic;
using Game.Logic.Phy.Object;
using Game.Logic.Actions;
using System.Drawing;
using Bussiness;


namespace GameServerScript.AI.NPC
{
    public class SimpleKingSecond : ABrain
    {
        private int m_attackTurn = 0;

        private int m_turn = 0;

        private PhysicalObj m_wallLeft = null;

        private PhysicalObj m_wallRight = null;

        private int isEixt = 0;

        private int npcID = 1010;

        private Point[] brithPoint = { new Point(682, 673), new Point(1092, 673) };

        #region NPC 说话内容
        private static string[] AllAttackChat = new string[] { 
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingSecond.msg1"),
       
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingSecond.msg2"),
       
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingSecond.msg3")
        };

        private static string[] ShootChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingSecond.msg4"),
                               
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingSecond.msg5"),

             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingSecond.msg6")
        };

        private static string[] AddBooldChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingSecond.msg7"),
               
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingSecond.msg8"),
                
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingSecond.msg9")
         
        };

        private static string[] KillAttackChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingSecond.msg10")
        };

        private static string[] FrostChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingSecond.msg11"),
               
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingSecond.msg12"),
               
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingSecond.msg13")
              
        };

        private static string[] WallChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingSecond.msg14"),

             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingSecond.msg15")
         };


        private static string[] KillPlayerChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingSecond.msg16"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingSecond.msg17"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingSecond.msg18")
        };

        private static string[] ShootedChat = new string[]{
           LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingSecond.msg19"),
                   
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingSecond.msg20")
        };

        private static string[] DiedChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingSecond.msg21")    
        };
        #endregion

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            m_body.CurrentDamagePlus = 1;
            m_body.CurrentShootMinus = 1;

        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            bool result = false;
            int maxdis = 0;
            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving && player.X > 620 && player.X < 1194)
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
                KillAttack(620, 1194);
                return;
            }

            if (m_attackTurn == 0)
            {
                AllAttack();
                if (isEixt == 1)
                {
                    m_wallLeft.CanPenetrate = true;
                    m_wallRight.CanPenetrate = true;
                    Game.RemovePhysicalObj(m_wallLeft, true);
                    Game.RemovePhysicalObj(m_wallRight, true);
                    isEixt = 0;
                }
                m_attackTurn++;
            }
            else if (m_attackTurn == 1)
            {
                FrostAttack();
                m_attackTurn++;
            }
            else if (m_attackTurn == 2)
            {
                ProtectingWall();
                m_attackTurn++;
            }
            else
            {

                CriticalStrikes();
                m_attackTurn = 0;
            }

        }

        private void CriticalStrikes()
        {
            Player target = Game.GetFrostPlayerRadom();
            List<Player> players = Game.GetAllFightPlayers();
            List<Living> NotFrostPlayers = new List<Living>();
            foreach (Player player in players)
            {
                if (player.IsFrost == false)
                {
                    NotFrostPlayers.Add(player);
                }
            }

            ((SimpleBoss)Body).CurrentDamagePlus = 30;
            if (NotFrostPlayers.Count != players.Count)
            {
                if (NotFrostPlayers.Count != 0)
                {
                    Body.PlayMovie("beat1", 0, 0);
                    Body.RangeAttacking(-1, Game.Map.Info.ForegroundWidth + 1, "beat1", 1500, NotFrostPlayers, 0);
                }
                else
                {
                    Body.PlayMovie("beat1", 0, 0);
                    Body.RangeAttacking(-1, Game.Map.Info.ForegroundWidth + 1, "beat1", 1500, null);
                }
            }
            else
            {
                Body.Say(LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingSecond.msg22"), 1, 3300);
                Body.PlayMovie("renew", 3500, 0);
                Body.CallFuction(new LivingCallBack(CreateChild), 6000);
            }
        }

        private void FrostAttack()
        {
            int mtX = Game.Random.Next(800, 980);
            Body.MoveTo(mtX, Body.Y, "walk", 0, new LivingCallBack(NextAttack));
        }

        private void AllAttack()
        {
            Body.CurrentDamagePlus = 0.5f;
            if (m_turn == 0)
            {
                int index = Game.Random.Next(0, AllAttackChat.Length);
                Body.Say(AllAttackChat[index], 1, 10000);
                Body.PlayMovie("beat1", 12000, 0);
                Body.RangeAttacking(-1, Game.Map.Bound.Width + 1, "cry", 14000, null);
                m_turn++;
            }
            else
            {
                int index = Game.Random.Next(0, AllAttackChat.Length);
                Body.Say(AllAttackChat[index], 1, 0);
                Body.PlayMovie("beat1", 1000, 0);
                Body.RangeAttacking(Body.X - 1000, Body.X + 1000, "cry", 3000, null);
            }
        }

        private void KillAttack(int fx, int tx)
        {
            int index = Game.Random.Next(0, KillAttackChat.Length);
            if (m_turn == 0)
            {
                Body.CurrentDamagePlus = 10;
                Body.Say(KillAttackChat[index], 1, 10000);
                Body.PlayMovie("beat1", 12000, 0);
                Body.RangeAttacking(fx, tx, "cry", 14000, null);
                m_turn++;
            }
            else
            {
                Body.CurrentDamagePlus = 10;
                Body.Say(KillAttackChat[index], 1, 0);
                Body.PlayMovie("beat1", 2000, 0);
                Body.RangeAttacking(fx, tx, "cry", 4000, null);
            }
        }

        private void ProtectingWall()
        {
            if (isEixt == 0)
            {
                m_wallLeft = ((PVEGame)Game).CreatePhysicalObj(Body.X - 15, 620, "wallLeft", "com.mapobject.asset.WaveAsset_01_left", "1", 1, 1, 0);
                m_wallRight = ((PVEGame)Game).CreatePhysicalObj(Body.X + 15, 620, "wallRight", "com.mapobject.asset.WaveAsset_01_right", "1", 1, 1, 0);
                m_wallLeft.CanPenetrate = false;
                m_wallRight.CanPenetrate = false;
                m_wallLeft.SetRect(-165, -169, 43, 330);
                m_wallRight.SetRect(128, -165, 41, 330);
                isEixt = 1;
            }
            int index = Game.Random.Next(0, WallChat.Length);
            Body.Say(WallChat[index], 1, 0);
        }

        public void CreateChild()
        {
            Body.PlayMovie("renew", 100, 2000);
            ((SimpleBoss)Body).CreateChild(npcID, brithPoint, 8, 2, 1);
        }

        private void NextAttack()
        {
            int count = Game.Random.Next(1, 2);
            for (int i = 0; i < count; i++)
            {
                Player target = Game.FindRandomPlayer();
                if (target != null && target.IsFrost == false)
                {
                    int index = Game.Random.Next(0, ShootChat.Length);
                    Body.Say(ShootChat[index], 1, 0);

                    if (target.X > Body.X)
                    {
                        Body.ChangeDirection(1, 500);
                    }
                    else
                    {
                        Body.ChangeDirection(-1, 500);
                    }

                    if (Body.ShootPoint(target.X, target.Y, 1, 1000, 10000, 1, 1.5f, 2000))
                    {
                        Body.PlayMovie("beat2", 1500, 0);
                    }

                }
            }
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        public override void OnKillPlayerSay()
        {
            base.OnKillPlayerSay();
            int index = Game.Random.Next(0, KillPlayerChat.Length);
            Body.Say(KillPlayerChat[index], 1, 0, 1500);
        }

        public override void OnDiedSay()
        {
            int index = Game.Random.Next(0, DiedChat.Length);
            Body.Say(DiedChat[index], 1, 0, 2000);
        }
    }
}
