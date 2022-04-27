using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Triton.Model.CRM.Tables;
using Triton.Model.FWWebservice.Custom;
using Vendor.Services.Freightware.PROD.GetPcodeList;
using Vendor.Services.Freightware.UAT.SetCollection;

namespace Freightware.WebApi.BusinessLogic
{
    public class PostalCodeBL
    {
        private readonly string inUserid;
        private readonly string inPassword;
        private readonly string inRequestCtr;
        private readonly string inUATUserid;
        private readonly string inUATPassword;

        public PostalCodeBL(IConfiguration config)
        {
            inUserid = config.GetSection("Freightware").GetSection("InUserid").Value;
            inPassword = config.GetSection("Freightware").GetSection("InPassword").Value;
            inRequestCtr = config.GetSection("Freightware").GetSection("InRequestCtr").Value;
            inUATUserid = config.GetSection("Freightware").GetSection("InUATUserid").Value;
            inUATPassword = config.GetSection("Freightware").GetSection("InUATPassword").Value;
        }

        public async Task<List<GetPcodeListResponsePcodeOutput>> GetPostalList(string searchPhrase)
        {
            GetPcodeListRequest postalRequest = new GetPcodeListRequest
            {
                InUserid = inUserid,
                InPassword = inPassword,
                Sequence = new GetPcodeListSequence
                {
                    ByNamePcode = true,
                    ByNamePcodeSpecified = true,
                    ByBranchPcode = false,
                    ByBranchPcodeSpecified = false,
                    ByHubBranchAreaPcode = false,
                    ByHubBranchAreaPcodeSpecified = false,
                    ByPcode = false,
                    ByPcodeSpecified = false,
                    ByPcodeName = false,
                    ByPcodeNameSpecified = false
                },
                StartValues = new GetPcodeListStartValues
                {
                    PcodeName = searchPhrase + "*"
                }
            };

            Vendor.Services.Freightware.PROD.GetPcodeList.FWV6WEBPortClient client = new Vendor.Services.Freightware.PROD.GetPcodeList.FWV6WEBPortClient(Vendor.Services.Freightware.PROD.GetPcodeList.FWV6WEBPortClient.EndpointConfiguration.FWV6WEBSOAP11Port);
            var pc = await client.GetPcodeListAsync(postalRequest);


            List<GetPcodeListResponsePcodeOutput> filtered = new List<GetPcodeListResponsePcodeOutput>();

            if (pc.PcodeOutputs != null && pc.PcodeOutputs.Count() > 0)
                filtered = (from c in pc.PcodeOutputs
                            select c
                            ).ToList();

            //Store the IOSession Data to retrieve more data
            string strNewIOSessionData = pc.IoSessionData;

            //Get all codes
            while (pc.ReturnCode == "0130")
            {
                postalRequest.IoSessionData = pc.IoSessionData;
                pc = await client.GetPcodeListAsync(postalRequest);

                foreach (var v in pc.PcodeOutputs)
                {
                    filtered.Add(v);
                }
            }
            return filtered;
        }

        public async Task<List<GetPcodeListResponsePcodeOutput>> GetPostalListByCode(string code)
        {
            GetPcodeListRequest postalRequest = new GetPcodeListRequest
            {
                InUserid = inUserid,
                InPassword = inPassword,
                Sequence = new GetPcodeListSequence
                {
                    ByNamePcode = false,
                    ByNamePcodeSpecified = false,
                    ByBranchPcode = false,
                    ByBranchPcodeSpecified = false,
                    ByHubBranchAreaPcode = false,
                    ByHubBranchAreaPcodeSpecified = false,
                    ByPcode = true,
                    ByPcodeSpecified = true,
                    ByPcodeName = false,
                    ByPcodeNameSpecified = false
                },
                StartValues = new GetPcodeListStartValues
                {
                    PostCode = code + "*"
                }
            };

            Vendor.Services.Freightware.PROD.GetPcodeList.FWV6WEBPortClient client = new Vendor.Services.Freightware.PROD.GetPcodeList.FWV6WEBPortClient(Vendor.Services.Freightware.PROD.GetPcodeList.FWV6WEBPortClient.EndpointConfiguration.FWV6WEBSOAP11Port);
            var pc = await client.GetPcodeListAsync(postalRequest);


            List<GetPcodeListResponsePcodeOutput> filtered = new List<GetPcodeListResponsePcodeOutput>();

            if (pc.PcodeOutputs != null && pc.PcodeOutputs.Count() > 0)
                filtered = (from c in pc.PcodeOutputs
                            select c
                            ).ToList();

            //Store the IOSession Data to retrieve more data
            string strNewIOSessionData = pc.IoSessionData;

            //Get all codes
            while (pc.ReturnCode == "0130")
            {
                postalRequest.IoSessionData = pc.IoSessionData;
                pc = await client.GetPcodeListAsync(postalRequest);

                foreach (var v in pc.PcodeOutputs)
                {
                    filtered.Add(v);
                }
            }
            return filtered;
        }
    }
}
