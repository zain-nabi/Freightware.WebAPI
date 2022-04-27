using System;
using System.Threading.Tasks;
using Triton.Service.Model.Applications.Custom;
using Triton.Service.Model.Applications.Tables;
using Triton.Service.Utils;

namespace Triton.Service.Data
{
    public class RoadFreightAgentHistoryService
    {
        public static async Task<bool> InsertAsync(RoadFreightAgentHistory roadFreightAgentHistory)
        {
            return await RestApiHelper.InsertAsync(new Uri(UrlHelper.Api.PurchaseOrderApi, $"{UrlHelper.Controller.RoadFreightAgentHistory}InsertAsync"), roadFreightAgentHistory);
        }


        public static async Task<RoadFreightAgentHistoryModel> GetByIdAsync(int roadFreightAgentId)
        {
            return await RestApiHelper.GetAsync<RoadFreightAgentHistoryModel>(new Uri(UrlHelper.Api.PurchaseOrderApi, $"{UrlHelper.Controller.RoadFreightAgentHistory}GetByIdAsync?roadFreightAgentId={roadFreightAgentId}"));
        }

    }
}
