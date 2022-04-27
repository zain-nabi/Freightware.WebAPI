using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Triton.Core;
using Vendor.Model;
using Vendor.Services.Freightware.PROD.SetQuote;

namespace Freightware.WebApi.BusinessLogic
{
    public class FWConverterBL
    {
        private readonly IConfiguration _config;
        private readonly string _url;

        public FWConverterBL(IConfiguration config)
        {
            _config = config;
            _url = _config.GetSection("WebApiUrl").GetSection("v1").Value;
        }

        public async Task<VendorQuoteModel> GetConvertFromFWQuote(SetQuoteResponseQuote fwQuote, VendorQuoteModel existQuote)
        {
            var transportTypeList = await RestApiHelper.GetAsync<List<TransportTypes>>(new Uri($"{_url}{StringHelpers.Controllers.TransportTypes}"));

            VendorQuoteModel quote = new VendorQuoteModel
            {
                QuoteAdditionals = new List<QuoteAdditionals>(),
                QuoteLines = new List<QuoteLines>(),
                QuoteSundrys = new List<QuoteSundrys>(),
                TransportTypes = transportTypeList
            };

            try
            {
                var customerList = await RestApiHelper.GetAsync<List<Customers>>(new Uri($"{_url}{StringHelpers.Controllers.Customer}/{fwQuote.CustCode}"));
                var customerId = customerList.FirstOrDefault().CustomerID;

                var t = fwQuote.ContactName;
                quote.Quote = new Quotes
                {
                    ContactName = fwQuote.ContactName,
                    CustCode = fwQuote.CustCode,
                    DateQuote = Convert.ToDateTime(fwQuote.DateQuote.ToString().Substring(0, 4) + "-" + fwQuote.DateQuote.ToString().Substring(4, 2) + "-" + fwQuote.DateQuote.ToString().Substring(6, 2)),
                    DiscountRate = fwQuote.DiscountRate.ToString(),
                    Email = fwQuote.Email,
                    InsuranceValue = fwQuote.InsuranceValue.ToString(),
                    QuoteNo = fwQuote.QuoteNo,
                    RecSite = fwQuote.RecSite,
                    RecAdd1 = fwQuote.ReceiverInformation.RecAdd1,
                    RecAdd2 = fwQuote.ReceiverInformation.RecAdd2,
                    RecAdd3 = fwQuote.ReceiverInformation.RecAdd3,
                    RecAdd4 = fwQuote.ReceiverInformation.RecAdd4,
                    RecAdd5 = fwQuote.ReceiverInformation.RecAdd5,
                    RecContact = fwQuote.ReceiverInformation.RecContact,
                    RecFaxNo = fwQuote.ReceiverInformation.RecFaxNo,
                    RecName = fwQuote.ReceiverInformation.RecName,
                    RecTelNo = fwQuote.ReceiverInformation.RecTelNo,
                    SenAdd1 = fwQuote.SenderInformation.SenAdd1,
                    SenAdd2 = fwQuote.SenderInformation.SenAdd2,
                    SenAdd3 = fwQuote.SenderInformation.SenAdd3,
                    SenAdd4 = fwQuote.SenderInformation.SenAdd4,
                    SenAdd5 = fwQuote.SenderInformation.SenAdd5,
                    SenContact = fwQuote.SenderInformation.SenContact,
                    SenFaxNo = fwQuote.SenderInformation.SenFaxNo,
                    SenName = fwQuote.SenderInformation.SenName,
                    SenTelNo = fwQuote.SenderInformation.SenTelNo,
                    SenSite = fwQuote.SendSite,
                    Remarks = fwQuote.Remarks,
                    ServiceTypeText = fwQuote.ServiceType,
                    SignatureReturned = fwQuote.SignatureReturned,
                    SigneeName = fwQuote.SigneeName,
                    TelNo = fwQuote.TelephoneNo,
                    ValDeclared = fwQuote.ValueDeclared.ToString(),
                    XrefNo = fwQuote.XrefNo,
                    CustomerID = customerId,
                    RecInstructions1 = fwQuote.ReceiveInstructions != null && fwQuote.ReceiveInstructions.Length > 0 ? fwQuote.ReceiveInstructions[0] : null,
                    RecInstructions2 = fwQuote.ReceiveInstructions != null && fwQuote.ReceiveInstructions.Length > 1 ? fwQuote.ReceiveInstructions[1] : null,
                    SenInstructions1 = fwQuote.SendInstructions != null && fwQuote.SendInstructions.Length > 0 ? fwQuote.SendInstructions[0] : null,
                    SenInstructions2 = fwQuote.SendInstructions != null && fwQuote.SendInstructions.Length > 1 ? fwQuote.SendInstructions[1] : null,
                    ServiceTypeID = quote.TransportTypes.Find(t => t.Description.Trim() == fwQuote.ServiceType).TransportTypeID,
                    AreaFrom = fwQuote.AreaFrom,
                    AreaFromName = fwQuote.AreaFromName,
                    AreaTo = fwQuote.AreaTo,
                    AreaToName = fwQuote.AreaToName,
                    Branch = fwQuote.Branch,
                    BranchName = fwQuote.BranchName,
                    DateReturned = fwQuote.DateReturned,
                    DiscountVal = fwQuote.DiscountVal,
                    FreightValue = fwQuote.FreightValue,
                    IdCapture = fwQuote.IdCapture,
                    IdModify = fwQuote.IdModify,
                    InsurancePercentage = fwQuote.InsurancePercentage,
                    OrderNo = fwQuote.OrderNo,
                    QuoteAccepted = fwQuote.QuoteAccepted,
                    QuoteExpiryDate = fwQuote.QuoteExpiryDate,
                    QuoteExpiryDateStamp = fwQuote.QuoteExpiryDate != 0 ? Convert.ToDateTime(fwQuote.QuoteExpiryDate.ToString().Substring(0, 4) + "-" + fwQuote.QuoteExpiryDate.ToString().Substring(4, 2) + "-" + fwQuote.QuoteExpiryDate.ToString().Substring(6, 2)) : (DateTime?)null,
                    QuoteLineCtr = fwQuote.QuoteLineCtr,
                    TimeReturned = fwQuote.TimeReturned,
                    StatusInd = fwQuote.StatusInd,
                    StatusName = fwQuote.StatusName,
                    TotalMass = fwQuote.TotalMass,
                    TotalQty = fwQuote.TotalQty,
                    TotalValue = fwQuote.TotalValue,
                    TotalVolume = fwQuote.TotalVolume,
                    ValueDeclared = fwQuote.ValueDeclared,
                    Vat = fwQuote.Vat,
                    WaybillNo = fwQuote.WaybillNo

                };
                //Try populate existing fields if already populated.
                if (existQuote.Quote != null)
                {
                    quote.Quote.QuoteID = existQuote.Quote.QuoteID;
                    quote.Quote.SenSiteID = existQuote.Quote.SenSiteID;
                    quote.Quote.RecSiteID = existQuote.Quote.RecSiteID;
                }

                foreach (SetQuoteResponseQuoteQuoteLineItem item in fwQuote.QuoteLineItems)
                {
                    quote.QuoteLines.Add(new QuoteLines
                    {
                        QuoteLineNo = item.LineNo.ToString(),
                        Breadth = item.Dimensions.Breadth.ToString(),
                        Description = item.ItemDescription,
                        Height = item.Dimensions.Height.ToString(),
                        Length = item.Dimensions.Length.ToString(),
                        Mass = item.ItemMass.ToString(),
                        ProdType = item.ItemLoadType,
                        RateType = item.RateType,
                        Vol = item.ItemVol.ToString(),
                        VolWeight = item.VolWeight.ToString(),
                        Qty = Convert.ToInt16(item.ItemQty),
                        Charge = item.ItemCharge,
                        ChargeUnits = item.ChargeUnits
                    });
                }
                if (fwQuote.Additionals != null)
                {
                    foreach (SetQuoteResponseQuoteAdditional additional in fwQuote.Additionals)
                    {
                        quote.QuoteAdditionals.Add(new QuoteAdditionals
                        {
                            AddService = additional.AddService,
                            AddCharge = additional.AddCharge,
                            AddDescr = additional.AddDescr
                        });
                    }
                }
                foreach (SetQuoteResponseQuoteSundry sundry in fwQuote.Sundrys)
                {
                    quote.QuoteSundrys.Add(new QuoteSundrys
                    {
                        SundryService = sundry.SundryService,
                        SundryCharge = sundry.SundryCharge,
                        SundryDescr = sundry.SundryDescr
                    });
                }
            }
            catch (Exception)
            {
            }
            return quote;
        }

