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
    public class PSMNpc5001Ai : ABrain
    {

        private Player m_targer;

        private int m_attackTurn = 0;

        int mindis = 90;

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

        public override void OnStartAttacking()
        {
            Body.Direction = Game.FindlivingbyDir(Body);

            m_targer = Game.FindNearestPlayer(Body.X, Body.Y);

            int dis = (int)Body.Distance(m_targer.X, m_targer.Y);
            if (dis < mindis)
            {
                Body.CurrentDamagePlus = 2.5f;
                MoveBeat();
            }
            else
            {
                PersonalAttack();
                m_attackTurn++;
            }
        }

        public void MoveBeat()
        {
            Body.Beat(m_targer, "beatA", 0, 0, 0);
        }

        private void PersonalAttack()
        {
            int dis = 0;
            if (m_targer.X > Body.X)
            {
                dis = Game.Random.Next(Body.X + 300, Body.X + 400);
            }
            else
            {
                dis = Game.Random.Next(Body.X - 400, Body.X - 300);
            }
            int direction = Body.Direction;
            dis = dis < 50 ? 50 : dis;
            dis = dis > Game.Map.Bound.Width - 50 ? Game.Map.Bound.Width - 50 : dis;
            Body.MoveTo(dis, Body.Y, "walk", 1000, new LivingCallBack(NextAttack));

            // Body.ChangeDirection(direction, 9000);
        }


        private void NextAttack()
        {
            int dis = (int)Body.Distance(m_targer.X, m_targer.Y);

            if (dis < mindis)
            {
                Body.CurrentDamagePlus = 2.5f;
                MoveBeat();
            }
            else
            {
                if (m_targer != null)
                {
                    Body.SetRect(0, 0, 0, 0);
                    Body.Direction = Game.FindlivingbyDir(Body);

                    Body.CurrentDamagePlus = 1.0f;

                    int mtX = Game.Random.Next(m_targer.X - 50, m_targer.X + 50);

                    if (Body.ShootPoint(mtX, m_targer.Y, 57, 1000, 10000, 1, 1, 1500))
                    {
                        Body.PlayMovie("beatB", 1700, 0);
                    }
                }
            }
        }
    }
}
