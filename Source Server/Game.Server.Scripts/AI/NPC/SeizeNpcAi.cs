using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Effects;

namespace GameServerScript.AI.NPC
{
    class SeizeNpcAi : ABrain
    {
        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            Body.AddEffect(new NoHoleEffect(1), 0);
        }
    }
}
