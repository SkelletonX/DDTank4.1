using System;
using System.Text;

namespace Ajax.JSON
{
	public interface IAjaxObjectConverter
	{
		Type[] SupportedTypes
		{
			get;
		}

		bool IncludeSubclasses
		{
			get;
		}

		string ClientScriptIdentifier
		{
			get;
		}

		void ToJSON(ref StringBuilder sb, object o);

		object FromString(string s, Type t);

		void RenderClientScript(ref StringBuilder sb);
	}
}
