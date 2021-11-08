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
    public class FourTerrorCattleBoss : ABrain
    {
        private int m_attackTurn = 0;

        private PhysicalObj m_effectAttack = null;

        private PhysicalObj m_powerUpEffect = null;

        private int m_totalNpc = 3;

        private int npcId = 4307;

        private float m_perPowerUp = 0.8f;

        private int m_reduceStreng = 150;

        private bool IsFear = false;

        private Player target = null;

        private float lastpowDamage = 0;

        private float m_currentPowDamage = 0;

        private int isSay = 0;

        private string[] CallNpcSay =
        {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg2"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg3")
        };

        private string[] KillAttackSay =
        {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg4"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg5"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg6"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg7")
        };

        private string[] AllAttackPlayerSay =
        {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg8"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg9"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg10"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg11")
        };

        private string[] TiredSay =
        {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg12"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg13"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg14")
        };

        private string[] FearSay =
        {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg15"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg16"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg17")
        };

        private string[] OnShootedChat =
        {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg18"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg19"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg20"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg21"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg22"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg23"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg24"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg25"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg26"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg27"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg28"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg29")
        };

        private string[] SpecialAttackSay =
        {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg30"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg31"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg32"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg33")
        };

        private string[] DiedChat =
        {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg34")
        };

        private string[] KillPlayerChat =
        {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg35"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg36"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg37"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourTerrorCattleBoss.msg38")
        };

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();

            ClearEffect();

            /*if(lastpowDamage > 0)
            {
                Body.CurrentDamagePlus = lastpowDamage;
                lastpowDamage = 0;
            }*/
            //Body.CurrentDamagePlus = 1;
            Body.CurrentShootMinus = 1;
            isSay = 0;

            if (IsFear == false)
                Body.Config.HaveShield = true;
            else
                Body.Config.HaveShield = false;
        }

        public override void OnCreated()
        {
            base.OnCreated();
            IsFear = false;
            Body.CurrentDamagePlus = 1;
            m_currentPowDamage = 1;
        }

        public override void OnStartAttacking()
        {
            // check near player
            if (!IsFear)
            {
                bool result = false;

                foreach (Player player in Game.GetAllFightPlayers())
                {
                    if (player.IsLiving && player.X > Body.X - 200 && player.X < Body.X + 200)
                    {
                        result = true;
                    }
                }

                if (result)
                {
                    KillAttack(Body.X - 200, Body.X + 200);
                    return;
                }
            }

            if (m_attackTurn == 0)
            {
                if (!IsFear)
                {
                    // up power and call npc
                    Body.CallFuction(new LivingCallBack(PowerUpEffect), 2000);
                    if (((SimpleBoss)Body).CurrentLivingNpcNum <= 0)
                    {
                        int index = Game.Random.Next(0, CallNpcSay.Length);
                        Body.Say(CallNpcSay[index], 0, 5000);
                        Body.CallFuction(new LivingCallBack(CallNpc), 7000);
                    }
                    else
                    {
                        Body.CallFuction(new LivingCallBack(AttackAllPlayer), 4000);
                    }
                }
                m_attackTurn++;
            }
            else if (m_attackTurn == 1)
            {
                if (!IsFear)
                {
                    // up power and attack near player
                    Body.CallFuction(new LivingCallBack(PowerUpEffect), 2000);
                    Body.CallFuction(new LivingCallBack(AttackPerson), 4000);
                    m_attackTurn++;
                }
                else
                {
                    m_attackTurn = 3;
                }
            }
            else if (m_attackTurn == 2)
            {
                if (!IsFear)
                {
                    // up power and attack all player
                    Body.CallFuction(new LivingCallBack(PowerUpEffect), 2000);
                    Body.CallFuction(new LivingCallBack(AttackAllPlayer), 4000);
                }
                m_attackTurn++;
            }
            else if (m_attackTurn == 3)
            {
                if (!IsFear)
                {
                    // up power and fear (only if not have any npc)
                    Body.CallFuction(new LivingCallBack(PowerUpEffect), 2000);
                    if (((SimpleBoss)Body).CurrentLivingNpcNum <= 0)
                    {
                        // fear
                        IsFear = true;
                        int index = Game.Random.Next(0, TiredSay.Length);
                        Body.Say(TiredSay[index], 0, 2200);
                        Body.CallFuction(new LivingCallBack(ChangeAtoB), 4000);
                    }
                    else
                    {
                        Body.CallFuction(new LivingCallBack(JumpAndAttack), 4000);
                    }
                }
                else
                {
                    // wake up
                    IsFear = false;
                    m_currentPowDamage = 1;
                    Body.CallFuction(new LivingCallBack(ChangeBtoA), 2000);
                }
                m_attackTurn = 0;
            }
        }

        private void KillAttack(int fx, int tx)
        {
            lastpowDamage = Body.CurrentDamagePlus;

            Body.CurrentDamagePlus = 1000f;

            Body.ChangeDirection(Game.FindlivingbyDir(Body), 100);

            ((SimpleBoss)Body).RandomSay(KillAttackSay, 0, 2000, 0);
            Body.PlayMovie("beatC", 2000, 0); //3s
            Body.PlayMovie("beatE", 5000, 0);
            Body.RangeAttacking(fx, tx, "cry", 7000, null);
            Body.CallFuction(new LivingCallBack(SetState), 8000);
        }

        private void AttackPerson()
        {
            target = Game.FindNearestPlayer(Body.X, Body.Y);
            if (target != null)
            {
                Body.ChangeDirection(target, 100);
                int index = Game.Random.Next(0, KillAttackSay.Length);
                Body.Say(KillAttackSay[index], 0, 1000);
                Body.PlayMovie("beatA", 1200, 0);

                ((PVEGame)Game).SendObjectFocus(target, 1, 3200, 0);
                Body.CallFuction(new LivingCallBack(CreateAttackEffect), 4000);
                if (Body.FindDirection(target) == -1)
                    Body.RangeAttacking(target.X - 50, Body.X, "cry", 4800, null);
                else
                    Body.RangeAttacking(Body.X, target.X + 50, "cry", 4800, null);
                Body.CallFuction(new LivingCallBack(SetState), 6000);
            }
        }

        private void AttackAllPlayer()
        {
            ((SimpleBoss)Body).RandomSay(AllAttackPlayerSay, 0, 1000, 0);
            Body.PlayMovie("beatB", 1000, 0);
            Body.RangeAttacking(Body.X - 10000, Body.X + 10000, "cry", 4100, null);
            foreach (Player p in Game.GetAllLivingPlayers())
            {
                p.AddEffect(new ReduceStrengthEffect(1, m_reduceStreng), 4200);
            }
            Body.CallFuction(new LivingCallBack(SetState), 5000);
        }

        private void ChangeAtoB()
        {
            Body.PlayMovie("beatD", 1000, 0);
            ((SimpleBoss)Body).RandomSay(FearSay, 0, 4100, 0);
            Body.PlayMovie("AtoB", 4000, 0);
            Body.CallFuction(new LivingCallBack(SetState), 7000);
        }

        private void ChangeBtoA()
        {
            Body.PlayMovie("AtoB", 1000, 0);
            ((SimpleBoss)Body).RandomSay(SpecialAttackSay, 0, 2200, 0);
            Body.CallFuction(new LivingCallBack(JumpAndAttack), 2000);
        }

        private void JumpAndAttack()
        {
            target = Game.FindRandomPlayer();

            if (target != null)
            {
                Body.PlayMovie("jump", 500, 0);

                ((PVEGame)Game).SendObjectFocus(target, 1, 2000, 0);

                Body.BoltMove(target.X, target.Y, 2500);

                Body.PlayMovie("fall", 2600, 0);

                Body.RangeAttacking(target.X - 100, target.X + 100, "cry", 3000, null);

                Body.CallFuction(new LivingCallBack(SetState), 4000);
            }
        }

        private void PowerUpEffect()
        {
            m_currentPowDamage += m_perPowerUp;
            Body.CurrentDamagePlus = m_currentPowDamage;
            // power up
            m_powerUpEffect = ((PVEGame)Game).Createlayer(Body.X, Body.Y - 60, "", "game.crazytank.assetmap.Buff_powup", "", 1, 0);
        }

        private void CreateAttackEffect()
        {
            if (target != null)
            {
                m_effectAttack = ((PVEGame)Game).Createlayer(target.X, target.Y, "", "asset.game.4.blade", "", 1, 0);
            }
        }

        private void ClearEffect()
        {
            if (m_powerUpEffect != null)
                Game.RemovePhysicalObj(m_powerUpEffect, true);

            if (m_effectAttack != null)
                Game.RemovePhysicalObj(m_effectAttack, true);
        }
        private void CallNpc()
        {
            LivingConfig config = ((PVEGame)Game).BaseLivingConfig();
            config.IsFly = true;

            for (int i = 0; i < m_totalNpc; i++)
            {
                int randX = Game.Random.Next(350, 1300);
                int randY = Game.Random.Next(100, 700);
                ((SimpleBoss)Body).CreateChild(npcId, randX, randY, 0, -1, true, config);
            }
        }

        private void RemoveAllNpc()
        {
            ((SimpleBoss)Body).RemoveAllChild();
        }

        private void SetState()
        {
            if (IsFear)
            {
                ((PVEGame)Game).SendLivingActionMapping(Body, "stand", "standB");
            }
            else
            {
                ((PVEGame)Game).SendLivingActionMapping(Body, "stand", "standA");
            }
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        public override void OnShootedSay(int delay)
        {
            base.OnShootedSay(delay);
            int index = Game.Random.Next(0, OnShootedChat.Length);
            if (isSay == 0 && Body.IsLiving == true)
            {
                Body.Say(OnShootedChat[index], 0, 1000 + delay, 0);
                isSay = 1;
            }
        }

        public override void OnKillPlayerSay()
        {
            base.OnKillPlayerSay();
            ((SimpleBoss)Body).RandomSay(KillPlayerChat, 0, 0, 2000);
        }

        public override void OnDiedSay()
        {
            base.OnDiedSay();
            int index = Game.Random.Next(0, DiedChat.Length);
            Body.Say(DiedChat[index], 1, 0, 1500);
        }

    }
}
