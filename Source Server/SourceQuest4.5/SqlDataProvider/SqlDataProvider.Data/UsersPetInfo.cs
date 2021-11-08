using System;
using System.Collections.Generic;

namespace SqlDataProvider.Data
{
	public class UsersPetInfo : DataObject
	{
		private string string_0;

		private string string_1;

		private int int_0;

		private int int_1;

		private int int_2;

		private string string_2;

		private int int_3;

		private int int_4;

		private int int_5;

		private int int_6;

		private int int_7;

		private int int_8;

		private int int_9;

		private int int_10;

		private int int_11;

		private int int_12;

		private int int_13;

		private int int_14;

		private int int_15;

		private int int_16;

		private int int_17;

		private int int_18;

		private int int_19;

		private int int_20;

		private int int_21;

		private int int_22;

		private bool bool_0;

		private int int_23;

		private int int_24;

		private bool bool_1;

		private int int_25;

		private int int_26;

		private int int_27;

		private int int_28;

		private int int_29;

		private int int_30;

		private List<PetEquipInfo> list_0;

		private string string_3;

		private string string_4;

		private int int_31;

		public string SkillEquip
		{
			get
			{
				return string_0;
			}
			set
			{
				string_0 = value;
				_isDirty = true;
			}
		}

		public string Skill
		{
			get
			{
				return string_1;
			}
			set
			{
				string_1 = value;
				_isDirty = true;
			}
		}

		public int ID
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

		public int PetID
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

		public int TemplateID
		{
			get
			{
				return int_2;
			}
			set
			{
				int_2 = value;
				_isDirty = true;
			}
		}

		public string Name
		{
			get
			{
				return string_2;
			}
			set
			{
				string_2 = value;
				_isDirty = true;
			}
		}

		public int UserID
		{
			get
			{
				return int_3;
			}
			set
			{
				int_3 = value;
				_isDirty = true;
			}
		}

		public int Attack
		{
			get
			{
				return int_4 - method_1(int_4);
			}
			set
			{
				int_4 = value;
				_isDirty = true;
			}
		}

		public int Defence
		{
			get
			{
				return int_5 - method_1(int_5);
			}
			set
			{
				int_5 = value;
				_isDirty = true;
			}
		}

		public int Luck
		{
			get
			{
				return int_6 - method_1(int_6);
			}
			set
			{
				int_6 = value;
				_isDirty = true;
			}
		}

		public int Agility
		{
			get
			{
				return int_7 - method_1(int_7);
			}
			set
			{
				int_7 = value;
				_isDirty = true;
			}
		}

		public int Blood
		{
			get
			{
				return int_8 - method_1(int_8);
			}
			set
			{
				int_8 = value;
				_isDirty = true;
			}
		}

		public int Damage
		{
			get
			{
				return int_9 - method_1(int_9);
			}
			set
			{
				int_9 = value;
				_isDirty = true;
			}
		}

		public int Guard
		{
			get
			{
				return int_10 - method_1(int_10);
			}
			set
			{
				int_10 = value;
				_isDirty = true;
			}
		}

		public int AttackGrow
		{
			get
			{
				return int_11;
			}
			set
			{
				int_11 = value;
				_isDirty = true;
			}
		}

		public int DefenceGrow
		{
			get
			{
				return int_12;
			}
			set
			{
				int_12 = value;
				_isDirty = true;
			}
		}

		public int LuckGrow
		{
			get
			{
				return int_13;
			}
			set
			{
				int_13 = value;
				_isDirty = true;
			}
		}

		public int AgilityGrow
		{
			get
			{
				return int_14;
			}
			set
			{
				int_14 = value;
				_isDirty = true;
			}
		}

		public int BloodGrow
		{
			get
			{
				return int_15;
			}
			set
			{
				int_15 = value;
				_isDirty = true;
			}
		}

		public int DamageGrow
		{
			get
			{
				return int_16;
			}
			set
			{
				int_16 = value;
				_isDirty = true;
			}
		}

		public int GuardGrow
		{
			get
			{
				return int_17;
			}
			set
			{
				int_17 = value;
				_isDirty = true;
			}
		}

		public int Level
		{
			get
			{
				return int_18;
			}
			set
			{
				int_18 = value;
				_isDirty = true;
			}
		}

