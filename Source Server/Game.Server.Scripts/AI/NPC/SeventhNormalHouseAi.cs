using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;

namespace GameServerScript.AI.NPC
{
    public class SeventhNormalHouseAi : ABrain
    {
        private int m_state = 0;

        private List<SimpleNpc> Children = new List<SimpleNpc>();

        private int npcID2 = 7122;

        private int npcID = 7121;

        private int m_maxNpc1_OnMap = 3;

        private int m_maxNpc2_OnMap = 10;

        private int m_countCall_Npc1;

        private int m_countCall_Npc2;

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();

            if (m_state == 0 && Body.Blood < Body.MaxBlood / 2)
            {
                Body.PlayMovie("toA", 0, 1200);
            }

            SetState();

            Body.CurrentDamagePlus = 1;
            Body.CurrentShootMinus = 1;
        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            bool result = false;
            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving && player.X > Body.X - 400 && player.X < Body.X + 400)
                {
                    result = true;
                }
            }

            if (result)
            {
                KillAttack(Body.X - 400, Body.X + 400);
                return;
            }
            m_countCall_Npc1 = 1;
            m_countCall_Npc2 = 4;
            CreateNpc1();
            CreateNpc2();
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }
        

        private void SetState()
        {
            if (m_state == 0)
                Body.PlayMovie("stand", 2000, 0);
            else
                Body.PlayMovie("standA", 2000, 0);
        }

        public void KillAttack(int fx, int tx)
        {
            Body.RangeAttacking(fx, tx, "cry", 1000, null);
        }

        public void CreateNpc1()
        {
            if (m_countCall_Npc1 > m_maxNpc1_OnMap)
                m_countCall_Npc1 = m_maxNpc1_OnMap;

            int countCanCreate = m_maxNpc1_OnMap - Game.GetLivedNpcs(npcID).Count;

            if (m_countCall_Npc1 > countCanCreate)
                m_countCall_Npc1 = countCanCreate;

            if (m_countCall_Npc1 > 0)
            {
                // create
                for (int i = 0; i < m_countCall_Npc1; i++)
                {
                    ((SimpleBoss)Body).CreateChild(npcID, 596, 955, 0, 1, true, ((PVEGame)Game).BaseLivingConfig());
                }
            }

        }

        public void CreateNpc2()
        {
            if (m_countCall_Npc2 > m_maxNpc2_OnMap)
                m_countCall_Npc2 = m_maxNpc2_OnMap;

            int countCanCreate = m_maxNpc2_OnMap - Game.GetLivedNpcs(npcID2).Count;

            if (m_countCall_Npc2 > countCanCreate)
                m_countCall_Npc2 = countCanCreate;

            // create
            if (m_countCall_Npc2 > 0)
            {
                for (int i = 0; i < m_countCall_Npc2; i++)
                {
                    ((SimpleBoss)Body).CreateChild(npcID2, 792 + (i * 50), 950, 0, 1, true, ((PVEGame)Game).BaseLivingConfig());
                }
            }
        }

    }
}
