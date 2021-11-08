using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using Game.Logic.Actions;
using System.Drawing;
using Bussiness;

namespace GameServerScript.AI.Messions
{
    public class THSM3003 : AMissionControl
    {
        private PhysicalObj m_kingMoive;

        private PhysicalObj m_kingFront;

        private PhysicalObj m_firstBossDead;

        private PhysicalObj m_seconBossDead;

        private List<SimpleBoss> longNpc = new List<SimpleBoss>();

        private SimpleBoss firstBoss = null;

        private SimpleBoss seconBoss = null;

        private int bossFrontID = 3008;

        private int bossSecondID = 3009;

        private int lockTotemID = 3011;

        private int FagTotemID = 3010;

        private int npcID = 3006;

        private bool updateBossCount = false;
        
        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 1870)
            {
                return 3;
            }
            else if (score > 1825)
            {
                return 2;
            }
            else if (score > 1780)
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
            int[] resources = { npcID, bossFrontID,lockTotemID,FagTotemID };
            int[] gameOverResources = { bossFrontID, bossFrontID };
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(gameOverResources);
            Game.AddLoadingFile(1, "bombs/58.swf", "tank.resource.bombs.Bomb58");
            Game.AddLoadingFile(1, "bombs/54.swf", "tank.resource.bombs.Bomb54");
            Game.AddLoadingFile(2, "image/map/1076/objects/1076MapAsset.swf", "com.mapobject.asset.WaveAsset_01_left");
            Game.AddLoadingFile(2, "image/map/1076/objects/1076MapAsset.swf", "com.mapobject.asset.WaveAsset_01_right");
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.ClanBrotherAsset");
            Game.AddLoadingFile(2, "image/map/1124/object/1124object.swf", "game.crazytank.assetmap.Dici");
            Game.AddLoadingFile(2, "image/map/1124/object/1124object2.swf", "game.crazytank.assetmap.Buff_powup");
            Game.AddLoadingFile(2, "image/map/1124/object/1124object3.swf", "game.crazyTank.view.Focus");
            
            Game.SetMap(1124);
            Game.IsBossWar = "萨满兄弟";
        }

        public override void OnPrepareStartGame()
        {
            base.OnPrepareStartGame();
        }
        public override void OnStartGame()
        {
            base.OnStartGame();
        }

        public override void OnPrepareNewGame()
        {
            base.OnPrepareNewGame();
            m_kingMoive = Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 0);
            m_kingFront = Game.Createlayer(630, 400, "font", "game.asset.living.ClanBrotherAsset", "out", 1, 0);

            seconBoss = Game.CreateBoss(bossSecondID, 1352, 300, -1, 1);
            firstBoss = Game.CreateBoss(bossFrontID, 241, 300, 1, 1);

            Game.AddAction(new ShowBloodItem(firstBoss.Id, 0, 0));
            Game.AddAction(new ShowBloodItem(seconBoss.Id, 7000, 0));
            seconBoss.FallFrom(1352, 346, "fall", 0, 2, 1000);
            firstBoss.FallFrom(241, 346, "fall", 0, 2, 1000);
            
            firstBoss.SetRelateDemagemRect(-12, -52, 26, 38);
            firstBoss.Degree = 1;
            firstBoss.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.THSM3003.msg1"), 0, 3000);
            firstBoss.PlayMovie("call",3000,0);
            firstBoss.AddDelay(16);

            Game.AddAction(new FocusAction(1352, 346, 0, 7000, 1000));

            seconBoss.SetRelateDemagemRect(-12, -52, 26, 38);
            seconBoss.Degree = 2;
            seconBoss.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.THSM3003.msg2"), 0, 9000);
            seconBoss.PlayMovie("castA", 9000, 0);
            Game.AddAction(new FocusAction(800, 500, 0, 13000, 1000));
            seconBoss.AddDelay(16);
            m_kingMoive.PlayMovie("in", 15000, 0);
            m_kingFront.PlayMovie("in", 15000, 0);
            m_kingMoive.PlayMovie("out", 20000, 0);
            m_kingFront.PlayMovie("out", 20400, 0);
        }


        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }
        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            if (m_kingMoive != null)
            {
                Game.RemovePhysicalObj(m_kingMoive, true);
                m_kingMoive = null;
            }

            if (m_kingFront != null)
            {
                Game.RemovePhysicalObj(m_kingFront, true);
                m_kingFront = null;
            }
        }

        public override bool CanGameOver()
        {
            base.CanGameOver();

            if ( !firstBoss.IsLiving && !seconBoss.IsLiving )
            {
                Game.SendUpdateUiData();
                Game.AddAction(new FocusAction(firstBoss.X, firstBoss.Y, 0, 2500, 1000));
                Game.AddAction(new LivingCallFunctionAction(null, CreateFirstBossDead, 4000));
                Game.AddAction(new FocusAction(seconBoss.X, seconBoss.Y, 0, 8000, 5000));
                Game.AddAction(new LivingCallFunctionAction(null, CreateSecondBossDead, 9500));
                Game.IsWin = true;
                return true;
            }

            return false;
        }

        public void CreateFirstBossDead()
        {
            m_firstBossDead = Game.CreatePhysicalObj(firstBoss.X, firstBoss.Y, "Dead", "game.crazyTank.view.Focus", "1", 1, 0);
        }

        public void CreateSecondBossDead()
        {
            m_seconBossDead = Game.CreatePhysicalObj(seconBoss.X, seconBoss.Y, "Dead", "game.crazyTank.view.Focus", "1", 1, 0);
        }

        public override int UpdateUIData()
        {
            int killcount = 0;
            List<TurnedLiving> living = Game.TurnQueue;
            foreach (TurnedLiving turnedLiving in living)
            {
                if (turnedLiving.IsLiving && (turnedLiving is SimpleBoss) && turnedLiving.Degree > 0)
                    killcount++;
            }
            if (killcount != 0)
            {
                killcount = 2 - killcount;
            }
            else if (!updateBossCount)
            {
                killcount = 0;
                updateBossCount = true;
            }
            else
            {
                killcount = 2;
            }
            return killcount;
        }

        public override void OnPrepareGameOver()
        {
            base.OnPrepareGameOver();
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            if ( !firstBoss.IsLiving && !seconBoss.IsLiving )
            {
                Game.IsWin = true;
            }
            else
            {
                Game.IsWin = false;
            }
            List<LoadingFileInfo> loadingFileInfos = new List<LoadingFileInfo>();
            loadingFileInfos.Add(new LoadingFileInfo(2, "image/map/2/show2", ""));
            Game.SendLoadResource(loadingFileInfos);
        }
    }
}
