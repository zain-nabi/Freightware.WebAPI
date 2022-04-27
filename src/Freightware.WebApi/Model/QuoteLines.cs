using Dapper.Contrib.Extensions;

namespace Vendor.Model
{
    [Table("QuoteLines")]
    public class QuoteLines
    {
        [Key]
        public int QuoteLineID { get; set; }
        public int QuoteID { get; set; }
        public int Qty { get; set; }
        public string Mass { get; set; }
        public string ProdType { get; set; }
        public string Vol { get; set; }
        public string QuoteLineNo { get; set; }
        public string RateType { get; set; }
        public string VolWeight { get; set; }
        public string Length { get; set; }
        public string Breadth { get; set; }
        public string Height { get; set; }

        public string Description { get; set; }

        public decimal? ChargeUnits { get; set; }
        public decimal? Charge { get; set; }
    }
}
