using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using SqlDataProvider.Data;
using Game.Logic;
using Bussiness;

namespace GameServerScript.AI.Messions
{
    public class TNSM3103 : AMissionControl
    {
        private SimpleBoss boss = null;

        private SimpleBoss m_boss = null;

        private SimpleBoss m_king = null;

        private int m_kill = 0;

        private int IsSay = 0;

        private int bossID = 3208;
        private int bossID2 = 3209;
        private int npcID = 3206;
        private int npcID2 = 3210;
        private int npcID3 = 3111;

        private PhysicalObj m_moive;

        private PhysicalObj m_front = null;

        private static string[] KillChat = new string[]{
            "Uma dor ~!",

            "Você se atreve a enfrentar?",

            "Vamos lutar de novo!"
        };

        private static string[] ShootedChat = new string[]{
            "Ah ~ ~ Tại sao bạn tấn công?<br/>Eu estou fazendo?",

            "Oh ~ ~ realmente dói! Por que eu tenho que lutar?<br/>Eu tenho que lutar..."
        };

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 1330)
            {
                return 3;
            }
            else if (score > 1150)
            {
                return 2;
            }
            else if (score > 970)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] resources = { bossID, bossID2, npcID, npcID2 };
            int[] gameOverResource = { bossID, bossID2 };
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(gameOverResource);
            Game.AddLoadingFile(1, "bombs/54.swf", "tank.resource.bombs.Bomb54");
            Game.AddLoadingFile(1, "bombs/58.swf", "tank.resource.bombs.Bomb58");
            Game.AddLoadingFile(2, "image/game/effect/3/buff.swf", "asset.game.4.buff");
            Game.AddLoadingFile(2, "image/game/effect/3/dici.swf", "asset.game.4.dici");
            //Game.AddLoadingFile(2, "image/game/effect/3/1124object3.swf", "game.crazyTank.view.Focus");
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.ClanBrotherAsset");
            Game.AddLoadingFile(2, "image/game/living/living117.swf", "living117_fla.walk_6");
            Game.SetMap(1124);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            Game.IsBossWar = "3203";
            m_moive = Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 0);
            m_front = Game.Createlayer(650, 400, "front", "game.asset.living.ClanBrotherAsset", "out", 1, 0);
            m_king = Game.CreateBoss(bossID, 1360, 357, -1, 1, "");
            m_king.FallFrom(m_king.X, m_king.Y, "", 0, 0, 1000, null);
            m_king.SetRelateDemagemRect(m_king.NpcInfo.X, m_king.NpcInfo.Y, m_king.NpcInfo.Width, m_king.NpcInfo.Height);
            boss = Game.CreateBoss(bossID2, 255, 357, 1, 1, "");
            boss.FallFrom(boss.X, boss.Y, "", 0, 0, 1000, null);
            boss.SetRelateDemagemRect(boss.NpcInfo.X, boss.NpcInfo.Y, boss.NpcInfo.Width, boss.NpcInfo.Height);
            m_moive.PlayMovie("in", 5000, 0);
            m_front.PlayMovie("in", 5000, 0);
            m_moive.PlayMovie("out", 9000, 0);
            m_front.PlayMovie("out", 9400, 0);

        }



        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();

            if (m_moive != null)
            {
                Game.RemovePhysicalObj(m_moive, true);
                m_moive = null;
            }
            if (m_front != null)
            {
                Game.RemovePhysicalObj(m_front, true);
                m_front = null;
            }
        }

        public override bool CanGameOver()
        {
            if (m_king != null && m_king.IsLiving == false && boss != null && boss.IsLiving == false)
            {
                m_kill++;
                return true;
            }
            return false;
        }


        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return m_kill;
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            bool IsAllPlayerDie = true;
            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving == true)
                {
                    IsAllPlayerDie = false;
                }
            }
            if (m_king != null && m_king.IsLiving == false && boss != null && boss.IsLiving == false && IsAllPlayerDie == false)
            {
                Game.IsWin = true;
            }
            else
            {
                Game.IsWin = false;
            }
        }

        public override void DoOther()
        {
            base.DoOther();
            if (m_king == null)
                return;
            if (m_king.IsLiving)
            {
                int index = Game.Random.Next(0, KillChat.Length);
                m_king.Say(KillChat[index], 0, 0);
                boss.Say(KillChat[index], 0, 0);
            }
            else
            {
                int index = Game.Random.Next(0, KillChat.Length);
                m_king.Say(KillChat[index], 0, 0);
                boss.Say(KillChat[index], 0, 0);
            }
        }

        public override void OnShooted()
        {
            if (IsSay == 0)
            {
                if (m_king.IsLiving)
                {
                    int index = Game.Random.Next(0, ShootedChat.Length);
                    m_king.Say(ShootedChat[index], 0, 1500);
                    boss.Say(ShootedChat[index], 0, 1500);
                }
                else
                {
                    int index = Game.Random.Next(0, ShootedChat.Length);
                    m_king.Say(ShootedChat[index], 0, 1500);
                    boss.Say(ShootedChat[index], 0, 1500);
                }

                IsSay = 1;
            }
        }
    }
}
