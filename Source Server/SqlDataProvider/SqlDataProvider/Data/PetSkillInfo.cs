// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.PetSkillInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
    public class PetSkillInfo
    {
        public void Clone(PetSkillInfo _clone)
        {
            this.ID = _clone.ID;
            this.Name = _clone.Name;
            this.ElementIDs = _clone.ElementIDs;
            this.Description = _clone.Description;
            this.BallType = _clone.BallType;
            this.NewBallID = _clone.NewBallID;
            this.CostMP = _clone.CostMP;
            this.Pic = _clone.Pic;
            this.Action = _clone.Action;
            this.EffectPic = _clone.EffectPic;
            this.Delay = _clone.Delay;
            this.ColdDown = _clone.ColdDown;
            this.GameType = _clone.GameType;
            this.Probability = _clone.Probability;
            this.Turn = _clone.Turn;
        }

        public int ID { set; get; }

        public string Name { set; get; }

        public string ElementIDs { set; get; }

        public string Description { set; get; }

        public int BallType { set; get; }

        public int NewBallID { set; get; }

        public int CostMP { set; get; }

        public int Pic { set; get; }

        public string Action { set; get; }

        public string EffectPic { set; get; }

        public int Delay { set; get; }

        public int ColdDown { set; get; }

        public int GameType { set; get; }

        public int Probability { set; get; }

        public int Turn { set; get; }

        public int Damage { set; get; }

        public int DamageCrit { set; get; }
    }
}
