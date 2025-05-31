using System.Runtime.Serialization;

namespace UserContacts.Core.Errors;

[Serializable]
public class InvalidImageException : BaseException
{
    public InvalidImageException() { }
    public InvalidImageException(string message) : base(message) { }
    public InvalidImageException(string message, Exception ex) : base(message, ex) { }
    protected InvalidImageException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
