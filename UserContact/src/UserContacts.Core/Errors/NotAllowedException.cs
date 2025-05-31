using System.Runtime.Serialization;

namespace UserContacts.Core.Errors;

[Serializable]
public class NotAllowedException : BaseException
{
    public NotAllowedException() { }
    public NotAllowedException(string message) : base(message) { }
    public NotAllowedException(string message, Exception inner) : base(message, inner) { }
    protected NotAllowedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}