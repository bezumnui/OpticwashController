namespace OpticwashController.Message;

public enum CommandCode
{
    Unknown = -1,
    Status = 0x01,
    Command = 0x02,

    RequestStatus = 0x03,
    ClearError = 0x04,
    UpdatePrice = 0x05,
    OpenCabinet = 0x06,

    FirmwareUpdate = 0xA0,
    EraseApp = 0xA1,
    SendBlock = 0xA2,
    BlocksEnd = 0xA3,
    ChecksumRequest = 0xA6,
}