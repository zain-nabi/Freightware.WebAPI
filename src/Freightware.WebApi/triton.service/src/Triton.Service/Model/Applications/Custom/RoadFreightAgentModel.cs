using System;

namespace Triton.Service.Model.Applications.Custom
{
    public class RoadFreightAgentModel
    {
        public int RoadFreightAgentID { get; set; }
        public string SupplierDate { get; set; }
        public string SubCode { get; set; }
        public string WaybillNo { get; set; }
        public string Consolidated { get; set; }
        public decimal? Volumetric { get; set; }
        public decimal? Mass { get; set; }
        public decimal? Surcharge { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Vat { get; set; }
        public decimal? Total { get; set; }
        public string Invoice { get; set; }
        public int StatusLCID { get; set; }
        public decimal? FWWayBillTotal { get; set; }
        public decimal? FWMass { get; set; }
        public int CategoryLCID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? DeletedByUserID { get; set; }
        public int? DeletedOn { get; set; }
        public string Comments { get; set; }
        public string IssueType { get; set; }
        public string FromRateArea { get; set; }
        public string ToRateArea { get; set; }
    }
}
