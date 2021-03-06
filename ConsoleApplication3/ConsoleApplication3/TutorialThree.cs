﻿using UAPIConsumptionSamples;
using UAPIConsumptionSamples.SystemReference;
using ConsoleApplication3.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace UAPIConsumptionSamples
{
    class TutorialThree
    {
        static void Main(string[] args)
        {
            //
            // PING REQUEST
            //
            String payload = "this my payload; there are many like it but this one is mine";
            String someTraceId = "doesntmatter-8176";
            String originApp = "UAPI";

            //set up the request parameters into a PingReq object
            PingReq req = new PingReq();
            req.Payload = payload;
            req.TraceId = someTraceId;

            BillingPointOfSaleInfo billSetInfo = new BillingPointOfSaleInfo();
            billSetInfo.OriginApplication = originApp;

            req.BillingPointOfSaleInfo = billSetInfo;
            req.TargetBranch = CommonUtility.GetConfigValue(ProjectConstants.G_TARGET_BRANCH);
            Console.WriteLine(req);



            try
            {
                //run the ping request
                //WSDLService.sysPing.showXML(true);
                SystemPingPortTypeClient client = new SystemPingPortTypeClient("SystemPingPort", WsdlService.SYSTEM_ENDPOINT);
                //Console.WriteLine(client.Endpoint);
                client.ClientCredentials.UserName.UserName = Helper.RetrunUsername();
                client.ClientCredentials.UserName.Password = Helper.ReturnPassword();
                /*var httpHeaders = new Dictionary<string, string>();
                httpHeaders.Add("Username", "travelportsuperadmin");
                httpHeaders.Add("Password", "abc123");
                client.Endpoint.EndpointBehaviors.Add(new HttpHeadersEndpointBehavior(httpHeaders));*/

                /*HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[HttpRequestHeader.Authorization] = "Basic " +
                    Convert.ToBase64String(Encoding.ASCII.GetBytes(client.ClientCredentials.UserName.UserName +
                    ":" + client.ClientCredentials.UserName.Password));

                 using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                    {
                        OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] =
                            httpRequestProperty;
                        return client.processRequest(castRequest) as TSRsp;
                    } 


                OperationContext.Current.OutgoingMessageProperties*/



                var httpHeaders = Helper.ReturnHttpHeader();
                client.Endpoint.EndpointBehaviors.Add(new HttpHeadersEndpointBehavior(httpHeaders));

                PingRsp rsp = client.service(req);
                //print results.. payload and trace ID are echoed back in response
                Console.WriteLine(rsp.Payload);
                //Console.WriteLine(rsp.TraceId);
                //Console.WriteLine(rsp.TransactionId);
            }
            catch (Exception e)
            {
                //usually only the error message is useful, not the full stack
                //trace, since the stack trace in is your address space...
                Console.WriteLine("Error : " + e.Message);
            }
        }

    }
}
