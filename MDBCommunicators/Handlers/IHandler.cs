namespace MDBCommunicators.Handlers;

public interface IHandler
{
    public bool TryConsume(string text);
}