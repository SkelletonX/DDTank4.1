using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using System.Drawing;
using Game.Logic.Actions;
using Bussiness;
using Game.Logic.Effects;

namespace GameServerScript.AI.NPC
{
    public class SeventhSimpleSecondBoss : ABrain
    {
        private int m_attackTurn = 0;
		
		protected Player m_targer;
		
		private PhysicalObj moive;

        private PhysicalObj effectPhy;

        private int m_bloodReduce = 120;

        private List<Player> players = null;

        #region
        private static string[] AssAttackSay = new string[] {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SeventhSimpleSecondBoss.msg1"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SeventhSimpleSecondBoss.msg2"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SeventhSimpleSecondBoss.msg3")
        };

        private static string[] ShootChat = new string[]{
              LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SeventhSimpleSecondBoss.msg4"),
              LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SeventhSimpleSecondBoss.msg5"),
              LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SeventhSimpleSecondBoss.msg6")
        };

        private static string[] KillAttackChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SeventhSimpleSecondBoss.msg7"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SeventhSimpleSecondBoss.msg8")
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
            Body.MaxBeatDis = 300;
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            Body.Direction = Game.FindlivingbyDir(Body);
            bool result = false;
            int maxdis = 0;
            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving && player.X > Body.X - 300 && player.X < Body.X + 300)
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
                KillAttack(Body.X - 300, Body.X + 300);
                return;
            }

            if (m_attackTurn == 0)
            {
                Attack1();
                m_attackTurn++;
            }
            else if (m_attackTurn == 1)
            {
                Attack3();
                m_attackTurn++;
            }
			else if (m_attackTurn == 2)
            {
                Attack4();
                m_attackTurn++;
            }
			else
            {
                Attack2();
                m_attackTurn = 0;
            }
        }

        private void KillAttack(int fx, int tx)
        {
			int index = Game.Random.Next(0, KillAttackChat.Length);
            Body.CurrentDamagePlus = 1000f;
            Body.Say(KillAttackChat[index], 1, 0);
            Body.PlayMovie("speak", 0, 0);
            Body.RangeAttacking(fx, tx, "cry", 2000, null);
        }

        private void Attack1()
        {
            m_targer = Game.FindNearestPlayer(Body.X, Body.Y);

            if(m_targer != null)
            {
                Body.ChangeDirection(m_targer, 100);
                Body.MoveTo(m_targer.X - 100, m_targer.Y, "run", 1000, new LivingCallBack(PersonAttack), 10);
            }
        }

        private void Attack2()
        {
            m_targer = Game.FindRandomPlayer();
            Body.CurrentDamagePlus = 3f;
            if (m_targer != null)
            {
                Body.ChangeDirection(m_targer, 100);
                Body.MoveTo(m_targer.X - 200, m_targer.Y, "run", 1000, new LivingCallBack(PersonAttack2), 10);
            }
        }

        private void Attack3()
        {
            m_targer = Game.FindRandomPlayer();
            Body.CurrentDamagePlus = 1.5f; 
            if(m_targer != null)
            {
                ((SimpleBoss)Body).RandomSay(ShootChat, 0, 1000, 0);
                if (Body.ShootPoint(m_targer.X, m_targer.Y, 83, 1000, 10000, 2, 2f, 3000))
                {
                    Body.PlayMovie("beatC", 1300, 0);
                }
            }
        }

        private void Attack4()
        {
            Body.MoveTo(1484, 950, "run", 1000, new LivingCallBack(EffectAttack), 10);
        }

        private void PersonAttack()
        {
            if(m_targer != null)
            {
                Body.Beat(m_targer, "beatB", 100, 0, 100);
                Body.CallFuction(new LivingCallBack(RunBackDefaultPoint), 3000);
            }
        }

        private void PersonAttack2()
        {
            if (m_targer != null)
            {
                ((SimpleBoss)Body).RandomSay(AssAttackSay, 0, 50, 0);
                Body.Beat(m_targer, "beatA", 100, 0, 100);
                Body.CallFuction(new LivingCallBack(RunBackDefaultPoint), 3000);
            }
        }

        private void EffectAttack()
        {
            Body.ChangeDirection(-1, 0);
            Body.PlayMovie("beatD", 500, 0);
            Body.Say(LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SeventhSimpleSecondBoss.msg6"), 4500, 0);
            Body.CallFuction(new LivingCallBack(CallEffectRandomPlace), 5000);
        }

        private void CallEffectRandomPlace()
        {
            ClearEffect();
            m_targer = Game.FindRandomPlayer();
            if(m_targer != null)
            {
                ((PVEGame)Game).SendObjectFocus(m_targer, 0, 0, 0);
                Body.CallFuction(new LivingCallBack(CreateEffectMovie), 1000);
                foreach(Player p in Game.GetAllLivingPlayers())
                {
                    if(p.X > m_targer.X - 180 && p.X < m_targer.X + 180)
                        p.AddEffect(new ContinueReduceGreenBloodEffect(5, m_bloodReduce, Body), 1500);
                }

                Body.CallFuction(new LivingCallBack(RunBackDefaultPoint), 3000);
            }
        }

        private void CreateEffectMovie()
        {
            if(m_targer != null)
            {
                moive = ((PVEGame)Game).Createlayer(m_targer.X, m_targer.Y, "", "asset.game.seven.choud", "", 1, 1);
                effectPhy = ((PVEGame)Game).Createlayer(m_targer.X, m_targer.Y, "", "asset.game.seven.du", "", 1, 1);
                Body.CallFuction(new LivingCallBack(ClearMovie), 3000);
            }
            
        }

        private void ClearEffect()
        {
            if(effectPhy != null)
            {
                Game.RemovePhysicalObj(effectPhy, true);
            }

            foreach(Player p in Game.GetAllLivingPlayers())
            {
                ContinueReduceGreenBloodEffect effectPlayer = p.EffectList.GetOfType(eEffectType.ContinueReduceGreenBloodEffect) as ContinueReduceGreenBloodEffect;
                if (effectPlayer != null)
                    effectPlayer.Stop();
            }
        }

        private void ClearMovie()
        {
            if (moive != null)
                Game.RemovePhysicalObj(moive, true);
        }

        private void RunBackDefaultPoint()
        {
            Body.MoveTo(200, 590, "run", 100, new LivingCallBack(ChangeDir), 11);
        }

        private void ChangeDir()
        {
            if (Body.X > 1000)
                Body.ChangeDirection(-1, 100);
            else
                Body.ChangeDirection(1, 100);
        }

        public override void OnDie()
        {
            base.OnDie();
            ClearEffect();
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }
    }
}
