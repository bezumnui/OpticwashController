using MDBCommunicators.Handlers;

namespace MDBCommunicators;

public class CommandCommunicator : ICommandCommunicator
{
    private readonly Sender _sender;
    private readonly MDBAddress _address;
    private readonly IResponseReceiver _responseReceiver;
    
    public CommandCommunicator(IResponseReceiver responseReceiver, Sender sender, MDBAddress address)
    {
        _responseReceiver = responseReceiver;
        _sender = sender;
        _address = address;
    }

    public MDBInputMessage GetVersion() =>
        SendWithAnswer("V", 'v');
    
    public MDBInputMessage GetHardware() =>
        SendWithAnswer("H", 'h');
    
    public void ResetAdapter() =>
        _sender.Send("F,RESET");
    
    public MDBInputMessage StartMasterMode() =>
        SendWithAnswer("M,1", 'm');
    
    public MDBInputMessage StopMasterMode() =>
        SendWithAnswer("M,0", 'm');
        
    public MDBInputMessage Poll() =>
        SendWithAnswer($"R,{GetAddress(12)}", 'p');
    
    public MDBInputMessage EnterReaderMode() =>
        SendWithAnswer($"R,{GetAddress(14)},01", 'p');

    public MDBInputMessage RequestVending(int amountCents)
    {
        string amountHex = amountCents.ToString("X");
        amountHex = amountHex.PadLeft(4, '0');
        return SendWithAnswer($"R,{GetAddress(13)},00{amountHex}ffff", 'p');
    }
    
    public void RequestReset() =>
        _sender.Send("R,10");
    
    public MDBInputMessage SuccessPayment() => 
        SendWithAnswer($"R,{GetAddress(13)},02ffff", 'p');
    
    public MDBInputMessage FailPayment() => 
        SendWithAnswer($"R,{GetAddress(13)},03ffff", 'p'); 
    
    public MDBInputMessage CompleteSession() => 
        SendWithAnswer($"R,{GetAddress(13)},04", 'p');
    
    
    private int GetAddress(int command) =>
        command + (int)_address;

    private MDBInputMessage SendWithAnswer(string text, char waitingCharacter)
    {
        if (string.IsNullOrEmpty(text))
            throw new ArgumentNullException(nameof(text));
        
        _sender.Send(text);
        
        if (_responseReceiver.TryGetFirstMessageById(waitingCharacter, out MDBInputMessage message) == false)
            throw new TimeoutException();
        
        return message;
    }
}