using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using System.IO;
using Pedamorf.Library;
using System.Configuration;
using NLog;
using System.Threading.Tasks;

namespace Pedamorf.Service
{
    public partial class Service : ServiceBase
    {
        private ServiceHost m_host;
        private Logger m_logger;

        public Service()
        {
            InitializeComponent();
            m_logger = LogManager.GetCurrentClassLogger();
        }

        protected override void OnStart(string[] args)
        {
            m_logger.Debug("Starting pedamorf service...");

            m_logger.Debug("Configuring  endpoints...");
            m_host = new ServiceHost(typeof(PedamorfService));
            m_host.Open();
        }

        protected override void OnStop()
        {
            if (m_host != null && m_host.State != CommunicationState.Closed)
            {
                m_host.Close();
            }
        }

        public void StartService()
        {
            OnStart(null);
            Console.WriteLine("Service started, press [ENTER] to terminate.");
            Console.ReadLine();
        }

        public void StopService()
        {
            OnStop();
        }
    }
}
