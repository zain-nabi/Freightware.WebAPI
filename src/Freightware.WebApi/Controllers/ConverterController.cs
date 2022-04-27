using System.Threading.Tasks;
using Freightware.WebApi.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using Vendor.Model;
using Vendor.Services.Freightware.PROD.SetQuote;

namespace Freightware.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConverterController : ControllerBase
    {
        private IConfiguration _config;

        public ConverterController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("Converter/GetConvertFromFWQuote")]
        [SwaggerOperation(Summary = "Convert from FW Quote", Description = "Returns a VendorQuoteModel model")]
        public async Task<ActionResult<VendorQuoteModel>> GetConvertFromFWQuote([FromBody] SetQuoteResponseQuote fwQuote, [FromQuery] VendorQuoteModel existQuote)
        {
            var converterBL = new FWConverterBL(_config);
            return await converterBL.GetConvertFromFWQuote(fwQuote, existQuote);
        }

        [HttpGet("Converter/GetConvertToFWInQuote")]
        [SwaggerOperation(Summary = "Convert To FW InQuote", Description = "Returns a SetQuoteInQuote model")]
        public async Task<ActionResult<SetQuoteInQuote>> GetConvertToFWInQuote([FromBody] QuoteModels quote)
        {
            var converterBL = new FWConverterBL(_config);
            return await converterBL.GetConvertToFWInQuote(quote);
        }

        [HttpGet("Converter/ConvertFromFWUATQuote")]
        [SwaggerOperation(Summary = "Convert From FW UAT Quote", Description = "Returns a VendorQuoteModel model")]
        public async Task<ActionResult<VendorQuoteModel>> ConvertFromFWUATQuote([FromBody] Vendor.Services.Freightware.UAT.SetQuote.SetQuoteResponseQuote fwQuote, [FromQuery] VendorQuoteModel existQuote)
        {
            var converterBL = new FWConverterBL(_config);
            return await converterBL.ConvertFromFWUATQuote(fwQuote, existQuote);
        }

        [HttpGet("Converter/ConvertToFWUATInQuote")]
        [SwaggerOperation(Summary = "Convert To FW UAT InQuote", Description = "Returns a SetQuoteInQuote model")]
        public async Task<ActionResult<Vendor.Services.Freightware.UAT.SetQuote.SetQuoteInQuote>> ConvertToFWUATInQuote([FromBody] VendorQuoteModel quote)
        {
            var converterBL = new FWConverterBL(_config);
            return await converterBL.ConvertToFWUATInQuote(quote);
        }

        [HttpGet("Converter/CovertFWWaybillToCustomerWaybill")]
        [SwaggerOperation(Summary = "Covert FW Waybill to CustomerWaybill", Description = "Returns a WaybillCustomerModel model")]
        public ActionResult<Triton.Model.CRM.Custom.WaybillCustomerModel> CovertFWWaybillToCustomerWaybill([FromBody] dynamic outWaybill)
        {
            var converterBL = new FWConverterBL(_config);
            return converterBL.CovertFWWaybillToCustomerWaybill(outWaybill);
        }
    }
}
