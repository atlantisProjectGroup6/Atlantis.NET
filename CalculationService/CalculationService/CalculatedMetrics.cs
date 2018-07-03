using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalculationService
{
    public class CalculatedMetrics
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId id;

        //per day
        [BsonElement("DayMin")]
        public float dayMin;
        [BsonElement("DayMax")]
        public float dayMax;
        [BsonElement("DayAvg")]
        public float dayAvg;

        //per week
        [BsonElement("WeekMin")]
        public float weekMin;
        [BsonElement("WeekMax")]
        public float weekMax;
        [BsonElement("WeekAvg")]
        public float weekAvg;

        // per Month
        [BsonElement("MonthMin")]
        public float monthMin;
        [BsonElement("MonthMax")]
        public float monthMax;
        [BsonElement("MonthAvg")]
        public float monthAvg;
        [BsonElement("deviceMAC")]


        public string deviceMAC;

        public CalculatedMetrics()
        {

        }

        public CalculatedMetrics(float dayMin, float dayMax, float dayAvg, float weekMin, float weekMax, float weekAvg, float monthMin, float monthMax, float monthAvg, string deviceMAC)
        {
            this.dayMin = dayMin;
            this.dayMax = dayMax;
            this.dayAvg = dayAvg;
            this.weekMin = weekMin;
            this.weekMax = weekMax;
            this.weekAvg = weekAvg;
            this.monthMin = monthMin;
            this.monthMax = monthMax;
            this.monthAvg = monthAvg;
            this.deviceMAC = deviceMAC;
        }
    }
}