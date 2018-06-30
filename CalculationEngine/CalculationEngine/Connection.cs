using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine
{
    public enum httpVerb
    {
        GET, POST, PUT, DELETE
    }

    class Connection
    {
        public string url { get; set; }

        public Connection(string url)
        {
            this.url = url;
        }

        public string makeRequest(httpVerb method, string endPoint)
        {
            string strResponse = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + endPoint);
            request.Method = method.ToString();
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new ApplicationException("Request failed : " + response.StatusCode.ToString());
                }

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
            return strResponse;
        }
    }
}
