using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine
{
    class Program
    {
        public void CalcMetricsFromJson(int nbValueDay, int nbValueWeek, int nbValueMonth)
        {
            Formulas f = new Formulas();
            //List<RawData> rawList = new JsonParser().RawDataJSONParser(filePath);
            /*RawData rd = rawList.First()*/
            
            //HTTP request getallmetrics()
            string json = "[\"25\",\"24\"]";

            List<String> rawList = JsonConvert.DeserializeObject<List<String>>(json);
            foreach (var item in rawList)
            {
                updateOneDayAverage(item, nbValueDay);
                updateOneWeekAverage(item, nbValueWeek);
                updateOneMonthAverage(item, nbValueMonth);
            }
        }

        static void Main(string[] args)
        {
        }
    }
}
