
namespace OpticwashController.Communication
{
    public interface IMessageListener
    {
        void Start();
        void Stop();
        void AddCommandConsumer(ICommandConsumer commandConsumer);
    }
}