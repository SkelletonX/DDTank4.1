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
    public class ThirteenSimpleBrotherNpc : ABrain
    {
        private int m_attackTurn = 0;
		
	    private PhysicalObj m_moive;

        #region NPC 说话内容
        private static string[] AllAttackChat = new string[] {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg1"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg2"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg3")
        };

        private static string[] ShootChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg4"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg5")  
        };

        private static string[] KillPlayerChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg6"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg7")
        };

        private static string[] CallChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg8"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg9")

        };

        private static string[] JumpChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg10"),

             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg11"),

             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg12")
        };

        private static string[] KillAttackChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg13"),

              LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg14")
        };

        private static string[] ShootedChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg15"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg16")

        };

        private static string[] DiedChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg17")
        };

        #endregion

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();

            Body.CurrentDamagePlus = 1;
            Body.CurrentShootMinus = 1;
        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            Body.Direction = Game.FindlivingbyDir(Body);     
       
			if (m_attackTurn == 0)
            {
                CallBuff();
                m_attackTurn++;
            }
            else 
            {
                PersonalAttack();
                m_attackTurn = 0;
            }
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }
		
        private void PersonalAttack()
        {
            bool canAdd = false;
            Player[] players = Game.GetAllPlayers();
            List<Player> playerArr = new List<Player>();
            foreach (Player player in players)
            {
                if (player.X > 990 && player.X < 1315 && player.Y < 606)
                {
                    canAdd = true;
                    playerArr.Add(player);
                    break;
                }
            }
            if (canAdd)
            {
                foreach (Player p in players)
                {
                    p.AddEffect(new DamageEffect(2), 0);
                    p.AddEffect(new GuardEffect(2), 0);
                }
            }
            Body.RangeAttacking(920, 1370, "cry", 1000, playerArr);
            Body.PlayMovie("call", 1000, 1000);
            Body.CallFuction(Remove, 1000);
        }

        public void CallBuff()
        {
            Body.JumpTo(Body.X, Body.Y - 300, "walk", 2000, -1);
            Body.PlayMovie("call", 1000, 1000);
            Body.CallFuction(CreateMovie, 1000);
        }

        public void Remove()
        {
            if (m_moive != null)
            {
                //Game.RemovePhysicalObj(m_moive, true);
                //m_moive = null;   
                m_moive.PlayMovie("beatA", 1000, 1000);
            }
            //m_moive = ((PVEGame)Game).CreatePhysicalObj(1146, 566, "moiveB", "asset.game.ten.jitan", "beatA", 1, 0);
        }

		public void CreateMovie()
        {            
            m_moive = ((PVEGame)Game).CreatePhysicalObj(1146, 566, "moiveA", "asset.game.ten.jitan", "born", 1, 0);
        }

    }
}