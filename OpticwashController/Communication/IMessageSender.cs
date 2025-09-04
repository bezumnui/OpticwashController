using OpticwashController.Message;

namespace OpticwashController.Communication;

public interface IMessageSender
{
    void SendMessage(OutputMessage message);
}