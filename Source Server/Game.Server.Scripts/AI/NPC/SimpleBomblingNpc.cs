using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Bussiness;


namespace GameServerScript.AI.NPC
{
    public class SimpleBomblingNpc : ABrain
    {
        private Player m_target = null;

        private int m_targetDis = 0;

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            Body.CurrentDamagePlus = 1;
            Body.CurrentShootMinus = 1;
            if (Body.IsSay)
            {
                string msg = GetOneChat();
                int delay = Game.Random.Next(0, 5000);
                Body.Say(msg, 0, delay);
            }
        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            m_target = Game.FindNearestPlayer(Body.X, Body.Y);
            m_targetDis = (int)m_target.Distance(Body.X, Body.Y);
            if (m_targetDis <= 50)
            {
                Body.PlayMovie("beatA", 100, 0);
                Body.RangeAttacking(Body.X - 50, Body.X + 50, "cry", 1500, null);
                Body.Die(1000);
            }
            else
            {
                MoveToPlayer(m_target);
            }
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        public void MoveToPlayer(Player player)
        {
            int dis = Game.Random.Next(((SimpleNpc)Body).NpcInfo.MoveMin, ((SimpleNpc)Body).NpcInfo.MoveMax);
            if (player.X > Body.X)
            {
                if (Body.X + dis >= player.X)
                {
                    Body.MoveTo(player.X - 10, Body.Y, "walk", 2000, new LivingCallBack(Beat));
                }
                else
                {
                    Body.MoveTo(Body.X + dis, Body.Y, "walk", 2000, new LivingCallBack(Beat));
                }
            }
            else
            {
                if (Body.X - dis <= player.X)
                {
                    Body.MoveTo(player.X + 10, Body.Y, "walk", 2000, new LivingCallBack(Beat));
                }
                else
                {
                    Body.MoveTo(Body.X - dis, Body.Y, "walk", 2000, new LivingCallBack(Beat));
                }
            }
        }

        public void Beat()
        {
            m_targetDis = (int)m_target.Distance(Body.X, Body.Y);
            if (m_targetDis <= 50)
            {
                Body.PlayMovie("beatA", 100, 0);
                Body.RangeAttacking(Body.X - 100, Body.X + 100, "cry", 1500, null);
                Body.Die(1000);
            }
        }

        #region NPC 小炸弹人说话

        private static Random random = new Random();
        private static string[] bombNpcChat = new string[] { 
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleBomblingNpc.msg1"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleBomblingNpc.msg2"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleBomblingNpc.msg3"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleBomblingNpc.msg4")
        };
        public static string GetOneChat()
        {
            int index = random.Next(0, bombNpcChat.Length);
            return bombNpcChat[index];
        }
        #endregion
    }
}
