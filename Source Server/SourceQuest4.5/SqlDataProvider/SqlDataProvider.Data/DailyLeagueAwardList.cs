using System.Collections.Generic;

namespace SqlDataProvider.Data
{
	public class DailyLeagueAwardList
	{
		public int Class
		{
			get;
			set;
		}

		public List<DailyLeagueAwardItems> AwardLists
		{
			get;
			set;
		}

		public int Grade
		{
			get;
			set;
		}

		public int Score
		{
			get;
			set;
		}

		public int Rank
		{
			get;
			set;
		}
	}
}
