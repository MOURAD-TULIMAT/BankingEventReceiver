namespace BankingApi.EventReceiver.Exeptions;
public class TransientException : Exception
{
    public TransientException(string message) : base(message) { }
    public TransientException(string message, Exception ex) : base(message, ex) { }
}