using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Freightware.WebApi.Interface;
using Freightware.WebApi.Model;
using Microsoft.Extensions.Configuration;
using Triton.Core;
using Triton.Model.FWWebservice.Custom;
using Triton.Service.Utils;
using Vendor.Services.Freightware.PROD.GetAccountRateList;

namespace Freightware.WebApi.Repository
{
    public class RateRepository : FWRepositoryGeneric, IRate
    {
        private readonly IConfiguration _config;
        private readonly string _inUserid;
        private readonly string _inPassword;
        private readonly string _inRequestCtr;



        public RateRepository(IConfiguration config)
        {
            _config = config;
            _inUserid = config.GetSection(StringHelper.Freightware.FreightWare).GetSection(StringHelper.Freightware.InUserid).Value;
            _inPassword = config.GetSection(StringHelper.Freightware.FreightWare).GetSection(StringHelper.Freightware.InPassword).Value;
            _inRequestCtr = config.GetSection(StringHelper.Freightware.FreightWare).GetSection(StringHelper.Freightware.InRequestCtr).Value;
        }

        public async Task<RateModels> GetAsync(string accountCode, string active)
        {
            var xml = string.Empty;
            long result = 0;
            var resultList = new List<GetAccountRateListResponseAccountRateOutput>();

            var startValues = new GetAccountRateListStartValues
            {
                InAccountCode = accountCode.ToUpper(),
                InActiveRateInd = active
            };

            var sequence = new GetAccountRateListSequence
            {
                //ByAccountCodeRateSpecified = true,
                ByAccountCodeRate = true
            };

            var request = new GetAccountRateListRequest
            {
                InUserid = _inUserid,
                InPassword = _inPassword,
                InRequestCtr = _inRequestCtr,
                InCustCode = accountCode,
                Sequence = sequence,
                StartValues = startValues
            };

            var client = new FWV6WEBPortClient(FWV6WEBPortClient.EndpointConfiguration.FWV6WEBSOAP11Port);
            var fwResponsePacket = new FWResponsePacket();
            try
            {
                var response = await client.GetAccountRateListAsync(request);

                if (response.ReturnCode == "0000" || response.ReturnCode == "0130")
                    resultList = response.AccountRateOutputs.Select(x => x).ToList();

                // Page through the results
                while (response.ReturnCode == "0130")
                {
                    request.IoSessionData = response.IoSessionData;
                    response = await client.GetAccountRateListAsync(request);
                    resultList.AddRange(response.AccountRateOutputs);
                }

                fwResponsePacket.ReturnCode = response.ReturnCode;
                fwResponsePacket.ReturnMessage = response.ReturnMessage;
                fwResponsePacket.DataObject = response.AccountRateOutputs;

                // Convert to xml
                xml = GetXmlFromObject(resultList);

                //Pass the xml to the stored procedure
                const string sql = "proc_CustomerUniqueRates_Insert @accountCode, @xml";
                await using var connection = DBConnection.GetOpenConnection(_config.GetConnectionString(StringHelpers.Database.Crm));
                result = connection.Query<long>(sql, new { accountCode, xml }, commandTimeout: 50000).FirstOrDefault();
            }
            catch (Exception x)
            {
                fwResponsePacket.ReturnCode = "Internal Error";
                fwResponsePacket.ReturnMessage = x.Message + " - " + x.InnerException;
            }

            var model = new RateModels
            {
                Xml = xml,
                RecordCount = result
            };

            return model;
        }

