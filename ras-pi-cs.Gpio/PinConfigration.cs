using System;
using System.Collections.Generic;
using System.Text;

namespace ras_pi_cs.Gpio
{
    public class PinConfigration
    {
        public int PinNumber { get; set; } = -1;

        // direction setting.
        // true is out.
        // false is in.
        public bool Output { get; set; } = false;

        public PinConfigration()
        { }

        public PinConfigration(int pinNumber, bool output = false)
        {
            PinNumber = pinNumber;
            Output = output;
        }
    }
}
