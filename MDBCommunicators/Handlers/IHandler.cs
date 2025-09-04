namespace MDBCommunicators.Handlers
{
    public interface IHandler
    { 
        bool TryConsume(string text);
    }
}