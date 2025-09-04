using System.IO.Ports;
using Microsoft.Extensions.Logging;

namespace OpticwashController.Communication;

public class Communicator
{
    private const int BaudRate = 9600;
    private const int DataBits = 8;
    private static readonly StopBits StopBits = StopBits.One;
    private static readonly Parity Parity = Parity.None;
    
    private MessageListener _messageListener;
    private MessageSender _messageSender;
    private string _port;
    private readonly SerialPort _serialPort;

    public Communicator(string port, ILoggerFactory loggerFactory)
    {
        _port = port;

        if (string.IsNullOrEmpty(_port))
            throw new ArgumentException(nameof(port));
        
        _serialPort = new SerialPort(port, BaudRate, Parity, DataBits, StopBits);
        _messageListener = new MessageListener(_serialPort, loggerFactory);
        _messageSender = new MessageSender(_serialPort);
    }

    public void Start()
    {
        if (_serialPort.IsOpen)
            throw new InvalidOperationException("Communicator is already open");
        
        _serialPort.Open();
        _messageListener.Start();
    }
}