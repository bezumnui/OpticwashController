namespace MDBCommunicators;

public class MessageStorage
{
    private const int TimoutMs = 200;
    private readonly List<MDBInputMessage> _clearBuffer = new List<MDBInputMessage>();
    private readonly List<MDBInputMessage> _messages = new List<MDBInputMessage>();

    public bool TryGetFirstMessageById(char messageId, out MDBInputMessage result)
    {
        foreach (MDBInputMessage message in _messages.Where(message => message.Identifier.Equals(messageId)))
        {
            result = message;

            _messages.Remove(message);
            return true;
        }

        result = null;

        return false;
    }

    public void AddMessage(MDBInputMessage mdbInputMessage)
    {
        if (mdbInputMessage == null)
            throw new ArgumentNullException(nameof(mdbInputMessage));

        if (GetTimeDifferenceMs(mdbInputMessage) > TimoutMs)
            throw new InvalidOperationException("MDBInputMessage already timed out.");

        _messages.Add(mdbInputMessage);
    }

    private int GetTimeDifferenceMs(MDBInputMessage mdbInputMessage) =>
        new TimeSpan((DateTime.UtcNow - mdbInputMessage.TimeReceivedUtc).Ticks).Milliseconds;

    private void ClearTimedMessages()
    {
        _clearBuffer.Clear();

        foreach (MDBInputMessage message in _messages)
        {
            if (GetTimeDifferenceMs(message) > TimoutMs)
                _clearBuffer.Add(message);
        }

        foreach (MDBInputMessage message in _clearBuffer)
            _messages.Remove(message);
    }
}