using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Bussiness.Managers
{
    public class PetMoePropertyMgr
    {
        private readonly static ILog ilog_0;

        private static Dictionary<int, PetMoePropertyInfo> dictionary_0;

        private static Random random_0;

        private static ReaderWriterLock readerWriterLock_0;

        static PetMoePropertyMgr()
        {

            PetMoePropertyMgr.ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            PetMoePropertyMgr.dictionary_0 = new Dictionary<int, PetMoePropertyInfo>();
            PetMoePropertyMgr.random_0 = new Random();
            PetMoePropertyMgr.readerWriterLock_0 = new ReaderWriterLock();
        }

        public PetMoePropertyMgr()
        {


        }

        public static int FindMaxLevel()
        {
            return PetMoePropertyMgr.dictionary_0.Count;
        }

        public static PetMoePropertyInfo FindPetMoeProperty(int Level)
        {
            PetMoePropertyInfo item;
            PetMoePropertyMgr.readerWriterLock_0.AcquireWriterLock(-1);
            try
            {
                if (!PetMoePropertyMgr.dictionary_0.ContainsKey(Level))
                {
                    return null;
                }
                else
                {
                    item = PetMoePropertyMgr.dictionary_0[Level];
                }
            }
            finally
            {
                PetMoePropertyMgr.readerWriterLock_0.ReleaseWriterLock();
            }
            return item;
        }

        public static PetMoePropertyInfo FindPetMoePropertyByGp(int exp)
        {
            PetMoePropertyInfo petMoePropertyInfo = PetMoePropertyMgr.FindPetMoeProperty(PetMoePropertyMgr.FindMaxLevel());
            if (petMoePropertyInfo != null && exp >= petMoePropertyInfo.Exp)
            {
                return petMoePropertyInfo;
            }
            for (int i = 1; i <= PetMoePropertyMgr.dictionary_0.Count; i++)
            {
                if (PetMoePropertyMgr.dictionary_0.ContainsKey(i) && exp < PetMoePropertyMgr.dictionary_0[i].Exp)
                {
                    if (i == 1)
                    {
                        return null;
                    }
                    return PetMoePropertyMgr.dictionary_0[i - 1];
                }
            }
            return null;
        }

        public static bool Init()
        {
            return PetMoePropertyMgr.ReLoad();
        }

        public static PetMoePropertyInfo[] LoadPetMoePropertyDb()
        {
            PetMoePropertyInfo[] allPetMoeProperty;
            using (ProduceBussiness produceBussiness = new ProduceBussiness())
            {
                allPetMoeProperty = produceBussiness.GetAllPetMoeProperty();
            }
            return allPetMoeProperty;
        }

        public static Dictionary<int, PetMoePropertyInfo> LoadPetMoePropertys(PetMoePropertyInfo[] PetMoeProperty)
        {
            Dictionary<int, PetMoePropertyInfo> nums = new Dictionary<int, PetMoePropertyInfo>();
            PetMoePropertyInfo[] petMoeProperty = PetMoeProperty;
            for (int i = 0; i < (int)petMoeProperty.Length; i++)
            {
                PetMoePropertyInfo petMoePropertyInfo = petMoeProperty[i];
                if (!nums.Keys.Contains<int>(petMoePropertyInfo.Level))
                {
                    nums.Add(petMoePropertyInfo.Level, petMoePropertyInfo);
                }
            }
            return nums;
        }

        public static bool ReLoad()
        {
            try
            {
                PetMoePropertyInfo[] petMoePropertyInfoArray = PetMoePropertyMgr.LoadPetMoePropertyDb();
                Dictionary<int, PetMoePropertyInfo> nums = PetMoePropertyMgr.LoadPetMoePropertys(petMoePropertyInfoArray);
                if (petMoePropertyInfoArray.Length != 0)
                {
                    Interlocked.Exchange<Dictionary<int, PetMoePropertyInfo>>(ref PetMoePropertyMgr.dictionary_0, nums);
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                if (PetMoePropertyMgr.ilog_0.IsErrorEnabled)
                {
                    PetMoePropertyMgr.ilog_0.Error("ReLoad PetMoeProperty", exception);
                }
                return false;
            }
            return true;
        }
    }
}