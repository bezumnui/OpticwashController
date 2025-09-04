namespace OpticwashController.Message
{
    public interface IMessage
    {
        CommandCode CommandCode { get; }
        PacketType PacketType { get; }
        int RawResponseType { get; }
        byte[] Address { get; }
        List<byte> RawPacketLabel { get; }
        List<byte> Data { get; }
        byte Checksum { get; }
    }
}