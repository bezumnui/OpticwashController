namespace MDBCommunicators.Polling;

public class MDBPoller : IMDBPoller
{
    private const int TimeoutMilliseconds = 50;
    private const int PollAnswerSize = 2;
    private const int PollArgumentIndex = 1;
    private const string VendApprovedTag = "05";
    private const string VendDeniedTag = "06";
    private const string SessionEndTag = "07";
    private bool _isRunning;
    private readonly ICommandCommunicator _commandCommunicator;
    private Thread? _thread;

    public MDBPoller(ICommandCommunicator commandCommunicator)
    {
        if (commandCommunicator == null)
            throw new ArgumentNullException(nameof(commandCommunicator));
        
        _commandCommunicator = commandCommunicator;
    }

    public event Action<int>? Approved;
    public event Action<string>? Failed;
    public event Action? SessionEnded;

    public void Start()
    {
        if (_isRunning)
            throw new InvalidOperationException("Already running.");
        
        _isRunning = true;
        
        _thread = new Thread(PollInThread);
        _thread.Start();
    }

    public void Stop(bool blocking = true)
    {
        if (_isRunning == false)
            throw new InvalidOperationException("Not running.");
        
        _isRunning = false;
        
        if (_thread == null)
            throw new InvalidOperationException("Thread is null.");
        
        if (blocking)
            _thread.Join();
        
        _thread = null;
    }

    private void PollInThread()
    {
        while (_isRunning)
        {
            Thread.Sleep(TimeoutMilliseconds);

            try
            {
                MDBInputMessage message = _commandCommunicator.Poll();
                OnMessageReceived(message);

            }
            catch (TimeoutException)
            {
                Console.WriteLine("Timed out waiting for MDB command.");
            }
            
        }
    }

    private void OnMessageReceived(MDBInputMessage message)
    {
        string[] splitMessage = message.Content.Split(",");
        
        if (splitMessage.Length < PollAnswerSize)
            return;
        
        string argument = splitMessage[PollArgumentIndex];
        
        if (argument.Contains("ACK"))
            return;

        Console.WriteLine("smth");

        if (argument.StartsWith(VendApprovedTag))
        {
            if (int.TryParse(argument.Substring(VendApprovedTag.Length), out int answer) == false)
                throw new InvalidOperationException("Invalid answer length.");
            
            Approved?.Invoke(answer);
        }
        
        else if (argument.StartsWith(VendDeniedTag))
        {
           Failed?.Invoke(argument.Substring(VendDeniedTag.Length));
        }  
        
        else if (argument.StartsWith(SessionEndTag))
        {
           SessionEnded?.Invoke();
        }
    }
}