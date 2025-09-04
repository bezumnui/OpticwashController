using OpticwashController.Communication;

namespace OpticwashController.Message
{
    public class CommandSender
    {
        private readonly IMessageSender _messageSender;
        private readonly IMessageStorage _messageStorage;
    
        public CommandSender(IMessageSender messageSender, IMessageStorage messageStorage)
        {
            _messageSender = messageSender;
            _messageStorage = messageStorage;
        }

        public void SendKeepAlive()
        {
            _messageSender.SendMessage(new OutputMessage(PacketType.Ack, CommandCode.Command, PacketType.Send, new byte[] { 0 }));
        }
    }
}