using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Freightware.WebApi.Repository;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace Freightware.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly RateRepository _rate;

        public RateController(IConfiguration config)
        {
            _rate = new RateRepository(config);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Freightware Rates", Description = "Get the rates for a customer")]
        public async Task<long> GetAsync(string accountCode, string isActive)
        {
            // Get the rates from Freightware
            var model = await _rate.GetAsync(accountCode, isActive);

            return model.RecordCount;
        }

        [HttpGet("GetStandardRateAsync")]
        [SwaggerOperation(Summary = "Freightware Standard Rates", Description = "Get the standard rates for a rate class")]
        public async Task<long> GetStandardRateAsync(string accountCode, string isActive, string rateArea)
        {
            // Get the rates from Freightware
            var model = await _rate.GetStandardRateAsync(accountCode, isActive, rateArea);

            return model.RecordCount;
        }


        [HttpGet("GetStandardRateInsertAsync")]
        [SwaggerOperation(Summary = "Freightware Standard Rates Insert", Description = "Standard Rates insert for rate increases")]
        public async Task<long> GetStandardRateInsertAsync()
        {
            //string accountCode, int rateCycleId, int rateYear
            // Get the rates from Freightware
            string[] rateclass = { "JN5","JN4","JN3","JN2","JN1","J","JP1","JP2","JP3","JP4","JP5","JA","DN5","DN4","DN3","DN2","DN1","D","DP1","DP2","DP3","DP4","DP5","DA","TE1","TE2","TE3","TE4","TE5","SE1","SE2","SE3","SE4","SE5" };
            //"" - not working
            //string[] rateclass = { "JA" };

            for (int i = 0; i < rateclass.Length; i++)
            {
                var model = await _rate.GetStandardRateInsertAsync(rateclass[i], 2, 2022);
            }
            


            
                
          
            //var model = await _rate.GetStandardRateInsertAsync(accountCode, rateCycleId, rateYear);

            //return model.RecordCount;
            return 1;
        }
    }
}
