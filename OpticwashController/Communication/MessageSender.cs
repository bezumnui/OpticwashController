using System.IO.Ports;
using OpticwashController.Message;

namespace OpticwashController.Communication
{
    public class MessageSender : IMessageSender
    {
        private readonly SerialPort _serialPort;

        public MessageSender(SerialPort serialPort)
        {
            _serialPort = serialPort;
        }

        public void SendMessage(OutputMessage message)
        {
            if (_serialPort.IsOpen == false)
                throw new InvalidOperationException("Serial port is not open");
        
            _serialPort.Write(message.RawMessage, 0, message.Length);
        }
    }
}