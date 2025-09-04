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

            Console.WriteLine("Trying to find device...");
            
            int vendorId =  0x4d8;

            if (Utils.TryGetDeviceByVendor(vendorId, out string result))
            {
                Console.WriteLine($"Found: {result}");
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