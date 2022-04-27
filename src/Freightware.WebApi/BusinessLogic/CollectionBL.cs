using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Triton.Model.CRM.Tables;
using Triton.Model.FWWebservice.Custom;
using Vendor.Services.Freightware.UAT.SetCollection;

namespace Freightware.WebApi.BusinessLogic
{
    public class CollectionBL
    {
        private readonly string inUserid;
        private readonly string inPassword;
        private readonly string inRequestCtr;
        private readonly string inUATUserid;
        private readonly string inUATPassword;

        public CollectionBL(IConfiguration config)
        {
            inUserid = config.GetSection("Freightware").GetSection("InUserid").Value;
            inPassword = config.GetSection("Freightware").GetSection("InPassword").Value;
            inRequestCtr = config.GetSection("Freightware").GetSection("InRequestCtr").Value;
            inUATUserid = config.GetSection("Freightware").GetSection("InUATUserid").Value;
            inUATPassword = config.GetSection("Freightware").GetSection("InUATPassword").Value;
        }

        public async Task<FWResponsePacket> PostCollectionUAT(CollectionRequests model, List<TransportTypes> transportTypeList, string accountCode)
        {
            string spIOSessionData = "";
            var transportType = transportTypeList.Find(x => x.TransportTypeID == model.ServiceTypeID).Description;
            Vendor.Services.Freightware.UAT.SetCollection.FWV6WEBPortClient client = new Vendor.Services.Freightware.UAT.SetCollection.FWV6WEBPortClient(Vendor.Services.Freightware.UAT.SetCollection.FWV6WEBPortClient.EndpointConfiguration.FWV6WEBSOAP11Port);

            Vendor.Services.Freightware.UAT.SetCollection.SetCollectionRequest request = new Vendor.Services.Freightware.UAT.SetCollection.SetCollectionRequest
            {
                InAction = new Vendor.Services.Freightware.UAT.SetCollection.SetCollectionInAction
                {
                    InAdd = true,
                    InAddSpecified = true
                },
                InUserId = inUATUserid,
                InPassword = inUATPassword,
                InCollection = new SetCollectionInCollection
                {
                    CallerContact = model.CallerContactInfo,
                    CallerName = model.CallerName,
                    CallerSurname = model.CallerSureName,
                    CallerRelationship = model.CallerRelationship,
                    CollectFromSecurity = model.CollFromSecurity ? "Y" : "N",
                    CustCode = accountCode,
                    //CustRefCode=model.CustomerXref,
                    DateCollection = model.DateCollReq.ToString("yyyyMMdd"),
                    ItemMass = model.EstMass.ToString(),
                    ItemQty = model.EstQty.ToString(),
                    ItemVol = model.EstVolume.ToString(),
                    ServiceType = transportType,
                    Remarks = model.Remarks,
                    SpecialInstructions = model.SpecialInstructions,
                    TimeCollBefore = model.TimeCollBefore,
                    TimeCollAfter = model.TimeCollAfter,
                    XrefNo = model.CustomerXref,
                    ReceiverInformation = new SetCollectionInCollectionReceiverInformation
                    {
                        RecAdd1 = model.RecAdd1,
                        RecAdd2 = model.RecAdd2,
                        RecAdd3 = model.RecAdd3,
                        RecAdd4 = model.RecAdd4,
                        RecAdd5 = model.RecAddPostalCode,
                        RecContact = model.RecContact,
                        RecFaxNo = model.recTellNo,
                        RecTelNo = model.recTellNo,
                        RecName = model.RecName,
                    },
                    RecInstructions = !String.IsNullOrEmpty(model.RecInstructions1) ? new string[] { model.RecInstructions1, model.RecInstructions2 } : new string[] { "", "" },
                    RecSite = model.RecSite,
                    SenderInformation = new SetCollectionInCollectionSenderInformation
                    {
                        SenAdd1 = model.SendAdd1,
                        SenAdd2 = model.SendAdd2,
                        SenAdd3 = model.SendAdd3,
                        SenAdd4 = model.SendAdd4,
                        SenAdd5 = model.SendAddPostalCode,
                        SenContact = model.SendContact,
                        SenFaxNo = model.SendTelNo,
                        SenTelNo = model.SendTelNo,
                        SenName = model.SendName
                    },
                    SendInstructions = !String.IsNullOrEmpty(model.SendInstructions1) ? new string[] { model.SendInstructions1, model.SendInstructions2 } : new string[] { "", "" },
                    SendSite = model.SendSite
                },
                InRequestCtr = inRequestCtr,
                IOSessionData = spIOSessionData
            };

            var collectionResponse = await client.SetCollectionAsync(request);

            return new FWResponsePacket
            {
                ReturnCode = collectionResponse.ReturnCode,
                ReturnMessage = collectionResponse.ReturnMessage,
                DataObject = collectionResponse.Collection
            };
        }
    }
}
