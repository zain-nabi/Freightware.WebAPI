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
    public class QuoteController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly QuoteBL quoteBL;

        public QuoteController(IConfiguration config)
        {
            _config = config;
            quoteBL = new QuoteBL(_config);
        }

        [HttpPost("Quotes/UAT")]
        [SwaggerOperation(Summary = "Post a quote to UAT ", Description = "Post a quote to UAT")]
        public async Task<ActionResult<FWResponsePacket>> SetQuoteUAT(Vendor.Services.Freightware.UAT.SetQuote.SetQuoteInQuote setQuoteInQuote)
        {
            return await quoteBL.SetQuoteUAT(setQuoteInQuote);
        }
    }
}
