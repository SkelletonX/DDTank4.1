using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic.Actions;
using Game.Logic.Effects;
using Game.Logic;
using System.Drawing;
using Bussiness;

namespace GameServerScript.AI.NPC
{
    public class ThirdSimpleKingFour : ABrain
    {
        private int m_attackTurn = 0;

        private SimpleBoss m_Bloom = null;

        private int m_isSay = 0;

        private int isSay = 0;

        private int m_BloomID = 3018;

        private int m_attackCount = 4;

        private int m_bloomCreateDir = 0;

        private List<PhysicalObj> m_Physical = new List<PhysicalObj>();

        private Point[] bloomLeftPoint = { new Point(188, 567), new Point(257, 571), new Point(329, 573), new Point(401, 558), new Point(474, 545) };

        private Point[] bloomRightPoint = { new Point(989, 546), new Point(1060, 545), new Point(1130, 551), new Point(1207, 567), new Point(1288, 565) };

        #region NPC 说话内容

        private static string[] AllBakeChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFour.msg1"),
        };
        private static string[] BurningChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFour.msg2"),
        };
        private static string[] ShootChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFour.msg6"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFour.msg7"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFour.msg8"),
        };
        private static string[] KillAttackChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFour.msg21"),
        };
        private static string[] AllAttackChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFour.msg3"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFour.msg4"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFour.msg5"),
        };
        private static string[] AllCarryChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFour.msg9"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFour.msg10"),
        };
        private static string[] KillBloomChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFour.msg11"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFour.msg12"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFour.msg13"),
        };
        private static string[] KillPlayerChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFour.msg14"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFour.msg15"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFour.msg16"),
        };
        private static string[] DieChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFour.msg17"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFour.msg18"),
        };
        private static string[] ShooteChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFour.msg20"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFour.msg201"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFour.msg202"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFour.msg203"),
        };
        private static string[] BloomReliveChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleBloomNpcS.msg6"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleBloomNpcS.msg7"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleBloomNpcS.msg8"),
        };

        #endregion

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            isSay = 0;
            m_body.CurrentDamagePlus = 1;
            m_body.CurrentShootMinus = 1;
        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            Body.Direction = Game.FindlivingbyDir(Body);
            bool result = false;
            int maxdis = 0;
            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving && player.X > Body.X - 100 && player.X < Body.X + 100)
                {
                    int dis = (int)Body.Distance(player.X, player.Y);
                    if (dis > maxdis)
                    {
                        maxdis = dis;
                    }
                    result = true;
                }
            }

            if (m_attackTurn == 0)
            {
                int index = Game.Random.Next(0, AllBakeChat.Length);
                Body.Say(AllBakeChat[index], 0, 1500, 3000);
                Body.CallFuction(new LivingCallBack(AllBakeAttack), 3500);
                Body.PlayMovie("", 9000, 0, new LivingCallBack(CreateBloomS));
                List<Living> listliving = Game.GetLivedLivings();
                foreach (Living living in listliving)
                {
                    if (!(living is Player) && !(living is SimpleBoss))
                    {
                        living.PlayMovie("die", 7000, 0);
                        living.Die(8500);
                    }
                }
                m_attackTurn++;
                return;
            }
            if (result)
            {
                KillAttack(Body.X - 100, Body.X + 100);
                return;
            }
            if (m_attackTurn == 1)
            {
                NextAttack();
                m_attackTurn++;
                return;
            }
            if (m_attackTurn == 2)
            {
                AllCarry();
                m_attackTurn++;
                return;
            }
            if (m_attackTurn == 3)
            {
                AllAttack();
                m_attackTurn++;
                return;
            }
            if (m_attackTurn == 4)
            {
                ClearBloom();
                m_attackTurn++;
                return;
            }
            if (m_attackTurn == 5)
            {
                AllAttack();
                m_attackTurn++;
                return;
            }
            if (m_attackTurn == 6)
            {
                AllBakeAttack();
                m_attackTurn = 1;
                return;
            }
        }

        private void KillAttack(int fx, int tx)
        {
            Body.CurrentDamagePlus = 20;
            int index = Game.Random.Next(0, KillAttackChat.Length);
            Body.Say(KillAttackChat[index], 1, 1500);
            Body.PlayMovie("beatB", 3000, 0);
            Body.RangeAttacking(fx, tx, "cry", 5000, null);
        }

        private void NextAttack()
        {
            int index = Game.Random.Next(645, 845);
            Body.MoveTo(index, Body.Y, "walk", 2000, new LivingCallBack(PersonalAttack));
        }

        private void PersonalAttack()
        {
            Body.CurrentDamagePlus = 0.4f;
            Player target = Game.FindNearestPlayer(Body.X, Body.Y);
            if (target != null)
            {
                int shootDelay = 4350;
                int movieDelay = 4200;
                int index = Game.Random.Next(0, ShootChat.Length);
                Body.Say(ShootChat[index], 1, 1500);
                Body.PlayMovie("aim", 2500, 0);
                if (target.X > Body.X)
                {
                    Body.ChangeDirection(1, 0);
                }
                else
                {
                    Body.ChangeDirection(-1, 0);
                }
                for (int i = 0; i < m_attackCount; i++)
                {
                    AttackPlayer(target, shootDelay, movieDelay, true);
                    shootDelay += 1600 - i * 80;
                    movieDelay += 1500;
                }
            }
        }

        private void AttackPlayer(Living player, int ShootDelay, int MovieDelay,bool Range )
        {
            if (player.X > Body.X)
            {
                Body.ChangeDirection(1, 800 + MovieDelay - 1500);
            }
            else
            {
                Body.ChangeDirection(-1, 800 + MovieDelay - 1500);
            }

            int mtX = Game.Random.Next(player.X - 30, player.X + 30);

            if (!Range)
            {
                if (Body.ShootPoint(mtX, player.Y, 53, 1000, 10000, 1, 1, ShootDelay))
                {
                    Body.PlayMovie("beatA", MovieDelay, 1000);
                }
            }
            else
            {
                float time = 1.0f;
                if (Body.X - player.X > 0)
                {
                    time = 1.8f;
                }

                if (Body.ShootPoint(mtX, player.Y, 53, 1000, 10000, 3, time, ShootDelay))
                {
                    Body.PlayMovie("beatA", MovieDelay, 1000);
                }
            }
        }

        private void AllBakeAttack()
        {
            int index = Game.Random.Next(0, BurningChat.Length);
            Body.Say(BurningChat[index], 0, 1000, 3000);
            Body.PlayMovie("beatC", 1500, 0);
            Body.PlayMovie("", 3500, 0, new LivingCallBack(BakeAttack));
        }

        private void BakeAttack()
        {
            List<Player> players = Game.GetAllLivingPlayers();
            foreach (Player player in players)
            {
                if (player.IsLiving)
                {
                    player.AddEffect(new ContinueReduceBloodEffect(3, 400, null), 0);
                    PhysicalObj obj = ((PVEGame)Body.Game).CreatePhysicalObj(player.X - 20, 500, "Flame", "game.assetmap.Flame", "1", 1, 0);
                    m_Physical.Add(obj);
                }
            }
            Body.CallFuction(new LivingCallBack(ClearFlame), 1800);
        }

        private void AllAttack()
        {
            Body.CurrentDamagePlus = 0.8f;
            int index = Game.Random.Next(0, AllAttackChat.Length);
            Body.Say(AllAttackChat[index], 1, 1500);
            Body.PlayMovie("beatB", 3500, 0);
            Body.RangeAttacking(-1, Game.Map.Info.ForegroundWidth + 1, "cry", 5500, null);
        }

        private void AllCarry()
        {
            int index = Game.Random.Next(0, AllCarryChat.Length);
            Body.Say(AllCarryChat[index], 1, 1500);
            Body.PlayMovie("beatC", 3500, 1000);
            Body.CallFuction(new LivingCallBack(BeginCarry), 4800);
            Body.CallFuction(new LivingCallBack(ClearFlame), 5800);
        }

        private void BeginCarry()
        {
            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving)
                {
                    int x = Game.Random.Next(50, 1400);
                    PhysicalObj obj = ((PVEGame)Body.Game).CreatePhysicalObj(x - 20, 500, "Flame", "game.assetmap.Flame", "1", 1, 0);
                    m_Physical.Add(obj);
                    ((PVEGame)Body.Game).AddAction(new LivingBoltMoveAction(player, x, 500, "", 0, 0));
                }
            }
            Body.CurrentDamagePlus = 0.8f;
            Body.RangeAttacking(-1, Game.Map.Info.ForegroundWidth + 1, "cry", 0, null);
        }

        private void ClearFlame()
        {
            PhysicalObj[] physics = Game.FindPhysicalObjByName("Flame");
            foreach (PhysicalObj p in physics)
            {
                Game.RemovePhysicalObj(p, true);
            }
            foreach (Player player in Game.GetAllFightPlayers())
            {
                player.StartFalling(false);
            }
        }

        private void ClearBloom()
        {
            if (m_Bloom.X > Body.X)
            {
                Body.ChangeDirection(1, 0);
            }
            else
            {
                Body.ChangeDirection(-1, 0);
            }
            int index = Game.Random.Next(0, KillBloomChat.Length);
            Body.Say(KillBloomChat[index], 1, 1500);
            Body.CallFuction(new LivingCallBack(BeginClearBloom), 3500);
        }

        private void BeginClearBloom()
        {
            if (m_Bloom != null)
            {
                Body.CurrentDamagePlus = 0.1f;
                AttackPlayer(m_Bloom, 1650, 1500, false);
                m_Bloom.PlayMovie("die", 1200, 2000);
                Body.CallFuction(new LivingCallBack(DieBloomS), 2500);
                Body.CallFuction(new LivingCallBack(CreateBloomS), 4000);
                return;
            }
            Body.CallFuction(new LivingCallBack(NextAttack), 0);
        }

        private void DieBloomS()
        {
            m_Bloom.Die(); 
        }

        private void CreateBloomS()
        {
            int x;
            if (m_bloomCreateDir == 0)
            {
                m_bloomCreateDir = 1;
                x = Game.Random.Next(0, bloomLeftPoint.Length);
                Body.ChangeDirection(-1, 0);
                m_Bloom = ((PVEGame)Game).CreateBoss(m_BloomID, bloomLeftPoint[x].X, bloomLeftPoint[x].Y, 1,3,100);
                if (m_isSay == 0)
                {
                    m_isSay = 1;
                    Game.AddAction(new FocusAction(m_Bloom.X, m_Bloom.Y, 0, 0, 1000));
                    m_Bloom.Say(LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFour.msg19"), 0, 2000, 3000);
                }
                else
                {
                    int index = Game.Random.Next(0, BloomReliveChat.Length);
                    m_Bloom.Say(BloomReliveChat[index], 0, 2000, 3000);
                }
            }
            else
            {
                m_bloomCreateDir = 0;
                x = Game.Random.Next(0, bloomRightPoint.Length);
                Body.ChangeDirection(1, 0);
                m_Bloom = ((PVEGame)Game).CreateBoss(m_BloomID, bloomRightPoint[x].X, bloomRightPoint[x].Y, -1, 3,100);
                int index = Game.Random.Next(0, BloomReliveChat.Length);
                m_Bloom.Say(BloomReliveChat[index], 0, 2000, 3000);
            }
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        public override void OnKillPlayerSay()
        {
            base.OnKillPlayerSay();
            int index = Game.Random.Next(0, KillPlayerChat.Length);
            Body.Say(KillPlayerChat[index], 1, 0);
        }

        public override void OnDiedSay()
        {
            int index = Game.Random.Next(0, DieChat.Length);
            Body.Say(DieChat[index], 1, 1000, 5000);
        }

        public override void OnShootedSay(int delay)
        {
            if (isSay == 0 && Body.IsLiving == true)
            {
                int index = Game.Random.Next(0, ShooteChat.Length);
                Body.Say(ShooteChat[index], 1, delay);
                isSay = 1;
            }
        }
    }
}
