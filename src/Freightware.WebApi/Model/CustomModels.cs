using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Vendor.Services.Freightware.PROD.GetChargesList;
using Vendor.Services.Freightware.PROD.GetStatement;

namespace Vendor.Model
{
    public class VendorQuoteModel : QuoteModels
    {
        public List<GetChargesListResponseChargesOutput> AllowedSundries { get; set; }

        public List<QuoteChargeItem> AllowedSundriesDropDown
        {
            get
            {
                if (AllowedSundries != null)
                    return (from sundry in AllowedSundries
                            select new QuoteChargeItem
                            {
                                Value = sundry.OutChargeCode,
                                Description = sundry.Heading + " " + sundry.Description
                            }
                           ).OrderBy(x => x.Description).ToList();
                else return null;
            }
        }
    }

    public class VendorQuoteSearchModel
    {

        public DateTime? DateFrom { get; set; } = DateTime.Now.AddDays(-14);
        public DateTime? DateTo { get; set; } = DateTime.Now;
        public string QuoteRef { get; set; }
        public int CustomerID { get; set; }
        public string AccountCode { get; set; }
        public List<Customers> AllowedCustomerList { get; set; }
        public List<Quotes> Quotes { get; set; }
        public List<QuoteSundrys> QuoteSundrys { get; set; }
    }

    public class VendorStatementSearch
    {
        public int customerId { get; set; }
        public List<Customers> AllowedCustomers { get; set; }
        public Customers Customers { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = false)]
        public DateTime Period { get; set; } = DateTime.Now.AddDays(-28);
        public GetStatementResponseStatementOutput GetStatementResponseStatementOutput { get; set; }
    }

}
