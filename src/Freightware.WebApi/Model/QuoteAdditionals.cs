using Dapper.Contrib.Extensions;
using System;

namespace Vendor.Model
{
    [Table("QuoteAdditionals")]
    public class QuoteAdditionals
    {
        [Key]
        public int QuoteAdditionalID { get; set; }
        public int QuoteID { get; set; }
        public string AddService { get; set; }
        public decimal? AddCharge { get; set; }
        public string AddDescr { get; set; }
    }
}
