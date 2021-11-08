namespace Newtonsoft.Json.Utilities
{
	internal class EnumValue<T> where T : struct
	{
		private readonly string _name;

		private readonly T _value;

		public string Name => _name;

		public T Value => _value;

		public EnumValue(string name, T value)
		{
			_name = name;
			_value = value;
		}
	}
}
