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
    public class PSMNpc5004Ai : ABrain
    {

        private Player m_targer;

        private int m_attackTurn = 0;

        private int mindis = 126;

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();

            Body.CurrentDamagePlus = 1;
            Body.CurrentShootMinus = 1;

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

            m_targer = Game.FindNearestPlayer(Body.X, Body.Y);

            int dis = (int)Body.Distance(m_targer.X, m_targer.Y);
            if (dis < mindis)
            {
                MoveBeat();
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

        public void MoveBeat()
        {
            Body.CurrentDamagePlus = 2.5f;
            Body.Beat(m_targer, "beatA", 0, 0, 0);
        }

        private void PersonalAttack()
        {
            int dis = 0;
            int direction = Body.Direction;

            if (m_targer.X > Body.X)
            {
                dis = Game.Random.Next(Body.X + 200, Body.X + 300);
            }
            else
            {
                dis = Game.Random.Next(Body.X - 300, Body.X - 200);
            }

            if ((Body.X <= 255 && Body.X >= 245 && m_targer.X < Body.X) || (Body.X <= 1195 && Body.X >= 1185 && m_targer.X > Body.X))
            {
                Shoot();
            }
            else
            {
                dis = dis < 350 ? 350 : dis;
                dis = dis > 1190 ? 1190 : dis;
                Body.MoveTo(dis, Body.Y, "walk", 1000, new LivingCallBack(NextAttack));
            }
        }

        private void NextAttack()
        {
            if (m_targer != null)
            {
                int dis = (int)Body.Distance(m_targer.X, m_targer.Y);
                if (dis < mindis)
                {
                    MoveBeat();
                }
                else
                {
                    Shoot();
                }
            }
        }

        private void Shoot()
        {
            Body.SetRect(0, 0, 0, 0);
            Body.Direction = Game.FindlivingbyDir(Body);

            Body.CurrentDamagePlus = 1.0f;
            int mtX = 0;
            if (m_targer.X < 90)
            {
                mtX = Game.Random.Next(m_targer.X + 20, m_targer.X + 30);
                if (Body.ShootPoint(mtX, m_targer.Y, 65, 10000, 40000, 1, 2, 2000))
                {
                    Body.PlayMovie("beatB", 1700, 0);
                }
            }
            else
            {
                mtX = Game.Random.Next(m_targer.X - 50, m_targer.X + 50);

                if (Body.ShootPoint(mtX, m_targer.Y, 65, 10000, 40000, 1, 1, 2000))
                {
                    Body.PlayMovie("beatB", 1700, 0);
                }
            }
        }
    }
}
