using OpticwashController.Message;

namespace OpticwashController.Communication
{
    public interface ICommandConsumer
    {
        bool TryConsume(InputMessage message);
    }
}