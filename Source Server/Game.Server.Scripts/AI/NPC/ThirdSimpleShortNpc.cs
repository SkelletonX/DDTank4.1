using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;

namespace GameServerScript.AI.NPC
{
    public class ThirdSimpleShortNpc : ABrain
    {
        protected Player m_targer;
        protected SimpleNpc m_bloom;
        protected Living attack;

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
            SimpleNpc npc = (SimpleNpc)Body;
            m_bloom = Game.FindNearestAdverseNpc(Body.X, Body.Y, npc.NpcInfo.Camp );

            if (m_bloom != null)
            {
                if (Math.Abs(Body.X - m_targer.X) > Math.Abs(Body.X - m_bloom.X))
                    attack = m_bloom;
                else
                    attack = m_targer;
            }
            else
            {
                attack = m_targer;
            }
            Beating();
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        public void MoveToPlayer()
        {
            int dis = (int)attack.Distance(Body.X, Body.Y);
            int ramdis = Game.Random.Next(((SimpleNpc)Body).NpcInfo.MoveMin, ((SimpleNpc)Body).NpcInfo.MoveMax);
            if (dis > 60)
            {
                if (dis > ((SimpleNpc)Body).NpcInfo.MoveMax)
                {
                    dis = ramdis;
                }
                else
                {
                    dis = dis - 60;
                }

                if (Body.X - attack.X > dis)
                {
                    Body.MoveTo(Body.X - dis, attack.Y, "walk", 1200, new LivingCallBack(MoveBeat));
                }
                else
                {
                    Body.MoveTo(Body.X + dis, attack.Y, "walk", 1200, new LivingCallBack(MoveBeat));
                }
            }
        }

        public void MoveBeat()
        {
            Body.Beat(attack, "beatA", 0, 0, 0);
        }

        public void FallBeat()
        {
            Body.Beat(attack, "beatA", 2000, 0, 0);
        }

        public void Jump()
        {
            Body.Direction = 1;
            Body.JumpTo(Body.X, Body.Y - 240, "Jump", 0, 2, 3, new LivingCallBack(Beating));
        }

        public void Beating()
        {
            if ( attack != null && Body.Beat(attack, "beatA", 0, 0, 0) == false)
            {
                MoveToPlayer();
            }
        }

        #region NPC 小怪说话

        private static Random random = new Random();
        private static string[] BoguChat = new string[] { 
            "想K我？你还嫩~！",
            "丫的还真敢K我！",
        };

        private static string[] AntChat = new string[] {
            "东西抢了！人给我！",
            "想跑？",
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
        #endregion
    }
}