		public int GP
		{
			get
			{
				return int_19;
			}
			set
			{
				int_19 = value;
				_isDirty = true;
			}
		}

		public int MaxGP
		{
			get
			{
				return int_20;
			}
			set
			{
				int_20 = value;
				_isDirty = true;
			}
		}

		public int Hunger
		{
			get
			{
				return int_21;
			}
			set
			{
				int_21 = value;
				_isDirty = true;
			}
		}

		public int PetHappyStar => method_0();

		public int MP
		{
			get
			{
				return int_22;
			}
			set
			{
				int_22 = value;
				_isDirty = true;
			}
		}

		public bool IsEquip
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

		public int Place
		{
			get
			{
				return int_23;
			}
			set
			{
				int_23 = value;
				_isDirty = true;
			}
		}

		public int currentStarExp
		{
			get
			{
				return int_24;
			}
			set
			{
				int_24 = value;
				_isDirty = true;
			}
		}

		public bool IsExit
		{
			get
			{
				return bool_1;
			}
			set
			{
				bool_1 = value;
				_isDirty = true;
			}
		}

		public int breakGrade
		{
			get
			{
				return int_25;
			}
			set
			{
				int_25 = value;
				_isDirty = true;
			}
		}

		public int breakAttack
		{
			get
			{
				return int_26;
			}
			set
			{
				int_26 = value;
				_isDirty = true;
			}
		}

		public int breakDefence
		{
			get
			{
				return int_27;
			}
			set
			{
				int_27 = value;
				_isDirty = true;
			}
		}

		public int breakAgility
		{
			get
			{
				return int_28;
			}
			set
			{
				int_28 = value;
				_isDirty = true;
			}
		}

		public int breakLuck
		{
			get
			{
				return int_29;
			}
			set
			{
				int_29 = value;
				_isDirty = true;
			}
		}

		public int breakBlood
		{
			get
			{
				return int_30;
			}
			set
			{
				int_30 = value;
				_isDirty = true;
			}
		}

		public List<PetEquipInfo> PetEquips
		{
			get
			{
				return list_0;
			}
			set
			{
				list_0 = value;
			}
		}

		public string eQPets
		{
			get
			{
				return string_3;
			}
			set
			{
				string_3 = value;
				_isDirty = true;
			}
		}

		public string BaseProp
		{
			get
			{
				return string_4;
			}
			set
			{
				string_4 = value;
				_isDirty = true;
			}
		}

		public int TotalAttack
		{
			get
			{
				int_31 = 0;
				foreach (PetEquipInfo petEquip in PetEquips)
				{
					if (petEquip.IsValidItem() && petEquip.Template != null)
					{
						int_31 += petEquip.Template.Attack;
					}
				}
				return Attack + int_31 + int_26;
			}
		}

		public int TotalDefence
		{
			get
			{
				int_31 = 0;
				foreach (PetEquipInfo petEquip in PetEquips)
				{
					if (petEquip.IsValidItem() && petEquip.Template != null)
					{
						int_31 += petEquip.Template.Defence;
					}
				}
				return Defence + int_31 + int_27;
			}
		}

		public int TotalLuck
		{
			get
			{
				int_31 = 0;
				foreach (PetEquipInfo petEquip in PetEquips)
				{
					if (petEquip.IsValidItem() && petEquip.Template != null)
					{
						int_31 += petEquip.Template.Luck;
					}
				}
				return Luck + int_31 + breakLuck;
			}
		}

		public int TotalAgility
		{
			get
			{
				int_31 = 0;
				foreach (PetEquipInfo petEquip in PetEquips)
				{
					if (petEquip.IsValidItem() && petEquip.Template != null)
					{
						int_31 += petEquip.Template.Agility;
					}
				}
				return Agility + int_31 + int_28;
			}
		}

		public int TotalBlood => Blood + int_30;

		public int TotalDamage => Damage;

		public int TotalGuard => Guard;

		public List<string> GetSkill()
		{
			List<string> stringList = new List<string>();
			string string1 = string_1;
			char[] chArray = new char[1]
			{
				'|'
			};
			string[] array = string1.Split(chArray);
			foreach (string str in array)
			{
				stringList.Add(str);
			}
			return stringList;
		}

