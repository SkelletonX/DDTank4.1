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

		private static string LanguageFile => ConfigurationManager.AppSettings["LanguagePath"];

		public static string GetTranslation(string translateId, params object[] args)
		{
			if (LangsSentences.ContainsKey(translateId))
			{
				string format = (string)LangsSentences[translateId];
				try
				{
					format = string.Format(format, args);
				}
				catch (Exception exception)
				{
					log.Error("Parameters number error, ID: " + translateId + " (Arg count=" + args.Length + ")", exception);
				}
				if (format != null)
				{
					return format;
				}
			}
			return translateId;
		}

		private static Hashtable LoadLanguage(string path)
		{
			Hashtable hashtable = new Hashtable();
			string str = path + LanguageFile;
			if (!File.Exists(str))
			{
				log.Error("Language file : " + str + " not found !");
				return hashtable;
			}
			foreach (string str2 in (IEnumerable)new ArrayList(File.ReadAllLines(str, Encoding.UTF8)))
			{
				if (!str2.StartsWith("#") && str2.IndexOf(':') != -1)
				{
					string[] strArray2 = new string[2]
					{
						str2.Substring(0, str2.IndexOf(':')),
						str2.Substring(str2.IndexOf(':') + 1)
					};
					strArray2[1] = strArray2[1].Replace("\t", "");
					hashtable[strArray2[0]] = strArray2[1];
				}
			}
			return hashtable;
		}

		public static bool Reload(string path)
		{
			try
			{
				Hashtable hashtable = LoadLanguage(path);
				if (hashtable.Count > 0)
				{
					Interlocked.Exchange(ref LangsSentences, hashtable);
					return true;
				}
			}
			catch (Exception exception)
			{
				log.Error("Load language file error:", exception);
			}
			return false;
		}

		public static bool Setup(string path)
		{
			return Reload(path);
		}
	}
}
