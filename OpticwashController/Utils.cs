using System.IO.Ports;

namespace OpticwashController
{
    public static class Utils
    {
        public static bool TryGetDeviceByVendor(int vendor, out string result)
        {
            foreach (string port in SerialPort.GetPortNames())
            {
                Console.WriteLine("Processing port: " + port);
                if (GetDeviceVendor(port) == vendor)
                {
                    result = port;

                    return true;
                }
            }

            result = null;
            return false;
        }
        
        private static string AscendToFirstNonHubUsbDevice(string path)
        {
            string current = Path.GetFullPath(path);

            while (true)
            {
                string vendorPath = Path.Combine(current, "idVendor");
                string productPath = Path.Combine(current, "idProduct");

                Console.WriteLine($"Processing path: {current}, vendorPath: {vendorPath}, productPath: {productPath}");

                if (File.Exists(vendorPath) && File.Exists(productPath))
                {
                    string bDeviceClassPath = Path.Combine(current, "bDeviceClass");
                    if (File.Exists(bDeviceClassPath))
                    {
                        string cls = File.ReadAllText(bDeviceClassPath).Trim();
                        
                        if (!string.Equals(cls, "09", StringComparison.OrdinalIgnoreCase))
                            return current;
                    }
                    else
                    {
                        return current;
                    }
                }

                string parent = Path.GetFullPath(Path.Combine(current, ".."));
                if (parent == current || parent == "/" || parent == ".")
                    throw new InvalidOperationException("USB device with idVendor/idProduct not found.");
                current = parent;
            }
        }
        

        private static int GetDeviceVendor(string device)
        {
            string path = $"/sys/class/tty/{device}/device";
            
            // string devicePath = Path.GetFullPath(Path.Combine(path, "../../"));
            Console.WriteLine("AscendToFirstNonHubUsbDevice: " + device);

            string devicePath = AscendToFirstNonHubUsbDevice(path);
            string vendorPath = Path.Combine(devicePath, "idVendor");

            if (File.Exists(vendorPath) == false)
                return 0;

            try
            {
                string vendorId = File.ReadAllText(vendorPath).Trim();

                if (int.TryParse(vendorId, out int result))
                    return result;

                return 0;

            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading vendor ID: " + e.Message);
                return 0;
            }
        }
    }
}