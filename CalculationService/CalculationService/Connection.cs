using System;
using System.IO;
using System.Net;

namespace CalculationService
{
    public enum httpVerb
    {
        GET, POST, PUT, DELETE
    }

    public class Connection
    {
        public string url { get; set; }

        public Connection(string url)
        {
            this.url = url;
        }

        public string sendData(httpVerb method, string endPoint, string postJSON)
        {
            string strResponse = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + endPoint);
            request.Method = method.ToString();

            if (method == httpVerb.POST && postJSON != null)
            {
                request.ContentType = "application/json";
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(postJSON);
                    writer.Close();
                }
            }
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Console.WriteLine("Request answer : " + response.StatusCode.ToString());
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                strResponse = reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return strResponse;
        }

        public string getData(httpVerb method, string endPoint)
        {
            string strResponse = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPoint);
            request.Method = method.ToString();


            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                strResponse = reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return strResponse;
        }


    }
}