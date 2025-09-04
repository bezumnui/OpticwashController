namespace OpticwashController.Message;

public interface IMessage
{
    CommandCode CommandCode { get; }
    PacketType PacketType { get; }
    int RawResponseType { get; init; }
    byte[] Address { get; init; }
    List<byte> RawPacketLabel { get; init; }
    List<byte> Data { get; init; }
    byte Checksum { get; init; }
}