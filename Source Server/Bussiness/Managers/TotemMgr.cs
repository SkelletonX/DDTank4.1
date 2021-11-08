using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Bussiness.Managers
{
    public class TotemMgr
    {
        private readonly static ILog ilog_0;

        private static Dictionary<int, TotemInfo> dictionary_0;

        private static Random random_0;

        private static ReaderWriterLock readerWriterLock_0;

        static TotemMgr()
        {

            TotemMgr.ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            TotemMgr.readerWriterLock_0 = new ReaderWriterLock();
        }

        public static TotemInfo FindTotemInfo(int id)
        {
            TotemInfo item;
            TotemMgr.readerWriterLock_0.AcquireWriterLock(-1);
            try
            {
                if (!TotemMgr.dictionary_0.ContainsKey(id))
                {
                    return null;
                }
                else
                {
                    item = TotemMgr.dictionary_0[id];
                }
            }
            finally
            {
                TotemMgr.readerWriterLock_0.ReleaseWriterLock();
            }
            return item;
        }

        public static int GetTotemProp(int id, string typeOf)
        {
            int addAttack = 0;
            for (int i = 10001; i <= id; i++)
            {
                TotemInfo totemInfo = TotemMgr.FindTotemInfo(i);
                if (typeOf == "att")
                {
                    addAttack += totemInfo.AddAttack;
                }
                if (typeOf == "gua")
                {
                    addAttack += totemInfo.AddGuard;
                }
                else if (typeOf == "agi")
                {
                    addAttack += totemInfo.AddAgility;
                }
                if (typeOf == "blo")
                {
                    addAttack += totemInfo.AddBlood;
                }
                else if (typeOf == "luc")
                {
                    addAttack += totemInfo.AddLuck;
                }
                if (typeOf == "dam")
                {
                    addAttack += totemInfo.AddDamage;
                }
                else if (typeOf == "def")
                {
                    addAttack += totemInfo.AddDefence;
                }

            }
            return addAttack;
        }

        public static bool Init()
        {
            bool flag;
            try
            {
                TotemMgr.dictionary_0 = new Dictionary<int, TotemInfo>();
                TotemMgr.random_0 = new Random();
                flag = TotemMgr.smethod_0(TotemMgr.dictionary_0);
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                if (TotemMgr.ilog_0.IsErrorEnabled)
                {
                    TotemMgr.ilog_0.Error("TotemMgr", exception);
                }
                flag = false;
            }
            return flag;
        }

        public static int MaxTotem()
        {
            return 10350;
        }

        public static bool ReLoad()
        {
            try
            {
                Dictionary<int, TotemInfo> nums = new Dictionary<int, TotemInfo>();
                if (TotemMgr.smethod_0(nums))
                {
                    try
                    {
                        TotemMgr.dictionary_0 = nums;
                        return true;
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                if (TotemMgr.ilog_0.IsErrorEnabled)
                {
                    TotemMgr.ilog_0.Error("TotemMgr", exception);
                }
            }
            return false;
        }

        private static bool smethod_0(Dictionary<int, TotemInfo> OcB1uOvYn1GxnwTrDU)
        {
            using (ProduceBussiness produceBussiness = new ProduceBussiness())
            {
                TotemInfo[] allTotem = produceBussiness.GetAllTotem();
                for (int i = 0; i < (int)allTotem.Length; i++)
                {
                    TotemInfo totemInfo = allTotem[i];
                    if (!OcB1uOvYn1GxnwTrDU.ContainsKey(totemInfo.ID))
                    {
                        OcB1uOvYn1GxnwTrDU.Add(totemInfo.ID, totemInfo);
                    }
                }
            }
            return true;
        }
    }
}