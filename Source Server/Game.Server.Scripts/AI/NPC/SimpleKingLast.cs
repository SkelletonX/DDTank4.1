using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic.Effects;
using Game.Logic.Actions;
using System.Drawing;
using Bussiness;


namespace GameServerScript.AI.NPC
{
    public class SimpleKingLast : ABrain
    {
        private int attackingTurn = 1;

        private int dander = 0;

        private int npcID = 1010;

        private int isSay = 0;

        private int isBomb = 0;

        private Point[] brithPoint = { new Point(620, 670), new Point(1194, 670) };

        #region NPC 说话内容
        private static string[] AllAttackChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingLast.msg1"),
          
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingLast.msg2"),
          
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingLast.msg3"),

             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingLast.msg4")
        };

        private static string[] ShootChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingLast.msg5"),
               
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingLast.msg6"),
             
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingLast.msg7"),
               
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingLast.msg8"),
               
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingLast.msg9")          
        };

        private static string[] CallChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingLast.msg10")                          
        };

        private static string[] AngryChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingLast.msg11")                          
        };

        private static string[] KillAttackChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingLast.msg12")                          
        };

        private static string[] SealChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingLast.msg13")                          
        };

        private static string[] KillPlayerChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingLast.msg14"),                  
 
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingLast.msg15")
        };

        private static string[] ShootedChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingLast.msg16"),
               
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingLast.msg17"),
               
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingLast.msg18")
        };

        private static string[] DiedChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingLast.msg19")
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

            if (result && Body.State != 1)
            {
                KillAttack(620, 1194);
                return;
            }

            if (result == true)
            {
                return;
            }

            if (attackingTurn == 1)
            {
                Body.Direction = Game.FindlivingbyDir(Body);
                HalfAttack();
            }
            else if (attackingTurn == 2)
            {
                Body.Direction = -Body.Direction;
                Summon();
            }
            else if (attackingTurn == 3)
            {
                Body.Direction = -Body.Direction;
                Seal();
            }
            else if (attackingTurn == 4)
            {
                Body.Direction = -Body.Direction;
                Angger();
            }
            else
            {
                Body.Direction = -Body.Direction;
                GoOnAngger();
                attackingTurn = 0;
            }
            attackingTurn++;
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        public void HalfAttack()
        {
            Body.CurrentDamagePlus = 0.5f;
            int index = Game.Random.Next(0, SealChat.Length);
            Body.Say(AllAttackChat[index], 1, 500);
            Body.PlayMovie("beatB", 2500, 0);
            if (Body.Direction == 1)
            {
                Body.RangeAttacking(Body.X - 1, Game.Map.Info.ForegroundWidth + 1, "cry", 3300, null);
            }
            else
            {
                Body.RangeAttacking(-1, Body.X + 1, "cry", 3300, null);
            }
        }

        public void Summon()
        {
            int index = Game.Random.Next(0, CallChat.Length);
            Body.Say(CallChat[index], 1, 0);
            Body.PlayMovie("beatA", 100, 0);
            Body.CallFuction(new LivingCallBack(CreateChild), 2500);
        }

        public void Seal()
        {
            int index = Game.Random.Next(0, SealChat.Length);
            ((SimpleBoss)Body).Say(SealChat[index], 1, 0);
            Player m_player = Game.FindRandomPlayer();
            if (m_player != null)
            {
                Body.PlayMovie("mantra", 2000, 2000);
                Body.Seal(m_player, 1, 3000);
            }
        }

        public void Angger()
        {
            int index = Game.Random.Next(0, AngryChat.Length);
            Body.Say(AngryChat[index], 1, 0);
            Body.State = 1;
            dander = dander + 100;
            ((SimpleBoss)Body).SetDander(dander);
            if (Body.Direction == -1)
            {
                ((SimpleBoss)Body).SetRelateDemagemRect(8, -252, 74, 50);
            }
            else
            {
                ((SimpleBoss)Body).SetRelateDemagemRect(-82, -252, 74, 50);
            }
        }

        public void GoOnAngger()
        {
            if (Body.State == 1)
            {
                Body.CurrentDamagePlus = 1000;
                Game.AddAction(new FocusAction(Body.X, Body.Y, 1, 0, 1500));
                Body.PlayMovie("beatC", 3500, 0);
                Body.RangeAttacking(-1, Game.Map.Info.ForegroundWidth + 1, "cry", 6600, null);
                Body.Die(5500);
                isBomb = 1;
            }
            else
            {
                Body.PlayMovie("cast", 0, 2000);
                List<Player> players = Game.GetAllLivingPlayers();

                foreach (Player player in players)
                {
                    player.AddEffect(new ContinueReduceBloodEffect(2, 200, null), 0);
                }
            }
        }

        public void KillAttack(int fx, int mx)
        {
            Body.CurrentDamagePlus = 10;
            int index = Game.Random.Next(0, KillAttackChat.Length);
            ((SimpleBoss)Body).Say(KillAttackChat[index], 1, 500);
            Body.PlayMovie("beatB", 2500, 0);
            Body.RangeAttacking(fx, mx, "cry", 3300, null);
        }

        public void CreateChild()
        {
            ((SimpleBoss)Body).CreateChild(npcID, brithPoint, 8, 2, 0);
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

            if (!Body.IsLiving && isBomb == 0)
            {
                index = Game.Random.Next(0, DiedChat.Length);
                Body.Say(DiedChat[index], 1, delay - 800, 2000);
            }
        }
    }
}
