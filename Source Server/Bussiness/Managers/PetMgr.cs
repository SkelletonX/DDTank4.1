

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
    private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static List<GameNeedPetSkillInfo> list_0 = new List<GameNeedPetSkillInfo>();
    private static ReaderWriterLock readerWriterLock_0 = new ReaderWriterLock();
    private static Dictionary<string, PetConfig> dictionary_0;
    private static Dictionary<int, PetLevel> dictionary_1;
    private static Dictionary<int, PetSkillElementInfo> dictionary_2;
    private static Dictionary<int, PetSkillInfo> dictionary_3;
    private static Dictionary<int, PetSkillTemplateInfo> dictionary_4;
    private static Dictionary<int, PetTemplateInfo> dictionary_5;
    private static Dictionary<int, PetFightPropertyInfo> dictionary_6;
    private static Dictionary<int, PetStarExpInfo> dictionary_7;
    private static Random random_0;

    public static bool Init()
    {
      try
      {
        PetMgr.random_0 = new Random();
        return PetMgr.ReLoad();
      }
      catch (Exception ex)
      {
        if (PetMgr.ilog_0.IsErrorEnabled)
          PetMgr.ilog_0.Error((object) "PetInfoMgr", ex);
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
        Dictionary<int, PetTemplateInfo> dictionary = new Dictionary<int, PetTemplateInfo>();
        Dictionary<int, PetTemplateInfo> w2gAkTbu06gsoxFCZU = new Dictionary<int, PetTemplateInfo>();
        Dictionary<int, PetFightPropertyInfo> PnKXOFTHDG3PZUPkBD = new Dictionary<int, PetFightPropertyInfo>();
        Dictionary<int, PetStarExpInfo> w84AGjgion3EqNro8x = new Dictionary<int, PetStarExpInfo>();
        if (PetMgr.smethod_0(sbgjaFH96yj7P79ekB, EvHSgt7QX8VsEd0yEZ, R11rtWo1p4Y2vEtvxF, m8Jor90v2RFloePXnt, YHCrVLAS0QefBk0Ur5, w2gAkTbu06gsoxFCZU, PnKXOFTHDG3PZUPkBD, w84AGjgion3EqNro8x))
        {
          try
          {
            PetMgr.dictionary_0 = sbgjaFH96yj7P79ekB;
            PetMgr.dictionary_1 = EvHSgt7QX8VsEd0yEZ;
            PetMgr.dictionary_2 = R11rtWo1p4Y2vEtvxF;
            PetMgr.dictionary_3 = m8Jor90v2RFloePXnt;
            PetMgr.dictionary_4 = YHCrVLAS0QefBk0Ur5;
            PetMgr.dictionary_5 = w2gAkTbu06gsoxFCZU;
            PetMgr.dictionary_6 = PnKXOFTHDG3PZUPkBD;
            PetMgr.dictionary_7 = w84AGjgion3EqNro8x;
            List<GameNeedPetSkillInfo> needPetSkillInfoList = PetMgr.LoadGameNeedPetSkill();
            if (needPetSkillInfoList.Count > 0)
              Interlocked.Exchange<List<GameNeedPetSkillInfo>>(ref PetMgr.list_0, needPetSkillInfoList);
            return true;
          }
          catch
          {
          }
        }
      }
      catch (Exception ex)
      {
        if (PetMgr.ilog_0.IsErrorEnabled)
          PetMgr.ilog_0.Error((object) nameof (PetMgr), ex);
      }
      return false;
    }

    public static PetStarExpInfo[] GetPetStarExp()
    {
      return PetMgr.dictionary_7.Values.ToArray<PetStarExpInfo>();
    }

    public static PetStarExpInfo FindPetStarExp(int oldID)
    {
      PetMgr.readerWriterLock_0.AcquireWriterLock(-1);
      try
      {
        if (PetMgr.dictionary_7.ContainsKey(oldID))
          return PetMgr.dictionary_7[oldID];
      }
      finally
      {
        PetMgr.readerWriterLock_0.ReleaseWriterLock();
      }
      return (PetStarExpInfo) null;
    }

    private static bool smethod_0(
      Dictionary<string, PetConfig> sbgjaFH96yj7P79ekB,
      Dictionary<int, PetLevel> EvHSgt7QX8VsEd0yEZ,
      Dictionary<int, PetSkillElementInfo> R11rtWo1p4Y2vEtvxF,
      Dictionary<int, PetSkillInfo> m8Jor90v2RFloePXnt,
      Dictionary<int, PetSkillTemplateInfo> YHCrVLAS0QefBk0Ur5,
      Dictionary<int, PetTemplateInfo> w2gAkTbu06gsoxFCZU,
      Dictionary<int, PetFightPropertyInfo> PnKXOFTHDG3PZUPkBD,
      Dictionary<int, PetStarExpInfo> w84AGjgion3EqNro8x)
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
        foreach (PetConfig petConfig in allPetConfig)
        {
          if (!sbgjaFH96yj7P79ekB.ContainsKey(petConfig.Name))
            sbgjaFH96yj7P79ekB.Add(petConfig.Name, petConfig);
        }
        foreach (PetLevel petLevel in allPetLevel)
        {
          if (!EvHSgt7QX8VsEd0yEZ.ContainsKey(petLevel.Level))
            EvHSgt7QX8VsEd0yEZ.Add(petLevel.Level, petLevel);
        }
        foreach (PetSkillElementInfo skillElementInfo2 in skillElementInfo1)
        {
          if (!R11rtWo1p4Y2vEtvxF.ContainsKey(skillElementInfo2.ID))
            R11rtWo1p4Y2vEtvxF.Add(skillElementInfo2.ID, skillElementInfo2);
        }
        foreach (PetSkillTemplateInfo skillTemplateInfo2 in skillTemplateInfo1)
        {
          if (!YHCrVLAS0QefBk0Ur5.ContainsKey(skillTemplateInfo2.SkillID))
            YHCrVLAS0QefBk0Ur5.Add(skillTemplateInfo2.SkillID, skillTemplateInfo2);
        }
        foreach (PetTemplateInfo petTemplateInfo in allPetTemplateInfo)
        {
          if (!w2gAkTbu06gsoxFCZU.ContainsKey(petTemplateInfo.TemplateID))
            w2gAkTbu06gsoxFCZU.Add(petTemplateInfo.TemplateID, petTemplateInfo);
        }
        foreach (PetSkillInfo petSkillInfo in allPetSkillInfo)
        {
          if (!m8Jor90v2RFloePXnt.ContainsKey(petSkillInfo.ID))
            m8Jor90v2RFloePXnt.Add(petSkillInfo.ID, petSkillInfo);
        }
        foreach (PetFightPropertyInfo fightPropertyInfo in petFightProperty)
        {
          if (!PnKXOFTHDG3PZUPkBD.ContainsKey(fightPropertyInfo.ID))
            PnKXOFTHDG3PZUPkBD.Add(fightPropertyInfo.ID, fightPropertyInfo);
        }
        foreach (PetStarExpInfo petStarExpInfo in allPetStarExp)
        {
          if (!w84AGjgion3EqNro8x.ContainsKey(petStarExpInfo.OldID))
            w84AGjgion3EqNro8x.Add(petStarExpInfo.OldID, petStarExpInfo);
        }
      }
      return true;
    }

    public static PetFightPropertyInfo FindFightProperty(int level)
    {
      PetMgr.readerWriterLock_0.AcquireWriterLock(-1);
      try
      {
        if (PetMgr.dictionary_6.ContainsKey(level))
          return PetMgr.dictionary_6[level];
      }
      finally
      {
        PetMgr.readerWriterLock_0.ReleaseWriterLock();
      }
      return (PetFightPropertyInfo) null;
    }

    public static int GetEvolutionGP(int level)
    {
      PetFightPropertyInfo fightProperty = PetMgr.FindFightProperty(level + 1);
      if (fightProperty != null)
        return fightProperty.Exp;
      return -1;
    }

    public static int GetEvolutionMax()
    {
      return PetMgr.dictionary_6.Count;
    }

    public static int GetEvolutionGrade(int GP)
    {
      int count = PetMgr.dictionary_6.Count;
      if (GP >= PetMgr.FindFightProperty(count).Exp)
        return count;
      for (int level = 1; level <= count; ++level)
      {
        PetFightPropertyInfo fightProperty = PetMgr.FindFightProperty(level);
        if (fightProperty == null)
          return 1;
        if (GP < fightProperty.Exp)
        {
          if (level - 1 >= 0)
            return level - 1;
          return 0;
        }
      }
      return 0;
    }

    public static PetConfig FindConfig(string key)
    {
      if (PetMgr.dictionary_0 == null)
        PetMgr.Init();
      if (PetMgr.dictionary_0.ContainsKey(key))
        return PetMgr.dictionary_0[key];
      return (PetConfig) null;
    }

    public static PetLevel FindPetLevel(int level)
    {
      if (PetMgr.dictionary_1 == null)
        PetMgr.Init();
      if (PetMgr.dictionary_1.ContainsKey(level))
        return PetMgr.dictionary_1[level];
      return (PetLevel) null;
    }

    public static PetSkillElementInfo FindPetSkillElement(int SkillID)
    {
      if (PetMgr.dictionary_2 == null)
        PetMgr.Init();
      if (PetMgr.dictionary_2.ContainsKey(SkillID))
        return PetMgr.dictionary_2[SkillID];
      return (PetSkillElementInfo) null;
    }

    public static PetSkillInfo FindPetSkill(int SkillID)
    {
      if (PetMgr.dictionary_3 == null)
        PetMgr.Init();
      if (PetMgr.dictionary_3.ContainsKey(SkillID))
        return PetMgr.dictionary_3[SkillID];
      return (PetSkillInfo) null;
    }

    public static PetSkillInfo[] GetPetSkills()
    {
      List<PetSkillInfo> petSkillInfoList = new List<PetSkillInfo>();
      if (PetMgr.dictionary_3 == null)
        PetMgr.Init();
      foreach (PetSkillInfo petSkillInfo in PetMgr.dictionary_3.Values)
        petSkillInfoList.Add(petSkillInfo);
      return petSkillInfoList.ToArray();
    }

    public static PetSkillTemplateInfo GetPetSkillTemplate(int ID)
    {
      if (PetMgr.dictionary_4 == null)
        PetMgr.Init();
      if (PetMgr.dictionary_4.ContainsKey(ID))
        return PetMgr.dictionary_4[ID];
      return (PetSkillTemplateInfo) null;
    }

    public static PetTemplateInfo FindPetTemplate(int TemplateID)
    {
      PetMgr.readerWriterLock_0.AcquireWriterLock(-1);
      try
      {
        if (PetMgr.dictionary_5.ContainsKey(TemplateID))
          return PetMgr.dictionary_5[TemplateID];
      }
      finally
      {
        PetMgr.readerWriterLock_0.ReleaseWriterLock();
      }
      return (PetTemplateInfo) null;
    }

    public static PetTemplateInfo FindPetTemplateByKind(int star, int kindId)
    {
      foreach (PetTemplateInfo petTemplateInfo in PetMgr.dictionary_5.Values)
      {
        if (petTemplateInfo.KindID == kindId && star == petTemplateInfo.StarLevel)
          return petTemplateInfo;
      }
      return (PetTemplateInfo) null;
    }

    public static List<int> GetPetSkillByKindID(int KindID, int lv, int playerLevel)
    {
      int num1 = playerLevel;
      List<int> intList = new List<int>();
      List<string> stringList = new List<string>();
      PetSkillTemplateInfo[] petSkillByKindId = PetMgr.GetPetSkillByKindID(KindID);
      int num2 = lv > num1 ? num1 : lv;
      for (int index = 1; index <= num2; ++index)
      {
        foreach (PetSkillTemplateInfo skillTemplateInfo in petSkillByKindId)
        {
          if (skillTemplateInfo.MinLevel == index)
          {
            string deleteSkillIds = skillTemplateInfo.DeleteSkillIDs;
            char[] chArray = new char[1]{ ',' };
            foreach (string str in deleteSkillIds.Split(chArray))
              stringList.Add(str);
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
      foreach (PetSkillTemplateInfo skillTemplateInfo in PetMgr.dictionary_4.Values)
      {
        if (skillTemplateInfo.KindID == KindID)
          skillTemplateInfoList.Add(skillTemplateInfo);
      }
      return skillTemplateInfoList.ToArray();
    }

    public static List<UsersPetInfo> CreateFirstAdoptList(
      int userID,
      int playerLevel)
    {
      List<int> intList = new List<int>()
      {
        100301,
        110301,
        120301,
        130301
      };
      List<UsersPetInfo> usersPetInfoList = new List<UsersPetInfo>();
      for (int place = 0; place < intList.Count; ++place)
      {
        UsersPetInfo pet = PetMgr.CreatePet(PetMgr.FindPetTemplate(intList[place]), userID, place, playerLevel);
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
        ++num;
      if (Level >= 30 && Level < 50)
        num += 2;
      if (Level >= 50 && Level < 60)
        num += 3;
      if (Level == 60 )
        num += 4;
      
      for (int index = 1; index < num; ++index)
        str = str + "|0," + (object) index;
      return str;
    }

    public static int UpdateEvolution(int TemplateID, int lv)
    {
      string str = TemplateID.ToString();
      int int32_1 = Convert.ToInt32(PetMgr.FindConfig("EvolutionLevel1").Value);
      int int32_2 = Convert.ToInt32(PetMgr.FindConfig("EvolutionLevel2").Value);
      PetTemplateInfo petTemplate = PetMgr.FindPetTemplate(!(str.Substring(str.Length - 1, 1) == "1") ? (!(str.Substring(str.Length - 1, 1) == "2") ? TemplateID : TemplateID + 1) : (lv >= int32_1 ? (lv >= int32_2 ? TemplateID + 2 : TemplateID + 1) : TemplateID));
      if (petTemplate != null)
        return petTemplate.TemplateID;
      return TemplateID;
    }

    public static string UpdateSkillPet(int Level, int TemplateID, int playerLevel)
    {
      PetTemplateInfo petTemplate = PetMgr.FindPetTemplate(TemplateID);
      if (petTemplate == null)
      {
        PetMgr.ilog_0.Error((object) ("Pet not found: " + (object) TemplateID));
        return "";
      }
      List<int> petSkillByKindId = PetMgr.GetPetSkillByKindID(petTemplate.KindID, Level, playerLevel);
      string str = petSkillByKindId[0].ToString() + ",0";
      for (int index = 1; index < petSkillByKindId.Count; ++index)
        str = str + "|" + (object) petSkillByKindId[index] + "," + (object) index;
      return str;
    }

    public static int GetLevel(int GP, int playerLevel)
    {
      int level1 = playerLevel;
      if (GP >= PetMgr.FindPetLevel(level1).GP)
        return level1;
      for (int level2 = 1; level2 <= level1; ++level2)
      {
        if (GP < PetMgr.FindPetLevel(level2).GP)
        {
          if ((uint) (level2 - 1) > 0U)
            return level2 - 1;
          return 1;
        }
      }
      return 1;
    }

    public static int GetGP(int level, int playerLevel)
    {
      int num = playerLevel;
      for (int level1 = 1; level1 <= num; ++level1)
      {
        if (level == PetMgr.FindPetLevel(level1).Level)
          return PetMgr.FindPetLevel(level1).GP;
      }
      return 0;
    }

    public static void GetEvolutionPropArr(
      UsersPetInfo _petInfo,
      PetTemplateInfo petTempleteInfo,
      ref double[] propArr,
      ref double[] growArr)
    {
      double[] numArray1 = new double[5]
      {
        (double) (_petInfo.Blood * 100),
        (double) (_petInfo.Attack * 100),
        (double) (_petInfo.Defence * 100),
        (double) (_petInfo.Agility * 100),
        (double) (_petInfo.Luck * 100)
      };
      double[] numArray2 = new double[5]
      {
        (double) _petInfo.BloodGrow,
        (double) _petInfo.AttackGrow,
        (double) _petInfo.DefenceGrow,
        (double) _petInfo.AgilityGrow,
        (double) _petInfo.LuckGrow
      };
      double[] numArray3 = new double[5]
      {
        (double) petTempleteInfo.HighBlood,
        (double) petTempleteInfo.HighAttack,
        (double) petTempleteInfo.HighDefence,
        (double) petTempleteInfo.HighAgility,
        (double) petTempleteInfo.HighLuck
      };
      double[] propArr1 = new double[5]
      {
        (double) petTempleteInfo.HighBloodGrow,
        (double) petTempleteInfo.HighAttackGrow,
        (double) petTempleteInfo.HighDefenceGrow,
        (double) petTempleteInfo.HighAgilityGrow,
        (double) petTempleteInfo.HighLuckGrow
      };
      double[] numArray4 = numArray3;
      double[] addedPropArr1 = PetMgr.GetAddedPropArr(1, propArr1);
      double[] numArray5 = numArray3;
      double[] addedPropArr2 = PetMgr.GetAddedPropArr(2, propArr1);
      double[] numArray6 = numArray3;
      double[] addedPropArr3 = PetMgr.GetAddedPropArr(3, propArr1);
      if (_petInfo.Level < 30)
      {
        for (int index = 0; index < numArray4.Length; ++index)
        {
          numArray4[index] = numArray4[index] + ((double) (_petInfo.Level - 1) * addedPropArr1[index] - numArray1[index]);
          addedPropArr1[index] = addedPropArr1[index] - numArray2[index];
          numArray4[index] = Math.Ceiling(numArray4[index] / 10.0) / 10.0;
          addedPropArr1[index] = Math.Ceiling(addedPropArr1[index] / 10.0) / 10.0;
        }
        propArr = numArray4;
        growArr = addedPropArr1;
      }
      else if (_petInfo.Level < 60)
      {
        for (int index = 0; index < numArray5.Length; ++index)
        {
          numArray5[index] = numArray5[index] + ((double) (_petInfo.Level - 30) * addedPropArr2[index] + 29.0 * addedPropArr1[index] - numArray1[index]);
          addedPropArr2[index] = addedPropArr2[index] - numArray2[index];
          numArray5[index] = Math.Ceiling(numArray5[index] / 10.0) / 10.0;
          addedPropArr2[index] = Math.Ceiling(addedPropArr2[index] / 10.0) / 10.0;
        }
        propArr = numArray5;
        growArr = addedPropArr2;
      }
      else
      {
        for (int index = 0; index < numArray6.Length; ++index)
        {
          numArray6[index] = numArray6[index] + ((double) (_petInfo.Level - 50) * addedPropArr3[index] + 20.0 * addedPropArr2[index] + 29.0 * addedPropArr1[index] - numArray1[index]);
          addedPropArr3[index] = addedPropArr3[index] - numArray2[index];
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
        propArr[0] * Math.Pow(2.0, (double) (grade - 1)),
        0.0,
        0.0,
        0.0,
        0.0
      };
      for (int index = 1; index < 5; ++index)
        numArray[index] = propArr[index] * Math.Pow(1.5, (double) (grade - 1));
      return numArray;
    }

    public static UsersPetInfo CreatePet(
      PetTemplateInfo info,
      int userID,
      int place,
      int playerLevel)
    {
      UsersPetInfo petInfo = new UsersPetInfo();
      double num = (double) info.StarLevel * 0.1;
      Random random = new Random();
      petInfo.BloodGrow = (int) Math.Ceiling((double) (random.Next((int) ((double) info.HighBlood / (1.7 - num)), info.HighBlood - (int) ((double) info.HighBlood / 17.1)) / 10));
      petInfo.AttackGrow = random.Next((int) ((double) info.HighAttack / (1.7 - num)), info.HighAttack - (int) ((double) info.HighAttack / 17.1));
      petInfo.DefenceGrow = random.Next((int) ((double) info.HighDefence / (1.7 - num)), info.HighDefence - (int) ((double) info.HighDefence / 17.1));
      petInfo.AgilityGrow = random.Next((int) ((double) info.HighAgility / (1.7 - num)), info.HighAgility - (int) ((double) info.HighAgility / 17.1));
      petInfo.LuckGrow = random.Next((int) ((double) info.HighLuck / (1.7 - num)), info.HighLuck - (int) ((double) info.HighLuck / 17.1));
      petInfo.ID = 0;
      petInfo.Hunger = 10000;
      petInfo.TemplateID = info.TemplateID;
      petInfo.Name = info.Name;
      petInfo.UserID = userID;
      petInfo.Place = place;
      petInfo.Level = 1;
      petInfo.BuildProp(petInfo);
      petInfo.Skill = PetMgr.UpdateSkillPet(1, info.TemplateID, playerLevel);
      petInfo.SkillEquip = PetMgr.ActiveEquipSkill(1);
      return petInfo;
    }

    public static UsersPetInfo CreateNewPet()
    {
      string[] strArray = PetMgr.FindConfig("NewPet").Value.Split(',');
      int index = PetMgr.random_0.Next(strArray.Length);
      return PetMgr.CreatePet(PetMgr.FindPetTemplate(Convert.ToInt32(strArray[index])), -1, -1, 60);
    }

    public static GameNeedPetSkillInfo[] GetGameNeedPetSkill()
    {
      return PetMgr.list_0.ToArray();
    }

    public static List<GameNeedPetSkillInfo> LoadGameNeedPetSkill()
    {
      List<GameNeedPetSkillInfo> needPetSkillInfoList = new List<GameNeedPetSkillInfo>();
      List<string> stringList = new List<string>();
      foreach (PetSkillInfo petSkillInfo in PetMgr.dictionary_3.Values)
      {
        string effectPic = petSkillInfo.EffectPic;
        if (!string.IsNullOrEmpty(effectPic) && !stringList.Contains(effectPic))
        {
          needPetSkillInfoList.Add(new GameNeedPetSkillInfo()
          {
            Pic = petSkillInfo.Pic,
            EffectPic = petSkillInfo.EffectPic
          });
          stringList.Add(effectPic);
        }
      }
      foreach (PetSkillElementInfo skillElementInfo in PetMgr.dictionary_2.Values)
      {
        string effectPic = skillElementInfo.EffectPic;
        if (!string.IsNullOrEmpty(effectPic) && !stringList.Contains(effectPic))
        {
          needPetSkillInfoList.Add(new GameNeedPetSkillInfo()
          {
            Pic = skillElementInfo.Pic,
            EffectPic = skillElementInfo.EffectPic
          });
          stringList.Add(effectPic);
        }
      }
      return needPetSkillInfoList;
    }
  }
}
