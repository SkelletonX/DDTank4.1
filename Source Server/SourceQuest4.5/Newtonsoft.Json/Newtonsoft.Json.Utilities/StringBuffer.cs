using System;

namespace Newtonsoft.Json.Utilities
{
	internal class StringBuffer
	{
		private char[] _buffer;

		private int _position;

		private static readonly char[] EmptyBuffer = new char[0];

		public int Position
		{
			get
			{
				return _position;
			}
			set
			{
				_position = value;
			}
		}

		public StringBuffer()
		{
			_buffer = EmptyBuffer;
		}

		public StringBuffer(int initalSize)
		{
			_buffer = new char[initalSize];
		}

		public void Append(char value)
		{
			if (_position == _buffer.Length)
			{
				EnsureSize(1);
			}
			_buffer[_position++] = value;
		}

		public void Append(char[] buffer, int startIndex, int count)
		{
			if (_position + count >= _buffer.Length)
			{
				EnsureSize(count);
			}
			Array.Copy(buffer, startIndex, _buffer, _position, count);
			_position += count;
		}

		public void Clear()
		{
			_buffer = EmptyBuffer;
			_position = 0;
		}

		private void EnsureSize(int appendLength)
		{
			char[] array = new char[(_position + appendLength) * 2];
			Array.Copy(_buffer, array, _position);
			_buffer = array;
		}

		public override string ToString()
		{
			return ToString(0, _position);
		}

		public string ToString(int start, int length)
		{
			return new string(_buffer, start, length);
		}

		public char[] GetInternalBuffer()
		{
			return _buffer;
		}
	}
}
