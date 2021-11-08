using Road.Flash;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using zlib;

namespace Tank.Request
{
	public class StaticFunction
	{
		public static RSACryptoServiceProvider RsaCryptor => CryptoHelper.GetRSACrypto(ConfigurationManager.AppSettings["privateKey"]);

		public static byte[] Compress(string str)
		{
			return Compress(Encoding.UTF8.GetBytes(str));
		}

		public static byte[] Compress(byte[] src)
		{
			return Compress(src, 0, src.Length);
		}

		public static byte[] Compress(byte[] src, int offset, int length)
		{
			MemoryStream memoryStream = new MemoryStream();
			ZOutputStream zOutputStream = new ZOutputStream(memoryStream, 9);
			zOutputStream.Write(src, offset, length);
			zOutputStream.Close();
			return memoryStream.ToArray();
		}

		public static string Uncompress(string str)
		{
			return Encoding.UTF8.GetString(Uncompress(Encoding.UTF8.GetBytes(str)));
		}

		public static byte[] Uncompress(byte[] src)
		{
			MemoryStream memoryStream = new MemoryStream();
			ZOutputStream zOutputStream = new ZOutputStream(memoryStream);
			zOutputStream.Write(src, 0, src.Length);
			zOutputStream.Close();
			return memoryStream.ToArray();
		}
	}
}
