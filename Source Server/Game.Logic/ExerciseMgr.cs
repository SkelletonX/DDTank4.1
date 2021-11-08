// Decompiled with JetBrains decompiler
// Type: Game.Logic.ExerciseMgr
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Game.Logic
{
  public class ExerciseMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static List<EliteGameRoundInfo> list_0 = new List<EliteGameRoundInfo>();
    private static Dictionary<int, PlayerEliteGameInfo> dictionary_1 = new Dictionary<int, PlayerEliteGameInfo>();
    private static Dictionary<int, ExerciseInfo> _exercises;
    private static ReaderWriterLock m_lock;
    private static ThreadSafeRandom rand;

    public static int EliteStatus { get; set; }

    public static ExerciseInfo FindExercise(int Grage)
    {
      if (Grage == 0)
        Grage = 1;
      ExerciseMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        if (ExerciseMgr._exercises.ContainsKey(Grage))
          return ExerciseMgr._exercises[Grage];
      }
      catch
      {
      }
      finally
      {
        ExerciseMgr.m_lock.ReleaseReaderLock();
      }
      return (ExerciseInfo) null;
    }

    public static int GetExercise(int GP, string type)
    {
      int num = 0;
      for (int Grage = 1; Grage <= ExerciseMgr.GetMaxLevel() && ExerciseMgr.FindExercise(Grage).GP < GP; ++Grage)
      {
        if (type != null)
        {
          if (type != "A")
          {
            if (type != "AG")
            {
              if (type != "D")
              {
                if (type != "H")
                {
                  if (type == "L")
                    num = ExerciseMgr.FindExercise(Grage).ExerciseL;
                }
                else
                  num = ExerciseMgr.FindExercise(Grage).ExerciseH;
              }
              else
                num = ExerciseMgr.FindExercise(Grage).ExerciseD;
            }
            else
              num = ExerciseMgr.FindExercise(Grage).ExerciseAG;
          }
          else
            num = ExerciseMgr.FindExercise(Grage).ExerciseA;
        }
      }
      return num;
    }

    public static int GetMaxLevel()
    {
      if (ExerciseMgr._exercises == null)
        ExerciseMgr.Init();
      return ExerciseMgr._exercises.Values.Count;
    }

    public static int getLv(int exp)
    {
      int num = 0;
      for (int index = 1; index <= ExerciseMgr.GetMaxLevel(); index = index + 1 + 1)
      {
        ExerciseInfo exercise = ExerciseMgr._exercises[index];
        if (exp >= exercise.GP)
          num = index;
        else
          break;
      }
      return num;
    }

    public static int getExp(int type, TexpInfo self)
    {
      switch (type)
      {
        case 0:
          return self.hpTexpExp;
        case 1:
          return self.attTexpExp;
        case 2:
          return self.defTexpExp;
        case 3:
          return self.spdTexpExp;
        case 4:
          return self.lukTexpExp;
        default:
          return 0;
      }
    }

    public static bool isUp(int type, int oldExp, TexpInfo self)
    {
      return ExerciseMgr.getLv(ExerciseMgr.getExp(type, self)) > ExerciseMgr.getLv(oldExp);
    }

    public static bool Init()
    {
      try
      {
        ExerciseMgr.m_lock = new ReaderWriterLock();
        ExerciseMgr._exercises = new Dictionary<int, ExerciseInfo>();
        ExerciseMgr.rand = new ThreadSafeRandom();
        ExerciseMgr.EliteStatus = 0;
        return ExerciseMgr.LoadExercise(ExerciseMgr._exercises);
      }
      catch (Exception ex)
      {
        if (ExerciseMgr.log.IsErrorEnabled)
          ExerciseMgr.log.Error((object) "ExercisesMgr", ex);
        return false;
      }
    }

    private static bool LoadExercise(Dictionary<int, ExerciseInfo> Exercise)
    {
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (ExerciseInfo exerciseInfo in produceBussiness.GetAllExercise())
        {
          if (!Exercise.ContainsKey(exerciseInfo.Grage))
            Exercise.Add(exerciseInfo.Grage, exerciseInfo);
        }
      }
      return true;
    }

    public static bool ReLoad()
    {
      try
      {
        Dictionary<int, ExerciseInfo> Exercise = new Dictionary<int, ExerciseInfo>();
        if (ExerciseMgr.LoadExercise(Exercise))
        {
          ExerciseMgr.m_lock.AcquireWriterLock(-1);
          try
          {
            ExerciseMgr._exercises = Exercise;
            return true;
          }
          catch
          {
          }
          finally
          {
            ExerciseMgr.m_lock.ReleaseWriterLock();
          }
        }
      }
      catch (Exception ex)
      {
        if (ExerciseMgr.log.IsErrorEnabled)
          ExerciseMgr.log.Error((object) nameof (ExerciseMgr), ex);
      }
      return false;
    }

    public static void ResetEliteGame()
    {
      ExerciseMgr.list_0 = new List<EliteGameRoundInfo>();
      ExerciseMgr.dictionary_1 = new Dictionary<int, PlayerEliteGameInfo>();
    }

    public static void SynEliteGameChampionPlayerList(
      Dictionary<int, PlayerEliteGameInfo> tempPlayerList)
    {
      ExerciseMgr.m_lock.AcquireReaderLock(-1);
      try
      {
        ExerciseMgr.dictionary_1 = tempPlayerList;
      }
      catch
      {
      }
      finally
      {
        ExerciseMgr.m_lock.ReleaseReaderLock();
      }
    }

    public static void UpdateEliteGameChapionPlayerList(PlayerEliteGameInfo p)
    {
      ExerciseMgr.m_lock.AcquireReaderLock(-1);
      try
      {
        if (ExerciseMgr.dictionary_1.ContainsKey(p.UserID))
          ExerciseMgr.dictionary_1[p.UserID] = p;
        else
          ExerciseMgr.dictionary_1.Add(p.UserID, p);
      }
      catch
      {
      }
      finally
      {
        ExerciseMgr.m_lock.ReleaseReaderLock();
      }
    }

    public static Dictionary<int, PlayerEliteGameInfo> EliteGameChampionPlayersList
    {
      get
      {
        return ExerciseMgr.dictionary_1;
      }
    }

    public static EliteGameRoundInfo FindEliteRoundByUser(int userId)
    {
      ExerciseMgr.m_lock.AcquireReaderLock(-1);
      try
      {
        foreach (EliteGameRoundInfo eliteGameRoundInfo in ExerciseMgr.list_0.OrderByDescending<EliteGameRoundInfo, int>((Func<EliteGameRoundInfo, int>) (a => a.RoundType)).ToList<EliteGameRoundInfo>())
        {
          if (eliteGameRoundInfo.PlayerOne.UserID == userId || eliteGameRoundInfo.PlayerTwo.UserID == userId)
            return eliteGameRoundInfo;
        }
      }
      catch (Exception ex)
      {
        ExerciseMgr.log.Error((object) ex.ToString());
      }
      finally
      {
        ExerciseMgr.m_lock.ReleaseReaderLock();
      }
      return (EliteGameRoundInfo) null;
    }

    public static void AddEliteRound(EliteGameRoundInfo elite)
    {
      ExerciseMgr.m_lock.AcquireReaderLock(-1);
      try
      {
        ExerciseMgr.list_0.Add(elite);
      }
      catch
      {
      }
      finally
      {
        ExerciseMgr.m_lock.ReleaseReaderLock();
      }
    }

    public static void RemoveEliteRound(EliteGameRoundInfo elite)
    {
      ExerciseMgr.m_lock.AcquireReaderLock(-1);
      try
      {
        if (!ExerciseMgr.list_0.Contains(elite))
          return;
        ExerciseMgr.list_0.Remove(elite);
      }
      catch
      {
      }
      finally
      {
        ExerciseMgr.m_lock.ReleaseReaderLock();
      }
    }

    public static bool IsBlockWeapon(int templateid)
    {
      bool flag = false;
      if (((IEnumerable<string>) GameProperties.EliteGameBlockWeapon.Split('|')).Contains<string>(templateid.ToString()))
        flag = true;
      return flag;
    }
  }
}
