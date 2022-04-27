using System;
using System.Threading.Tasks;
using Triton.Service.Model.Applications.Custom;
using Triton.Service.Model.Applications.Tables;
using Triton.Service.Utils;

namespace Triton.Service.Data
{
    public class PurchaseOrderService
    {
        public static async Task<bool> UploadRoadFreightAgentAsync(string fileName, int userId) 
        {
            return await RestApiHelper.GetAsync<bool>(new Uri(UrlHelper.Api.PurchaseOrderApi, $"{UrlHelper.Controller.RoadFreightAgent}UploadRoadFreightAgentAsync?fileName={fileName}&userId={userId}"));
        }

        public static async Task<PurchaseOrder_Overview> PurcharseOrderOverviewAsync(int lookupcodeId, string selectedDate)
        {
            return await RestApiHelper.GetAsync<PurchaseOrder_Overview>(new Uri(UrlHelper.Api.PurchaseOrderApi, $"{UrlHelper.Controller.RoadFreightAgent}PurcharseOrderOverviewAsync?lookupcodeId={lookupcodeId}&selectedDate={selectedDate}"));
        }


        public static async Task<RoadFreightAgent> GetByIdAsync(int roadFreightAgentId)
        {
            return await RestApiHelper.GetAsync<RoadFreightAgent>(new Uri(UrlHelper.Api.PurchaseOrderApi, $"{UrlHelper.Controller.RoadFreightAgent}GetByIdAsync?roadFreightAgentId={roadFreightAgentId}"));
        }

        public static async Task<bool> UpdateAsync(RoadFreightAgent roadFreightAgent)
        {
            return await RestApiHelper.PutAsync(new Uri(UrlHelper.Api.PurchaseOrderApi, $"{UrlHelper.Controller.RoadFreightAgent}UpdateAsync"), roadFreightAgent);
        }

        public static async Task<bool> InsertAsync(RoadFreightAgent roadFreightAgent)
        {
            return await RestApiHelper.InsertAsync(new Uri(UrlHelper.Api.PurchaseOrderApi, $"{UrlHelper.Controller.RoadFreightAgent}UpdateAsync"), roadFreightAgent);
        }
    }
}
