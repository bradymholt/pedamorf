using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;
using System.IO;
using Pedamorf.Service;
using Pedamorf.Service.Client;

namespace PedamorfTest
{
    [TestClass]
    public class TestService
    {
        ServiceHost m_host;
        PedamorfServiceClient m_client;

        [TestInitialize]
        public void Initialize()
        {
            m_host = new ServiceHost(typeof(PedamorfService));
            m_host.Open();
            m_client = ServiceManager.GetClient("localhost");
        }

        [TestMethod]
        [ExpectedException(typeof(EndpointNotFoundException))]
        public void TestServiceUnreachable()
        {
            PedamorfServiceClient client = ServiceManager.GetClient("bad_host");
            Pedamorf.Service.Client.PedamorfResponse response = client.ConvertHtml("<p>Test.</p>");
        }

        [TestMethod]
        [ExpectedException(typeof(EndpointNotFoundException))]
        public void TestWrongPort()
        {
            PedamorfServiceClient client = ServiceManager.GetClient("localhost", 1000);
            Pedamorf.Service.Client.PedamorfResponse response = client.ConvertHtml("<p>Test.</p>");
        }

        [TestMethod]
        public void TestResultPdf()
        {
            Pedamorf.Service.Client.PedamorfResponse response = m_client.ConvertHtml("<p>Test.</p>");
            Assert.IsTrue(response.ResultPdf.Length > 0);
        }

        [TestMethod]
        public void TestInvalidUrl()
        {
            Pedamorf.Service.Client.PedamorfResponse response = m_client.ConvertUrl("invalidUrl.com");
            Assert.IsNull(response.ResultPdf);
        }

        [TestMethod]
        public void TestUnsupportedTypeError()
        {
            Pedamorf.Service.Client.PedamorfResponse response = m_client.ConvertFile(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\unsupported\\image.tga"));
            Assert.IsNull(response.ResultPdf);
            Assert.IsTrue(response.Error);
            Assert.AreEqual(Pedamorf.Service.Client.ErrorCodeEnum.UNSUPPORTED_SOURCE, response.ErrorCode);
        }

        [TestCleanup]
        public void Cleanup()
        {
            m_client.Dispose();
            m_host.Close();
        }
    }
}
