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
    public class FiveHardFourNpc1 : ABrain
    {
        private int m_attackTurn = 0;

        protected Player m_targer;

        private SimpleBoss m_enemyBoss;

        private int m_bossId = 5231;

        private int m_helperId = 5234;

        private static string[] HelpAttackSay = new string[] {
                    LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FiveHardElderFour.msg2"),
                    LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FiveHardElderFour.msg3"),
                    LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FiveHardElderFour.msg4")
                };

        private static string[] AttackBossSay = new string[] {
                    LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FiveHardElderFour.msg5"),
                    LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FiveHardElderFour.msg6"),
                    LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FiveHardElderFour.msg7"),
                    LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FiveHardElderFour.msg7")
                };
        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            m_body.CurrentDamagePlus = 1;
            m_body.CurrentShootMinus = 1;

            if (m_enemyBoss == null)
            {
                // find enemyboss
                SimpleBoss[] allBoss = ((PVEGame)Game).FindLivingTurnBossWithID(m_bossId);
                if (allBoss.Length > 0)
                {
                    m_enemyBoss = allBoss[0];
                }
            }
        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();

            if (m_enemyBoss != null && (int)m_enemyBoss.Properties1 == 1 && (int)m_enemyBoss.Properties2 == 0)
            {
                // boss chinh chuan bi khe lua
                // check have npc helper
                SimpleNpc[] allNpcs = ((PVEGame)Game).GetNPCLivingWithID(m_helperId);
                if (allNpcs.Length <= 0)
                {
                    // create new helper
                    ((SimpleBoss)Body).RandomSay(HelpAttackSay, 0, 1000, 0);
                    if (Body.ShootPoint(1340 + 50, 709, 56, 1000, 10000, 1, 1.5f, 2700, CreateHelperNpc))
                    {
                        Body.PlayMovie("beatA", 1500, 0);
                    }
                }
            }
            else if (m_enemyBoss != null && (int)m_enemyBoss.Properties2 == 1)
            {
                StartAttackEnemy();
            }
            else if (m_enemyBoss != null && (int)m_enemyBoss.Properties1 > 0 && (int)m_enemyBoss.Properties2 > 0)
            {
                StartAttackEnemy();
            }
        }

        private void StartAttackEnemy()
        {
            ((SimpleBoss)Body).RandomSay(AttackBossSay, 0, 1000, 0);
            Body.PlayMovie("beatC", 1500, 4000);
            Body.BeatDirect(m_enemyBoss, "", 3500, 4, 1);


            m_attackTurn++;
            // boss dang bi mu`
            if (m_attackTurn >= 2)
            {
                m_attackTurn = 0;
                m_enemyBoss.Properties2 = 3;
                m_enemyBoss.BlockTurn = false;
                Body.CallFuction(new LivingCallBack(CallBackPlayDao), 5000);
            }
        }

        private void CallBackPlayDao()
        {
            if (m_enemyBoss.IsLiving)
                m_enemyBoss.PlayMovie("dao", 0, 6000);
        }

        private void CreateHelperNpc()
        {
            LivingConfig config = ((PVEGame)Game).BaseLivingConfig();
            config.IsHelper = true;
            config.IsTurn = false;
            config.CanTakeDamage = false;
            ((SimpleBoss)Body).CreateChild(m_helperId, 1340, 709, true, config);
        }

        public override void OnDie()
        {
            base.OnDie();
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

    }
}
