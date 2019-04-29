using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Win32;

namespace TurnOffLANSettingsAutodetection.Service
{
    public class LANSettingsGuard
    {
        int changedCounter = 0;
        readonly Timer _timer;
        public LANSettingsGuard()
        {
            _timer = new Timer(5000) { AutoReset = true };
            _timer.Elapsed += OnTimerElapsed;
        }

        public void Start() { _timer.Start(); }
        public void Stop() { _timer.Stop(); }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                using (RegistryKey RegKey = Registry.CurrentUser.OpenSubKey(@"Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Connections", true))
                {
                    byte[] defConnection = (byte[])RegKey.GetValue("DefaultConnectionSettings");

                    if (defConnection[8] != 1)
                    {
                        defConnection[8] = 1;
                        Console.WriteLine($"Registry value changed at {DateTime.Now}, counter = {++changedCounter}");
                    }

                    RegKey.SetValue("DefaultConnectionSettings", defConnection);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
