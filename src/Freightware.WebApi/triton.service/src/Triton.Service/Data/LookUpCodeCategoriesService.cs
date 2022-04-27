using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Triton.Model.TritonGroup.Tables;
using Triton.Service.Model.TritonGroup.Custom;
using Triton.Service.Utils;

namespace Triton.Service.Data
{
    public class LookUpCodeCategoriesService
    {
        public static async Task<List<LookupCodeCategoriesModel>> GetAllLookUpCodeCategories()
        {
            return await RestApiHelper.GetAsync<List<LookupCodeCategoriesModel>>(new Uri(UrlHelper.Api.FleetManagementApi, $"{UrlHelper.Controller.LookUpCodeCategories}Lookupcodecategories"));
        }

        public static async Task<bool> UpdateLookUpCodeAsync(LookupCodeCategories model)
        {
            return await RestApiHelper.PutAsync(new Uri(UrlHelper.Api.FleetManagementApi, $"{UrlHelper.Controller.LookUpCodeCategories}UpdateLookUpCodeCategoryAsync/Update"), model);
        }

        public static async Task<LookupCodeCategories> GetLookUpCodeCategoryByID(int LookupcodeCategoryID)
        {
            return await RestApiHelper.GetAsync<LookupCodeCategories>(new Uri(UrlHelper.Api.FleetManagementApi, $"{UrlHelper.Controller.LookUpCodeCategories}GetLookUpCodeCategoryByID/{LookupcodeCategoryID}"));
        }
    }
}
