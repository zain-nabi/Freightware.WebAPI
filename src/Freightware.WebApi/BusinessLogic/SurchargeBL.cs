using System;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Triton.Model.FWWebservice.Custom;
using System.Linq;
using System.Collections.Generic;
using Vendor.Services.Freightware.PROD.GetChargesList;

namespace Freightware.WebApi.BusinessLogic
{
    public class SurchargeBL
    {
        private readonly string inUserid;
        private readonly string inPassword;
        private readonly string inRequestCtr;
        private readonly string inUATUserid;
        private readonly string inUATPassword;

        public SurchargeBL(IConfiguration config)
        {
            inUserid = config.GetSection("Freightware").GetSection("InUserid").Value;
            inPassword = config.GetSection("Freightware").GetSection("InPassword").Value;
            inRequestCtr = config.GetSection("Freightware").GetSection("InRequestCtr").Value;
            inUATUserid = config.GetSection("Freightware").GetSection("InUATUserid").Value;
            inUATPassword = config.GetSection("Freightware").GetSection("InUATPassword").Value;
        }

        public async Task<FWResponsePacket> GetSurcharges()
        {
            var request = new GetChargesListRequest
            {
                InUserid = inUserid,
                InPassword = inPassword,
                InRequestCtr = inRequestCtr,
                Sequence = new GetChargesListSequence
                {
                    ByTypeCodeSpecified = false
                },
                EffectiveDate = DateTime.Now.ToString("yyyyMMdd")
            };
            var client = new Vendor.Services.Freightware.PROD.GetChargesList.FWV6WEBPortClient(Vendor.Services.Freightware.PROD.GetChargesList.FWV6WEBPortClient.EndpointConfiguration.FWV6WEBSOAP11Port);
            var response = new FWResponsePacket();
            try
            {
                List<GetChargesListResponseChargesOutput> filtered = new List<GetChargesListResponseChargesOutput>();
                var x = await client.GetChargesListAsync(request);
                //Store initial Results
                if (x.ReturnCode == "0000" || x.ReturnCode == "0130")
                    filtered = (from c in x.ChargesOutputs
                                select c
                                ).ToList();

                //Get remaining charges if anyes
                while (x.ReturnCode == "0130")
                {
                    request.IoSessionData = x.IoSessionData;
                    x = await client.GetChargesListAsync(request);
                    foreach (var v in x.ChargesOutputs)
                    {
                        filtered.Add(v);
                    }
                }

                response.ReturnCode = x.ReturnCode;
                response.ReturnMessage = x.ReturnMessage;
                response.DataObject = filtered;
            }
            catch (Exception x)
            {
                response.ReturnCode = "Internal Error";
                response.ReturnMessage = x.Message + " - " + x.InnerException;
            }
            return response;
        }
    }
}
