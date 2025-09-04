namespace OpticwashController.Message
{
    public enum PacketType
    {
        Unknown = -1,
        Send = 0,
        Ack = 1,
        InvalidCommandResend = 2,
        InvalidPacketResend = 3,
    
    }
}