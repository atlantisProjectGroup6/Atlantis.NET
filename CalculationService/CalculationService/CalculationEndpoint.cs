using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CalculationService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface ICalculationEndpoint
    {
        [OperationContract]
        void InsertMetrics(CalculatedMetrics metrics);

        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "JEEUpdate")]
        string JEEUpdateDB(MetricContract mc);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "device/getAllCalculatedMetricsById",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        CalculatedMetrics getAllCalculatedMetricsByMac(DeviceMacReceived dm);
    }
}
