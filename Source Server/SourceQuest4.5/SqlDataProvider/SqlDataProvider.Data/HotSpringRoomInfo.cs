using System;
using System.Runtime.CompilerServices;

namespace SqlDataProvider.Data
{
	public class HotSpringRoomInfo
	{
		[CompilerGenerated]
		private DateTime dateTime_0;

		[CompilerGenerated]
		private DateTime dateTime_1;

		[CompilerGenerated]
		private int FxiFhyuqdi8;

		[CompilerGenerated]
		private int int_0;

		[CompilerGenerated]
		private int int_1;

		[CompilerGenerated]
		private int int_2;

		[CompilerGenerated]
		private int int_3;

		[CompilerGenerated]
		private int int_4;

		[CompilerGenerated]
		private int int_5;

		[CompilerGenerated]
		private string string_0;

		[CompilerGenerated]
		private string string_1;

		[CompilerGenerated]
		private string string_2;

		[CompilerGenerated]
		private string string_3;

		public int curCount
		{
			get;
			set;
		}

		public int effectiveTime
		{
			get;
			set;
		}

		public DateTime endTime
		{
			get;
			set;
		}

		public int maxCount
		{
			get;
			set;
		}

		public int playerID
		{
			get;
			set;
		}

		public string playerName
		{
			get;
			set;
		}

		public int roomID
		{
			get;
			set;
		}

		public string roomIntroduction
		{
			get;
			set;
		}

		public string roomName
		{
			get;
			set;
		}

		public int roomNumber
		{
			get;
			set;
		}

		public string roomPassword
		{
			get;
			set;
		}

		public int roomType
		{
			get;
			set;
		}

		public DateTime startTime
		{
			get;
			set;
		}

		public bool CanEnter()
		{
			return curCount < maxCount;
		}
	}
}
