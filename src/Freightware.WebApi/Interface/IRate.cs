using System.Threading.Tasks;
using Freightware.WebApi.Model;

namespace Freightware.WebApi.Interface
{
    public interface IRate
    {
        Task<RateModels> GetAsync(string accountCode, string active);
        Task<RateModels> GetStandardRateAsync(string accountCode, string active, string rateArea);
        Task<RateModels> GetStandardRateInsertAsync(string accountCode, int rateCycleId, int rateYear);
    }
}
