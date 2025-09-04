namespace OpticwashController.Message;

public abstract class Message : IMessage
{
    private byte[] _address = new byte[2];

    protected int RawCommandCode { get; init; }
    public CommandCode CommandCode => Enum.IsDefined(typeof(CommandCode), RawCommandCode) ? (CommandCode)RawCommandCode : CommandCode.Unknown;
    public int RawPacketType { get; init; }
    public PacketType PacketType => Enum.IsDefined(typeof(PacketType), RawCommandCode) ? (PacketType)RawCommandCode : PacketType.Unknown;

    public int RawResponseType { get; init; }

    public byte[] Address { 
        get => [_address[0], _address[1]];
        init => _address = value ?? throw new ArgumentNullException(nameof(value)); 
    }
    public List<byte> RawPacketLabel { get; init; }
    
    public int PacketLabel => (RawPacketLabel[0] << 8) | RawPacketLabel[1];
    public List<byte> Data { get; init; }
    public byte Checksum { get; init; }
}