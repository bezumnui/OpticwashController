namespace MDBCommunicators.Polling
{
    public interface IMDBPoller
    {
        event Action<int> Approved;
        event Action<string> Failed;
        event Action SessionEnded;
        void Start();
        void Stop(bool blocking = true);
    }
}