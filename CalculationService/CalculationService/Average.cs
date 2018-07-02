using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CalculationService
{
    [DataContract]
    public class AverageSend
    {
        [DataMember]
        public float average { get; set; }
    }
}