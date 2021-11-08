using System;

namespace Ajax
{
	[Obsolete("Please use [AjaxMethod(HttpSessionStateRequirement.ReadWrite)] instead.", true)]
	[AttributeUsage(AttributeTargets.Method)]
	public class AjaxRequireSessionStateAttribute : Attribute
	{
	}
}
