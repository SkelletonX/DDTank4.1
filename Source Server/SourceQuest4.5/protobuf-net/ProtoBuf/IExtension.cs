using System.IO;

namespace ProtoBuf
{
	public interface IExtension
	{
		Stream BeginAppend();

		void EndAppend(Stream stream, bool commit);

		Stream BeginQuery();

		void EndQuery(Stream stream);

		int GetLength();
	}
}
