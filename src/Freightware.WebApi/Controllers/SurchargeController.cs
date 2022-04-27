using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Triton.Model.FWWebservice.Custom;
using Freightware.WebApi.BusinessLogic;

namespace Freightware.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurchargeController : ControllerBase
    {
        private readonly IConfiguration _config;

        public SurchargeController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("Surcharges")]
        [SwaggerOperation(Summary = "Freightware Surcharge", Description = "Get currently active surcharges.")]
        public async Task<ActionResult<FWResponsePacket>> Get()
        {
            var surchargeBL = new SurchargeBL(_config);
            return await surchargeBL.GetSurcharges();
        }       
    }
}
