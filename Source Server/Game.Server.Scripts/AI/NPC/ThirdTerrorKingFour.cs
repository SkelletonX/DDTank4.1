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
    public class ThirdTerrorKingFour : ABrain
    {
        private int m_attackTurn = 0;

        private SimpleBoss m_Bloom = null;

        private int m_isSay = 0;

        private int isSay = 0;

        private int m_BloomID = 3318;

        private int m_attackCount = 4;

        private int m_bloomCreateDir = 0;

        private List<PhysicalObj> m_Physical = new List<PhysicalObj>();

        private Point[] bloomLeftPoint = { new Point(474, 545) };

        private Point[] bloomRightPoint = { new Point(1035, 547) };

        #region NPC 说话内容

        private static string[] AllBakeChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingFour.msg1"),
        };
        private static string[] BurningChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingFour.msg2"),
        };
        private static string[] ShootChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingFour.msg6"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingFour.msg7"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingFour.msg8"),
        };
        private static string[] KillAttackChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingFour.msg21"),
        };
        private static string[] AllAttackChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingFour.msg3"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingFour.msg4"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingFour.msg5"),
        };
        private static string[] AllCarryChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingFour.msg9"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingFour.msg10"),
        };
        private static string[] KillBloomChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingFour.msg11"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingFour.msg12"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingFour.msg13"),
        };
        private static string[] KillPlayerChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingFour.msg14"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingFour.msg15"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingFour.msg16"),
        };
        private static string[] DieChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingFour.msg17"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingFour.msg18"),
        };
        private static string[] ShooteChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingFour.msg20"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingFour.msg201"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingFour.msg202"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingFour.msg203"),
        };
        private static string[] BloomReliveChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorBloomNpcS.msg6"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorBloomNpcS.msg7"),
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorBloomNpcS.msg8"),
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

            if (m_attackTurn == 0)
            {
                int index = Game.Random.Next(0, AllBakeChat.Length);
                Body.Say(AllBakeChat[index], 0, 1500, 3000);
                Body.MoveTo(750, Body.Y, "walk", 4000, new LivingCallBack(AllBakeAttack));
                Body.PlayMovie("", 11000, 0, new LivingCallBack(CreateBloomS));
                List<Living> listliving = Game.GetLivedLivings();
                foreach (Living living in listliving)
                {
                    if (!(living is Player) && !(living is SimpleBoss))
                    {
                        living.PlayMovie("die", 9000, 0);
                        living.Die(10500);
                    }
                }
                m_attackTurn++;
                int delay = GetMaxDelay();
                ((SimpleBoss)Body).Delay = delay + 100;
                return;
            }

            if (IsKillAttack())
                return;

            if (m_attackTurn == 1)
            {
                PersonalAttack();
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


        public int GetMaxDelay()
        {
            List<Player> players = Game.GetAllFightPlayers();

            int maxDelay = 0;

            foreach (Player player in players)
            {
                if (player.Delay > maxDelay)
                {
                    maxDelay = player.Delay;
                }
            }
            return maxDelay;
        }

        private bool IsKillAttack()
        {
            bool result = false;
            int maxdis = 0;
            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving && player.X > Body.X - 150 && player.X < Body.X + 150)
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
                KillAttack(Body.X - 150, Body.X + 150);
                return true;
            }
            return false;
        }

        private void KillAttack(int fx, int tx)
        {
            Body.CurrentDamagePlus = 20;
            int index = Game.Random.Next(0, KillAttackChat.Length);
            Body.Say(KillAttackChat[index], 1, 1500);
            Body.PlayMovie("beatB", 3000, 0);
            Body.RangeAttacking(fx, tx, "cry", 5000, null);
        }

        private void PersonalAttack()
        {
            if (IsKillAttack())
                return;
            Body.CurrentDamagePlus = 0.25f;
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

        private void AttackPlayer(Living player, int ShootDelay, int MovieDelay, bool Range)
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
                if (Body.X - player.X > 271)
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
                    player.AddEffect(new ContinueReduceBloodEffect(3, 1000, null), 0);
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
            Body.CurrentDamagePlus = 0.1f;
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
            Body.CallFuction(new LivingCallBack(PersonalAttack), 0);
        }

        private void DieBloomS()
        {
            m_Bloom.Die();
        }

        private void CreateBloomS()
        {
            int x;
            LivingConfig config = ((PVEGame)Game).BaseLivingConfig();
            config.IsHelper = true;
            config.CanTakeDamage = false;
            if (m_bloomCreateDir == 0)
            {
                m_bloomCreateDir = 1;
                x = Game.Random.Next(0, bloomLeftPoint.Length);
                Body.ChangeDirection(-1, 0);
                m_Bloom = ((PVEGame)Game).CreateBoss(m_BloomID, bloomLeftPoint[x].X, bloomLeftPoint[x].Y, 1, 3, 100, "", config);
                if (m_isSay == 0)
                {
                    m_isSay = 1;
                    Game.AddAction(new FocusAction(m_Bloom.X, m_Bloom.Y, 0, 0, 1000));
                    m_Bloom.Say(LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingFour.msg19"), 0, 2000, 3000);
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
                m_Bloom = ((PVEGame)Game).CreateBoss(m_BloomID, bloomRightPoint[x].X, bloomRightPoint[x].Y, -1, 3, 100, "", config);
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
        }

        public override void OnShootedSay(int delay)
        {
            if (isSay == 0 && Body.IsLiving == true)
            {
                int index = Game.Random.Next(0, ShooteChat.Length);
                Body.Say(ShooteChat[index], 1, delay);
                isSay = 1;
            }

            if (!Body.IsLiving && isSay == 0)
            {
                int index = Game.Random.Next(0, DieChat.Length);
                Body.Say(DieChat[index], 1, delay - 800, 5000);
            }
        }
    }
}