		public List<string> GetSkillEquip()
		{
			List<string> stringList1 = new List<string>();
			string string1 = string_1;
			char[] chArray = new char[1]
			{
				'|'
			};
			string[] array = string1.Split(chArray);
			foreach (string str in array)
			{
				stringList1.Add(str.Split(',')[0]);
			}
			List<string> stringList2 = new List<string>();
			int num = 1;
			if (Level >= 20 && Level < 30)
			{
				num = 2;
			}
			if (Level >= 30 && Level < 50)
			{
				num = 3;
			}
			if (Level >= 50 && Level < 60)
			{
				num = 4;
			}
			if (Level >= 60)
			{
				num = 5;
			}
			string[] strArray = string_0.Split('|');
			for (int index = 0; index < num; index++)
			{
				if (index < strArray.Length && stringList1.Contains(strArray[index].Split(',')[0]))
				{
					stringList2.Add(strArray[index]);
				}
				else
				{
					stringList2.Add("0," + index);
				}
			}
			return stringList2;
		}

		public int MaxLevel()
		{
			switch (int_25)
			{
			case 1:
				return 63;
			case 2:
				return 65;
			case 3:
				return 68;
			case 4:
				return 70;
			default:
				return 60;
			}
		}

		private int method_0()
		{
			double num1 = (double)int_21 / 10000.0 * 100.0;
			int num2 = 0;
			if (num1 >= 80.0)
			{
				num2 = 3;
			}
			if (num1 < 80.0 && num1 >= 60.0)
			{
				num2 = 2;
			}
			if (num1 < 60.0 && num1 > 0.0)
			{
				num2 = 1;
			}
			return num2;
		}

		private int method_1(int int_32)
		{
			if (method_0() == 2)
			{
				return int_32 * 20 / 100;
			}
			if (method_0() == 1)
			{
				return int_32 * 40 / 100;
			}
			return 0;
		}

		private double[] method_2(int int_32, double[] double_0)
		{
			double[] numArray = new double[double_0.Length];
			numArray[0] = double_0[0] * Math.Pow(2.0, int_32 - 1);
			for (int index = 1; index < double_0.Length; index++)
			{
				numArray[index] = double_0[index] * Math.Pow(1.5, int_32 - 1);
			}
			return numArray;
		}

		public void BuildProp(UsersPetInfo petInfo)
		{
			double[] numArray1 = new double[5]
			{
				BloodGrow * 10,
				AttackGrow,
				DefenceGrow,
				AgilityGrow,
				LuckGrow
			};
			double[] double_0 = new double[5]
			{
				BloodGrow,
				AttackGrow,
				DefenceGrow,
				AgilityGrow,
				LuckGrow
			};
			double[] numArray2 = numArray1;
			double[] numArray3 = numArray1;
			double[] numArray4 = numArray1;
			double[] numArray5 = method_2(1, double_0);
			double[] numArray6 = method_2(2, double_0);
			double[] numArray7 = method_2(3, double_0);
			_ = new double[numArray2.Length];
			double[] numArray8;
			if (Level < 30)
			{
				for (int index3 = 0; index3 < numArray2.Length; index3++)
				{
					numArray2[index3] += (double)(Level - 1) * numArray5[index3];
					numArray2[index3] = Math.Ceiling(numArray2[index3] / 10.0) / 10.0;
				}
				numArray8 = numArray2;
			}
			else if (Level < 50)
			{
				for (int index2 = 0; index2 < numArray3.Length; index2++)
				{
					numArray3[index2] += (double)(Level - 30) * numArray6[index2] + 29.0 * numArray5[index2];
					numArray3[index2] = Math.Ceiling(numArray3[index2] / 10.0) / 10.0;
				}
				numArray8 = numArray3;
			}
			else
			{
				for (int index = 0; index < numArray4.Length; index++)
				{
					numArray4[index] += (double)(Level - 50) * numArray7[index] + 20.0 * numArray6[index] + 29.0 * numArray5[index];
					numArray4[index] = Math.Ceiling(numArray4[index] / 10.0) / 10.0;
				}
				numArray8 = numArray4;
			}
			int_8 = (int)numArray8[0];
			int_4 = (int)numArray8[1];
			int_5 = (int)numArray8[2];
			int_7 = (int)numArray8[3];
			int_6 = (int)numArray8[4];
		}

		public UsersPetInfo()
		{
			list_0 = new List<PetEquipInfo>();
		}
	}
}
