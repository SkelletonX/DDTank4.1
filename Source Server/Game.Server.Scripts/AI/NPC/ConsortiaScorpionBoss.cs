using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameServerScript.AI.NPC
{
    public class ConsortiaScorpionBoss : ABrain
    {
        private int m_attackTurn = 0;

        public int currentCount = 0;

        public int Dander = 0;

        private PhysicalObj moive;

        Player target = null;

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();

            Body.CurrentDamagePlus = 1;
            Body.CurrentShootMinus = 1;
            Body.SetRect(((SimpleBoss)Body).NpcInfo.X, ((SimpleBoss)Body).NpcInfo.Y, ((SimpleBoss)Body).NpcInfo.Width, ((SimpleBoss)Body).NpcInfo.Height);

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

            switch (m_attackTurn)
            {
                case 0:
                    Attack1();
                    m_attackTurn++;
                    break;

                case 1:
                    Attack2();
                    m_attackTurn++;
                    break;

                case 2:
                    Attack3();
                    m_attackTurn++;
                    break;

                default:
                    Attack4();
                    m_attackTurn = 0;
                    break;
            }
        }

        private void Attack1()
        {
            Body.PlayMovie("beatA", 1000, 0);
            target = Game.FindRandomPlayer();
            Body.CurrentDamagePlus = 2f;
            Body.RangeAttacking(Body.X - 10000, Body.X + 10000, "cry", 3300, null);
        }

        private void Attack2()
        {
            Body.Say("Hãy đỡ phi tiêu của ta!", 0 , 500);
            Body.PlayMovie("beatB", 1000, 0);
            target = Game.FindRandomPlayer();
            Body.CurrentDamagePlus = 5f;
            Body.RangeAttacking(Body.X - 10000, Body.X + 10000, "cry", 3500, null);
        }

        private void Attack3()
        {
            Body.Say("Chọt chọt chọt!!!", 0, 500);
            Body.PlayMovie("beatC", 1000, 0);
            target = Game.FindRandomPlayer();
            Body.CurrentDamagePlus = 15f;
            Body.RangeAttacking(Body.X - 10000, Body.X + 10000, "cry", 3000, null);
        }

        private void Attack4()
        {
            Body.PlayMovie("beatD", 1000, 0);
            target = Game.FindRandomPlayer();
            Body.CurrentDamagePlus = 10f;
            Body.RangeAttacking(Body.X - 10000, Body.X + 10000, "cry", 3000, null);
        }
    }
}
