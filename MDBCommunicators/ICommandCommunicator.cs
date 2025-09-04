namespace MDBCommunicators;

public interface ICommandCommunicator
{
    MDBInputMessage GetVersion();
    MDBInputMessage GetHardware();
    void ResetAdapter();
    MDBInputMessage StartMasterMode();
    MDBInputMessage StopMasterMode();
    MDBInputMessage Poll();
    MDBInputMessage EnterReaderMode();
    MDBInputMessage RequestVending(int amountCents);
    void RequestReset();
    MDBInputMessage SuccessPayment();
    MDBInputMessage FailPayment();
    MDBInputMessage CompleteSession();
}