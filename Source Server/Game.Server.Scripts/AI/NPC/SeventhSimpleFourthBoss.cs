using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using System.Drawing;
using Game.Logic.Actions;
using Bussiness;


namespace GameServerScript.AI.NPC
{
    public class SeventhSimpleFourthBoss : ABrain
    {
        private int m_attackTurn = 0;

        private List<PhysicalObj> moives = new List<PhysicalObj>();

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            RemoveMovie();

            Body.CurrentDamagePlus = 1;
            Body.CurrentShootMinus = 1;
        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            Body.Direction = Game.FindlivingbyDir(Body);

            m_attackTurn++;

            if(m_attackTurn == 1)
            {
                AllAttackPlayer();
            }
            else
            {
                PersonAttack();
                m_attackTurn = 0;
            }
        }

        private void AllAttackPlayer()
        {
            Body.CurrentDamagePlus = 2f;
            Body.PlayMovie("beatB", 1000, 0);
            Body.CallFuction(new LivingCallBack(GoMovie), 4000);
            Body.RangeAttacking(Body.X - 10000, Body.Y + 10000, "cry", 4500, null);
        }

        private void PersonAttack()
        {
            Player target = Game.FindRandomPlayer();
            
            if (Body.ShootPoint(target.X, target.Y, 84, 1200, 10000, 1, 3.0f, 3000))
            {
                Body.PlayMovie("beat", 1000, 0);
            }
        }

        private void GoMovie()
        {
            foreach (Player p in Game.GetAllLivingPlayers())
            {
                moives.Add(((PVEGame)Game).Createlayer(p.X, p.Y, "moive", "asset.game.seven.cao", "in", 1, 0));
            }
        }

        private void RemoveMovie()
        {
            foreach (PhysicalObj phy in moives)
            {
                if (phy != null)
                    Game.RemovePhysicalObj(phy, true);
            }

            moives = new List<PhysicalObj>();
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }
        
    }
}
