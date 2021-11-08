using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bussiness.Managers
{
    public class TotemHonorMgr
    {
        private readonly static ILog ilog_0;

        private static Dictionary<int, TotemHonorTemplateInfo> dictionary_0;

        private static Random random_0;

        static TotemHonorMgr()
        {

            TotemHonorMgr.ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        public TotemHonorMgr()
        {


        }

        public static TotemHonorTemplateInfo FindTotemHonorTemplateInfo(int ID)
        {
            if (!TotemHonorMgr.dictionary_0.ContainsKey(ID))
            {
                return null;
            }
            return TotemHonorMgr.dictionary_0[ID];
        }

        public static bool Init()
        {
            bool flag;
            try
            {
                TotemHonorMgr.dictionary_0 = new Dictionary<int, TotemHonorTemplateInfo>();
                TotemHonorMgr.random_0 = new Random();
                flag = TotemHonorMgr.smethod_0(TotemHonorMgr.dictionary_0);
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                if (TotemHonorMgr.ilog_0.IsErrorEnabled)
                {
                    TotemHonorMgr.ilog_0.Error("TotemHonorMgr", exception);
                }
                flag = false;
            }
            return flag;
        }

        public static bool ReLoad()
        {
            try
            {
                Dictionary<int, TotemHonorTemplateInfo> nums = new Dictionary<int, TotemHonorTemplateInfo>();
                if (TotemHonorMgr.smethod_0(nums))
                {
                    try
                    {
                        TotemHonorMgr.dictionary_0 = nums;
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
                if (TotemHonorMgr.ilog_0.IsErrorEnabled)
                {
                    TotemHonorMgr.ilog_0.Error("TotemHonorMgr", exception);
                }
            }
            return false;
        }

        private static bool smethod_0(Dictionary<int, TotemHonorTemplateInfo> FRaJ0V7QZuRXWJvZwy)
        {
            using (ProduceBussiness produceBussiness = new ProduceBussiness())
            {
                TotemHonorTemplateInfo[] allTotemHonorTemplate = produceBussiness.GetAllTotemHonorTemplate();
                for (int i = 0; i < (int)allTotemHonorTemplate.Length; i++)
                {
                    TotemHonorTemplateInfo totemHonorTemplateInfo = allTotemHonorTemplate[i];
                    if (!FRaJ0V7QZuRXWJvZwy.ContainsKey(totemHonorTemplateInfo.ID))
                    {
                        FRaJ0V7QZuRXWJvZwy.Add(totemHonorTemplateInfo.ID, totemHonorTemplateInfo);
                    }
                }
            }
            return true;
        }
    }
}