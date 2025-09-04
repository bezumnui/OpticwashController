using System.IO.Ports;

namespace MDBCommunicators
{
    public class Sender : ISender
    {
        private readonly SerialPort _serialPort;
    
        public Sender(SerialPort serialPort)
        {
            _serialPort = serialPort;
        
            if (_serialPort == null)
                throw new ArgumentNullException(nameof(serialPort));
        
        }

        public void Send(string message)
        {
            if (_serialPort.IsOpen == false)
                throw new InvalidOperationException("Serial port is not open");
        
            _serialPort.Write($"{message}\r\n");
        }
    }
}