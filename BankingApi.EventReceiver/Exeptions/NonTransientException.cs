namespace BankingApi.EventReceiver.Exeptions;
public class NonTransientException : Exception
{
    public NonTransientException(string message) : base(message) { }
    public NonTransientException(string message, Exception ex) : base(message, ex) { }
}