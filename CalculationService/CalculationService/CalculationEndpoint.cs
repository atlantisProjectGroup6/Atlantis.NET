﻿using System;
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

        //[OperationContract]
        //[WebInvoke(Method = "POST",
        //    ResponseFormat = WebMessageFormat.Xml,
        //    BodyStyle = WebMessageBodyStyle.Wrapped,
        //   UriTemplate ="MongoUpdate" )]
        //string update3Average(string id, float value, int nbValueDay, int nbValueWeek, int nbValueMonth);


        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "JEEUpdate")]
        void JEEUpdateDB(MetricContract mc);

        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
           UriTemplate = "post")]
        string post(MetricContract rd);

        [OperationContract]
        [WebGet(UriTemplate = "/GetMetricDetails",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        MetricContract GetMetricDetails();


        //[OperationContract]
        //[WebGet(UriTemplate = "/update1average",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json)]
        //string updateAverage(string deviceMAC, float value);


        [OperationContract]
        [WebInvoke(Method ="POST",
            UriTemplate = "device/monthAverage",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        AverageSend getOneMonthAverage(DeviceMacReceived devicemac);
    }
}
