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
        [BsonElement("dayAvg")]
        public float dayAvg;
        [BsonElement("weekAvg")]
        public float weekAvg;
        [BsonElement("monthAvg")]
        public float monthAvg;

        public CalculatedMetrics()
        {

        }

        public CalculatedMetrics(float _dayAvg, float _weekAvg, float _monthAvg)
        {

            dayAvg = _dayAvg;
            weekAvg = _weekAvg;
            monthAvg = _monthAvg;

        }

    }
}