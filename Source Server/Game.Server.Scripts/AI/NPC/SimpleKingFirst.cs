using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic.Actions;
using Bussiness;


namespace GameServerScript.AI.NPC
{
    public class SimpleKingFirst : ABrain
    {
        private int m_attackTurn = 0;

        private int isSay = 0;

        #region NPC 说话内容
        private static string[] AllAttackChat = new string[] { 
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingFirst.msg1"),
       
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingFirst.msg2"),
       
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingFirst.msg3")
        };

        private static string[] ShootChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingFirst.msg4"),
                               
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingFirst.msg5"),

             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingFirst.msg6")
        };

        private static string[] AddBooldChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingFirst.msg7"),
               
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingFirst.msg8"),
                
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingFirst.msg9")
         
        };

        private static string[] KillAttackChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingFirst.msg10")
        };

        private static string[] KillPlayerChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingFirst.msg11"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingFirst.msg12"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingFirst.msg13")
        };

        private static string[] ShootedChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingFirst.msg14"),
                   
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingFirst.msg15")
        };

        private static string[] DiedChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleKingFirst.msg16")    
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
            Body.Direction = Game.FindlivingbyDir(Body);
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
                m_attackTurn++;
            }
            else if (m_attackTurn == 1)
            {
                PersonalAttack();
                m_attackTurn++;
            }
            else
            {
                Healing();
                m_attackTurn = 0;
            }

        }

        private void KillAttack(int fx, int tx)
        {
            Body.CurrentDamagePlus = 10;
            int index = Game.Random.Next(0, KillAttackChat.Length);
            Body.Say(KillAttackChat[index], 1, 1000);
            Body.PlayMovie("beat", 3000, 0);
            Game.AddAction(new PlaySoundAction("078", 0));
            Body.RangeAttacking(fx, tx, "cry", 4000, null);
        }

        private void AllAttack()
        {
            Body.CurrentDamagePlus = 0.5f;

            int index = Game.Random.Next(0, AllAttackChat.Length);
            Body.Say(AllAttackChat[index], 1, 0);
            Body.PlayMovie("beat", 1000, 0);
            Body.RangeAttacking(-1, Game.Map.Info.ForegroundWidth + 1, "cry", 3000, null);
        }

        private void PersonalAttack()
        {
            int dis = Game.Random.Next(800, 980);
            Body.MoveTo(dis, Body.Y, "walk", 1000, new LivingCallBack(NextAttack));
        }

        private void Healing()
        {
            int index = Game.Random.Next(0, AddBooldChat.Length);
            Body.Say(AddBooldChat[index], 1, 0);
            Body.SyncAtTime = true;
            Body.AddBlood(5000);
            Body.PlayMovie("", 1000, 4500);
        }


        private void NextAttack()
        {
            Player target = Game.FindRandomPlayer();
            if (target != null)
            {
                if (target.X > Body.Y)
                {
                    Body.ChangeDirection(1, 800);
                }
                else
                {
                    Body.ChangeDirection(-1, 800);
                }

                Body.CurrentDamagePlus = 0.8f;

                int index = Game.Random.Next(0, ShootChat.Length);
                Body.Say(ShootChat[index], 1, 0);


                int mtX = Game.Random.Next(target.X - 50, target.X + 50);

                if (Body.ShootPoint(mtX, target.Y, 61, 1000, 10000, 1, 1, 2300))
                {
                    Body.PlayMovie("beat2", 1500, 0);
                }

                if (Body.ShootPoint(mtX, target.Y, 61, 1000, 10000, 1, 1, 4100))
                {
                    Body.PlayMovie("beat2", 3300, 0);
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
            Body.Say(KillPlayerChat[index], 1, 0, 2000);
        }

        public override void OnDiedSay()
        {
            base.OnDiedSay();
        }

        public override void OnShootedSay(int delay)
        {
            if (isSay == 0 && Body.IsLiving == true)
            {
                int index = Game.Random.Next(0, ShootedChat.Length);
                Body.Say(ShootedChat[index], 1, delay);
                isSay = 1;
            }
        }
    }
}
