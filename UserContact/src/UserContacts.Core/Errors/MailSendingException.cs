using System.Runtime.Serialization;
namespace UserContacts.Core.Errors;

[Serializable]
public class MailSendingException : BaseException
{
    public MailSendingException() { }
    public MailSendingException(string message) : base(message) { }
    public MailSendingException(string message, Exception inner) : base(message, inner) { }
    protected MailSendingException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}