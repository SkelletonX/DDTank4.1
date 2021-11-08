using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Bussiness;


namespace GameServerScript.AI.NPC
{
    public class TrainingNormalNpc : ABrain
    {
        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            int toX = Game.Random.Next(585, 918);
            int dis = toX - Body.X;
            if (Math.Abs(dis) > 100)
            {
                if (dis > 0)
                {
                    toX = Body.X + 100;
                }
                else
                {
                    toX = Body.X - 100;
                }
            }
            else if (Math.Abs(dis) < 50)
            {
                if (dis > 0)
                {
                    toX = Body.X + 50;
                }
                else
                {
                    toX = Body.X - 50;
                }
            }
            Body.MoveTo(toX, Body.Y, "walk", 1000);
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }
    }
}
