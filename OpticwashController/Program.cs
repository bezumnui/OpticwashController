using System.IO.Ports;
using MDBCommunicators;

namespace OpticwashController
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            foreach (string portName in SerialPort.GetPortNames())
            {
                Console.WriteLine($"PortName: {portName}");
            }
        
            // MDBCommunicator communicator = new MDBCommunicator(new SerialPort("/dev/tty.usbmodem01"), MDBAddress.First);
            // communicator.Start();
            //
            // communicator.CommandCommunicator.StartMasterMode();
            // communicator.CommandCommunicator.EnterReaderMode();
            // communicator.Poller.Start();
            // Console.ReadKey();
            // communicator.CommandCommunicator.RequestVending(500);
            // Console.ReadKey();
            // communicator.Poller.Stop();
            // communicator.Stop();
        }
    }
}