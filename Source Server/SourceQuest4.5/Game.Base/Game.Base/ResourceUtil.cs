using System.IO;
using System.Reflection;

namespace Game.Base
{
	public class ResourceUtil
	{
		public static Stream GetResourceStream(string fileName, Assembly assem)
		{
			fileName = fileName.ToLower();
			string[] manifestResourceNames = assem.GetManifestResourceNames();
			foreach (string text in manifestResourceNames)
			{
				if (text.ToLower().EndsWith(fileName))
				{
					return assem.GetManifestResourceStream(text);
				}
			}
			return null;
		}

		public static void ExtractResource(string fileName, Assembly assembly)
		{
			ExtractResource(fileName, fileName, assembly);
		}

		public static void ExtractResource(string resourceName, string fileName, Assembly assembly)
		{
			FileInfo fileInfo = new FileInfo(fileName);
			if (!fileInfo.Directory.Exists)
			{
				fileInfo.Directory.Create();
			}
			using (StreamReader streamReader = new StreamReader(GetResourceStream(resourceName, assembly)))
			{
				using (StreamWriter streamWriter = new StreamWriter(File.Create(fileName)))
				{
					streamWriter.Write(streamReader.ReadToEnd());
				}
			}
		}
	}
}
