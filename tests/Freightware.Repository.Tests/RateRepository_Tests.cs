using System.Diagnostics.CodeAnalysis;
using System.Transactions;
using Freightware.WebApi.Model;
using Freightware.WebApi.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;

namespace Freightware.Repository.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class RateRepository_Tests
    {
        private static IConfiguration GetConfig()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        [TestMethod]
        public void PostCustomerUniqueRateAsync_Success()
        {
            var repository = new RateRepository(GetConfig());

            using (new TransactionScope())
            {
                const string accountCode = "TRI122";
                var result = repository.GetAsync(accountCode, "Y");

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Result.RecordCount > 0);
                Assert.IsInstanceOfType(result.Result, typeof(RateModels));
            }
        }
    }
}
