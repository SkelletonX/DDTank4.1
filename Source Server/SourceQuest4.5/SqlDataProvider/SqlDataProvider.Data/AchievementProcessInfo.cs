namespace SqlDataProvider.Data
{
	public class AchievementProcessInfo : DataObject
	{
		private int int_0;

		private int int_1;

		public int CondictionType
		{
			get
			{
				return int_0;
			}
			set
			{
				int_0 = value;
				_isDirty = true;
			}
		}

		public int Value
		{
			get
			{
				return int_1;
			}
			set
			{
				int_1 = value;
				_isDirty = true;
			}
		}

		public AchievementProcessInfo()
		{
		}

		public AchievementProcessInfo(int type, int value)
		{
			CondictionType = type;
			Value = value;
		}
	}
}
