namespace Newtonsoft.Json.Utilities
{
	internal struct StringReference
	{
		private readonly char[] _chars;

		private readonly int _startIndex;

		private readonly int _length;

		public char[] Chars => _chars;

		public int StartIndex => _startIndex;

		public int Length => _length;

		public StringReference(char[] chars, int startIndex, int length)
		{
			_chars = chars;
			_startIndex = startIndex;
			_length = length;
		}

		public override string ToString()
		{
			return new string(_chars, _startIndex, _length);
		}
	}
}
