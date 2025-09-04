using MDBCommunicators.Handlers;

namespace OpticwashController.Handlers;

public class StatusHandler : IHandler
{
    private readonly KeepAliveLookup _keepAliveLookup;
    public bool TryConsume(string text) =>
        throw new NotImplementedException();
    
    
}