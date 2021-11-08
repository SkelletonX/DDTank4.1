// Decompiled with JetBrains decompiler
// Type: Bussiness.LanguageMgr
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using log4net;
using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Bussiness
{
  public class LanguageMgr
  {
    private static Hashtable LangsSentences = new Hashtable();
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public static string GetTranslation(string translateId, params object[] args)
    {
      if (LanguageMgr.LangsSentences.ContainsKey((object) translateId))
      {
        string format = (string) LanguageMgr.LangsSentences[(object) translateId];
        try
        {
          format = string.Format(format, args);
        }
        catch (Exception ex)
        {
          LanguageMgr.log.Error((object) ("Parameters number error, ID: " + translateId + " (Arg count=" + (object) args.Length + ")"), ex);
        }
        if (format != null)
          return format;
      }
      return translateId;
    }

    private static Hashtable LoadLanguage(string path)
    {
      Hashtable hashtable = new Hashtable();
      string path1 = path + LanguageMgr.LanguageFile;
      if (!File.Exists(path1))
      {
        LanguageMgr.log.Error((object) ("Language file : " + path1 + " not found !"));
        return hashtable;
      }
      foreach (string str in (IEnumerable) new ArrayList((ICollection) File.ReadAllLines(path1, Encoding.UTF8)))
      {
        if (!str.StartsWith("#") && str.IndexOf(':') != -1)
        {
          string[] strArray = new string[2]
          {
            str.Substring(0, str.IndexOf(':')),
            str.Substring(str.IndexOf(':') + 1)
          };
          strArray[1] = strArray[1].Replace("\t", "");
          hashtable[(object) strArray[0]] = (object) strArray[1];
        }
      }
      return hashtable;
    }

    public static bool Reload(string path)
    {
      try
      {
        Hashtable hashtable = LanguageMgr.LoadLanguage(path);
        if (hashtable.Count > 0)
        {
          Interlocked.Exchange<Hashtable>(ref LanguageMgr.LangsSentences, hashtable);
          return true;
        }
      }
      catch (Exception ex)
      {
        LanguageMgr.log.Error((object) "Load language file error:", ex);
      }
      return false;
    }

    public static bool Setup(string path)
    {
      return LanguageMgr.Reload(path);
    }

    private static string LanguageFile
    {
      get
      {
        return ConfigurationManager.AppSettings["LanguagePath"];
      }
    }
  }
}
