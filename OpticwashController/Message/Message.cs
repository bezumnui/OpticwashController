namespace OpticwashController.Message
{
    public abstract class Message : IMessage
    {
        private byte[] _address = new byte[2];

        protected int RawCommandCode { get; set; }
        public CommandCode CommandCode => Enum.IsDefined(typeof(CommandCode), RawCommandCode) ? (CommandCode)RawCommandCode : CommandCode.Unknown;
        public int RawPacketType { get; protected set; }
        public PacketType PacketType => Enum.IsDefined(typeof(PacketType), RawCommandCode) ? (PacketType)RawCommandCode : PacketType.Unknown;

        public int RawResponseType { get; protected set; }

        public byte[] Address
        {
            get => new[] { _address[0], _address[1] };
            protected set => _address = value ?? throw new ArgumentNullException(nameof(value)); 
        }
        public List<byte> RawPacketLabel { get; protected set; }
    
        public int PacketLabel => (RawPacketLabel[0] << 8) | RawPacketLabel[1];
        public List<byte> Data { get; protected set; }
        public byte Checksum { get; protected set; }
    }
}