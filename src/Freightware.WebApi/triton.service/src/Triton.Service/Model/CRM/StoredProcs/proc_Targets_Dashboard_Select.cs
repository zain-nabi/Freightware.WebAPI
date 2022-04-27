using System;
using System.Collections.Generic;
using System.Text;

namespace Triton.Service.Model.CRM.StoredProcs
{
    public class proc_Targets_Dashboard_Select
    {
        public string Period { get; set; }
        public decimal Target { get; set; }
        public decimal FreightBilling { get; set; }
        public decimal Percentage { get; set; }
        public int Variance { get; set; }
        public string TargetType { get; set; }
        public bool IsSouthAfrican { get; set; }
    }
}
