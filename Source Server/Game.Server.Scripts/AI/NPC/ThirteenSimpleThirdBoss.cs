using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic.Effects;

namespace GameServerScript.AI.NPC
{
    public class ThirteenSimpleThirdBoss : ABrain
    {
        public int attackingTurn = 0;

        public int orchinIndex = 1;

        public int currentCount = 0;

        public int Dander = 0;

        public List<SimpleNpc> orchins = new List<SimpleNpc>();

        #region As falar do NPC
        private static string[] AllAttackChat = new string[]{
             "Olhe minhas acrobacias！",

             "Isso é legal.，<br/>Quer aprender？",

             "Desaparecer！！！<br/>Poeira humilde！",

             "Você pagará por isso.！ "
        };

        private static string[] ShootChat = new string[]{
             "Você está me marcando?？",

             "Não serei derrotado por você como o lixo.！",

             "Ei, você me machucou.，<br/>Hahahaha！",

             "A hora da verdade, este é o poder de ataque!",

             "É uma honra me ver.！"          
        };

        private static string[] CallChat = new string[]{
            "Vem cá，<br/>Deixe-os provar o poder da bomba！"                          
        };

        private static string[] AngryChat = new string[]{
            "Você me forçou a fazer um truque.！"                          
        };

        private static string[] KillAttackChat = new string[]{
            "Você vem para morrer?？"                          
        };

        private static string[] SealChat = new string[]{
            "Exílio exógeno！"                          
        };

        private static string[] KillPlayerChat = new string[]{
            "A destruição é o seu único destino!",

            "Muito vulnerável！"
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
            bool result = false;
            int maxdis = 0;
			Body.Direction = Game.FindlivingbyDir(Body);
            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving && player.X > 0 && player.X < 0)
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
                KillAttack(0, 0);
                return;
            }
            Body.DoAction = -1;
            if (attackingTurn == 0)
            {
                //Call Niutou
                attackingTurn++;
            }			
			else if (attackingTurn == 1)
            {
                Jump();
                attackingTurn++;
            }
            if (attackingTurn == 2)
            {
                //Call Niutou
                attackingTurn++;
            }			
            else
            {
                //run, jump back and remove box
                Run();
                attackingTurn = 0;
            }
           
            
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }
        public void CallNiutou()
        {            
            Player target = Game.FindRandomPlayer();

        }
		public void Jump()
        {
			Body.PlayMovie("jump", 1000, 6000);	
			Player target = Game.FindRandomPlayer();
			Body.JumpToSpeed(target.X, Body.Y - 1000, "", 2500, 1, 10, new LivingCallBack(fall));
        }
		
		public void fall()
        {
            Body.CurrentDamagePlus = 3;
			Body.PlayMovie("fall", 0, 0);
            Body.RangeAttacking(0, Game.Map.Info.ForegroundWidth + 1, "cry", 0, null);
        }
				
		private void StandC()
        {
            Body.PlayMovie("standC", 0, 0);
            Body.DoAction = 5;
        }
		
		private void Run()
        {
            Body.CurrentDamagePlus = 5;
            int dis = Game.Random.Next(1800, 1800);
            Body.MoveTo(dis, Body.Y, "walk", 1000, "", 25, JumpBack);
            Body.RangeAttacking(0, Game.Map.Info.ForegroundWidth + 1, "cry", 1000, null);
        }
        public void JumpBack()
        {
            Body.Direction = Game.FindlivingbyDir(Body);
            Body.PlayMovie("jump", 1000, 6000);	
            Player target = Game.FindRandomPlayer();
            Body.JumpToSpeed(target.X, Body.Y - 1000, "", 2500, 1, 10, new LivingCallBack(FallBack));
        }
        public void FallBack()
        {
            Body.PlayMovie("fall", 0, 0);
            Body.Direction = Game.FindlivingbyDir(Body);
        }
				
		private void KillAttack(int fx, int tx)
        {
            Body.CurrentDamagePlus = 10;
			Body.Direction = Game.FindlivingbyDir(Body);
            int index = Game.Random.Next(0, KillAttackChat.Length);
            Body.Say(KillAttackChat[index], 1, 1000);
            Body.PlayMovie("beat", 3000, 0);
            Body.RangeAttacking(fx, tx, "cry", 4000, null);
        }		
    }
}
