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
        void CalcMetricsFromRaw(RawData rd, int nbValueDay, int nbValueWeek, int nbValueMonth);

        [OperationContract]
        void CalcMetricsFromJson( int nbValueDay, int nbValueWeek, int nbValueMonth);
    }
}
