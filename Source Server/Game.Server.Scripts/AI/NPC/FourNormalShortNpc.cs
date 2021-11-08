using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using Bussiness;

namespace GameServerScript.AI.NPC
{
    public class FourNormalShortNpc : ABrain
    {

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
            Move();
        }

        public void Move()
        {
            int dis = Game.Random.Next(610, 700);
            int ramdis = Game.Random.Next(((SimpleNpc)Body).NpcInfo.MoveMin, ((SimpleNpc)Body).NpcInfo.MoveMax);
            if (dis > 97)
            {
                if (dis > ((SimpleNpc)Body).NpcInfo.MoveMax)
                {
                    dis = ramdis;
                }
                else
                {
                    dis = dis - 90;
                }

                if (Body.X - dis > dis)
                {
                    Body.MoveTo(Body.X - dis, Body.Y, "walk", 0, null);
                }
                else
                {
                    Body.MoveTo(Body.X + dis, Body.Y, "walk", 0, null);
                }
            }
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        #region NPC 小怪说话
        private static string[] NormalSay = 
        {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalShortNpc.msg1"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalShortNpc.msg2"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalShortNpc.msg3"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalShortNpc.msg4"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalShortNpc.msg5")
        };

        private static Random random = new Random();
        public static string GetOneChat(SimpleNpc Body)
        {
            int index = 0;
            string chat = "";
            index = random.Next(0, NormalSay.Length);
            chat = NormalSay[index];
            return chat;
        }
        #endregion
    }
}