using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CalculationService
{
    [DataContract]
    public class JeeMetric
    {
        [DataMember]
        public string mac { get; set; }

        [DataMember]
        public int timestamp { get; set; }

        [DataMember]
        public string value { get; set; }

        [DataMember]
        public int type { get; set; }

        [DataMember]
        public string name { get; set; }
    }
}