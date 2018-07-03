using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace CalculationService
{
    public class CalculationService : ICalculationEndpoint
    {
        const string dbURL = "mongodb://localhost:27017";
        const string dataBase = "Metrics";

        static public IMongoCollection<CalculatedMetrics> createDatabase()
        {
            var client = new MongoClient(MongoUrl.Create(dbURL));
            IMongoDatabase db = client.GetDatabase(dataBase);
            IMongoCollection<CalculatedMetrics> collection = db.GetCollection<CalculatedMetrics>("Metrics");
            return collection;
        }

        public void InsertMetrics(CalculatedMetrics metrics)
        {
            var collection = createDatabase();
            collection.InsertOneAsync(metrics);
        }

        public CalculatedMetrics getAllCalculatedMetricsByMac(DeviceMacReceived dm)
        {
            var collection = createDatabase();
            var query =
                from e in collection.AsQueryable<CalculatedMetrics>()
                where e.deviceMAC == dm.deviceMac
                select e;
            var a = new CalculatedMetrics();
            foreach (var item in query)
            {
                a = item;
            }
            return a;
        }

        string url = "http://192.168.0.10:21080/AtlantisJavaEE-war/services/mobile";
        public string JEEUpdateDB(MetricContract metric)
        {
            if (!(metric == null))
            {
                
                Connection connection = new Connection(url);
                var json = new JavaScriptSerializer().Serialize(metric);
                Task.Run(() => connection.sendData(httpVerb.POST, "/addMetric", json));
                
                if (metric.type > 1 && metric.type < 7)
                {
                    var collection = createDatabase();
                    var filter = Builders<CalculatedMetrics>.Filter.Eq("deviceMAC", metric.mac);
                    if (collection.Find(filter).ToList().Count == 0)
                    {
                        InsertMetrics(new CalculatedMetrics(float.Parse(metric.value), float.Parse(metric.value), float.Parse(metric.value), float.Parse(metric.value), float.Parse(metric.value), float.Parse(metric.value), float.Parse(metric.value), float.Parse(metric.value), float.Parse(metric.value), metric.mac));
                    }
                    else
                    {
                        //Mise a jour de la liste de valeurs 
                        

                        CalculatedMetrics calculated = new CalculatedMetrics();

                        calculated.dayMin = updateMinimum(metric, "day", collection, filter, connection);
                        calculated.dayMax = updateMaximum(metric, "day", collection, filter, connection);
                        calculated.dayAvg = updateAverage(metric, "day", collection, filter, connection);

                        calculated.weekMin = updateMinimum(metric, "week", collection, filter, connection);
                        calculated.weekMax = updateMaximum(metric, "week", collection, filter, connection);
                        calculated.weekAvg = updateAverage(metric, "week", collection, filter, connection);

                        calculated.monthMin = updateMinimum(metric, "month", collection, filter, connection);
                        calculated.monthMax = updateMaximum(metric, "month", collection, filter, connection);
                        calculated.monthAvg = updateAverage(metric, "month", collection, filter, connection);

                        updateDb(calculated, collection, filter);
                    }
                    return " UPDATE SUCCESS";
                }
                else
                    return "wrong device Type";
            }
            else
                return "parameter is null";
        }

        private float updateMinimum(MetricContract metric, string timeSpan, IMongoCollection<CalculatedMetrics> collection, FilterDefinition<CalculatedMetrics> filter, Connection connection)
        {
            string urlTmp = url + "/device/" + metric.mac + "/" + timeSpan;
            string res = connection.getData(httpVerb.GET, urlTmp);
            
            List<MetricForCalculation> result = new List<MetricForCalculation>();
            result = JsonConvert.DeserializeObject<List<MetricForCalculation>>(res);
            
            float lesser = float.Parse(metric.value);
            if (result!= null)
            {
                foreach (var item in result)
                {
                    if (lesser > float.Parse(item.value))
                    {
                        lesser = float.Parse(item.value);
                    }
                }
            }
            return lesser;
            
        }



        private float updateMaximum(MetricContract metric, string timeSpan, IMongoCollection<CalculatedMetrics> collection, FilterDefinition<CalculatedMetrics> filter, Connection connection)
        {
            string urlTmp = url + "/device/" + metric.mac + "/" + timeSpan;
            string res = connection.getData(httpVerb.GET, urlTmp);
            List<MetricForCalculation> result = new List<MetricForCalculation>();
            result = JsonConvert.DeserializeObject<List<MetricForCalculation>>(res);

            float higher = float.Parse(metric.value);
            if (result != null)
            {
                foreach (var item in result)
                {
                    if (higher < float.Parse(item.value))
                    {
                        higher = float.Parse(item.value);
                    }
                }
            }
            return higher;
            
        }

        public float updateAverage(MetricContract metric, string timeSpan, IMongoCollection<CalculatedMetrics> collection, FilterDefinition<CalculatedMetrics> filter, Connection connection)
        {

            //var filter = Builders<CalculatedMetrics>.Filter.Eq("deviceMAC", metric.mac);

            string urlTmp = url + "/device/" + metric.mac + "/" + timeSpan;
            string res = connection.getData(httpVerb.GET, urlTmp);
            List<MetricForCalculation> result = new List<MetricForCalculation>();
            result = JsonConvert.DeserializeObject<List<MetricForCalculation>>(res);

            //calcul de moyenne
            float total = 0;

            if (result != null)
            {
                foreach (var item in result)
                {
                    total = total + float.Parse(item.value);
                }
                float moyenne = total / result.Count();
                return moyenne;
            }
            return float.Parse(metric.value);
        }

        public void updateDb(CalculatedMetrics calculatedMetrics, IMongoCollection<CalculatedMetrics> collection, FilterDefinition<CalculatedMetrics> filter)
        {
            //insertion en base
            if (collection.Find(filter).ToList().Count == 0)
            {
                InsertMetrics(calculatedMetrics);
            }
            else
            {
                var updateDayMin = Builders<CalculatedMetrics>.Update.Set("DayMin", calculatedMetrics.dayMin);
                var updateDayMax = Builders<CalculatedMetrics>.Update.Set("DayMax", calculatedMetrics.dayMax);
                var updateDayAvg = Builders<CalculatedMetrics>.Update.Set("DayAvg", calculatedMetrics.dayAvg);

                var updateWeekMin = Builders<CalculatedMetrics>.Update.Set("WeekMin", calculatedMetrics.weekMin);
                var updateWeekMax = Builders<CalculatedMetrics>.Update.Set("WeekMax", calculatedMetrics.weekMax);
                var updateWeekAvg = Builders<CalculatedMetrics>.Update.Set("WeekAvg", calculatedMetrics.weekAvg);

                var updateMonthMin = Builders<CalculatedMetrics>.Update.Set("MonthMin", calculatedMetrics.monthMin);
                var updateMonthMax = Builders<CalculatedMetrics>.Update.Set("MonthMax", calculatedMetrics.monthMax);
                var updateMonthAvg = Builders<CalculatedMetrics>.Update.Set("MonthAvg", calculatedMetrics.monthAvg);


                collection.UpdateOne(filter, updateDayMin);
                collection.UpdateOne(filter, updateDayMax);
                collection.UpdateOne(filter, updateDayAvg);

                collection.UpdateOne(filter, updateWeekMin);
                collection.UpdateOne(filter, updateWeekMax);
                collection.UpdateOne(filter, updateWeekAvg);

                collection.UpdateOne(filter, updateMonthMin);
                collection.UpdateOne(filter, updateMonthMax);
                collection.UpdateOne(filter, updateMonthAvg);
            }
        }



    }
}
