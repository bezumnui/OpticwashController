namespace MDBCommunicators.Handlers;

public interface IResponseReceiver
{
    bool TryGetFirstMessageById(char messageId, out MDBInputMessage result);
}