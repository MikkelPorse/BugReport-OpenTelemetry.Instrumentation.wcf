using System.Net.Security;
using System.ServiceModel;
using OpenTelemetry.Instrumentation.Wcf;

namespace OpenTelemetry.Instrumentation.Wcf_bug
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			var binding = new NetTcpBinding(SecurityMode.TransportWithMessageCredential);
			binding.TransferMode = TransferMode.Buffered;
			binding.Security.Transport.ProtectionLevel = ProtectionLevel.EncryptAndSign;
			binding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
			
			EndpointAddress endpoint = new EndpointAddress("net.tcp://anything.will.do:1234");
			var factory = new ChannelFactory<IDummyService>(binding, endpoint);
			factory.Endpoint.Behaviors.Add(new TelemetryEndpointBehavior());
			factory.CreateChannel().Ping();
		}
	}

	[ServiceContract]
	interface IDummyService
	{
		[OperationContract]
		void Ping();
	}
}
