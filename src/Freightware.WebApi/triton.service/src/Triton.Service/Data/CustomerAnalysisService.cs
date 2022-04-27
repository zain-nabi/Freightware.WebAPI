using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Triton.Model.CRM.Tables;
using Triton.Service.Model.CRM.Custom;
using Triton.Service.Model.CRM.StoredProcs;
using Triton.Service.Utils;

namespace Triton.Service.Data
{
    public class CustomerAnalysisService
    {
        public static async Task<bool> InsertCustomerAnalysisAsync(proc_CustomerAnalysis_Insert model)
        {
            return await RestApiHelper.InsertAsync(new Uri(UrlHelper.Api.CustomerApi, $"{UrlHelper.Controller.CustomerAnalysis}InsertCustomerAnalysisAsync/Insert"), model);
        }

        public static async Task<bool> DeleteCustomerAnalysisAsync(int CustomerID)
        {
            return await RestApiHelper.DeleteAsync(new Uri(UrlHelper.Api.CustomerApi, $"{UrlHelper.Controller.CustomerAnalysis}DeleteCustomerAnalysisAsync/{CustomerID}"));
        }

        public static async Task<bool> InsertAsync(CustomerAnalysisModel customerAnalyses)
        {
            return await RestApiHelper.InsertAsync(new Uri(UrlHelper.Api.CustomerApi, $"{UrlHelper.Controller.CustomerAnalysis}InsertAsync/Insert"), customerAnalyses);
        }

        public static async Task<bool> UpdateAsync(CustomerAnalysisModel customerAnalyses)
        {
            return await RestApiHelper.PutAsync(new Uri(UrlHelper.Api.CustomerApi, $"{UrlHelper.Controller.CustomerAnalysis}UpdateAsync"), customerAnalyses);
        }
    }
}
