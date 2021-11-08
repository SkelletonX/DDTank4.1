namespace SqlDataProvider.Data
{
	public class PveInfo
	{
		public string AdviceTips
		{
			get;
			set;
		}

		public string BossFightNeedMoney
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string EpicGameScript
		{
			get;
			set;
		}

		public string EpicTemplateIds
		{
			get;
			set;
		}

		public string HardGameScript
		{
			get;
			set;
		}

		public string HardTemplateIds
		{
			get;
			set;
		}

		public int ID
		{
			get;
			set;
		}

		public int LevelLimits
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string NormalGameScript
		{
			get;
			set;
		}

		public string NormalTemplateIds
		{
			get;
			set;
		}

		public int Ordering
		{
			get;
			set;
		}

		public string Pic
		{
			get;
			set;
		}

		public string SimpleGameScript
		{
			get;
			set;
		}

		public string SimpleTemplateIds
		{
			get;
			set;
		}

		public string TerrorGameScript
		{
			get;
			set;
		}

		public string TerrorTemplateIds
		{
			get;
			set;
		}

		public int Type
		{
			get;
			set;
		}

		public int getPrice(int selectedLevel)
		{
			string[] strArray = BossFightNeedMoney.Split('|');
			if (strArray.Length != 0)
			{
				switch (selectedLevel)
				{
				case 0:
					return int.Parse(strArray[0]);
				case 1:
					return int.Parse(strArray[1]);
				case 2:
					return int.Parse(strArray[2]);
				case 3:
					return int.Parse(strArray[3]);
				case 4:
					return int.Parse(strArray[4]);
				}
			}
			return 0;
		}
	}
}
