using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CalculationService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface CalculationEndpoint
    {
        [OperationContract]
        void InsertMetrics(CalculatedMetrics metrics);

        [OperationContract]
        [WebInvoke(Method = "POST")]
        string updateAverage(string id, float value, int nbValueDay, int nbValueWeek, int nbValueMonth);


        [OperationContract]
        [WebInvoke(Method ="POST")]
        void JEEUpdateDB(string json);
    }
}
