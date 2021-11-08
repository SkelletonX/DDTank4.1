using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Bussiness.Managers
{
	public class PetMgr
	{
		private static readonly ILog ilog_0;

		private static Dictionary<string, PetConfig> dictionary_0;

		private static Dictionary<int, PetLevel> dictionary_1;

		private static Dictionary<int, PetSkillElementInfo> dictionary_2;

		private static Dictionary<int, PetSkillInfo> dictionary_3;

		private static Dictionary<int, PetSkillTemplateInfo> dictionary_4;

		private static Dictionary<int, PetTemplateInfo> dictionary_5;

		private static Dictionary<int, PetFightPropertyInfo> dictionary_6;

		private static Dictionary<int, PetStarExpInfo> dictionary_7;

		private static List<GameNeedPetSkillInfo> list_0;

		private static Random random_0;

		private static ReaderWriterLock readerWriterLock_0;

		public static bool Init()
		{
			try
			{
				random_0 = new Random();
				return ReLoad();
			}
			catch (Exception ex)
			{
				if (ilog_0.IsErrorEnabled)
				{
					ilog_0.Error("PetInfoMgr", ex);
				}
				return false;
			}
		}

		public static bool ReLoad()
		{
			try
			{
				Dictionary<string, PetConfig> sbgjaFH96yj7P79ekB = new Dictionary<string, PetConfig>();
				Dictionary<int, PetLevel> EvHSgt7QX8VsEd0yEZ = new Dictionary<int, PetLevel>();
				Dictionary<int, PetSkillElementInfo> R11rtWo1p4Y2vEtvxF = new Dictionary<int, PetSkillElementInfo>();
				Dictionary<int, PetSkillInfo> m8Jor90v2RFloePXnt = new Dictionary<int, PetSkillInfo>();
				Dictionary<int, PetSkillTemplateInfo> YHCrVLAS0QefBk0Ur5 = new Dictionary<int, PetSkillTemplateInfo>();
				new Dictionary<int, PetTemplateInfo>();
				Dictionary<int, PetTemplateInfo> w2gAkTbu06gsoxFCZU = new Dictionary<int, PetTemplateInfo>();
				Dictionary<int, PetFightPropertyInfo> PnKXOFTHDG3PZUPkBD = new Dictionary<int, PetFightPropertyInfo>();
				Dictionary<int, PetStarExpInfo> w84AGjgion3EqNro8x = new Dictionary<int, PetStarExpInfo>();
				if (smethod_0(sbgjaFH96yj7P79ekB, EvHSgt7QX8VsEd0yEZ, R11rtWo1p4Y2vEtvxF, m8Jor90v2RFloePXnt, YHCrVLAS0QefBk0Ur5, w2gAkTbu06gsoxFCZU, PnKXOFTHDG3PZUPkBD, w84AGjgion3EqNro8x))
				{
					try
					{
						dictionary_0 = sbgjaFH96yj7P79ekB;
						dictionary_1 = EvHSgt7QX8VsEd0yEZ;
						dictionary_2 = R11rtWo1p4Y2vEtvxF;
						dictionary_3 = m8Jor90v2RFloePXnt;
						dictionary_4 = YHCrVLAS0QefBk0Ur5;
						dictionary_5 = w2gAkTbu06gsoxFCZU;
						dictionary_6 = PnKXOFTHDG3PZUPkBD;
						dictionary_7 = w84AGjgion3EqNro8x;
						List<GameNeedPetSkillInfo> needPetSkillInfoList = LoadGameNeedPetSkill();
						if (needPetSkillInfoList.Count > 0)
						{
							Interlocked.Exchange(ref list_0, needPetSkillInfoList);
						}
						return true;
					}
					catch
					{
					}
				}
			}
			catch (Exception ex)
			{
				if (ilog_0.IsErrorEnabled)
				{
					ilog_0.Error("PetMgr", ex);
				}
			}
			return false;
		}

		public static PetStarExpInfo[] GetPetStarExp()
		{
			return dictionary_7.Values.ToArray();
		}

		public static PetStarExpInfo FindPetStarExp(int oldID)
		{
			readerWriterLock_0.AcquireWriterLock(-1);
			try
			{
				if (dictionary_7.ContainsKey(oldID))
				{
					return dictionary_7[oldID];
				}
			}
			finally
			{
				readerWriterLock_0.ReleaseWriterLock();
			}
			return null;
		}

		private static bool smethod_0(Dictionary<string, PetConfig> sbgjaFH96yj7P79ekB, Dictionary<int, PetLevel> EvHSgt7QX8VsEd0yEZ, Dictionary<int, PetSkillElementInfo> R11rtWo1p4Y2vEtvxF, Dictionary<int, PetSkillInfo> m8Jor90v2RFloePXnt, Dictionary<int, PetSkillTemplateInfo> YHCrVLAS0QefBk0Ur5, Dictionary<int, PetTemplateInfo> w2gAkTbu06gsoxFCZU, Dictionary<int, PetFightPropertyInfo> PnKXOFTHDG3PZUPkBD, Dictionary<int, PetStarExpInfo> w84AGjgion3EqNro8x)
		{
			using (ProduceBussiness produceBussiness = new ProduceBussiness())
			{
				PetConfig[] allPetConfig = produceBussiness.GetAllPetConfig();
				PetLevel[] allPetLevel = produceBussiness.GetAllPetLevel();
				PetSkillElementInfo[] skillElementInfo1 = produceBussiness.GetAllPetSkillElementInfo();
				PetSkillInfo[] allPetSkillInfo = produceBussiness.GetAllPetSkillInfo();
				PetSkillTemplateInfo[] skillTemplateInfo1 = produceBussiness.GetAllPetSkillTemplateInfo();
				PetTemplateInfo[] allPetTemplateInfo = produceBussiness.GetAllPetTemplateInfo();
				PetFightPropertyInfo[] petFightProperty = produceBussiness.GetAllPetFightProperty();
				PetStarExpInfo[] allPetStarExp = produceBussiness.GetAllPetStarExp();
				PetConfig[] array = allPetConfig;
				foreach (PetConfig petConfig in array)
				{
					if (!sbgjaFH96yj7P79ekB.ContainsKey(petConfig.Name))
					{
						sbgjaFH96yj7P79ekB.Add(petConfig.Name, petConfig);
					}
				}
				PetLevel[] array2 = allPetLevel;
				foreach (PetLevel petLevel in array2)
				{
					if (!EvHSgt7QX8VsEd0yEZ.ContainsKey(petLevel.Level))
					{
						EvHSgt7QX8VsEd0yEZ.Add(petLevel.Level, petLevel);
					}
				}
				PetSkillElementInfo[] array3 = skillElementInfo1;
				foreach (PetSkillElementInfo skillElementInfo2 in array3)
				{
					if (!R11rtWo1p4Y2vEtvxF.ContainsKey(skillElementInfo2.ID))
					{
						R11rtWo1p4Y2vEtvxF.Add(skillElementInfo2.ID, skillElementInfo2);
					}
				}
				PetSkillTemplateInfo[] array4 = skillTemplateInfo1;
				foreach (PetSkillTemplateInfo skillTemplateInfo2 in array4)
				{
					if (!YHCrVLAS0QefBk0Ur5.ContainsKey(skillTemplateInfo2.SkillID))
					{
						YHCrVLAS0QefBk0Ur5.Add(skillTemplateInfo2.SkillID, skillTemplateInfo2);
					}
				}
				PetTemplateInfo[] array5 = allPetTemplateInfo;
				foreach (PetTemplateInfo petTemplateInfo in array5)
				{
					if (!w2gAkTbu06gsoxFCZU.ContainsKey(petTemplateInfo.TemplateID))
					{
						w2gAkTbu06gsoxFCZU.Add(petTemplateInfo.TemplateID, petTemplateInfo);
					}
				}
				PetSkillInfo[] array6 = allPetSkillInfo;
				foreach (PetSkillInfo petSkillInfo in array6)
				{
					if (!m8Jor90v2RFloePXnt.ContainsKey(petSkillInfo.ID))
					{
						m8Jor90v2RFloePXnt.Add(petSkillInfo.ID, petSkillInfo);
					}
				}
				PetFightPropertyInfo[] array7 = petFightProperty;
				foreach (PetFightPropertyInfo fightPropertyInfo in array7)
				{
					if (!PnKXOFTHDG3PZUPkBD.ContainsKey(fightPropertyInfo.ID))
					{
						PnKXOFTHDG3PZUPkBD.Add(fightPropertyInfo.ID, fightPropertyInfo);
					}
				}
				PetStarExpInfo[] array8 = allPetStarExp;
				foreach (PetStarExpInfo petStarExpInfo in array8)
				{
					if (!w84AGjgion3EqNro8x.ContainsKey(petStarExpInfo.OldID))
					{
						w84AGjgion3EqNro8x.Add(petStarExpInfo.OldID, petStarExpInfo);
					}
				}
			}
			return true;
		}

		public static PetFightPropertyInfo FindFightProperty(int level)
		{
			readerWriterLock_0.AcquireWriterLock(-1);
			try
			{
				if (dictionary_6.ContainsKey(level))
				{
					return dictionary_6[level];
				}
			}
			finally
			{
				readerWriterLock_0.ReleaseWriterLock();
			}
			return null;
		}

		public static int GetEvolutionGP(int level)
		{
			return FindFightProperty(level + 1)?.Exp ?? (-1);
		}

		public static int GetEvolutionMax()
		{
			return dictionary_6.Count;
		}

		public static int GetEvolutionGrade(int GP)
		{
			int count = dictionary_6.Count;
			if (GP >= FindFightProperty(count).Exp)
			{
				return count;
			}
			for (int level = 1; level <= count; level++)
			{
				PetFightPropertyInfo fightProperty = FindFightProperty(level);
				if (fightProperty == null)
				{
					return 1;
				}
				if (GP < fightProperty.Exp)
				{
					if (level - 1 >= 0)
					{
						return level - 1;
					}
					return 0;
				}
			}
			return 0;
		}

		public static PetConfig FindConfig(string key)
		{
			if (dictionary_0 == null)
			{
				Init();
			}
			if (dictionary_0.ContainsKey(key))
			{
				return dictionary_0[key];
			}
			return null;
		}

		public static PetLevel FindPetLevel(int level)
		{
			if (dictionary_1 == null)
			{
				Init();
			}
			if (dictionary_1.ContainsKey(level))
			{
				return dictionary_1[level];
			}
			return null;
		}

		public static PetSkillElementInfo FindPetSkillElement(int SkillID)
		{
			if (dictionary_2 == null)
			{
				Init();
			}
			if (dictionary_2.ContainsKey(SkillID))
			{
				return dictionary_2[SkillID];
			}
			return null;
		}

		public static PetSkillInfo FindPetSkill(int SkillID)
		{
			if (dictionary_3 == null)
			{
				Init();
			}
			if (dictionary_3.ContainsKey(SkillID))
			{
				return dictionary_3[SkillID];
			}
			return null;
		}

		public static PetSkillInfo[] GetPetSkills()
		{
			List<PetSkillInfo> petSkillInfoList = new List<PetSkillInfo>();
			if (dictionary_3 == null)
			{
				Init();
			}
			foreach (PetSkillInfo petSkillInfo in dictionary_3.Values)
			{
				petSkillInfoList.Add(petSkillInfo);
			}
			return petSkillInfoList.ToArray();
		}

		public static PetSkillTemplateInfo GetPetSkillTemplate(int ID)
		{
			if (dictionary_4 == null)
			{
				Init();
			}
			if (dictionary_4.ContainsKey(ID))
			{
				return dictionary_4[ID];
			}
			return null;
		}

		public static PetTemplateInfo FindPetTemplate(int TemplateID)
		{
			readerWriterLock_0.AcquireWriterLock(-1);
			try
			{
				if (dictionary_5.ContainsKey(TemplateID))
				{
					return dictionary_5[TemplateID];
				}
			}
			finally
			{
				readerWriterLock_0.ReleaseWriterLock();
			}
			return null;
		}

		public static PetTemplateInfo FindPetTemplateByKind(int star, int kindId)
		{
			foreach (PetTemplateInfo petTemplateInfo in dictionary_5.Values)
			{
				if (petTemplateInfo.KindID == kindId && star == petTemplateInfo.StarLevel)
				{
					return petTemplateInfo;
				}
			}
			return null;
		}

		public static List<int> GetPetSkillByKindID(int KindID, int lv, int playerLevel)
		{
			List<int> intList = new List<int>();
			List<string> stringList = new List<string>();
			PetSkillTemplateInfo[] petSkillByKindId = GetPetSkillByKindID(KindID);
			int num2 = (lv > playerLevel) ? playerLevel : lv;
			for (int index = 1; index <= num2; index++)
			{
				PetSkillTemplateInfo[] array = petSkillByKindId;
				foreach (PetSkillTemplateInfo skillTemplateInfo in array)
				{
					if (skillTemplateInfo.MinLevel == index)
					{
						string deleteSkillIds = skillTemplateInfo.DeleteSkillIDs;
						char[] chArray = new char[1]
						{
							','
						};
						string[] array2 = deleteSkillIds.Split(chArray);
						foreach (string str in array2)
						{
							stringList.Add(str);
						}
						intList.Add(skillTemplateInfo.SkillID);
					}
				}
			}
			foreach (string s in stringList)
			{
				if (!string.IsNullOrEmpty(s))
				{
					int num3 = int.Parse(s);
					intList.Remove(num3);
				}
			}
			intList.Sort();
			return intList;
		}

		public static PetSkillTemplateInfo[] GetPetSkillByKindID(int KindID)
		{
			List<PetSkillTemplateInfo> skillTemplateInfoList = new List<PetSkillTemplateInfo>();
			foreach (PetSkillTemplateInfo skillTemplateInfo in dictionary_4.Values)
			{
				if (skillTemplateInfo.KindID == KindID)
				{
					skillTemplateInfoList.Add(skillTemplateInfo);
				}
			}
			return skillTemplateInfoList.ToArray();
		}

		public static List<UsersPetInfo> CreateFirstAdoptList(int userID, int playerLevel)
		{
			List<int> intList = new List<int>
			{
				100301,
				110301,
				120301,
				130301
			};
			List<UsersPetInfo> usersPetInfoList = new List<UsersPetInfo>();
			for (int place = 0; place < intList.Count; place++)
			{
				UsersPetInfo pet = CreatePet(FindPetTemplate(intList[place]), userID, place, playerLevel);
				pet.IsExit = true;
				usersPetInfoList.Add(pet);
			}
			return usersPetInfoList;
		}

		public static string ActiveEquipSkill(int Level)
		{
			string str = "0,0";
			int num = 1;
			if (Level >= 20 && Level < 30)
			{
				num++;
			}
			if (Level >= 30 && Level < 50)
			{
				num += 2;
			}
			if (Level >= 50 && Level < 60)
			{
				num += 3;
			}
			if (Level >= 60)
			{
				num += 4;
			}
			for (int index = 1; index < num; index++)
			{
				str = str + "|0," + index;
			}
			return str;
		}

		public static int UpdateEvolution(int TemplateID, int lv)
		{
			string str = TemplateID.ToString();
			int int32_1 = Convert.ToInt32(FindConfig("EvolutionLevel1").Value);
			int int32_2 = Convert.ToInt32(FindConfig("EvolutionLevel2").Value);
			return FindPetTemplate((str.Substring(str.Length - 1, 1) == "1") ? ((lv < int32_1) ? TemplateID : ((lv >= int32_2) ? (TemplateID + 2) : (TemplateID + 1))) : ((!(str.Substring(str.Length - 1, 1) == "2")) ? TemplateID : (TemplateID + 1)))?.TemplateID ?? TemplateID;
		}

		public static string UpdateSkillPet(int Level, int TemplateID, int playerLevel)
		{
			PetTemplateInfo petTemplate = FindPetTemplate(TemplateID);
			if (petTemplate == null)
			{
				ilog_0.Error("Pet not found: " + TemplateID);
				return "";
			}
			List<int> petSkillByKindId = GetPetSkillByKindID(petTemplate.KindID, Level, playerLevel);
			string str = petSkillByKindId[0] + ",0";
			for (int index = 1; index < petSkillByKindId.Count; index++)
			{
				str = str + "|" + petSkillByKindId[index] + "," + index;
			}
			return str;
		}

		public static int GetLevel(int GP, int playerLevel)
		{
			if (GP >= FindPetLevel(playerLevel).GP)
			{
				return playerLevel;
			}
			for (int level2 = 1; level2 <= playerLevel; level2++)
			{
				if (GP < FindPetLevel(level2).GP)
				{
					if (level2 - 1 != 0)
					{
						return level2 - 1;
					}
					return 1;
				}
			}
			return 1;
		}

		public static int GetGP(int level, int playerLevel)
		{
			for (int level2 = 1; level2 <= playerLevel; level2++)
			{
				if (level == FindPetLevel(level2).Level)
				{
					return FindPetLevel(level2).GP;
				}
			}
			return 0;
		}

		public static void GetEvolutionPropArr(UsersPetInfo _petInfo, PetTemplateInfo petTempleteInfo, ref double[] propArr, ref double[] growArr)
		{
			double[] numArray1 = new double[5]
			{
				_petInfo.Blood * 100,
				_petInfo.Attack * 100,
				_petInfo.Defence * 100,
				_petInfo.Agility * 100,
				_petInfo.Luck * 100
			};
			double[] numArray2 = new double[5]
			{
				_petInfo.BloodGrow,
				_petInfo.AttackGrow,
				_petInfo.DefenceGrow,
				_petInfo.AgilityGrow,
				_petInfo.LuckGrow
			};
			double[] numArray3 = new double[5]
			{
				petTempleteInfo.HighBlood,
				petTempleteInfo.HighAttack,
				petTempleteInfo.HighDefence,
				petTempleteInfo.HighAgility,
				petTempleteInfo.HighLuck
			};
			double[] propArr2 = new double[5]
			{
				petTempleteInfo.HighBloodGrow,
				petTempleteInfo.HighAttackGrow,
				petTempleteInfo.HighDefenceGrow,
				petTempleteInfo.HighAgilityGrow,
				petTempleteInfo.HighLuckGrow
			};
			double[] numArray4 = numArray3;
			double[] addedPropArr1 = GetAddedPropArr(1, propArr2);
			double[] numArray5 = numArray3;
			double[] addedPropArr2 = GetAddedPropArr(2, propArr2);
			double[] numArray6 = numArray3;
			double[] addedPropArr3 = GetAddedPropArr(3, propArr2);
			if (_petInfo.Level < 30)
			{
				for (int index3 = 0; index3 < numArray4.Length; index3++)
				{
					numArray4[index3] += (double)(_petInfo.Level - 1) * addedPropArr1[index3] - numArray1[index3];
					addedPropArr1[index3] -= numArray2[index3];
					numArray4[index3] = Math.Ceiling(numArray4[index3] / 10.0) / 10.0;
					addedPropArr1[index3] = Math.Ceiling(addedPropArr1[index3] / 10.0) / 10.0;
				}
				propArr = numArray4;
				growArr = addedPropArr1;
			}
			else if (_petInfo.Level < 50)
			{
				for (int index2 = 0; index2 < numArray5.Length; index2++)
				{
					numArray5[index2] += (double)(_petInfo.Level - 30) * addedPropArr2[index2] + 29.0 * addedPropArr1[index2] - numArray1[index2];
					addedPropArr2[index2] -= numArray2[index2];
					numArray5[index2] = Math.Ceiling(numArray5[index2] / 10.0) / 10.0;
					addedPropArr2[index2] = Math.Ceiling(addedPropArr2[index2] / 10.0) / 10.0;
				}
				propArr = numArray5;
				growArr = addedPropArr2;
			}
			else
			{
				for (int index = 0; index < numArray6.Length; index++)
				{
					numArray6[index] += (double)(_petInfo.Level - 50) * addedPropArr3[index] + 20.0 * addedPropArr2[index] + 29.0 * addedPropArr1[index] - numArray1[index];
					addedPropArr3[index] -= numArray2[index];
					numArray6[index] = Math.Ceiling(numArray6[index] / 10.0) / 10.0;
					addedPropArr3[index] = Math.Ceiling(addedPropArr3[index] / 10.0) / 10.0;
				}
				propArr = numArray6;
				growArr = addedPropArr3;
			}
		}

		public static double[] GetAddedPropArr(int grade, double[] propArr)
		{
			double[] numArray = new double[5]
			{
				propArr[0] * Math.Pow(2.0, grade - 1),
				0.0,
				0.0,
				0.0,
				0.0
			};
			for (int index = 1; index < 5; index++)
			{
				numArray[index] = propArr[index] * Math.Pow(1.5, grade - 1);
			}
			return numArray;
		}

		public static UsersPetInfo CreatePet(PetTemplateInfo info, int userID, int place, int playerLevel)
		{
			UsersPetInfo usersPetInfo = new UsersPetInfo();
			double num = (double)info.StarLevel * 0.1;
			Random random = new Random();
			usersPetInfo.BloodGrow = (int)Math.Ceiling((double)(random.Next((int)((double)info.HighBlood / (1.7 - num)), info.HighBlood - (int)((double)info.HighBlood / 17.1)) / 10));
			usersPetInfo.AttackGrow = random.Next((int)((double)info.HighAttack / (1.7 - num)), info.HighAttack - (int)((double)info.HighAttack / 17.1));
			usersPetInfo.DefenceGrow = random.Next((int)((double)info.HighDefence / (1.7 - num)), info.HighDefence - (int)((double)info.HighDefence / 17.1));
			usersPetInfo.AgilityGrow = random.Next((int)((double)info.HighAgility / (1.7 - num)), info.HighAgility - (int)((double)info.HighAgility / 17.1));
			usersPetInfo.LuckGrow = random.Next((int)((double)info.HighLuck / (1.7 - num)), info.HighLuck - (int)((double)info.HighLuck / 17.1));
			usersPetInfo.ID = 0;
			usersPetInfo.Hunger = 10000;
			usersPetInfo.TemplateID = info.TemplateID;
			usersPetInfo.Name = info.Name;
			usersPetInfo.UserID = userID;
			usersPetInfo.Place = place;
			usersPetInfo.Level = 1;
			usersPetInfo.BuildProp(usersPetInfo);
			usersPetInfo.Skill = UpdateSkillPet(1, info.TemplateID, playerLevel);
			usersPetInfo.SkillEquip = ActiveEquipSkill(1);
			return usersPetInfo;
		}

		public static UsersPetInfo CreateNewPet()
		{
			string[] strArray = FindConfig("NewPet").Value.Split(',');
			int index = random_0.Next(strArray.Length);
			return CreatePet(FindPetTemplate(Convert.ToInt32(strArray[index])), -1, -1, 60);
		}

		public static GameNeedPetSkillInfo[] GetGameNeedPetSkill()
		{
			return list_0.ToArray();
		}

		public static List<GameNeedPetSkillInfo> LoadGameNeedPetSkill()
		{
			List<GameNeedPetSkillInfo> needPetSkillInfoList = new List<GameNeedPetSkillInfo>();
			List<string> stringList = new List<string>();
			foreach (PetSkillInfo petSkillInfo in dictionary_3.Values)
			{
				string effectPic2 = petSkillInfo.EffectPic;
				if (!string.IsNullOrEmpty(effectPic2) && !stringList.Contains(effectPic2))
				{
					needPetSkillInfoList.Add(new GameNeedPetSkillInfo
					{
						Pic = petSkillInfo.Pic,
						EffectPic = petSkillInfo.EffectPic
					});
					stringList.Add(effectPic2);
				}
			}
			foreach (PetSkillElementInfo skillElementInfo in dictionary_2.Values)
			{
				string effectPic = skillElementInfo.EffectPic;
				if (!string.IsNullOrEmpty(effectPic) && !stringList.Contains(effectPic))
				{
					needPetSkillInfoList.Add(new GameNeedPetSkillInfo
					{
						Pic = skillElementInfo.Pic,
						EffectPic = skillElementInfo.EffectPic
					});
					stringList.Add(effectPic);
				}
			}
			return needPetSkillInfoList;
		}

		static PetMgr()
		{
			ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
			list_0 = new List<GameNeedPetSkillInfo>();
			readerWriterLock_0 = new ReaderWriterLock();
		}
	}
}
