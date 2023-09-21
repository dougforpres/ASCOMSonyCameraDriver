// This implements a console application that can be used to test an ASCOM driver
//

// This is used to define code in the template that is specific to one class implementation
// unused code can be deleted and this definition removed.

#define Camera
// remove this to bypass the code that uses the chooser to select the driver
#define UseChooser

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;

namespace ASCOM.SonyMirrorless
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Uncomment the code that's required
#if UseChooser
            // choose the device
            string id = ASCOM.DriverAccess.Camera.Choose("");
            if (string.IsNullOrEmpty(id))
                return;
            // create this device
            ASCOM.DriverAccess.Camera device = new ASCOM.DriverAccess.Camera(id);
#else
            // this can be replaced by this code, it avoids the chooser and creates the driver class directly.
            ASCOM.DriverAccess.Camera device = new ASCOM.DriverAccess.Camera("ASCOM.SonyMirrorless.Camera");
#endif

            device.Connected = true;
//            device.SetupDialog();
            // now run some tests, adding code to your driver so that the tests will pass.
            // these first tests are common to all drivers.
            Console.WriteLine("name " + device.Name);
            Console.WriteLine("description " + device.Description);
            Console.WriteLine("DriverInfo " + device.DriverInfo);
            Console.WriteLine("driverVersion " + device.DriverVersion);
            //            Console.WriteLine("sensorName " + device.SensorName);

            // TODO add more code to test the driver.
            int count = 0;

            ArrayList modes = device.ReadoutModes;

            device.FastReadout = false;

//            Console.WriteLine(device.Gain);
//            device.Gain = 10;
//            Console.WriteLine(device.Gains.ToString());
            Console.WriteLine(device.MaxADU.ToString());

//            while (true)
              {
                device.StartExposure(0.1, true);

                for (int i = 0; i < 100 && !device.ImageReady; i++)
                    Thread.Sleep(250);

                object o = device.ImageArray;
                count++;
                Console.WriteLine("Got an image #" + count.ToString());
                GC.Collect();
            }

            Console.WriteLine(device.CCDTemperature);
//            */
            device.Connected = false;
            Console.WriteLine("Press Enter to finish");
            Console.ReadLine();
        }
    }
}
