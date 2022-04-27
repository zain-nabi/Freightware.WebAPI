using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Triton.Model.FWWebservice.Custom;

namespace Freightware.WebApi.BusinessLogic
{
    public class CustomerStatementBL
    {
        private readonly string inUserid;
        private readonly string inPassword;
        private readonly string inRequestCtr;

        public CustomerStatementBL(IConfiguration config)
        {
            inUserid = config.GetSection("Freightware").GetSection("InUserid").Value;
            inPassword = config.GetSection("Freightware").GetSection("InPassword").Value;
            inRequestCtr = config.GetSection("Freightware").GetSection("InRequestCtr").Value;
        }

        public async Task<FWResponsePacket> GetCustomerStatement(string accountCode, DateTime period)
        {
            var request = new Vendor.Services.Freightware.PROD.GetStatement.GetStatementRequest
            {
                InUserId = inUserid,
                InPassword = inPassword,
                InRequestCtr = inRequestCtr,
                Sequence = new Vendor.Services.Freightware.PROD.GetStatement.GetStatementSequence
                {
                    ByCustPostedType = true,
                    ByCustPostedTypeSpecified = true
                },
                StartValues = new Vendor.Services.Freightware.PROD.GetStatement.GetStatementStartValues
                {
                    CustCode = accountCode,
                    PostedPeriod = period.ToString("yyyyMMdd")
                }
            };

            var client = new Vendor.Services.Freightware.PROD.GetStatement.FWV6WEBPortClient(Vendor.Services.Freightware.PROD.GetStatement.FWV6WEBPortClient.EndpointConfiguration.FWV6WEBSOAP11Port);
            var response = new FWResponsePacket();
            try
            {
                var x = await client.GetStatementAsync(request);

                response.ReturnCode = x.ReturnCode;
                response.ReturnMessage = x.ReturnMessage;
                response.DataObject = x.StatementOutput;
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
