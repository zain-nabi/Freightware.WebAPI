using System.Threading.Tasks;
using Freightware.WebApi.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using Triton.Model.FWWebservice.Custom;

namespace Freightware.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly SiteBL siteBL;

        public SiteController(IConfiguration config)
        {
            _config = config;
            siteBL = new SiteBL(_config);
        }

        [HttpGet("CustomerSites/Customer/{customerCode}/SiteCode/{siteCode}")]
        [SwaggerOperation(Summary = "Get list of Customer Sites", Description = "Get a list of customer sites.")]
        public async Task<ActionResult<FWResponsePacket>> GetSiteList(string customerCode, string siteCode)
        {            
            return await siteBL.GetSiteList(customerCode, siteCode);
        }


        [HttpGet("UAT/CustomerSites/Customer/{customerCode}/SiteCode/{siteCode}")]
        [SwaggerOperation(Summary = "Get list of Customer Sites", Description = "Get a list of customer sites.")]
        public async Task<ActionResult<FWResponsePacket>> GetSiteListUAT(string customerCode, string siteCode)
        {
            return await siteBL.GetSiteListUAT(customerCode, siteCode);
        }

    }
}
