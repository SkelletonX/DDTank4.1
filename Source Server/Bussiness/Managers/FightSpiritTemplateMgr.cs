using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Bussiness.Managers
{
    public class FightSpiritTemplateMgr
    {
        private static readonly ILog ilog_0;
        private static Dictionary<int, List<FightSpiritTemplateInfo>> dictionary_0;
        private static ReaderWriterLock readerWriterLock_0;

        public static bool ReLoad()
        {
            try
            {
                FightSpiritTemplateInfo[] fightSpiritTemplates = FightSpiritTemplateMgr.LoadFightSpiritTemplateDb();
                Dictionary<int, List<FightSpiritTemplateInfo>> dictionary = FightSpiritTemplateMgr.LoadFightSpiritTemplates(fightSpiritTemplates);
                if (fightSpiritTemplates.Length != 0)
                    Interlocked.Exchange<Dictionary<int, List<FightSpiritTemplateInfo>>>(ref FightSpiritTemplateMgr.dictionary_0, dictionary);
            }
            catch (Exception ex)
            {
                if (FightSpiritTemplateMgr.ilog_0.IsErrorEnabled)
                    FightSpiritTemplateMgr.ilog_0.Error((object)"ReLoad FightSpiritTemplate", ex);
                return false;
            }
            return true;
        }

        public static bool Init()
        {
            return FightSpiritTemplateMgr.ReLoad();
        }

        public static FightSpiritTemplateInfo[] LoadFightSpiritTemplateDb()
        {
            using (ProduceBussiness produceBussiness = new ProduceBussiness())
                return produceBussiness.GetAllFightSpiritTemplate();
        }

        public static Dictionary<int, List<FightSpiritTemplateInfo>> LoadFightSpiritTemplates(FightSpiritTemplateInfo[] fightSpiritTemplates)
        {
            Dictionary<int, List<FightSpiritTemplateInfo>> dictionary = new Dictionary<int, List<FightSpiritTemplateInfo>>();
            foreach (FightSpiritTemplateInfo fightSpiritTemplate in fightSpiritTemplates)
            {
                FightSpiritTemplateInfo info = fightSpiritTemplate;
                if (!dictionary.Keys.Contains<int>(info.FightSpiritID))
                {
                    IEnumerable<FightSpiritTemplateInfo> source = ((IEnumerable<FightSpiritTemplateInfo>)fightSpiritTemplates).Where<FightSpiritTemplateInfo>((Func<FightSpiritTemplateInfo, bool>)(s => s.FightSpiritID == info.FightSpiritID));
                    dictionary.Add(info.FightSpiritID, source.ToList<FightSpiritTemplateInfo>());
                }
            }
            return dictionary;
        }

        public static List<FightSpiritTemplateInfo> FindFightSpiritTemplates(int id)
        {
            FightSpiritTemplateMgr.readerWriterLock_0.AcquireWriterLock(-1);
            try
            {
                if (FightSpiritTemplateMgr.dictionary_0.ContainsKey(id))
                    return FightSpiritTemplateMgr.dictionary_0[id];
            }
            finally
            {
                FightSpiritTemplateMgr.readerWriterLock_0.ReleaseWriterLock();
            }
            return new List<FightSpiritTemplateInfo>();
        }

        public static FightSpiritTemplateInfo FindFightSpiritTemplateInfo(int FigSpiritId, int lv)
        {
            foreach (FightSpiritTemplateInfo fightSpiritTemplate in FightSpiritTemplateMgr.FindFightSpiritTemplates(FigSpiritId))
            {
                if (fightSpiritTemplate.Level == lv)
                    return fightSpiritTemplate;
            }
            return (FightSpiritTemplateInfo)null;
        }

        public static int GOLDEN_LEVEL(int lv)
        {
            try
            {
                string spiritLevelAddDamage = GameProperties.FightSpiritLevelAddDamage;
                char[] chArray = new char[1] { '|' };
                foreach (string str in spiritLevelAddDamage.Split(chArray))
                {
                    if (str.Split(',')[0] == lv.ToString())
                        return int.Parse(str.Split(',')[1]);
                }
            }
            catch (Exception ex)
            {
                if (FightSpiritTemplateMgr.ilog_0.IsErrorEnabled)
                    FightSpiritTemplateMgr.ilog_0.Error((object)"FightSpiritTemplate.GOLDEN_LEVEL: ", ex);
            }
            return 0;
        }

        public static int[] Exps()
        {
            List<FightSpiritTemplateInfo> fightSpiritTemplates = FightSpiritTemplateMgr.FindFightSpiritTemplates(100001);
            List<int> intList = new List<int>();
            foreach (FightSpiritTemplateInfo spiritTemplateInfo in fightSpiritTemplates)
                intList.Add(spiritTemplateInfo.Exp);
            return intList.ToArray();
        }

        public static int GetProp(int figSpiritId, int lv, int place, ref int addAtt, ref int rdcDama)
        {
            FightSpiritTemplateInfo spiritTemplateInfo = FightSpiritTemplateMgr.FindFightSpiritTemplateInfo(figSpiritId, lv);
            if (spiritTemplateInfo == null)
            {
                List<FightSpiritTemplateInfo> fightSpiritTemplates = FightSpiritTemplateMgr.FindFightSpiritTemplates(figSpiritId);
                if (fightSpiritTemplates.Count > 0)
                {
                    spiritTemplateInfo = fightSpiritTemplates[fightSpiritTemplates.Count - 1];
                    FightSpiritTemplateMgr.ilog_0.ErrorFormat("FigSpiritId: {0}, level: {1} not found! Return Max level in database is {2}", (object)figSpiritId, (object)lv, (object)spiritTemplateInfo.Level);
                }
                else
                {
                    FightSpiritTemplateMgr.ilog_0.ErrorFormat("FigSpiritId: {0} not found! Return 0", (object)figSpiritId);
                    return 0;
                }
            }
            if (figSpiritId != 100001 && figSpiritId != 100003)
                rdcDama = rdcDama + FightSpiritTemplateMgr.GOLDEN_LEVEL(lv);
            else
                addAtt = addAtt + FightSpiritTemplateMgr.GOLDEN_LEVEL(lv);
            switch (place)
            {
                case 2:
                    return spiritTemplateInfo.Attack;
                case 3:
                    return spiritTemplateInfo.Lucky;
                case 5:
                    return spiritTemplateInfo.Agility;
                case 11:
                    return spiritTemplateInfo.Defence;
                case 13:
                    return spiritTemplateInfo.Blood;
                default:
                    return 0;
            }
        }

        static FightSpiritTemplateMgr()
        {

            FightSpiritTemplateMgr.ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            FightSpiritTemplateMgr.dictionary_0 = new Dictionary<int, List<FightSpiritTemplateInfo>>();
            FightSpiritTemplateMgr.readerWriterLock_0 = new ReaderWriterLock();
        }
    }
}
