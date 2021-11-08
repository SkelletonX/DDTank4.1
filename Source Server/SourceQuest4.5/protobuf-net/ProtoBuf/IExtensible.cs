namespace ProtoBuf
{
	public interface IExtensible
	{
		IExtension GetExtensionObject(bool createIfMissing);
	}
}
