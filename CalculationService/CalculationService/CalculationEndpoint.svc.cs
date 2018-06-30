using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using static CalculationService.JsonParser;

namespace CalculationService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez Service1.svc ou Service1.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
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


        public float getOneDayAverage(string id)
        {
            var collection = createDatabase();
            float result = 0;
            var filter = Builders<CalculatedMetrics>.Filter.Eq("id", ObjectId.Parse(id));
            List<CalculatedMetrics> c = collection.Find(filter).ToList();
            foreach (var item in c)
            {
                result = item.dayAvg;
            }

            return result;
        }

        public float getOneWeekAverage(string id)
        {
            var collection = createDatabase();
            float result = 0;
            var filter = Builders<CalculatedMetrics>.Filter.Eq("id", ObjectId.Parse(id));
            List<CalculatedMetrics> c = collection.Find(filter).ToList();
            foreach (var item in c)
            {
                result = item.weekAvg;
            }

            return result;
        }

        public float getOneMonthAverage(string id)
        {
            var collection = createDatabase();
            float result = 0;
            var filter = Builders<CalculatedMetrics>.Filter.Eq("id", ObjectId.Parse(id));
            List<CalculatedMetrics> c = collection.Find(filter).ToList();
            foreach (var item in c)
            {
                result = item.monthAvg;
            }

            return result;
        }

        //ToDO centraliser updateAverage(id, moyd, moyw, moyM)
        public void updateOneDayAverage(RawData rd, int nbValue)
        {
            var collection = createDatabase();
            Formulas f = new Formulas();
            var update = Builders<CalculatedMetrics>.Update.Set("dayAvg", f.AverageFromPrevAverage(getOneDayAverage(rd.DeviceMac), rd.Value, nbValue));
            var filter = Builders<CalculatedMetrics>.Filter.Eq("id", ObjectId.Parse(rd.DeviceMac));
            collection.UpdateOne(filter, update);

        }
        public void updateOneWeekAverage(RawData rd, int nbValue)
        {
            var collection = createDatabase();
            Formulas f = new Formulas();
            var update = Builders<CalculatedMetrics>.Update.Set("weekAvg", f.AverageFromPrevAverage(getOneWeekAverage(rd.DeviceMac), rd.Value, nbValue));
            var filter = Builders<CalculatedMetrics>.Filter.Eq("id", ObjectId.Parse(rd.DeviceMac));
            collection.UpdateOne(filter, update);

        }
        public void updateOneMonthAverage(RawData rd, int nbValue)
        {
            var collection = createDatabase();
            Formulas f = new Formulas();
            var update = Builders<CalculatedMetrics>.Update.Set("monthAvg", f.AverageFromPrevAverage(getOneMonthAverage(rd.DeviceMac), rd.Value, nbValue));
            var filter = Builders<CalculatedMetrics>.Filter.Eq("id", ObjectId.Parse(rd.DeviceMac));
            collection.UpdateOne(filter, update);

        }

        public void CalcMetricsFromRaw(RawData rd, int nbValueDay, int nbValueWeek, int nbValueMonth)
        {
            Formulas f = new Formulas();

            ////utilisation d'update pour chaque metrics
            updateOneDayAverage(rd, nbValueDay);
            updateOneWeekAverage(rd, nbValueWeek);
            updateOneMonthAverage(rd, nbValueMonth);


        }


    }
}
