using System;
using System.Linq;
using System.Windows.Forms;
using Linearstar.Windows.RawInput;

namespace RawInput.Sharp.SimpleExample
{
    class Program
    {
        static void Main()
        {
            // Get the devices that can be handled with Raw Input.
            var devices = RawInputDevice.GetDevices();

            // Keyboards will be returned as a RawInputKeyboard.
            //var keyboards = devices.OfType<RawInputKeyboard>();

            // List them up.
            foreach (var device in devices)
            {
                try
                {
                    Console.WriteLine($"{device.DeviceType} {device.VendorId:X4}:{device.ProductId:X4} {device.ProductName}, {device.ManufacturerName} (0x{device.UsageAndPage.UsagePage:X2}:0x{device.UsageAndPage.Usage:X2})");

                }
                catch { }
            }
                

            // To begin catching inputs, first make a window that listens WM_INPUT.
            var window = new RawInputReceiverWindow();

            window.Input += (sender, e) =>
            {
                // Catch your input here!
                var data = e.Data;

                Console.WriteLine(data);
            };

            try
            {
                // Register the HidUsageAndPage to watch any device.
                //RawInputDevice.RegisterDevice(HidUsageAndPage.Keyboard, RawInputDeviceFlags.ExInputSink | RawInputDeviceFlags.NoLegacy, window.Handle);

                RawInputDevice.RegisterDevice(HidUsageAndPage.Pen, RawInputDeviceFlags.InputSink, window.Handle);

                Application.Run();
            }
            finally
            {
                RawInputDevice.UnregisterDevice(HidUsageAndPage.Pen);
            }
        }
    }
}
