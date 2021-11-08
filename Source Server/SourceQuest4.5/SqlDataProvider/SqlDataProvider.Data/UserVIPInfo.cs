using System;

namespace SqlDataProvider.Data
{
	public class UserVIPInfo : DataObject
	{
		private bool _CanTakeVipReward;

		private DateTime _LastVIPPackTime;

		private byte _typeVIP;

		private int _UserID;

		private int _VIPExp;

		private DateTime _VIPExpireDay;

		private DateTime _VIPLastdate;

		private int _VIPLevel;

		private int _VIPNextLevelDaysNeeded;

		private int _VIPOfflineDays;

		private int _VIPOnlineDays;

		public bool CanTakeVipReward
		{
			get
			{
				return _CanTakeVipReward;
			}
			set
			{
				_CanTakeVipReward = value;
				_isDirty = true;
			}
		}

		public DateTime LastVIPPackTime
		{
			get
			{
				return _LastVIPPackTime;
			}
			set
			{
				_LastVIPPackTime = value;
				_isDirty = true;
			}
		}

		public byte typeVIP
		{
			get
			{
				return _typeVIP;
			}
			set
			{
				_typeVIP = value;
				_isDirty = true;
			}
		}

		public int UserID
		{
			get
			{
				return _UserID;
			}
			set
			{
				_UserID = value;
				_isDirty = true;
			}
		}

		public int VIPExp
		{
			get
			{
				return _VIPExp;
			}
			set
			{
				_VIPExp = value;
				_isDirty = true;
			}
		}

		public DateTime VIPExpireDay
		{
			get
			{
				return _VIPExpireDay;
			}
			set
			{
				_VIPExpireDay = value;
				_isDirty = true;
			}
		}

		public DateTime VIPLastdate
		{
			get
			{
				return _VIPLastdate;
			}
			set
			{
				_VIPLastdate = value;
				_isDirty = true;
			}
		}

		public int VIPLevel
		{
			get
			{
				return _VIPLevel;
			}
			set
			{
				_VIPLevel = value;
				_isDirty = true;
			}
		}

		public int VIPNextLevelDaysNeeded
		{
			get
			{
				return _VIPNextLevelDaysNeeded;
			}
			set
			{
				_VIPNextLevelDaysNeeded = value;
				_isDirty = true;
			}
		}

		public int VIPOfflineDays
		{
			get
			{
				return _VIPOfflineDays;
			}
			set
			{
				_VIPOfflineDays = value;
				_isDirty = true;
			}
		}

		public int VIPOnlineDays
		{
			get
			{
				return _VIPOnlineDays;
			}
			set
			{
				_VIPOnlineDays = value;
				_isDirty = true;
			}
		}

		public UserVIPInfo()
		{
		}

		public UserVIPInfo(int userId)
		{
			UserID = userId;
			typeVIP = 0;
			VIPLevel = 0;
			VIPExp = 0;
			VIPOnlineDays = 0;
			VIPOfflineDays = 0;
			VIPExpireDay = DateTime.Now;
			LastVIPPackTime = DateTime.Now;
			VIPLastdate = DateTime.Now;
			VIPNextLevelDaysNeeded = 0;
			CanTakeVipReward = false;
		}

		public void ContinousVIP(int days)
		{
			DateTime now2 = DateTime.Now;
			now2 = (VIPExpireDay = ((!(VIPExpireDay < DateTime.Now)) ? VIPExpireDay.AddDays(days) : DateTime.Now.AddDays(days)));
			typeVIP = SetType(days);
		}

		public bool IsLastVIPPackTime()
		{
			return StartOfWeek(_LastVIPPackTime.Date, DayOfWeek.Monday) < StartOfWeek(DateTime.Now.Date, DayOfWeek.Monday);
		}

		public bool IsVIP()
		{
			if (!IsVIPExpire())
			{
				return _typeVIP > 0;
			}
			return false;
		}

		public bool IsVIPExpire()
		{
			return _VIPExpireDay.Date < DateTime.Now.Date;
		}

		public void OpenVIP(int days)
		{
			DateTime time = DateTime.Now.AddDays(days);
			typeVIP = SetType(days);
			VIPLevel = 1;
			VIPExp = 0;
			VIPExpireDay = time;
			VIPLastdate = DateTime.Now;
			VIPNextLevelDaysNeeded = 0;
			CanTakeVipReward = true;
		}

		public void SetLastVIPPackTime()
		{
			LastVIPPackTime = DateTime.Now;
			CanTakeVipReward = false;
		}

		private byte SetType(int days)
		{
			byte num = 1;
			if (days / 31 >= 3)
			{
				num = 2;
			}
			return num;
		}

		public DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
		{
			int num = dt.DayOfWeek - startOfWeek;
			if (num < 0)
			{
				num += 7;
			}
			return dt.AddDays(-1 * num).Date;
		}
	}
}
