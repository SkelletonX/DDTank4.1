using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic.Effects;
using Game.Logic;
using Game.Logic.Actions;
namespace GameServerScript.AI.NPC
{
    public class FourNormalHawkNpc : ABrain
    {
        private int m_attackTurn = 0;

        private List<PhysicalObj> m_featherEffect = new List<PhysicalObj>();

        private int m_totalNpc = 3;

        private int npcId = 4102;

        private int friendlyNpcId = 4106;

        private SimpleBoss friendBoss = null;

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            
            ClearFeatherEffect();

            Body.CurrentDamagePlus = 1;
            Body.CurrentShootMinus = 1;

            if ((int)Body.Properties1 == 0)
                Body.Config.HaveShield = true;
            else
                Body.Config.HaveShield = false;
            //Console.WriteLine("x: " + Body.Bound.X + " - y: " + Body.Bound.Y + " - w: " + Body.Bound.Width + " - h: " + Body.Bound.Height);
        }

        public override void OnCreated()
        {
            base.OnCreated();
            Body.Properties1 = 0;
        }

        public override void OnStartAttacking()
        {
            if (m_attackTurn == 0)
            {
                if((int)Body.Properties1 == 0)
                    FlyRandom(new LivingCallBack(AllAttackFeather));
                m_attackTurn++;
            }
            else if (m_attackTurn == 1)
            {
                if ((int)Body.Properties1 == 0)
                    FlyRandom(new LivingCallBack(AllAttackFeather));
                m_attackTurn++;

            }
            else if (m_attackTurn == 2)
            {
                if ((int)Body.Properties1 == 0)
                {
                    FlyRandom(new LivingCallBack(ChangeAToB));
                    Body.Properties1 = 1;
                }
                else
                {
                    ChangeBToA();
                    Body.Properties1 = 0;
                }
                m_attackTurn = 0;
            }
        }

        public override void OnAfterTakedBomb()
        {
            base.OnAfterTakedBomb();
            if ((int)Body.Properties1 == 1)
            {
                // takedamage friend boss
                if (friendBoss == null)
                    GetFriendBoss();

                // get blood
                int bloodReduce = friendBoss.Blood - Body.Blood;

                friendBoss.AddBlood(-bloodReduce, 1);


            }
        }

        private void GetFriendBoss()
        {
            // create friend bosss
            foreach (SimpleBoss boss in Game.FindLivingTurnBossWithID(friendlyNpcId))
            {
                friendBoss = boss;
                break;
            }
        }

        private void FlyRandom(LivingCallBack callBack)
        {
            int randX = Game.Random.Next(400, 1283);
            int randY = Game.Random.Next(400, 654);

            Body.MoveTo(randX, randY, "fly", 1000, callBack, 7);
        }

        private void AllAttackFeather()
        {
            Body.PlayMovie("beatA", 500, 0);

            Player rand = Game.FindRandomPlayer();
            ((PVEGame)Game).SendObjectFocus(rand, 1, 1500, 0);

            Body.CallFuction(new LivingCallBack(CreateFeatherEffect), 2000);
            Body.RangeAttacking(0, Game.Map.Info.DeadWidth, "cry", 2100, null);
            //m_featherEffect = ((PVEGame)Game).Createlayer(0, 0, "", "asset.game.4.feather", "", 1, 0);
        }

        private void CreateFeatherEffect()
        {
            foreach (Player p in Game.GetAllLivingPlayers())
            {
                m_featherEffect.Add(((PVEGame)Game).Createlayer(p.X, p.Y, "", "asset.game.4.feather", "", 1, 0));
            }
        }

        private void ClearFeatherEffect()
        {
            foreach (PhysicalObj phy in m_featherEffect)
            {
                Game.RemovePhysicalObj(phy, true);
            }
            m_featherEffect = new List<PhysicalObj>();
        }

        private void ChangeAToB()
        {
            Body.PlayMovie("AtoB", 500, 0);
        }

        private void ChangeBToA()
        {
            Body.PlayMovie("BtoA", 1000, 0);
            Body.CallFuction(new LivingCallBack(SetState), 4000);
        }

        private void SetState()
        {
            if((int)Body.Properties1 == 1)
            {
                //Body.PlayMovie("standB", 0, 0);
                ((PVEGame)Game).SendLivingActionMapping(Body, "stand", "standB");
            }
            else
            {
                //Body.PlayMovie("standA", 0, 0);
                ((PVEGame)Game).SendLivingActionMapping(Body, "stand", "standA");
            }
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }
        
    }
}