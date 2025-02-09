﻿using System;
using System.Diagnostics;

namespace Linearstar.Windows.RawInput
{
    public struct HidUsageAndPage : IEquatable<HidUsageAndPage>
    {
        public static readonly HidUsageAndPage Mouse = new HidUsageAndPage(0x01, 0x02);
        public static readonly HidUsageAndPage Joystick = new HidUsageAndPage(0x01, 0x04);
        public static readonly HidUsageAndPage GamePad = new HidUsageAndPage(0x01, 0x05);
        public static readonly HidUsageAndPage Keyboard = new HidUsageAndPage(0x01, 0x06);
        public static readonly HidUsageAndPage Pen = new HidUsageAndPage(0x0D, 0x02);
        public static readonly HidUsageAndPage TouchScreen = new HidUsageAndPage(0x0D, 0x04);
        public static readonly HidUsageAndPage TouchPad = new HidUsageAndPage(0x0D, 0x05);

        public HidUsageAndPage(ushort usagePage, ushort usage)
        {
            UsagePage = usagePage;
            Usage = usage;
        }

        public ushort Usage
        {
            get;
        }

        public ushort UsagePage
        {
            get;
        }

        public static bool operator ==(HidUsageAndPage a, HidUsageAndPage b) =>
            a.UsagePage == b.UsagePage &&
            a.Usage == b.Usage;

        public static bool operator !=(HidUsageAndPage a, HidUsageAndPage b) =>
            a.UsagePage != b.UsagePage ||
            a.Usage != b.Usage;

        public bool Equals(HidUsageAndPage other) =>
            GetHashCode() == other.GetHashCode();

        public override bool Equals(object obj) =>
            obj is HidUsageAndPage huap ? Equals(huap) : base.Equals(obj);

        public override int GetHashCode()
        {
            return UsagePage << 16 + Usage;

//            var hashcode = typeof(HidUsageAndPage).GetHashCode() ^
//UsagePage.GetHashCode() ^
//Usage.GetHashCode();

//            Debug.WriteLine($"UsagePage:{UsagePage}, Usage:{Usage}, HashCode:{hashcode}");

           // return hashcode;
        }

        public override string ToString() =>
            $"{UsagePage:X2}:{Usage:X2}";
    }
}
