using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Freightware.WebApi.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using Triton.Core;
using Triton.Model.CRM.Tables;
using Triton.Model.FWWebservice.Custom;

namespace Freightware.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly CollectionBL collectionBL;
        private static Connection _connection;

        public CollectionController(IConfiguration config, IHttpClientFactory factory)
        {
            _config = config;
            collectionBL = new CollectionBL(_config);
            _connection = new Connection(config, factory);
        }


        [HttpPost("UAT/Collection/")]
        [SwaggerOperation(Summary = "Post UAT collection", Description = "Post a UAT collection.")] 
        public async Task<ActionResult<FWResponsePacket>> PostCollectionUAT(CollectionRequests model)
        {
            // Get the transport types
            var transportTypeList = await _connection.GetAsync<List<TransportTypes>>(StringHelpers.Controllers.TransportTypes, "GetTransportTypes");

            // Get the customers
            var customer = await _connection.GetAsync<Customers>(StringHelpers.Controllers.Customer, $"GetCRMCustomerByID/id={model.CustomerID}");

            return await collectionBL.PostCollectionUAT(model, transportTypeList, customer.AccountCode);
        }
    }
}
