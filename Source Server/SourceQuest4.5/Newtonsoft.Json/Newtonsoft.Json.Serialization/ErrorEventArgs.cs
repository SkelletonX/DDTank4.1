using System;

namespace Newtonsoft.Json.Serialization
{
	public class ErrorEventArgs : EventArgs
	{
		public object CurrentObject
		{
			get;
			private set;
		}

		public ErrorContext ErrorContext
		{
			get;
			private set;
		}

		public ErrorEventArgs(object currentObject, ErrorContext errorContext)
		{
			CurrentObject = currentObject;
			ErrorContext = errorContext;
		}
	}
}
