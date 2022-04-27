using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Triton.Model.FWWebservice.Custom;

namespace Freightware.WebApi.BusinessLogic
{
    public class QuoteBL
    {
        private readonly string inUserid;
        private readonly string inPassword;
        private readonly string inRequestCtr;
        private readonly string inUATUserid;
        private readonly string inUATPassword;

        public QuoteBL(IConfiguration config)
        {
            inUserid = config.GetSection("Freightware").GetSection("InUserid").Value;
            inPassword = config.GetSection("Freightware").GetSection("InPassword").Value;
            inRequestCtr = config.GetSection("Freightware").GetSection("InRequestCtr").Value;
            inUATUserid = config.GetSection("Freightware").GetSection("InUATUserid").Value;
            inUATPassword = config.GetSection("Freightware").GetSection("InUATPassword").Value;
        }

        public async Task<FWResponsePacket> SetQuoteUAT(Vendor.Services.Freightware.UAT.SetQuote.SetQuoteInQuote setQuoteInQuote)
        {
            var request = new Vendor.Services.Freightware.UAT.SetQuote.SetQuoteRequest
            {
                InUserid = inUATUserid,
                InPassword = inUATPassword,
                InRequestCtr = inRequestCtr,
                InAction = new Vendor.Services.Freightware.UAT.SetQuote.SetQuoteInAction
                {
                    InAddSpecified = true,
                    InAdd = true
                },
                InQuote = setQuoteInQuote
            };
            var client = new Vendor.Services.Freightware.UAT.SetQuote.FWV6WEBPortClient(Vendor.Services.Freightware.UAT.SetQuote.FWV6WEBPortClient.EndpointConfiguration.FWV6WEBSOAP11Port);
            var response = new FWResponsePacket();
            try
            {
                var x = await client.SetQuoteAsync(request);

                response.ReturnCode = x.ReturnCode;
                response.ReturnMessage = x.ReturnMessage;
                response.DataObject = x.Quote;
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
