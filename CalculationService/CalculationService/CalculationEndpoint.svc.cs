﻿using MongoDB.Bson;
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

        public AverageSend getOneMonthAverage(DeviceMacReceived dm)
        {
            var collection = createDatabase();
            float result = 0;
            var filter = Builders<CalculatedMetrics>.Filter.Eq("deviceMAC", dm);
            List<CalculatedMetrics> c = collection.Find(filter).ToList();
            foreach (var item in c)
            {
                    result = item.monthAvg;
            }

            AverageSend a = new AverageSend();
            a.average = result;
            return a;
        }

        //public string update3Average(string deviceMAC, float value, int nbValueDay, int nbValueWeek, int nbValueMonth)
        //{
        //    var collection = createDatabase();
        //    Formulas f = new Formulas();
        //    var filter = Builders<CalculatedMetrics>.Filter.Eq("deviceMAC", deviceMAC);

        //    if (collection.Find(filter).ToList().Count == 0)
        //    {
        //        InsertMetrics(new CalculatedMetrics(value, value, value, deviceMAC));
        //    }
        //    else
        //    {
        //        var updateDay = Builders<CalculatedMetrics>.Update.Set("dayAvg", f.AverageFromPrevAverage(getOneDayAverage(deviceMAC), value, nbValueDay));
        //        var updateWeek = Builders<CalculatedMetrics>.Update.Set("weekAvg", f.AverageFromPrevAverage(getOneWeekAverage(deviceMAC), value, nbValueWeek));
        //        var updateMonth = Builders<CalculatedMetrics>.Update.Set("monthAvg", f.AverageFromPrevAverage(getOneMonthAverage(deviceMAC), value, nbValueMonth));


        //        collection.UpdateOne(filter, updateDay);
        //        collection.UpdateOne(filter, updateWeek);
        //        collection.UpdateOne(filter, updateMonth);
        //    }
        //    return "Injection en BDD OK";
        //}


        public string updateAverage(string deviceMAC, float value)
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
                var updateDay = Builders<CalculatedMetrics>.Update.Set("dayAvg", value);
                var updateWeek = Builders<CalculatedMetrics>.Update.Set("weekAvg", value);
                var updateMonth = Builders<CalculatedMetrics>.Update.Set("monthAvg", value);


                collection.UpdateOne(filter, updateDay);
                collection.UpdateOne(filter, updateWeek);
                collection.UpdateOne(filter, updateMonth);
            }
            return "Injection en BDD OK";
        }


        public void JEEUpdateDB(MetricContract metric)
        {

            string url = "http://192.168.1.9:21080/AtlantisJavaEE-war/services/mobile";
            Connection connection = new Connection(url);
            var json = new JavaScriptSerializer().Serialize(metric);
            Task.Run(() => connection.sendData(httpVerb.POST, "/addMetric", json));

            
            if (!(metric == null))
            {
                string res = connection.getData(httpVerb.GET, "/allMetrics", "/device/" + metric.mac);
                var result = JsonConvert.DeserializeObject<List<MetricForCalculation>>(res);
                MetricForCalculation mfc = new MetricForCalculation();
                mfc.date = metric.timestamp;
                mfc.value = metric.value;
                result.Add(mfc);
                float total = 0;
                foreach (var item in result)
                {
                    total = total + float.Parse(item.value);
                }
                float moyenne = total / result.Count();
                updateAverage(metric.mac, moyenne);
            }
            
        }

        public string post(MetricContract rd)
        {
            return "";
        }

        public MetricContract GetMetricDetails()
        {
            MetricContract metric = new MetricContract();

            metric.mac = "A5-A6";

            return metric;
        }



    }
}
