namespace Ajax
{
	internal class AjaxSettingsTemporaryFiles
	{
		private string m_Path = "~/images";

		private int m_DeleteAfter = 60;

		internal string Path
		{
			get
			{
				return m_Path;
			}
			set
			{
				m_Path = value;
			}
		}

		internal int DeleteAfter
		{
			get
			{
				return m_DeleteAfter;
			}
			set
			{
				m_DeleteAfter = value;
			}
		}

		internal AjaxSettingsTemporaryFiles()
		{
		}

		internal AjaxSettingsTemporaryFiles(string path, int deleteAfter)
		{
			m_Path = path;
			m_DeleteAfter = deleteAfter;
		}
	}
}
