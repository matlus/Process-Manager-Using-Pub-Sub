using System.Runtime.Serialization;

namespace PostBindOrchestrator.Core;

[Serializable]
public class MessageHeaderKeyNotFoundException : PostBindOrchestratorTechnicalBaseException
{
	public MessageHeaderKeyNotFoundException() { }
	public MessageHeaderKeyNotFoundException(string message) : base(message) { }
	public MessageHeaderKeyNotFoundException(string message, Exception inner) : base(message, inner) { }
	protected MessageHeaderKeyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public override string Reason => "Message Header Key Not Found.";
}