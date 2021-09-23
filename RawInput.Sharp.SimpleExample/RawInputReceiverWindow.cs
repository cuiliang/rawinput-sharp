using System;
using System.Diagnostics;
using System.Linq;
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


        // 按钮是否按下
        private bool _isBarrelButtonDown = false;
        private bool _isEraserButtonDown = false;
        
        private DateTime? _BarrelButtnonDownTime;
        private DateTime?_EraserButtonDownTime;

        // 按键过程中是否绘制了
        private bool _hasTipDown = false;

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
            if (pen.Contacts.Length > 0)
            {
                var contract = pen.Contacts.First();

               

                Debug.WriteLine($"Pen= TipDown:{contract.IsTipDown} IsButtonDown:{contract.IsButtonDown} Earser:{contract.IsEraser} Kind:{contract.Kind}  ======================");

                if (_isBarrelButtonDown == false)
                {
                    if (contract.IsTipDown == false
                        && contract.IsButtonDown == true)
                    {
                        _isBarrelButtonDown = true;
                        _BarrelButtnonDownTime = DateTime.Now;
                        _hasTipDown = false;

                        Debug.WriteLine("按下了右键按钮");
                    }
                }
                else
                {
                    if (contract.IsButtonDown == false)
                    {
                        // 抬起了
                        _isBarrelButtonDown = false;

                        if (!_hasTipDown)
                        {
                            var ms = (DateTime.Now - _BarrelButtnonDownTime.Value).TotalMilliseconds;
                            Debug.WriteLine($"抬起了右键按钮。按下时长：{ms}");
                        }
                        

                    }
                }

                if (_isEraserButtonDown == false)
                {
                    if (contract.IsTipDown == false
                        && contract.IsEraser == true)
                    {
                        _isEraserButtonDown = true;
                        _EraserButtonDownTime = DateTime.Now;
                        _hasTipDown = false;

                        Debug.WriteLine("按下了橡皮擦按钮");
                    }
                }
                else
                {
                    if (contract.IsEraser == false)
                    {
                        // 抬起了
                        _isEraserButtonDown = false;

                        if (!_hasTipDown)
                        {
                            var ms = (DateTime.Now - _EraserButtonDownTime.Value).TotalMilliseconds;
                            Debug.WriteLine($"抬起了橡皮擦按钮。按下时长：{ms}");
                        }
                    }
                }

                if (contract.IsTipDown && !_hasTipDown)
                {
                    _hasTipDown = true;
                }



                //if (_isBarrelButtonDown == false && contract.Kind == RawInputDigitizerContactKind.Hover)
            }

            

            //Debug.WriteLine($"ButtonSetStates:");
            //foreach(var item in pen.ButtonSetStates)
            //{
            //    Debug.WriteLine($"\t{item}");
            //}

            //Debug.WriteLine("Contract:");
            //foreach(var item in pen.Contacts)
            //{
            //    Debug.WriteLine(item);
            //}

            //Debug.WriteLine($"DataSetStates：");

            //foreach(var item in pen.ValueSetStates)
            //{
            //    Debug.WriteLine($"\t{item}");
            //}
        }
    }
}
