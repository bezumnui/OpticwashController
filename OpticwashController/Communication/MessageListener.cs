using System.ComponentModel.DataAnnotations;
using System.IO.Ports;
using Microsoft.Extensions.Logging;
using OpticwashController.Message;

namespace OpticwashController.Communication
{
    public class MessageListener : IMessageListener
    {
        private const int StartMessageByte = 0x02;
        private const int BufferSize = 65;
        private const float TimeoutSeconds = 0.5f;
        private readonly List<ICommandConsumer> _commandConsumers = new List<ICommandConsumer>();
        private readonly List<byte> _buffer = new List<byte>(BufferSize);
        private readonly ILogger _logger;
        private readonly SerialPort _serialPort;

        private bool _isListening;

        public MessageListener(SerialPort serialPort, ILoggerFactory factory)
        {
            _serialPort = serialPort;
            _logger = factory.CreateLogger(nameof(MessageListener));
        }

        public void Start()
        {
            if (_serialPort.IsOpen == false)
                throw new InvalidOperationException("Port is not open");

            if (_isListening)
                throw new InvalidOperationException("Already listening");

            _isListening = true;
            Thread thread = new Thread(StartInThread);
            thread.Start();
        }

        public void Stop()
        {
            if (_isListening == false)
                throw new InvalidOperationException("Is not listening");

            _isListening = false;
        }
    
        public void AddCommandConsumer(ICommandConsumer commandConsumer)
        {
            if (commandConsumer == null)
                throw new ArgumentNullException(nameof(commandConsumer));

            if (_commandConsumers.Contains(commandConsumer))
                throw new InvalidOperationException("Command consumer is already registered");

            _commandConsumers.Add(commandConsumer);
        }

        private void StartInThread()
        {
            while (_serialPort.IsOpen && _isListening)
            {
                Thread.Sleep(100);

                if (ShouldReadNewMessage(_serialPort.ReadByte()) == false)
                {
                    _logger.LogError("Could not get awaited byte. Got {int}, instead of {StartMessageByte}", StartMessageByte, StartMessageByte);

                    continue;
                }
        
                ReadNewMessage();

                if (TryParseNewMessage(out InputMessage message) == false || message == null)
                    continue;

                HandleInputMessage(message);
            }
        }

        private void HandleInputMessage(InputMessage inputMessage)
        {
            _logger.LogDebug("Received InputMessage: {InputMessage}", inputMessage);

            foreach (ICommandConsumer consumer in _commandConsumers)
            {
                if (consumer.TryConsume(inputMessage))
                    return;
            }
        }

        private bool ShouldReadNewMessage(int readByte) =>
            readByte == StartMessageByte;

        private void ReadNewMessage()
        {
            _buffer.Clear();

            _buffer[0] = StartMessageByte;

            int bytesToRead = _serialPort.BytesToRead;

            while (_buffer.Count < bytesToRead && _isListening && _serialPort.IsOpen)
            {
                DateTime beforePacketTime = DateTime.Now;
                int read = _serialPort.ReadByte();
                DateTime lastPacketTime = DateTime.Now;

                if (IsTimeout(beforePacketTime, lastPacketTime))
                {
                    if (ShouldReadNewMessage(_serialPort.ReadByte()))
                        ReadNewMessage();

                    return;
                }

                _buffer.Add((byte)read);
            }
        }

        private bool TryParseNewMessage(out InputMessage inputMessage)
        {
            inputMessage = null;

            try
            {
                inputMessage = new InputMessage(_buffer);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(e, "Could not parse message");

                return false;
            }
            catch (ValidationException e)
            {
                _logger.LogError(e, "Could not validate message");

                return false;
            }

            return true;
        }

        private bool IsTimeout(DateTime beforeTime, DateTime afterTime)
        {
            TimeSpan time = afterTime - beforeTime;

            return time.TotalSeconds >= TimeoutSeconds;
        }
    }
}