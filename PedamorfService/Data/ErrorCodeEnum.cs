using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Pedamorf.Service
{
    [DataContract]
    public enum ErrorCodeEnum
    {
        [EnumMember]
        ENGINE_ERROR,

        [EnumMember]
        COULD_NOT_FETCH_URL,

        [EnumMember]
        UNSUPPORTED_SOURCE,

        [EnumMember]
        UNDEFINED_SOURCE,

        [EnumMember]
        UNSPECIFIED_ERROR,

        [EnumMember]
        TIMEOUT,

        [EnumMember]
        OTHER
    }
}
