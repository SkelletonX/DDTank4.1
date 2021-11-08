using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using SqlDataProvider.Data;
using Game.Logic.Actions;
using System.Drawing;
using Bussiness;

namespace GameServerScript.AI.Messions
{
    public class CTM1377 : AMissionControl
    {
        private SimpleBoss m_king = null;

        private SimpleBoss m_boss = null;

        private int m_kill = 0;

        private int bossID = 1307;

        private int npcID = 1304;

        private int kingID = 1305;

        private PhysicalObj m_kingMoive;

        private PhysicalObj m_front;

        private bool isSend =false;
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
            int[] resources = { npcID, bossID, kingID };
            int[] gameOverResource = { bossID };
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(gameOverResource);

            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BombKingAsset");
            Game.AddLoadingFile(2, "image/map/1076/objects/1076MapAsset.swf", "com.mapobject.asset.wordtip75");
            Game.AddLoadingFile(1, "bombs/61.swf", "tank.resource.bombs.Bomb61");

            Game.SetMap(1076);
            Game.IsBossWar = LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg1");
        }

        public override void OnPrepareStartGame()
        {
            base.OnPrepareStartGame();

            int i = 0;
            Point p;
            foreach (Player player in Game.GetAllFightPlayers())
            {
                player.Direction = 1;
                p = new Point((i + 1) * 100, player.Y - 10);
                player.SetXY(p);
                i++;
            }

        }
        public override void OnStartGame()
        {
            base.OnStartGame();
            Game.SendPassDrama(true);

            //再试一次跳过剧情动画
            if (Game.WantTryAgain == 1 || Game.IsPassDrama)
            {
                return;
            }

            Game.SendPlayBackgroundSound(false);
            Game.AddAction(new LockFocusAction(false, 0, 0));
            m_kingMoive = Game.Createlayer(0, 0, "kingmoive", "game.asset.living.BossBgAsset", "out", 1, 0);
            m_front = Game.Createlayer(725, 395, "font", "game.asset.living.BombKingAsset", "out", 1, 0);
            m_king = Game.CreateBoss(kingID, 888, 715, 1, 0);
            Game.AddAction(new FocusAction(m_king.X, m_king.Y - 90, 0, 0, 0));

            m_king.PlayMovie("cry", 4000, 2000);
            m_king.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg2"), 2, 4000);
            m_king.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg3"), 2, 9000);

            m_king.MoveTo(500, m_king.Y, "walk", 13000);
            m_king.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg4"), 2, 21000);
            m_king.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg5"), 2, 25000);
            m_king.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg6"), 2, 29000);
            m_king.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg7"), 2, 33000);
            m_king.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg8"), 2, 37000);
            m_king.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg9"), 2, 41000);
            m_king.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg10"), 2, 45000);
            m_king.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg11"), 2, 49000);
            m_king.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg12"), 2, 53000, 4000);

            foreach (Player player in Game.GetAllFightPlayers())
            {
                Game.SendLivingToTop(player);
            }
        }

