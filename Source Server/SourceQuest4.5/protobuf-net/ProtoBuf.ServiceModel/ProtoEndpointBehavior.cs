using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace ProtoBuf.ServiceModel
{
	public class ProtoEndpointBehavior : IEndpointBehavior
	{
		void IEndpointBehavior.AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
		{
		}

		void IEndpointBehavior.ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
		{
			ReplaceDataContractSerializerOperationBehavior(endpoint);
		}

		void IEndpointBehavior.ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
		{
			ReplaceDataContractSerializerOperationBehavior(endpoint);
		}

		void IEndpointBehavior.Validate(ServiceEndpoint endpoint)
		{
		}

		private static void ReplaceDataContractSerializerOperationBehavior(ServiceEndpoint serviceEndpoint)
		{
			foreach (OperationDescription operationDescription in serviceEndpoint.Contract.Operations)
			{
				ReplaceDataContractSerializerOperationBehavior(operationDescription);
			}
		}

		private static void ReplaceDataContractSerializerOperationBehavior(OperationDescription description)
		{
			DataContractSerializerOperationBehavior dcsOperationBehavior = description.Behaviors.Find<DataContractSerializerOperationBehavior>();
			if (dcsOperationBehavior != null)
			{
				description.Behaviors.Remove(dcsOperationBehavior);
				ProtoOperationBehavior newBehavior = new ProtoOperationBehavior(description);
				newBehavior.MaxItemsInObjectGraph = dcsOperationBehavior.MaxItemsInObjectGraph;
				description.Behaviors.Add(newBehavior);
			}
		}
	}
}
