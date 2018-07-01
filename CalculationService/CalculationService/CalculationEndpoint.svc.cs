using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace CalculationService
{
    public class Service1 : CalculationEndpoint
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


        public float getOneDayAverage(string deviceMAC)
        {
            var collection = createDatabase();
            float result = 0;
            var filter = Builders<CalculatedMetrics>.Filter.Eq("deviceMAC", deviceMAC);
            List<CalculatedMetrics> c = collection.Find(filter).ToList();
            foreach (var item in c)
            {
                result = item.dayAvg;
            }

            return result;
        }

        public float getOneWeekAverage(string deviceMAC)
        {
            var collection = createDatabase();
            float result = 0;
            var filter = Builders<CalculatedMetrics>.Filter.Eq("deviceMAC", deviceMAC);
            List<CalculatedMetrics> c = collection.Find(filter).ToList();
            foreach (var item in c)
            {
                result = item.weekAvg;
            }

            return result;
        }

        public float getOneMonthAverage(string deviceMAC)
        {
            var collection = createDatabase();
            float result = 0;
            var filter = Builders<CalculatedMetrics>.Filter.Eq("deviceMAC", deviceMAC);
            List<CalculatedMetrics> c = collection.Find(filter).ToList();
            foreach (var item in c)
            {
                result = item.monthAvg;
            }

            return result;
        }

        public string updateAverage(string deviceMAC, float value, int nbValueDay, int nbValueWeek, int nbValueMonth)
        {
            var collection = createDatabase();
            Formulas f = new Formulas();
            var filter = Builders<CalculatedMetrics>.Filter.Eq("deviceMAC", deviceMAC);

            if (collection.Find(filter).ToList().Count == 0)
            {
                InsertMetrics(new CalculatedMetrics(value, value, value, deviceMAC));
            }
            else
            {
                var updateDay = Builders<CalculatedMetrics>.Update.Set("dayAvg", f.AverageFromPrevAverage(getOneDayAverage(deviceMAC), value, nbValueDay));
                var updateWeek = Builders<CalculatedMetrics>.Update.Set("weekAvg", f.AverageFromPrevAverage(getOneWeekAverage(deviceMAC), value, nbValueWeek));
                var updateMonth = Builders<CalculatedMetrics>.Update.Set("monthAvg", f.AverageFromPrevAverage(getOneMonthAverage(deviceMAC), value, nbValueMonth));


                collection.UpdateOne(filter, updateDay);
                collection.UpdateOne(filter, updateWeek);
                collection.UpdateOne(filter, updateMonth);
            }
            return "Injection en BDD OK";
        }
       
        public void JEEUpdateDB(string json)
        {
            
            string url = "http://192.168.0.10:21080/AtlantisJavaEE-war/services/mobile";
            Connection connection = new Connection(url);
            Task.Run(() => connection.sendData(httpVerb.POST, "/addMetric", json.ToString()));
            



        }


    }
}
