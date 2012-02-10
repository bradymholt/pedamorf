using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Pedamorf.Service.Client
{
    public static class PedamorfServiceManager
    {
        private const int DEFAULT_PORT = 8474;
        private const int DEFAULT_MAX_DEPTH = 32;
        private const long DEFAULT_MAX_RECEIVED_MESSAGE_SIZE = 2147483647;
        private const int DEFAULT_MAX_ARRAY_LENGTH = 2147483647;
        private const int DEFAULT_MAX_BYTES_PER_READ = 4096;
        private const int DEFAULT_MAX_NAME_TABLE_CHAR_COUNT = 16384;

        public static PedamorfServiceClient GetClient(string host)
        {
            return GetClient(host, DEFAULT_PORT);
        }

        public static PedamorfServiceClient GetClient(string host, int port)
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.MaxReceivedMessageSize = DEFAULT_MAX_RECEIVED_MESSAGE_SIZE;
            binding.ReaderQuotas.MaxDepth = DEFAULT_MAX_DEPTH;
            binding.ReaderQuotas.MaxArrayLength = DEFAULT_MAX_ARRAY_LENGTH;
            binding.ReaderQuotas.MaxBytesPerRead = DEFAULT_MAX_BYTES_PER_READ;
            binding.ReaderQuotas.MaxNameTableCharCount = DEFAULT_MAX_NAME_TABLE_CHAR_COUNT;
            binding.ReliableSession.Enabled = false;
            binding.Security.Mode = SecurityMode.None;

            return GetClient(host, port, binding);
        }

        public static PedamorfServiceClient GetClient(string host, NetTcpBinding binding)
        {
            return GetClient(host, DEFAULT_PORT, binding);
        }

        public static PedamorfServiceClient GetClient(string host, int port, NetTcpBinding binding)
        {
            EndpointAddress endpoint = new EndpointAddress(string.Format("net.tcp://{0}:{1}/Pedamorf", host, port));
            PedamorfServiceClient client = new PedamorfServiceClient(binding, endpoint);
            return client;
        }
    }
}
