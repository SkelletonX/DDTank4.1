using System;
using System.ServiceModel.Configuration;

namespace ProtoBuf.ServiceModel
{
	public class ProtoBehaviorExtension : BehaviorExtensionElement
	{
		public override Type BehaviorType => typeof(ProtoEndpointBehavior);

		protected override object CreateBehavior()
		{
			return new ProtoEndpointBehavior();
		}
	}
}
