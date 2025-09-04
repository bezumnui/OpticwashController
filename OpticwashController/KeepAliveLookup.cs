namespace OpticwashController
{
    public class KeepAliveLookup
    {
        private Thread _thread;
        private bool _isRunning;
        private readonly int _intervalMs;
    
    
        public KeepAliveLookup(int intervalMs)
        {
            _intervalMs = intervalMs;
        }

        public void Enable()
        {
            if (_isRunning)
                throw new InvalidOperationException($"{nameof(KeepAliveLookup)} is already running");

            _isRunning = true;
            _thread = new Thread(Run);
            _thread.Start();
        }

        public void Disable(bool block = true)
        {
            if (_isRunning == false)
                throw new InvalidOperationException($"{nameof(KeepAliveLookup) } is not running");

            if (_thread == null)
                throw new NullReferenceException("Thread is null");
        
            _isRunning = false;
        
            if (block)
                _thread.Join();
        }
    
        private void Run()
        {
            while (_isRunning)
            {
                Thread.Sleep(_intervalMs);
                // TODO: Implement keep-alive logic
            }
        }
    }
}