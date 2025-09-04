using System.IO.Ports;
using MDBCommunicators.Handlers;

namespace MDBCommunicators
{
    public class Listener
    {
        private const int SleepTimeMs = 5;

        private readonly List<IHandler> _handlers = new List<IHandler>();
        private readonly SerialPort _serialPort;
        private bool _isListening;
        private Thread _thread;

        public Listener(SerialPort serialPort)
        {
            _serialPort = serialPort;

            if (serialPort == null)
                throw new ArgumentNullException(nameof(serialPort));
        }

        public void AddHandler(IHandler handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
        
            if (_handlers.Contains(handler))
                throw new InvalidOperationException("Handler is already registered.");
        
            _handlers.Add(handler);
        }
    

        public void Listen()
        {
            if (_serialPort.IsOpen == false)
                throw new InvalidOperationException("The serial port is not opened.");

            if (_isListening)
                throw new InvalidOperationException("The serial port is already listening.");

            _isListening = true;

            _thread = new Thread(ListenInThread);
            _thread.Start();
        }

        public void Stop()
        {
            if (_isListening == false)
                throw new InvalidOperationException("The serial port is not listening.");
        
            _isListening = false;
            _thread?.Join();
        }

        private void ListenInThread()
        {
            while (_isListening && _serialPort.IsOpen)
            {
                Thread.Sleep(SleepTimeMs);

                if (_serialPort.BytesToRead > 0)
                    ReadBytes();
            }
        }

        private void ReadBytes()
        {
            string data = _serialPort.ReadTo("\r\n");

            foreach (IHandler handler in _handlers)
            {
                if (handler.TryConsume(data))
                    return;
            }
        }
    }
}