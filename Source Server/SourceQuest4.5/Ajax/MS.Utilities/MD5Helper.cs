using System.Security.Cryptography;

namespace MS.Utilities
{
	internal class MD5Helper
	{
		internal static string GetHash(byte[] data)
		{
			string text = "";
			string[] array = new string[16];
			MD5 mD = new MD5CryptoServiceProvider();
			byte[] array2 = mD.ComputeHash(data);
			for (int i = 0; i < array2.Length; i++)
			{
				array[i] = array2[i].ToString("x");
				text += array[i];
			}
			return text;
		}
	}
}
