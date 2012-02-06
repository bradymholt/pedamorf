using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Pedamorf.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            Service s = new Service();
            s.StartService();
            s.StopService();

#else
            ServiceBase[] ServicesToRun;
            Service service = new Service();
            ServicesToRun = new ServiceBase[] { service };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
