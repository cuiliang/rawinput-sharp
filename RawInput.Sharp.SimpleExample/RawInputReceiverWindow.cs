using System;
using System.Diagnostics;
using System.Windows.Forms;
using Linearstar.Windows.RawInput;

namespace RawInput.Sharp.SimpleExample
{
    class RawInputReceiverWindow : NativeWindow
    {
        public event EventHandler<RawInputEventArgs> Input;

        public RawInputReceiverWindow()
        {
            CreateHandle(new CreateParams
            {
                X = 0,
                Y = 0,
                Width = 0,
                Height = 0,
                Style = 0x800000,
            });
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_INPUT = 0x00FF;

            if (m.Msg == WM_INPUT)
            {
                var data = RawInputData.FromHandle(m.LParam);

                //Input?.Invoke(this, new RawInputEventArgs(data));

                // You can identify the source device using Header.DeviceHandle or just Device.
                var sourceDeviceHandle = data.Header.DeviceHandle;
                var sourceDevice = data.Device;

                // The data will be an instance of either RawInputMouseData, RawInputKeyboardData, or RawInputHidData.
                // They contain the raw input data in their properties.
                switch (data)
                {
                    case RawInputMouseData mouse:
                        Debug.WriteLine(mouse.Mouse);
                        break;
                    case RawInputKeyboardData keyboard:
                        Debug.WriteLine(keyboard.Keyboard);
                        break;
                    case RawInputDigitizerData pen:
                        OutputInfo(pen);
                        break;
                    case RawInputHidData hid:
                        Debug.WriteLine(hid.Hid);
                        break;
                    
                }
            }

            base.WndProc(ref m);
        }

        private void OutputInfo(RawInputDigitizerData pen)
        {
            Debug.WriteLine($"Pen= {pen} ======================");

            Debug.WriteLine($"ButtonSetStates:");
            foreach(var item in pen.ButtonSetStates)
            {
                Debug.WriteLine($"\t{item}");
            }

            Debug.WriteLine("Contract:");
            foreach(var item in pen.Contacts)
            {
                Debug.WriteLine(item);
            }

            //Debug.WriteLine($"DataSetStates：");

            //foreach(var item in pen.ValueSetStates)
            //{
            //    Debug.WriteLine($"\t{item}");
            //}
        }
    }
}
