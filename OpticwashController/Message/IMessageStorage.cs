namespace OpticwashController.Message
{
    public interface IMessageStorage
    {
        void AddMessage(InputMessage message);
        void ClearTimeoutMessages();
        InputMessage GetMessageByPacketLabel(int packetLabel);
    }
}