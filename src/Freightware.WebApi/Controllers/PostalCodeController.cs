using System.Collections.Generic;
using System.Threading.Tasks;
using Freightware.WebApi.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using Vendor.Services.Freightware.PROD.GetPcodeList;

namespace Freightware.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostalCodeController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly PostalCodeBL postalCodeBL;

        public PostalCodeController(IConfiguration config)
        {
            _config = config;
            postalCodeBL = new PostalCodeBL(_config);
        }

        [HttpGet("PostalList/{searchPhrase}")]
        [SwaggerOperation(Summary = "Get postal code list by phrase", Description = "Get the postal codes with names matching a phrase")]
        public async Task<List<GetPcodeListResponsePcodeOutput>> GetPostalList(string searchPhrase)
        {
            return await postalCodeBL.GetPostalList(searchPhrase);
        }

        [HttpGet("PostalList/GetByCode/{code}")]
        [SwaggerOperation(Summary = "Get postal code list by code", Description = "Get the postal codes with code matching")]
        public async Task<List<GetPcodeListResponsePcodeOutput>> GetPostalListByCode(string code)
        {
            return await postalCodeBL.GetPostalListByCode(code);
        }
    }
}
