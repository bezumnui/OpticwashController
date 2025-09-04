using System.IO.Ports;
using MDBCommunicators.Handlers;
using MDBCommunicators.Polling;

namespace MDBCommunicators;

public class MDBCommunicator : IDisposable
{
    private readonly MDBAddress _address;
    private readonly Listener _listener;
    private readonly IResponseReceiver _responseReceiver;
    private readonly Sender _sender;
    private readonly SerialPort _serialPort;

    public MDBCommunicator(SerialPort serialPort, MDBAddress address)
    {
        _serialPort = serialPort;
        _address = address;
        _listener = new Listener(serialPort);
        _sender = new Sender(serialPort);
        ResponseReceiver responseReceiver = new ResponseReceiver();
        _responseReceiver = responseReceiver;
        CommandCommunicator = new CommandCommunicator(responseReceiver, _sender, address);
        Poller = new MDBPoller(CommandCommunicator);
        _listener.AddHandler(responseReceiver);
    }

    public ICommandCommunicator CommandCommunicator { get; }
    public IMDBPoller Poller { get; }

    public void Dispose()
    {
        _serialPort.Dispose();
    }

    public void Start()
    {
        if (_serialPort.IsOpen)
            throw new InvalidOperationException("MDBMDBCommunicators already open");

        _serialPort.Open();
        _listener.Listen();
    }

    public void Stop()
    {
        if (_serialPort.IsOpen == false)
            throw new InvalidOperationException("MDBMDBCommunicators not open");

        _listener.Stop();
        _serialPort.Close();
    }
}