using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using Bussiness;


namespace GameServerScript.AI.NPC
{
    public class SimpleNpcAi : ABrain
    {
        protected Player m_targer;

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();

            if (m_body.IsSay)
            {
                string msg = GetOneChat(((SimpleNpc)Body));
                int delay = Game.Random.Next(0, 5000);
                m_body.Say(msg, 0, delay);
            }
        }

        public override void OnCreated()
        {
            base.OnCreated();
        }


        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            m_body.CurrentDamagePlus = 1;
            m_body.CurrentShootMinus = 1;
            m_targer = Game.FindNearestPlayer(Body.X, Body.Y);
            Beating();
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        public void MoveToPlayer(Player player)
        {
            int dis = (int)player.Distance(Body.X, Body.Y);
            int ramdis = Game.Random.Next(((SimpleNpc)Body).NpcInfo.MoveMin, ((SimpleNpc)Body).NpcInfo.MoveMax);
            if (dis > 97)
            {
                if (dis > ((SimpleNpc)Body).NpcInfo.MoveMax)
                {
                    dis = ramdis;
                }
                else
                {
                    dis = dis - 90;
                }

                if (player.Y < 420 && player.X < 210)
                {
                    if (Body.Y > 420)
                    {
                        if (Body.X - dis < 50)
                        {
                            Body.MoveTo(25, Body.Y, "walk", 1200, new LivingCallBack(Jump));
                        }
                        else
                        {
                            Body.MoveTo(Body.X - dis, Body.Y, "walk", 1200, new LivingCallBack(MoveBeat));
                        }
                    }
                    else
                    {
                        if (player.X > Body.X)
                        {
                            Body.MoveTo(Body.X + dis, Body.Y, "walk", 1200, new LivingCallBack(MoveBeat));
                        }
                        else
                        {
                            Body.MoveTo(Body.X - dis, Body.Y, "walk", 1200, new LivingCallBack(MoveBeat));
                        }
                    }
                }
                else
                {
                    if (Body.Y < 420)
                    {
                        if (Body.X + dis > 200)
                        {
                            Body.MoveTo(200, Body.Y, "walk", 1200, new LivingCallBack(Fall));
                        }
                    }
                    else
                    {
                        if (player.X > Body.X)
                        {
                            Body.MoveTo(Body.X + dis, Body.Y, "walk", 1200, new LivingCallBack(MoveBeat));
                        }
                        else
                        {
                            Body.MoveTo(Body.X - dis, Body.Y, "walk", 1200, new LivingCallBack(MoveBeat));
                        }
                    }
                }               
            }
        }

        public void MoveBeat()
        {
            Body.Beat(m_targer, "beatA", 0, 0, 0);
        }

        public void FallBeat()
        {
            Body.Beat(m_targer, "beatA", 2000, 0, 0);
        }

        public void Jump()
        {
            Body.Direction = 1;
            Body.JumpTo(Body.X, Body.Y - 240, "Jump", 0, 2, 3, null);
            Body.CallFuction(Beating, 4500);
        }

        public void Beating()
        {
            if (m_targer != null && Body.Beat(m_targer, "beatA", 0, 0, 0) == false)
            {
                MoveToPlayer(m_targer);
            }
        }

        public void Fall()
        {
            Body.FallFrom(Body.X, Body.Y + 240, null, 0, 0, 12, new LivingCallBack(Beating));
        }

        #region NPC 小怪说话

        private static Random random = new Random();
        private static string[] BoguChat = new string[] { 
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleNpcAi.msg1"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleNpcAi.msg2"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleNpcAi.msg3"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleNpcAi.msg4"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleNpcAi.msg5"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleNpcAi.msg6"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleNpcAi.msg7"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleNpcAi.msg8"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleNpcAi.msg9"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleNpcAi.msg10"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleNpcAi.msg11"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleNpcAi.msg12"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleNpcAi.msg13")
        };

        private static string[] AntChat = new string[] {
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleNpcAi.msg14"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleNpcAi.msg15"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleNpcAi.msg16"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleNpcAi.msg17"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleNpcAi.msg18"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleNpcAi.msg19"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleNpcAi.msg20"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleNpcAi.msg21"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleNpcAi.msg22"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleNpcAi.msg23")
        };
        public static string GetOneChat(SimpleNpc Body)
        {
            int index = 0;
            string chat = "";

            switch (Body.NpcInfo.ID)
            {
                case 2001:
                case 2002:
                case 2004:
                case 2101:
                case 2102:
                case 2104:
                    index = random.Next(0, AntChat.Length);
                    chat = AntChat[index];
                    break;
                default:
                    index = random.Next(0, BoguChat.Length);
                    chat = BoguChat[index];
                    break;
            }

            return chat;
        }

        internal static string GetOneChat()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
