using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Triton.Model.FWWebservice.Custom;
using Vendor.Services.Freightware.PROD.GetSiteList;
using Microsoft.Extensions.Configuration;

namespace Freightware.WebApi.BusinessLogic
{
    public class SiteBL
    {
        private readonly string inUserid;
        private readonly string inPassword;
        private readonly string inRequestCtr;
        private readonly string inUATUserid;
        private readonly string inUATPassword;


        public SiteBL(IConfiguration config)
        {
            inUserid = config.GetSection("Freightware").GetSection("InUserid").Value;
            inPassword = config.GetSection("Freightware").GetSection("InPassword").Value;
            inRequestCtr = config.GetSection("Freightware").GetSection("InRequestCtr").Value;
            inUATUserid = config.GetSection("Freightware").GetSection("InUATUserid").Value;
            inUATPassword = config.GetSection("Freightware").GetSection("InUATPassword").Value;
        }

        public async Task<FWResponsePacket> GetSiteList(string customerCode, string siteCode)
        {
            var request = new GetSiteListRequest
            {
                InUserid = inUserid,
                InPassword = inPassword,
                InRequestCtr = inRequestCtr,
                StartValues = new GetSiteListStartValues
                {
                    CustomerCode = customerCode.ToUpper(),
                    SiteCode = siteCode.ToUpper() + "*"
                },
                Sequence = new GetSiteListSequence
                {
                    ByCustomerSite = true,
                    ByCustomerSiteSpecified = true,
                    ByNameSite = true,
                    ByNameSiteSpecified = true
                }
            };

            var client = new FWV6WEBPortClient(FWV6WEBPortClient.EndpointConfiguration.FWV6WEBSOAP11Port);
            var response = new FWResponsePacket();
            try
            {
                List<GetSiteListResponseSiteOutput> filtered = new List<GetSiteListResponseSiteOutput>();
                var x = await client.GetSiteListAsync(request);
                //Store initial Results
                if (x.ReturnCode == "0000" || x.ReturnCode == "0130")
                    filtered = (from c in x.SiteOutputs
                                select c
                                ).ToList();

                //Get remaining charges if anyes
                while (x.ReturnCode == "0130")
                {
                    request.IoSessionData = x.IoSessionData;
                    x = await client.GetSiteListAsync(request);
                    foreach (var v in x.SiteOutputs)
                    {
                        filtered.Add(v);
                    }
                    request.IoSessionData = x.IoSessionData;
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

        public async Task<FWResponsePacket> GetSiteListUAT(string customerCode, string siteCode)
        {
            var request = new Vendor.Services.Freightware.UAT.GetSiteList.GetSiteListRequest
            {
                InUserid = inUATUserid,
                InPassword = inUATPassword,
                InRequestCtr = inRequestCtr,
                StartValues = new Vendor.Services.Freightware.UAT.GetSiteList.GetSiteListStartValues
                {
                    CustomerCode = customerCode.ToUpper(),
                    SiteCode = siteCode.ToUpper() + "*"
                },
                Sequence = new Vendor.Services.Freightware.UAT.GetSiteList.GetSiteListSequence
                {
                    ByCustomerSite = true,
                    ByCustomerSiteSpecified = true,
                    ByNameSite = true,
                    ByNameSiteSpecified = true
                }
            };

            var client = new Vendor.Services.Freightware.UAT.GetSiteList.FWV6WEBPortClient(Vendor.Services.Freightware.UAT.GetSiteList.FWV6WEBPortClient.EndpointConfiguration.FWV6WEBSOAP11Port);
            var response = new FWResponsePacket();
            try
            {
                List<Vendor.Services.Freightware.UAT.GetSiteList.GetSiteListResponseSiteOutput> filtered = new List<Vendor.Services.Freightware.UAT.GetSiteList.GetSiteListResponseSiteOutput>();
                var x = await client.GetSiteListAsync(request);
                //Store initial Results
                if (x.ReturnCode == "0000" || x.ReturnCode == "0130")
                    filtered = (from c in x.SiteOutputs
                                select c
                                ).ToList();

                //Get remaining charges if anyes
                while (x.ReturnCode == "0130")
                {
                    request.IoSessionData = x.IoSessionData;
                    x = await client.GetSiteListAsync(request);
                    foreach (var v in x.SiteOutputs)
                    {
                        filtered.Add(v);
                    }
                    request.IoSessionData = x.IoSessionData;
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

                public async Task<FWResponsePacket> SetSiteUAT(string customerCode, string siteCode)
        {
            var request = new Vendor.Services.Freightware.UAT.GetSiteList.GetSiteListRequest
            {
                InUserid = inUATUserid,
                InPassword = inUATPassword,
                InRequestCtr = inRequestCtr,
                StartValues = new Vendor.Services.Freightware.UAT.GetSiteList.GetSiteListStartValues
                {
                    CustomerCode = customerCode.ToUpper(),
                    SiteCode = siteCode.ToUpper() + "*"
                },
                Sequence = new Vendor.Services.Freightware.UAT.GetSiteList.GetSiteListSequence
                {
                    ByCustomerSite = true,
                    ByCustomerSiteSpecified = true,
                    ByNameSite = true,
                    ByNameSiteSpecified = true
                }
            };

            var client = new Vendor.Services.Freightware.UAT.GetSiteList.FWV6WEBPortClient(Vendor.Services.Freightware.UAT.GetSiteList.FWV6WEBPortClient.EndpointConfiguration.FWV6WEBSOAP11Port);
            var response = new FWResponsePacket();
            try
            {
                List<Vendor.Services.Freightware.UAT.GetSiteList.GetSiteListResponseSiteOutput> filtered = new List<Vendor.Services.Freightware.UAT.GetSiteList.GetSiteListResponseSiteOutput>();
                var x = await client.GetSiteListAsync(request);
                //Store initial Results
                if (x.ReturnCode == "0000" || x.ReturnCode == "0130")
                    filtered = (from c in x.SiteOutputs
                                select c
                                ).ToList();

                //Get remaining charges if anyes
                while (x.ReturnCode == "0130")
                {
                    request.IoSessionData = x.IoSessionData;
                    x = await client.GetSiteListAsync(request);
                    foreach (var v in x.SiteOutputs)
                    {
                        filtered.Add(v);
                    }
                    request.IoSessionData = x.IoSessionData;
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
