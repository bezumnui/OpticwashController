namespace OpticwashController.Message
{
    public class OutputMessage : Message
    {
        public readonly int Length = 61;

        private static readonly int DataLength = 50;

        private static readonly byte StartOfMessage = 2;

        private static readonly byte EndOfMessage = 3;
    
        public byte[] RawMessage { get; }
    
        public OutputMessage(PacketType packetLabel, CommandCode command, PacketType packetType, byte[] data, byte[] address = null)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (packetLabel == PacketType.Unknown)
                throw new ArgumentNullException(nameof(packetLabel));
        
            if (data.Length > DataLength)
                throw new ArgumentOutOfRangeException($"The length of data must be less than or equal to {DataLength}");

            if (address == null)
                address = new byte[]{0, 0};
        
            Address = address;
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.WriteByte(StartOfMessage);
            memoryStream.Write(Address, 0, Address.Length); // TODO: not sure whether it works or not.
        
            byte[] packetLabelBytes = GetRawPacketLabel(packetLabel);
            memoryStream.Write(packetLabelBytes, 0, packetLabelBytes.Length);
            memoryStream.WriteByte((byte)command);
            memoryStream.WriteByte((byte)packetType);
            Stream stream = memoryStream;
        
            byte[] array = new byte[2];
            array[0] = (byte)DataLength;
            stream.Write(array, 0, 2);
            Data = new List<byte>();
            Data.AddRange(data);

            if (data.Length < DataLength)
                Data.AddRange(new byte[DataLength - data.Length]);

            memoryStream.Write(Data.ToArray(), 0, Data.Count);
            Checksum = MessageHelper.GenerateChecksum(memoryStream.GetBuffer().Take(Length - 2).ToArray());
            memoryStream.WriteByte(Checksum);
            memoryStream.WriteByte(EndOfMessage);
            this.RawMessage = memoryStream.GetBuffer().Take(Length).ToArray();
            RawPacketLabel = packetLabelBytes.ToList();
            RawCommandCode = (byte)command;
            RawPacketType = (byte)packetType;
        }

        private byte[] GetRawPacketLabel(PacketType packetLabel)
        {
            return new byte[]{(byte)packetLabel, 0};
        }
    }
}