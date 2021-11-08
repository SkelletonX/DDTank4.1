using Game.Logic.AI;

namespace GameServerScript.AI.Game
{
    public class Labyrinth : APVEGameControl
    {
        public override void OnCreated()
        {
            string missionIds = "40001,40002,40003,40004,40005,40006,40007,40008,40009,40010" + ",40011,40012,40013,40014,40015,40016,40017,40018,40019,40020" + ",40021,40022,40023,40024,40025,40026,40027,40028,40029,40030";
            this.Game.SetupMissions(missionIds);
            this.Game.TotalMissionCount = missionIds.Split(',').Length;
        }

        public override void OnPrepated()
        {
        }

        public override int CalculateScoreGrade(int score)
        {
            if (score > 800)
                return 3;
            if (score > 725)
                return 2;
            return score > 650 ? 1 : 0;
        }

        public override void OnGameOverAllSession()
        {
        }
    }
}
