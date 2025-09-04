namespace MDBCommunicators;

public class MDBInputMessage
{
    public MDBInputMessage(string content, char identifier)
    {
        Content = content;
        Identifier = identifier;
        TimeReceivedUtc = DateTime.UtcNow;

        Console.WriteLine($"Created: {this}");
    }
    

    public char Identifier { get; init; }
    public string Content { get; init; }
    public DateTime TimeReceivedUtc { get; init; }

    public override string ToString() =>
        $"MDBInputMessage({nameof(Identifier)}: \'{Identifier}\', {nameof(Content)}: \"{Content}\", {nameof(TimeReceivedUtc)}: {TimeReceivedUtc})";
}