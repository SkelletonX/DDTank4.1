using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using System.Drawing;
using Game.Logic.Actions;
using Bussiness;


namespace GameServerScript.AI.NPC
{
    public class SeventhHardFirstBoss : ABrain
    {
        private int m_attackTurn = 0;

        private bool m_openShield = false;

        private List<PhysicalObj> moives = new List<PhysicalObj>();

        private Player target;

        #region NPC 说话内容

        private static string[] BeatSay = new string[]{
            "Eu impeço, impeço, impeço!",
            "Que dor, não bagunça minhas penas!!!!!",
            "Eu transmitirei minhas habilidades, vingue-se por mim! ."
        };

        private static string[] BeatASay = new string[]{
            "Eu não aguento~partiu ",
            "Se tá querendo nos ultrapassar saiba que não será moleza!!",
            "Se vocês correrem agora dá tempo! ."
        };

        private static string[] BeatBSay = new string[]{
            "Ficar tão perto de mim é a sua escolha mais errada! ",
            "Com certeza vocês vão ser aniquilados!!!!",
            "Saia daqui agora!"
        };

        private static string[] KillSay = new string[]{
            "O que você está fazendo aqui?",
            "Você está com doença.",
        };

        private static string[] DieSay = new string[]{
            "Venha, venha",
            "Estou com medo",
            "É muito forte para você sair",
            "As peras ficam saudáveis e escapam.",
            "Morto, muito ruim. Fonte aberta!!!!"
        };

        #endregion

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            if (m_openShield)
                Body.Config.HaveShield = true;
            else
                Body.Config.HaveShield = false;

            Body.CurrentDamagePlus = 1;
            Body.CurrentShootMinus = 1;

            RemoveMovie();
        }

        public override void OnCreated()
        {
            base.OnCreated();
            Body.Properties1 = 0;
        }

        public override void OnDie()
        {
            base.OnDie();
            Body.Properties1 = 0;
            ((SimpleBoss)Body).RandomSay(DieSay, 0, 500, 2000);
        }

        public override void OnStartAttacking()
        {
            Body.Direction = Game.FindlivingbyDir(Body);
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

            // check can attack
            if ((int)Body.Properties1 == 0 && m_openShield == false)
            {
                m_openShield = true;
                ChangeToA();
            }
            else if ((int)Body.Properties1 == 1 && m_openShield == true)
            {
                m_openShield = false;
                ChangeToNomal(new LivingCallBack(AttackSkill));
            }
            else if ((int)Body.Properties1 == 1)
            {
                AttackSkill();
            }
        }

        private void ChangeToA()
        {
            Body.PlayMovie("toA", 1500, 3500);
        }

        private void ChangeToNomal(LivingCallBack callBack)
        {
            Body.PlayMovie("Ato", 1500, 3000);
            if (callBack != null)
                Body.CallFuction(callBack, 3500);
        }

        private void AttackSkill()
        {
            m_attackTurn++;
            switch (m_attackTurn)
            {
                case 1:
                    AllAttackPlayer();
                    break;
                case 2:
                    PersonAttack();
                    break;
                case 3:
                    PersonAttack2();
                    m_attackTurn = 0;
                    break;
            }
            Body.Properties1 = 0;
        }

        private void AllAttackPlayer()
        {
            Body.CurrentDamagePlus = 2f;
            ((SimpleBoss)Body).RandomSay(BeatBSay, 0, 500, 0);
            Body.PlayMovie("beatB", 1000, 0);
            Body.CallFuction(new LivingCallBack(GoMovie), 4000);
            Body.RangeAttacking(Body.X - 10000, Body.Y + 10000, "cry", 4500, null);
        }

        private void PersonAttack()
        {
            target = Game.FindRandomPlayer();

            ((SimpleBoss)Body).RandomSay(BeatSay, 0, 500, 0);
            if (Body.ShootPoint(target.X, target.Y, 84, 1200, 10000, 1, 3.0f, 2000))
            {
                Body.PlayMovie("beat", 1000, 0);
            }
        }

        private void PersonAttack2()
        {
            target = Game.FindRandomPlayer();

            ((SimpleBoss)Body).RandomSay(BeatASay, 0, 500, 0);
            if (Body.ShootPoint(target.X, target.Y, 84, 1200, 10000, 2, 3.0f, 2000))
            {
                Body.PlayMovie("beatA", 1000, 0);
            }
        }

        private void GoMovie()
        {
            foreach (Player p in Game.GetAllLivingPlayers())
            {
                moives.Add(((PVEGame)Game).Createlayer(p.X, p.Y, "moive", "asset.game.seven.cao", "in", 1, 0));
            }
        }

        private void RemoveMovie()
        {
            foreach (PhysicalObj phy in moives)
            {
                if (phy != null)
                    Game.RemovePhysicalObj(phy, true);
            }

            moives = new List<PhysicalObj>();
        }

        private void KillAttack(int fx, int tx)
        {
            Body.CurrentDamagePlus = 1000f;
            ((SimpleBoss)Body).RandomSay(KillSay, 0, 500, 2000);
            Body.PlayMovie("beatB", 1000, 0);
            Body.RangeAttacking(fx, tx, "cry", 3000, null);
        }


        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }
    }
}
