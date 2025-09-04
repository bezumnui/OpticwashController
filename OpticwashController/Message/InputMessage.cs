using System.ComponentModel.DataAnnotations;

namespace OpticwashController.Message;

public class InputMessage : Message
{
    private static readonly int Length = 62;

    private static readonly int DataLength = 50;

    private static readonly byte StartOfMessage = 2;

    private static readonly byte EndOfMessage = 3;

    public InputMessage(List<byte> bytes)
    {
        if (bytes == null)
            throw new ArgumentNullException(nameof(bytes));

        if (bytes.Count != Length)
            throw new InvalidOperationException($"Message length was {bytes.Count} but expected length is {Length}");

        if (bytes[0] != StartOfMessage)
            throw new InvalidOperationException($"Start of message was 0x{bytes[0]:X2} when 0x{StartOfMessage:X2} was expected.");

        if (bytes[Length - 1] != EndOfMessage)
            throw new InvalidOperationException($"End of message was {bytes[Length - 1]} when {EndOfMessage} was expected.");

        RawPacketLabel = bytes.Skip(3).Take(2).ToList();
        RawCommandCode = bytes[5];
        RawPacketType = bytes[6];
        RawResponseType = bytes[7];
        Data = bytes.Skip(10).Take(DataLength).ToList();
        Checksum = bytes[Length - 2];

        ValidateDataLength(bytes);
    }

    private static void ValidateDataLength(List<byte> bytes)
    {
        byte[] array = bytes.Skip(8).Take(2).ToArray();

        if (array[0] != DataLength || array[1] != 0)
            throw new ValidationException(string.Format("Message data length {0:X2} {1:X2} doesn't match {2:X2} {3:X2}",
                new object[]
                {
                    array[0],
                    array[1],
                    DataLength,
                    0,
                }));
    }

    public override string ToString() =>
        $"InputMessage(RawPacketLabel: \"{RawPacketLabel}\", CommandCode: \"{CommandCode}\", PacketType: \"{PacketType}\", RawResponseType: \"{RawResponseType}\", Data: \"{Data}\")";
}