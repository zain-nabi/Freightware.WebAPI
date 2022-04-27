using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Triton.Model.LeaveManagement.Tables;
using Triton.Service.Model.LeaveManagement.Custom;
using Triton.Service.Utils;

namespace Triton.Service.Data
{
    public class OrgOrganogramService
    {
        public static async Task<List<CheckListModel>> IsEmployeeCodeExists(string employeeCode)
        {
            return await RestApiHelper.GetAsync<List<CheckListModel>>(new Uri(UrlHelper.Api.RecruitmentApi, $"orgOrganogram/IsEmployeeCodeExists?employeeCode={employeeCode}"));
        }

        public static async Task<orgOrganogram> GetorgOrganogramByEmployeeIDAsync(int employeeId)
        {
            return await RestApiHelper.GetAsync<orgOrganogram>(new Uri(UrlHelper.Api.RecruitmentApi, $"orgOrganogram/GetorgOrganogramByEmployeeIDAsync?EmployeeID={employeeId}"));
        }

        public static async Task<bool> PutAsync(orgOrganogram orgOrganogram)
        {
            return await RestApiHelper.PutAsync(new Uri(UrlHelper.Api.RecruitmentApi, "orgOrganogram/PutAsync"), orgOrganogram);
        }

    }
}
