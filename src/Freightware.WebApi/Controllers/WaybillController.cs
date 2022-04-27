using System.Threading.Tasks;
using Freightware.WebApi.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using Triton.Model.CRM.Custom;
using Triton.Model.FWWebservice.Custom;

namespace Freightware.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaybillController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly WaybillBL waybillBL;

        public WaybillController(IConfiguration config)
        {
            _config = config;
            waybillBL = new WaybillBL(_config);
        }

        [HttpGet("UAT/Waybill/")]
        [SwaggerOperation(Summary = "Get UAT waybill", Description = "Get a uat waybill.")]
        public async Task<ActionResult<FWResponsePacket>> GetWaybillUAT(string waybillNo)
        {
            return await waybillBL.GetWaybillUAT(waybillNo);
        }

        [HttpGet("Waybill/")]
        [SwaggerOperation(Summary = "Get waybill", Description = "Get a waybill.")]
        public async Task<ActionResult<FWResponsePacket>> GetWaybill(string waybillNo)
        {
            return await waybillBL.GetWaybill(waybillNo);
        }

        [HttpPost("UAT/Waybill/")]
        [SwaggerOperation(Summary = "Post UAT waybill", Description = "Post a UAT waybill.")]
        public async Task<ActionResult<FWResponsePacket>> PostWaybillUAT(CustomerWaybillSubmitModels model, string dbName = "CRMTest")
        {
            return await waybillBL.PostWaybillUAT(model, dbName);
        }

        [HttpPost("Waybill/")]
        [SwaggerOperation(Summary = "Post waybill", Description = "Post a waybill.")]
        public async Task<ActionResult<FWResponsePacket>> PostWaybill(CustomerWaybillSubmitModels model, string dbName = "CRM")
        {
            return await waybillBL.PostWaybill(model, dbName);
        }

        [HttpGet("PROD/Waybill/")]
        [SwaggerOperation(Summary = "Get prod waybill", Description = "Get a prod waybill.")]
        public async Task<ActionResult<FWResponsePacket>> GetWaybillDelivery(string waybillNo)
        {
            return await waybillBL.GetWaybillDeliverys(waybillNo);
        }
    }
}