        public override void OnPrepareNewGame()
        {
            base.OnPrepareNewGame();
            m_boss = Game.CreateBoss(bossID, 888, 590, -1, 0);     
            m_boss.SetRelateDemagemRect(-41, -187, 83, 140);
            m_boss.FallFrom(888, 690, "fall", 0, 2, 1000);
            m_front.PlayMovie("in", 2000, 0);
            m_kingMoive.PlayMovie("in", 1000, 0);
            m_kingMoive.PlayMovie("out", 4800, 0);
            m_boss.AddDelay(16);

            Game.BossCardCount = 1;

            //再试一次跳过剧情动画
            if (Game.WantTryAgain == 1 || Game.IsPassDrama)
            {
                Game.AddAction(new PlayBackgroundSoundAction(true, 0));
                if (m_king != null)
                {
                    m_king.Die();
                }
                return;
            }

            m_boss.SetRect(-191, -226, 44, 58);
            m_king.ChangeDirection(1, 5000);

            m_boss.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg13"), 2, 6000);
            m_boss.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg14"), 2, 10000);
            m_boss.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg15"), 2, 14000);
            m_boss.PlayMovie("cast", 18500, 0);
            m_boss.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg16"), 2, 18000);


            Game.AddAction(new FocusAction(m_king.X, m_king.Y - 30, 0, 22000, 0));
            m_king.ChangeDirection(-1, 22000);
            m_king.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg17"), 2, 23000);
            m_king.ChangeDirection(1, 25000);
            m_king.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg18"), 2, 27000);
            if (m_king.ShootPoint(m_boss.X, m_boss.Y, 61, 1000, 10000, 1, 1, 28300))
            {
                m_king.PlayMovie("beat2", 27500, 0);
            }
            m_king.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg19"), 2, 31000);

            Game.AddAction(new FocusAction(736, 515, 0, 29500, 0));

            m_boss.PlayMovie("beatA", 28300, 0);
            m_boss.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg20"), 2, 28500);
            m_boss.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg21"), 2, 36000);
            m_boss.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg22"), 2, 40000);

            m_boss.PlayMovie("mantra", 46000, 0);
            m_boss.Seal(m_king, 2, 48000);
            m_boss.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg23"), 2, 44000);

            m_king.ChangeDirection(-1, 50000);
            m_king.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg24"), 2, 51000);
            m_king.OffSeal(m_king, 55000);
            m_king.PlayMovie("out", 55000, 0);
            Game.AddAction(new PlayBackgroundSoundAction(true, 55000));
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
            if (m_boss.State == 0)
            {
                m_boss.SetRelateDemagemRect(-41, -187, 83, 140);
            }

            if (m_king != null)
            {
                Game.RemoveLiving(m_king.Id);
                m_king.Die();
                m_king = null;
                Game.AddAction(new LockFocusAction(true, 0, 0));
                m_boss.SetRect(m_boss.NpcInfo.X, m_boss.NpcInfo.Y, m_boss.NpcInfo.Width, m_boss.NpcInfo.Height);
            }

            if (!isSend)
            {
                Game.SendPassDrama(false);
                isSend = true;
            }
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();

            if (m_kingMoive != null)
            {
                Game.RemovePhysicalObj(m_kingMoive, true);
                m_kingMoive = null;
            }
            if (m_front != null)
            {
                Game.RemovePhysicalObj(m_front, true);
                m_front = null;
            }

            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving == false)
                    player.OffSeal(player, 0);
            }
        }

        public override bool CanGameOver()
        {
            if (m_boss.IsLiving == false && Game.GetAllLivingPlayers().Count != 0)
            {
                m_kill++;
                return true;
            }
            return false;

        }

        public override int UpdateUIData()
        {
            return m_kill;
        }

        public override void OnPrepareGameOver()
        {
            base.OnPrepareGameOver();

            if (m_boss.IsLiving == false && Game.GetAllLivingPlayers().Count != 0)
            {
                int i = 0;
                Point p;
                foreach (Player player in Game.GetAllFightPlayers())
                {
                    player.Direction = 1;
                    p = new Point((i + 1) * 100, 700);
                    player.SetXY(p);
                    i++;
                }
                m_king = Game.CreateBoss(kingID, 500, 650, -1, 0);
                m_king.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg25"), 2, 1000);
                m_king.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1377.msg26"), 2, 5000, 4000);
            }
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
                    break;
                }
            }
            if (m_boss.IsLiving == false && IsAllPlayerDie == false)
            {
                Game.IsWin = true;
            }
            else
            {
                Game.IsWin = false;
            }

            List<LoadingFileInfo> loadingFileInfos = new List<LoadingFileInfo>();
            loadingFileInfos.Add(new LoadingFileInfo(2, "image/map/8", ""));
            Game.SendLoadResource(loadingFileInfos);
        }
    }
}
