using Dapper.Contrib.Extensions;
using System;

namespace Vendor.Model
{
    [Table("QuoteSundrys")]
    public class QuoteSundrys
    {
        [Key]
        public int QuoteSudryID { get; set; }
        public int QuoteID { get; set; }
        public string SundryService { get; set; }
        public Decimal? SundryCharge { get; set; }
        public string SundryDescr { get; set; }
    }
}
