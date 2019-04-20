using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ras_pi_cs.Gpio
{
    public class GpioController : IDisposable
    {
        private const string GPIO = "/sys/class/gpio/";

        private readonly PinConfigration config;

        public GpioController(PinConfigration pinConfigration)
        {
            config = pinConfigration;

            Open();
            Direction = config.Output;
        }

        /// <summary>
        /// <see langword="true"/> = 1.
        /// <see langword="false"/> = 0.
        /// </summary>
        public bool Value
        {
            get {
                using (var reader = new StreamReader(GetPinPath(config.PinNumber) + "value"))
                {
                    var value = reader.ReadToEnd().IndexOf("1") != -1;
                    Console.WriteLine($"{DateTime.Now}  : Read Value > {value}");
                    return value;
                }
            }
            set {
                if (!Direction)
                {
                    // 入力設定時には出力出来ない
                    throw new NotSupportedException();
                }

                Console.WriteLine($"{DateTime.Now}  : write {GetPinPath(config.PinNumber) + "value"} / {(value?1:0)}");
                using (var writer = new StreamWriter(GetPinPath(config.PinNumber) + "value"))
                {
                    writer.Write(value ? 1 : 0);
                }
            }
        }

        /// <summary>
        /// <see langword="true"/> = out.
        /// <see langword="false"/> = in.
        /// </summary>
        public bool Direction
        {
            get {
                using (var reader = new StreamReader(GetPinPath(config.PinNumber) + "direction"))
                {
                    return reader.ReadToEnd().IndexOf("out") != -1;
                }
            }
            protected set {
                using (var writer = new StreamWriter(GetPinPath(config.PinNumber) + "direction"))
                {
                    writer.Write(value ? "out" : "in");
                }
            }
        }

        public bool IsOpen
        {
            get {
                return Directory.Exists(GPIO + $"gpio{config.PinNumber}");
            }
        }

        public void Open()
        {
            if (IsOpen)
            {
                return;
            }

            try
            {
                using (var writer = new StreamWriter(GPIO + "export"))
                {
                    writer.Write(config.PinNumber);
                }
            }
            catch
            { }
        }

        public void Close()
        {
            if (!IsOpen)
            {
                return;
            }

            try
            {
                using (var writer = new StreamWriter(GPIO + "unexport"))
                {
                    writer.Write(config.PinNumber);
                }
            }
            catch
            { }
        }

        protected string GetPinPath(int pinNumber)
        {
            return GPIO + $"gpio{pinNumber}/";
        }

        void IDisposable.Dispose()
        {
            if (config.AutoClose)
            {
                Close();
            }
        }
    }
}
