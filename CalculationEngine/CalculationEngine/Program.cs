using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine
{
    class Program
    {
        public void CalcMetricsFromJson(int nbValueDay, int nbValueWeek, int nbValueMonth)
        {
            Formulas f = new Formulas();
            string json = "{\"mac\":\"C4: 67:2A: FB: 1B: 4E\",\"timestamp\":1530459131,\"value\":\"True\",\"type\":1,\"name\":\"Presence sensor garage\"}";




            List<String> rawList = JsonConvert.DeserializeObject<List<String>>(json);
            foreach (var item in rawList)
            {
               //pdateOneDayAverage(item, nbValueDay);
                //updateOneWeekAverage(item, nbValueWeek);
                //updateOneMonthAverage(item, nbValueMonth);
            }
        }

        static void Main(string[] args)
        {

            //HttpClient clientHttp = new HttpClient();
            //clientHttp.BaseAddress = new Uri("http://127.0.0.1:3000/");
            //clientHttp.DefaultRequestHeaders.Accept.Clear();
            //clientHttp.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            //HttpResponseMessage msg =   clientHttp.GetAsync("devicesData").Result;
            //Console.WriteLine(msg.Content.ReadAsStringAsync().Result);

            ServiceReference1.CalculationEndpointClient client = new ServiceReference1.CalculationEndpointClient();
            String retour = client.updateAverage("FF:FF:FF:01", 3, 1, 1, 1);
            Console.WriteLine(retour);
            Console.ReadLine();

        }
    }
}
