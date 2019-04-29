using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace TurnOffLANSettingsAutodetection.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            var rc = HostFactory.Run(x =>                                   //1
            {
                x.Service<LANSettingsGuard>(s =>                                   //2
                {
                    s.ConstructUsing(name => new LANSettingsGuard());                //3
                    s.WhenStarted(tc => tc.Start());                         //4
                    s.WhenStopped(tc => tc.Stop());                          //5
                });
                x.RunAsLocalSystem();                                       //6

                x.SetDescription("Description is empty");                   //7
                x.SetDisplayName("ABB.LANSettingsGuard");                                  //8
                x.SetServiceName("ABB.LANSettingsGuard");                                  //9
            });                                                             //10

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());  //11
            Environment.ExitCode = exitCode;
        }
    }
}
