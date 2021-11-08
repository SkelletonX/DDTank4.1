using System;

namespace SqlDataProvider.Data
{
	public class AchievementData : DataObject
	{
		private bool bool_0;

		private DateTime dateTime_0;

		private int int_0;

		private int int_1;

		public int AchievementID
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

		public DateTime CompletedDate
		{
			get
			{
				return dateTime_0;
			}
			set
			{
				dateTime_0 = value;
				_isDirty = true;
			}
		}

		public bool IsComplete
		{
			get
			{
				return bool_0;
			}
			set
			{
				bool_0 = value;
				_isDirty = true;
			}
		}

		public int UserID
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

		public AchievementData()
		{
			UserID = 0;
			AchievementID = 0;
			IsComplete = false;
			CompletedDate = DateTime.Now;
		}
	}
}
