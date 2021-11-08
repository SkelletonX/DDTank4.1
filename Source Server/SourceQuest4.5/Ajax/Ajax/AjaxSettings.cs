using System.Collections;

namespace Ajax
{
	internal class AjaxSettings
	{
		private string m_CommonScript = null;

		private string m_CommonLanguage = "javascript";

		private AjaxSettingsTemporaryFiles m_TemporaryFiles = new AjaxSettingsTemporaryFiles();

		private Hashtable m_UrlNamespaceMappings = new Hashtable();

		internal string CommonScript
		{
			get
			{
				return m_CommonScript;
			}
			set
			{
				m_CommonLanguage = value;
			}
		}

		internal string ScriptLanguage
		{
			get
			{
				return m_CommonLanguage;
			}
			set
			{
				m_CommonLanguage = value;
			}
		}

		internal AjaxSettingsTemporaryFiles TemporaryFiles
		{
			get
			{
				return m_TemporaryFiles;
			}
			set
			{
				m_TemporaryFiles = value;
			}
		}

		internal Hashtable UrlNamespaceMappings
		{
			get
			{
				return m_UrlNamespaceMappings;
			}
			set
			{
				m_UrlNamespaceMappings = value;
			}
		}

		internal AjaxSettings()
		{
		}
	}
}
