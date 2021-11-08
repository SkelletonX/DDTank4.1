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
    public class TerrorKingLast : ABrain
    {
        private int attackingTurn = 1;

        private int dander = 0;

        private int npcID = 1304;

        private int isSay = 0;

        private int isBomb = 0;

        private Point[] brithPoint = { new Point(682, 673), new Point(1092, 673) };

        #region NPC 说话内容
        private static string[] AllAttackChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorKingLast.msg1"),
          
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorKingLast.msg2"),
          
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorKingLast.msg3"),

             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorKingLast.msg4")
        };

        private static string[] ShootChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorKingLast.msg5"),
               
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorKingLast.msg6"),
             
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorKingLast.msg7"),
               
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorKingLast.msg8"),
               
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorKingLast.msg9")          
        };

        private static string[] CallChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorKingLast.msg10")                          
        };

        private static string[] AngryChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorKingLast.msg11")                          
        };

        private static string[] KillAttackChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorKingLast.msg12")                          
        };

        private static string[] SealChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorKingLast.msg13")                          
        };

        private static string[] KillPlayerChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorKingLast.msg14"),                  
 
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorKingLast.msg15")
        };

        private static string[] ShootedChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorKingLast.msg16"),
               
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorKingLast.msg17"),
               
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorKingLast.msg18")
        };

        private static string[] DiedChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.TerrorKingLast.msg19")
        };
        private static string[] GoOnAnggerChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.HardKingLast.msg17")
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
            Body.Direction = Game.FindlivingbyDir(Body);
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

            if (attackingTurn == 1)
            {
                HalfAttack();
            }
            else if (attackingTurn == 2)
            {
                Summon();
            }
            else if (attackingTurn == 3)
            {
                Seal();
            }
            else if (attackingTurn == 4)
            {
                Angger();
            }
            else
            {
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
            Body.CurrentDamagePlus = 1.0f;
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
                (Body as SimpleBoss).TowardsToPlayer(m_player.X, 500);
                Body.PlayMovie("mantra", 2000, 2000);
                Body.Seal(m_player, 1, 3000);
            }
        }

        public void Angger()
        {
            int index = Game.Random.Next(0, AngryChat.Length);
            Body.Say(AngryChat[index], 1, 0);
            Body.State = 1;
            dander += 100;
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
                ((PVEGame)Game).CreateTip(0, 0, "com.mapobject.asset.wordtip75", "com.mapobject.asset.wordtip75", "Empty", 1, 0);
                Body.CurrentDamagePlus = 1000;
                Game.AddAction(new FocusAction(Body.X, Body.Y - 150, 2, 0, 1500));
                Body.Say(GoOnAnggerChat[0], 0, 3500);
                Body.PlayMovie("beatC", 9500, 0);
                Body.RangeAttacking(-1, Game.Map.Info.ForegroundWidth + 1, "cry", 12600, null);
                Game.AddAction(new LockFocusAction(false, 0, 0));
                Body.Die(11500);
                isBomb = 1;
            }
            else
            {
                Body.PlayMovie("cast", 0, 2000);
                List<Player> players = Game.GetAllLivingPlayers();

                foreach (Player player in players)
                {
                    player.AddEffect(new ContinueReduceBloodEffect(2, 714, null), 0);
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
            if (Body.IsLiving == true && Body.State == 0)
            {
                int index = Game.Random.Next(0, KillPlayerChat.Length);
                Body.Say(KillPlayerChat[index], 1, 0, 2000);
            }
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
