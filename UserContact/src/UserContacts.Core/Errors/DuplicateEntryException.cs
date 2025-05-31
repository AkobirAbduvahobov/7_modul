using System.Runtime.Serialization;

namespace UserContacts.Core.Errors;

[Serializable]
public class DuplicateEntryException : NotAllowedException
{
    public DuplicateEntryException() { }
    public DuplicateEntryException(string message) : base(message) { }
    public DuplicateEntryException(string message, Exception inner) : base(message, inner) { }
    protected DuplicateEntryException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}