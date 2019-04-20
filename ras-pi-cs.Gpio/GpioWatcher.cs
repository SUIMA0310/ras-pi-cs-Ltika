using System;
using System.IO;
using System.Threading.Tasks;

namespace ras_pi_cs.Gpio
{
    public class GpioWatcher : IDisposable
    {
        private readonly PinConfigration config;
        private readonly FileSystemWatcher watcher;

        public event Action ValueChanged;
        public event Action DirectionChanged;

        public GpioWatcher(PinConfigration pinConfigration)
        {
            config = pinConfigration;
            watcher = new FileSystemWatcher();

            // watcher configration.
            watcher.Path = $"/sys/class/gpio/gpio{config.PinNumber}/";
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += (o, e) => 
            {
                Console.WriteLine($"{DateTime.Now} : watcher :{e.Name}");
                switch (e.Name)
                {
                    case "value":
                        OnValueChanged();
                        break;
                    case "direction":
                        OnDirectionChanged();
                        break;
                }
            };

            watcher.EnableRaisingEvents = true;
        }

        protected virtual void OnValueChanged()
        {
            ValueChanged?.Invoke();
        }

        protected virtual void OnDirectionChanged()
        {
            DirectionChanged?.Invoke();
        }



        public void Dispose()
        {
            watcher.Dispose();
        }
    }
}