        public async Task<RateModels> GetStandardRateAsync(string accountCode, string active, string rateArea)
        {
            var xml = string.Empty;
            long recordCount = 0;
            var resultList = new List<GetAccountRateListResponseAccountRateOutput>();

            var startValues = new GetAccountRateListStartValues
            {
                InAccountCode = accountCode.ToUpper(),
                InActiveRateInd = active.ToUpper()
            };

            var sequence = new GetAccountRateListSequence
            {
                //ByAccountCodeRateSpecified = true,
                ByAccountCodeRate = true
            };

            var request = new GetAccountRateListRequest
            {
                InUserid = _inUserid,
                InPassword = _inPassword,
                InRequestCtr = _inRequestCtr,
                InCustCode = accountCode,
                Sequence = sequence,
                StartValues = startValues
            };

            var client = new FWV6WEBPortClient(FWV6WEBPortClient.EndpointConfiguration.FWV6WEBSOAP11Port);
            var fwResponsePacket = new FWResponsePacket();
            try
            {
                var response = await client.GetAccountRateListAsync(request);

                if (response.ReturnCode == "0000" || response.ReturnCode == "0130")
                    resultList = response.AccountRateOutputs.Select(x => x).ToList();

                // Page through the results
                while (response.ReturnCode == "0130")
                {
                    request.IoSessionData = response.IoSessionData;
                    response = await client.GetAccountRateListAsync(request);
                    resultList.AddRange(response.AccountRateOutputs);
                }

                fwResponsePacket.ReturnCode = response.ReturnCode;
                fwResponsePacket.ReturnMessage = response.ReturnMessage;
                fwResponsePacket.DataObject = response.AccountRateOutputs;

                // Convert to xml
                if (rateArea != null)
                {
                    var filteredList = resultList.Where(x => x.AreaFrom.Contains(rateArea.ToUpper()) || x.AreaTo.Contains(rateArea.ToUpper()));
                    xml = GetXmlFromObject(filteredList.ToList());
                    recordCount = filteredList.Count();
                }
                else
                {
                    xml = GetXmlFromObject(resultList);
                    recordCount = resultList.Count();
                }

                const string sql = "proc_Standard_Rates_Insert @accountCode, @xml";
                await using var connection = DBConnection.GetOpenConnection(_config.GetConnectionString(StringHelpers.Database.Crm));
                recordCount = connection.Query<long>(sql, new { accountCode, xml }).FirstOrDefault();
            }
            catch (Exception x)
            {
                fwResponsePacket.ReturnCode = "Internal Error";
                fwResponsePacket.ReturnMessage = x.Message + " - " + x.InnerException;
                throw x;
            }

            var model = new RateModels
            {
                Xml = xml,
                RecordCount = recordCount
            };

            return model;
        }

        public async Task<RateModels> GetStandardRateInsertAsync(string accountCode, int rateCycleId, int rateYear)
        {
            var xml = string.Empty;
            long recordCount = 0;
            var resultList = new List<GetAccountRateListResponseAccountRateOutput>();

            var startValues = new GetAccountRateListStartValues
            {
                InAccountCode = accountCode.ToUpper(),
                InActiveRateInd = "N"
            };

            var sequence = new GetAccountRateListSequence
            {
                //ByAccountCodeRateSpecified = true,
                ByAccountCodeRate = true
            };

            var request = new GetAccountRateListRequest
            {
                InUserid = _inUserid,
                InPassword = _inPassword,
                InRequestCtr = _inRequestCtr,
                InCustCode = accountCode,
                Sequence = sequence,
                StartValues = startValues
            };

            var client = new FWV6WEBPortClient(FWV6WEBPortClient.EndpointConfiguration.FWV6WEBSOAP11Port);
            var fwResponsePacket = new FWResponsePacket();
            try
            {
                var response = await client.GetAccountRateListAsync(request);

                if (response.ReturnCode == "0000" || response.ReturnCode == "0130")
                    resultList = response.AccountRateOutputs.Select(x => x).ToList();

                // Page through the results
                while (response.ReturnCode == "0130")
                {
                    request.IoSessionData = response.IoSessionData;
                    response = await client.GetAccountRateListAsync(request);
                    resultList.AddRange(response.AccountRateOutputs);
                }

                fwResponsePacket.ReturnCode = response.ReturnCode;
                fwResponsePacket.ReturnMessage = response.ReturnMessage;
                fwResponsePacket.DataObject = response.AccountRateOutputs;

                // Convert to xml
                var filteredList = resultList.Where(x => x.DateCease == 0);
                xml = GetXmlFromObject(filteredList.ToList());
                recordCount = filteredList.Count();


                const string sql = "proc_Standard_Rates_Insert_From_WS @rateCycleId, @rateYear, @xml";
                await using var connection = DBConnection.GetOpenConnection(_config.GetConnectionString(StringHelpers.Database.Crm));
                recordCount = connection.Query<long>(sql, new { rateCycleId, rateYear, xml }).FirstOrDefault();
            }
            catch (Exception x)
            {
                fwResponsePacket.ReturnCode = "Internal Error";
                fwResponsePacket.ReturnMessage = x.Message + " - " + x.InnerException;
                throw x;
            }

            var model = new RateModels
            {
                Xml = xml,
                RecordCount = recordCount
            };

            return model;
        }
    }
}