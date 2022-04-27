using System;

namespace Triton.Core
{
    public static class UrlHelper
    {
        private static readonly string url = "http://neo:7000/";

        public static class Api
        {
            public static Uri GateWay = new Uri(url);
            public static Uri TritonApi => new Uri($"{url}triton-api/");
            //public static Uri TritonApi => new Uri("http://neo:8089/api/"); 
        }

        public static class Controller
        {
            public const string Branch = "Branch/";
            public const string BusinessOnline = "BusinessOnline/";
            public const string Customer = "Customer/";
            public const string Countrys = "Country/";
            public const string CountryCurrencySpots = "CountryCurrencySpots/";
            public const string Edocs = "eDocs/";
            public const string Employee = "Employee/";
            public const string Freightware = "Freightware/";
            public const string FreightwareUAT = "Freightware/UAT/";
            public const string FuelSurchargeClass = "FuelSurchargeClass/";
            public const string Questionnaire = "Questionnaire/";
            public const string Quotes = "Customer/Quotations/";
            public const string Roles = "Roles/";
            public const string TritonGroupCustom = "TritonGroupCustom/";
            public const string TritonGroupStoredProcs = "TritonGroupStoredProcs/";
            public const string TransportTypes = "TransportTypes/";
            public const string Waybills = "Waybills/";
            public const string WaybillTandT = "WaybillTrackandTraces/";
            public const string WaybillQuery = "WaybillQuery/";
            public const string WaybillQueryMaster = "WaybillQueryMaster/";
            public const string Users = "User/";
            public const string UserMap = "UserMap/";
            public const string UserRoles = "UserRoles/";
            public const string Collection = "Collection/";
            public const string Invoices = "Customer/Invoice/";
            public const string VwOpsWaybills = "vwOpsWaybills/";
            public const string Insurance = "Insurance/";
            public const string ReportManager = "ReportManager/";
            public const string CollectionRequests = "CollectionRequests/";
            public const string CollectionManifestLineItems = "CollectionManifestLineItems/";
            public const string CollectionRequestTrackAndTrace = "CollectionRequestTrackAndTrace/";
            public const string CollectionManifests = "CollectionManifests/";
            public const string InsuranceTypes = "InsuranceTypes/";
            public const string Statements = "Statements/";
            public const string PostalCode = "PostalCode/";
            public const string CustomerNotificationMap = "CustomerNotificationMap/";
        }

        /// <summary>
        /// Parameters that are passed to the WebApi
        /// </summary>
        public static class Parameters
        {
            // Parameters
            public const string CustomerId = "CustomerID";
            public const string DbName = "DBName";
            public const string UserId = "UserID";
            public const string waybillId = "waybillId";
        }
    }
}
