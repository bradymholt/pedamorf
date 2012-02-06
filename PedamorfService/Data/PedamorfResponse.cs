using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Pedamorf.Service
{
    [DataContract]
    public class PedamorfResponse
    {
        [DataMember]
        public byte[] ResultPdf { get; set; }

        [DataMember]
        public bool Error { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public ErrorCodeEnum ErrorCode { get; set; }
    }
}
