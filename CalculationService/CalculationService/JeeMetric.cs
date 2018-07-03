using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CalculationService
{
    [DataContract]
    public class MetricForCalculation
    {
        //public MetricForCalculation(int id, float value, int date, int deviceType)
        //{
        //    this.id = id;
        //    this.value = value;
        //    this.date = date;
        //    this.deviceType = deviceType;
        //}

        //public MetricForCalculation()
        //{
            
        //}

        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string value { get; set; }

        [DataMember]
        public int date { get; set; }

        [DataMember]
        public int deviceType { get; set; }

    }
}