        public async Task<SetQuoteInQuote> GetConvertToFWInQuote(QuoteModels quote)
        {
            try
            {
                var customerList = await RestApiHelper.GetAsync<List<Customers>>(new Uri($"{_url}{StringHelpers.Controllers.Customer}/{quote.Quote.CustomerID}"));
                var accountCode = customerList.FirstOrDefault().AccountCode;

                #region MainQuote
                //Set Main Body of quote.
                SetQuoteInQuote fwInQuote = new SetQuoteInQuote
                {

                    ContactName = quote.Quote.ContactName,
                    CustCode = accountCode,
                    CustRefCode = quote.Quote.XrefNo,
                    DiscountRate = quote.Quote.DiscountRate,
                    Email = quote.Quote.Email,
                    InsuranceValue = quote.Quote.InsuranceValue,
                    RecSite = quote.Quote.RecSite,
                    ReceiverInformation = new SetQuoteInQuoteReceiverInformation
                    {
                        RecAdd1 = quote.Quote.RecAdd1,
                        RecAdd2 = quote.Quote.RecAdd2,
                        RecAdd3 = quote.Quote.RecAdd3,
                        RecAdd4 = quote.Quote.RecAdd4,
                        RecAdd5 = quote.Quote.RecAdd5,
                        RecContact = quote.Quote.RecContact,
                        RecFaxNo = quote.Quote.RecFaxNo,
                        RecName = quote.Quote.RecName,
                        RecTelNo = quote.Quote.RecTelNo
                    },
                    ReceiveInstructions = !String.IsNullOrEmpty(quote.Quote.RecInstructions1) ?
                            new string[] {
                                            quote.Quote.RecInstructions1,
                                            quote.Quote.RecInstructions2 ?? null
                            } : null,
                    Remarks = quote.Quote.Remarks,
                    SendSite = quote.Quote.SenSite,
                    SenderInformation = new SetQuoteInQuoteSenderInformation
                    {
                        SenAdd1 = quote.Quote.SenAdd1,
                        SenAdd2 = quote.Quote.SenAdd2,
                        SenAdd3 = quote.Quote.SenAdd3,
                        SenAdd4 = quote.Quote.SenAdd4,
                        SenAdd5 = quote.Quote.SenAdd5,
                        SenContact = quote.Quote.SenContact,
                        SenFaxNo = quote.Quote.SenFaxNo,
                        SenName = quote.Quote.SenName,
                        SenTelNo = quote.Quote.SenTelNo
                    },
                    SendInstructions = !String.IsNullOrEmpty(quote.Quote.SenInstructions1) ?
                            new string[] {
                                            quote.Quote.SenInstructions1,
                                            quote.Quote.SenInstructions2 ?? null
                            } : null,

                    ServiceType = quote.Quote.ServiceTypeText,
                    SignatureReturned = quote.Quote.SignatureReturned,
                    SigneeName = quote.Quote.SigneeName,
                    TelNo = quote.Quote.TelNo,
                    ValDeclared = quote.Quote.ValDeclared,
                    XRefNo = quote.Quote.XrefNo


                };
                #endregion
                #region Add Lines
                //Create Lines
                List<SetQuoteInQuoteInQuoteLine> inLines = new List<SetQuoteInQuoteInQuoteLine>();
                foreach (QuoteLines line in quote.QuoteLines)
                {
                    inLines.Add(new SetQuoteInQuoteInQuoteLine
                    {
                        Dimensions = new SetQuoteInQuoteInQuoteLineDimensions
                        {
                            InBreadth = line.Breadth,
                            InHeight = line.Height,
                            InLength = line.Length
                        },
                        InItemDescription = line.Description,
                        InItemMass = line.Mass,
                        InItemProdType = line.ProdType,
                        InItemQty = line.Qty.ToString(),
                        InItemVol = line.Vol,
                        InQuoteLineNo = line.QuoteLineNo,
                        InRateType = line.RateType,
                        InVolWeight = line.VolWeight
                    });
                }
                fwInQuote.InQuoteLines = inLines.ToArray();
                #endregion
                #region Add Additionals
                //Create Additionals
                if (quote.QuoteAdditionals != null)
                {
                    List<SetQuoteInQuoteInAdditional> inAdditionals = new List<SetQuoteInQuoteInAdditional>();
                    foreach (QuoteAdditionals additional in quote.QuoteAdditionals)
                    {
                        SetQuoteInQuoteInAdditional inAdditional = new SetQuoteInQuoteInAdditional
                        {
                            AddCharge = additional.AddCharge.Value.ToString(),
                            AddService = additional.AddService
                        };
                    }
                    fwInQuote.InAdditionals = inAdditionals.Count > 0 ? inAdditionals.ToArray() : null;
                }
                #endregion
                #region Add Sundrys
                //Create Sundrys
                if (quote.QuoteSundrys != null)
                {
                    List<SetQuoteInQuoteInSundry> inSundrys = new List<SetQuoteInQuoteInSundry>();
                    foreach (QuoteSundrys sundry in quote.QuoteSundrys)
                    {
                        SetQuoteInQuoteInSundry inSundry = new SetQuoteInQuoteInSundry
                        {
                            SundryCharge = sundry.SundryCharge.Value.ToString(),
                            SundryService = sundry.SundryService
                        };
                    }
                    fwInQuote.InSundrys = inSundrys.Count > 0 ? inSundrys.ToArray() : null;
                }
                #endregion

                return fwInQuote;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<VendorQuoteModel> ConvertFromFWUATQuote(Vendor.Services.Freightware.UAT.SetQuote.SetQuoteResponseQuote fwQuote, VendorQuoteModel existQuote)
        {
            var transportTypeList = await RestApiHelper.GetAsync<List<TransportTypes>>(new Uri($"{_url}{StringHelpers.Controllers.TransportTypes}"));

            VendorQuoteModel quote = new VendorQuoteModel
            {
                QuoteAdditionals = new List<QuoteAdditionals>(),
                QuoteLines = new List<QuoteLines>(),
                QuoteSundrys = new List<QuoteSundrys>(),
                TransportTypes = transportTypeList
            };

            try
            {
                var customerList = await RestApiHelper.GetAsync<List<Customers>>(new Uri($"{_url}{StringHelpers.Controllers.Customer}/{fwQuote.CustCode}"));
                var customerId = customerList.FirstOrDefault().CustomerID;

                quote.Quote = new Quotes
                {
                    ContactName = fwQuote.ContactName,
                    CustCode = fwQuote.CustCode,
                    DateQuote = Convert.ToDateTime(fwQuote.DateQuote.ToString().Substring(0, 4) + "-" + fwQuote.DateQuote.ToString().Substring(4, 2) + "-" + fwQuote.DateQuote.ToString().Substring(6, 2)),
                    DiscountRate = fwQuote.DiscountRate.ToString(),
                    Email = fwQuote.Email,
                    InsuranceValue = fwQuote.InsuranceValue.ToString(),
                    QuoteNo = fwQuote.QuoteNo,
                    RecSite = fwQuote.RecSite,
                    RecAdd1 = fwQuote.ReceiverInformation.RecAdd1,
                    RecAdd2 = fwQuote.ReceiverInformation.RecAdd2,
                    RecAdd3 = fwQuote.ReceiverInformation.RecAdd3,
                    RecAdd4 = fwQuote.ReceiverInformation.RecAdd4,
                    RecAdd5 = fwQuote.ReceiverInformation.RecAdd5,
                    RecContact = fwQuote.ReceiverInformation.RecContact,
                    RecFaxNo = fwQuote.ReceiverInformation.RecFaxNo,
                    RecName = fwQuote.ReceiverInformation.RecName,
                    RecTelNo = fwQuote.ReceiverInformation.RecTelNo,
                    SenAdd1 = fwQuote.SenderInformation.SenAdd1,
                    SenAdd2 = fwQuote.SenderInformation.SenAdd2,
                    SenAdd3 = fwQuote.SenderInformation.SenAdd3,
                    SenAdd4 = fwQuote.SenderInformation.SenAdd4,
                    SenAdd5 = fwQuote.SenderInformation.SenAdd5,
                    SenContact = fwQuote.SenderInformation.SenContact,
                    SenFaxNo = fwQuote.SenderInformation.SenFaxNo,
                    SenName = fwQuote.SenderInformation.SenName,
                    SenTelNo = fwQuote.SenderInformation.SenTelNo,
                    SenSite = fwQuote.SendSite,
                    Remarks = fwQuote.Remarks,
                    ServiceTypeText = fwQuote.ServiceType,
                    SignatureReturned = fwQuote.SignatureReturned,
                    SigneeName = fwQuote.SigneeName,
                    TelNo = fwQuote.TelephoneNo,
                    ValDeclared = fwQuote.ValueDeclared.ToString(),
                    XrefNo = fwQuote.XrefNo,
                    CustomerID = customerId,
                    RecInstructions1 = fwQuote.ReceiveInstructions != null && fwQuote.ReceiveInstructions.Length > 0 ? fwQuote.ReceiveInstructions[0] : null,
                    RecInstructions2 = fwQuote.ReceiveInstructions != null && fwQuote.ReceiveInstructions.Length > 1 ? fwQuote.ReceiveInstructions[1] : null,
                    SenInstructions1 = fwQuote.SendInstructions != null && fwQuote.SendInstructions.Length > 0 ? fwQuote.SendInstructions[0] : null,
                    SenInstructions2 = fwQuote.SendInstructions != null && fwQuote.SendInstructions.Length > 1 ? fwQuote.SendInstructions[1] : null,
                    ServiceTypeID = quote.TransportTypes.Find(t => t.Description.Trim() == fwQuote.ServiceType).TransportTypeID,
                    AreaFrom = fwQuote.AreaFrom,
                    AreaFromName = fwQuote.AreaFromName,
                    AreaTo = fwQuote.AreaTo,
                    AreaToName = fwQuote.AreaToName,
                    Branch = fwQuote.Branch,
                    BranchName = fwQuote.BranchName,
                    DateReturned = fwQuote.DateReturned,
                    DiscountVal = fwQuote.DiscountVal,
                    FreightValue = fwQuote.FreightValue,
                    IdCapture = fwQuote.IdCapture,
                    IdModify = fwQuote.IdModify,
                    InsurancePercentage = fwQuote.InsurancePercentage,
                    OrderNo = fwQuote.OrderNo,
                    QuoteAccepted = fwQuote.QuoteAccepted,
                    QuoteExpiryDate = fwQuote.QuoteExpiryDate,
                    QuoteExpiryDateStamp = fwQuote.QuoteExpiryDate != 0 ? Convert.ToDateTime(fwQuote.QuoteExpiryDate.ToString().Substring(0, 4) + "-" + fwQuote.QuoteExpiryDate.ToString().Substring(4, 2) + "-" + fwQuote.QuoteExpiryDate.ToString().Substring(6, 2)) : (DateTime?)null,
                    QuoteLineCtr = fwQuote.QuoteLineCtr,
                    TimeReturned = fwQuote.TimeReturned,
                    StatusInd = fwQuote.StatusInd,
                    StatusName = fwQuote.StatusName,
                    TotalMass = fwQuote.TotalMass,
                    TotalQty = fwQuote.TotalQty,
                    TotalValue = fwQuote.TotalValue,
                    TotalVolume = fwQuote.TotalVolume,
                    ValueDeclared = fwQuote.ValueDeclared,
                    Vat = fwQuote.Vat,
                    WaybillNo = fwQuote.WaybillNo

                };
                //Try populate existing fields if already populated.
                if (existQuote != null)
                {
                    quote.Quote.QuoteID = existQuote.Quote.QuoteID;
                    quote.Quote.SenSiteID = existQuote.Quote.SenSiteID;
                    quote.Quote.RecSiteID = existQuote.Quote.RecSiteID;
                }

                foreach (Vendor.Services.Freightware.UAT.SetQuote.SetQuoteResponseQuoteQuoteLineItem item in fwQuote.QuoteLineItems)
                {
                    quote.QuoteLines.Add(new QuoteLines
                    {
                        QuoteLineNo = item.LineNo.ToString(),
                        Breadth = item.Dimensions.Breadth.ToString(),
                        Description = item.ItemDescription,
                        Height = item.Dimensions.Height.ToString(),
                        Length = item.Dimensions.Length.ToString(),
                        Mass = item.ItemMass.ToString(),
                        ProdType = item.ItemLoadType,
                        RateType = item.RateType,
                        Vol = item.ItemVol.ToString(),
                        VolWeight = item.VolWeight.ToString(),
                        Qty = Convert.ToInt16(item.ItemQty),
                        Charge = item.ItemCharge,
                        ChargeUnits = item.ChargeUnits
                    });
                }
                if (fwQuote.Additionals != null)
                {
                    foreach (Vendor.Services.Freightware.UAT.SetQuote.SetQuoteResponseQuoteAdditional additional in fwQuote.Additionals)
                    {
                        quote.QuoteAdditionals.Add(new QuoteAdditionals
                        {
                            AddService = additional.AddService,
                            AddCharge = additional.AddCharge,
                            AddDescr = additional.AddDescr
                        });
                    }
                }
                foreach (Vendor.Services.Freightware.UAT.SetQuote.SetQuoteResponseQuoteSundry sundry in fwQuote.Sundrys)
                {
                    quote.QuoteSundrys.Add(new QuoteSundrys
                    {
                        SundryService = sundry.SundryService,
                        SundryCharge = sundry.SundryCharge,
                        SundryDescr = sundry.SundryDescr
                    });
                }
            }
            catch (Exception)
            {
            }
            return quote;
        }

        public async Task<Vendor.Services.Freightware.UAT.SetQuote.SetQuoteInQuote> ConvertToFWUATInQuote(VendorQuoteModel quote)
        {
            try
            {
                var accountCode = (await RestApiHelper.GetAsync<Customers>(new Uri($"{_url}{StringHelpers.Controllers.Customer}/{quote.Quote.CustomerID}"))).AccountCode;

                #region MainQuote
                //Set Main Body of quote.
                Vendor.Services.Freightware.UAT.SetQuote.SetQuoteInQuote fwInQuote = new Vendor.Services.Freightware.UAT.SetQuote.SetQuoteInQuote
                {
                    ContactName = quote.Quote.ContactName,
                    CustCode = accountCode,
                    CustRefCode = quote.Quote.XrefNo,
                    DiscountRate = quote.Quote.DiscountRate,
                    Email = quote.Quote.Email,
                    InsuranceValue = quote.Quote.InsuranceValue,
                    RecSite = quote.Quote.RecSite,
                    ReceiverInformation = new Vendor.Services.Freightware.UAT.SetQuote.SetQuoteInQuoteReceiverInformation
                    {
                        RecAdd1 = quote.Quote.RecAdd1,
                        RecAdd2 = quote.Quote.RecAdd2,
                        RecAdd3 = quote.Quote.RecAdd3,
                        RecAdd4 = quote.Quote.RecAdd4,
                        RecAdd5 = quote.Quote.RecAdd5,
                        RecContact = quote.Quote.RecContact,
                        RecFaxNo = quote.Quote.RecFaxNo,
                        RecName = quote.Quote.RecName,
                        RecTelNo = quote.Quote.RecTelNo
                    },
                    ReceiveInstructions = !String.IsNullOrEmpty(quote.Quote.RecInstructions1) ?
                            new string[] {
                                            quote.Quote.RecInstructions1,
                                            quote.Quote.RecInstructions2 ?? null
                            } : null,
                    Remarks = quote.Quote.Remarks,
                    SendSite = quote.Quote.SenSite,
                    SenderInformation = new Vendor.Services.Freightware.UAT.SetQuote.SetQuoteInQuoteSenderInformation
                    {
                        SenAdd1 = quote.Quote.SenAdd1,
                        SenAdd2 = quote.Quote.SenAdd2,
                        SenAdd3 = quote.Quote.SenAdd3,
                        SenAdd4 = quote.Quote.SenAdd4,
                        SenAdd5 = quote.Quote.SenAdd5,
                        SenContact = quote.Quote.SenContact,
                        SenFaxNo = quote.Quote.SenFaxNo,
                        SenName = quote.Quote.SenName,
                        SenTelNo = quote.Quote.SenTelNo
                    },
                    SendInstructions = !String.IsNullOrEmpty(quote.Quote.SenInstructions1) ?
                            new string[] {
                                            quote.Quote.SenInstructions1,
                                            quote.Quote.SenInstructions2 ?? null
                            } : null,

                    ServiceType = quote.Quote.ServiceTypeText,
                    SignatureReturned = quote.Quote.SignatureReturned,
                    SigneeName = quote.Quote.SigneeName,
                    TelNo = quote.Quote.TelNo,
                    ValDeclared = quote.Quote.ValDeclared,
                    XRefNo = quote.Quote.XrefNo


                };
                #endregion
                #region Add Lines
                //Create Lines
                List<Vendor.Services.Freightware.UAT.SetQuote.SetQuoteInQuoteInQuoteLine> inLines = new List<Vendor.Services.Freightware.UAT.SetQuote.SetQuoteInQuoteInQuoteLine>();
                foreach (QuoteLines line in quote.QuoteLines)
                {
                    inLines.Add(new Vendor.Services.Freightware.UAT.SetQuote.SetQuoteInQuoteInQuoteLine
                    {
                        Dimensions = new Vendor.Services.Freightware.UAT.SetQuote.SetQuoteInQuoteInQuoteLineDimensions
                        {
                            InBreadth = line.Breadth,
                            InHeight = line.Height,
                            InLength = line.Length
                        },
                        InItemDescription = line.Description,
                        InItemMass = line.Mass,
                        InItemProdType = line.ProdType,
                        InItemQty = line.Qty.ToString(),
                        InItemVol = line.Vol,
                        InQuoteLineNo = line.QuoteLineNo,
                        InRateType = line.RateType,
                        InVolWeight = line.VolWeight
                    });
                }
                fwInQuote.InQuoteLines = inLines.ToArray();
                #endregion
                #region Add Additionals
                //Create Additionals
                if (quote.QuoteAdditionals != null)
                {
                    List<Vendor.Services.Freightware.UAT.SetQuote.SetQuoteInQuoteInAdditional> inAdditionals = new List<Vendor.Services.Freightware.UAT.SetQuote.SetQuoteInQuoteInAdditional>();
                    foreach (QuoteAdditionals additional in quote.QuoteAdditionals)
                    {
                        Vendor.Services.Freightware.UAT.SetQuote.SetQuoteInQuoteInAdditional inAdditional = new Vendor.Services.Freightware.UAT.SetQuote.SetQuoteInQuoteInAdditional
                        {
                            AddCharge = additional.AddCharge.Value.ToString(),
                            AddService = additional.AddService
                        };
                    }
                    fwInQuote.InAdditionals = inAdditionals.Count > 0 ? inAdditionals.ToArray() : null;
                }
                #endregion
                #region Add Sundrys
                //Create Sundrys
                if (quote.QuoteSundrys != null)
                {
                    List<Vendor.Services.Freightware.UAT.SetQuote.SetQuoteInQuoteInSundry> inSundrys = new List<Vendor.Services.Freightware.UAT.SetQuote.SetQuoteInQuoteInSundry>();
                    foreach (QuoteSundrys sundry in quote.QuoteSundrys)
                    {
                        Vendor.Services.Freightware.UAT.SetQuote.SetQuoteInQuoteInSundry inSundry = new Vendor.Services.Freightware.UAT.SetQuote.SetQuoteInQuoteInSundry
                        {
                            SundryCharge = sundry.SundryCharge.Value.ToString(),
                            SundryService = sundry.SundryService
                        };
                    }
                    fwInQuote.InSundrys = inSundrys.Count > 0 ? inSundrys.ToArray() : null;
                }
                #endregion

                return fwInQuote;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Triton.Model.CRM.Custom.WaybillCustomerModel CovertFWWaybillToCustomerWaybill(dynamic outWaybill)
        {
            return new Triton.Model.CRM.Custom.WaybillCustomerModel
            {
                AcountCode = outWaybill.CustCode,
                CustXRef = outWaybill.CustXref,
                DiscountValue = outWaybill.DiscountValSpecified ? outWaybill.DiscountVal : (Decimal?)null,
                ExpectedDeliveryDate = outWaybill.DateDelExptSpecified && outWaybill.DateDelExpt != 0 ? Convert.ToDateTime(outWaybill.DateDelExpt.ToString().Substring(0, 4) + "-" + outWaybill.DateDelExpt.ToString().Substring(4, 2) + "-" + outWaybill.DateDelExpt.ToString().Substring(6, 2)) : (DateTime?)null,
                FreightValue = outWaybill.FreightValue,
                ReceiverAddress1 = outWaybill.RecAdd1,
                ReceiverAddress2 = outWaybill.RecAdd2,
                ReceiverAddress3 = outWaybill.RecAdd3,
                ReceiverCell = outWaybill.RecCell,
                ReceiverContact = outWaybill.RecContact,
                ReceiverEmail = outWaybill.RecEmail,
                ReceiverFaxNo = outWaybill.RecFaxNo,
                ReceiverInstructions = outWaybill.RecInstructions.ToString(),
                ReceiverName = outWaybill.RecName,
                ReceiverPostalCode = outWaybill.RecAdd5,
                ReceiverSuburbCode = outWaybill.RecAdd4,
                ReceiverTelNo = outWaybill.RecTelNo,
                SenderAddress1 = outWaybill.SenAdd1,
                SenderAddress2 = outWaybill.SenAdd2,
                SenderAddress3 = outWaybill.SenAdd3,
                SenderCell = outWaybill.SenCell,
                SenderContact = outWaybill.SenContact,
                SenderEmail = outWaybill.SenEmail,
                SenderFaxNo = outWaybill.SenFaxNo,
                SenderInstructions = outWaybill.SendInstructions.ToString(),
                SenderName = outWaybill.SenName,
                SenderPostalCode = outWaybill.SenAdd5,
                SenderSuburbCode = outWaybill.SenAdd4,
                SenderTelNo = outWaybill.SenTelNo,
                ServiceType = outWaybill.ServiceType,
                TotalChargeUnits = outWaybill.TotalChargeableUnits,
                TotalMass = outWaybill.TotalMass,
                TotalParcels = outWaybill.TotalParcels,
                TotalQty = outWaybill.TotalQty,
                TotalValue = outWaybill.TotalValue,
                TotalVolume = outWaybill.TotalVolume,
                Vat = outWaybill.Vat,
                WaybillDate = Convert.ToDateTime(outWaybill.DateWaybill.ToString().Substring(0, 4) + "-" + outWaybill.DateWaybill.ToString().Substring(4, 2) + "-" + outWaybill.DateWaybill.ToString().Substring(6, 2)),
                WaybillNo = outWaybill.WaybillNo

            };
        }

    }
}
