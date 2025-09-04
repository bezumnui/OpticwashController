using OpticwashController.Message;

namespace OpticwashController.Communication;

public interface ICommandConsumer
{
    public bool TryConsume(InputMessage message);
}