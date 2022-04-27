using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Freightware.WebApi.Repository;
using Triton.Model.CRM.Custom;
using Triton.Model.FWWebservice.Custom;
using Triton.Core;
using Triton.Model.CRM.Tables;
using Dapper;

namespace Freightware.WebApi.BusinessLogic
{
    public class WaybillBL : FWRepositoryGeneric
    {
        private readonly string inUserid;
        private readonly string inPassword;
        private readonly string inRequestCtr;
        private readonly string inUATUserid;
        private readonly string inUATPassword;
        private IConfiguration _config;
        private const string _successfullReturnCode = "0000";

        public WaybillBL(IConfiguration config)
        {
            _config = config;
            inUserid = config.GetSection("Freightware").GetSection("InUserid").Value;
            inPassword = config.GetSection("Freightware").GetSection("InPassword").Value;
            inRequestCtr = config.GetSection("Freightware").GetSection("InRequestCtr").Value;
            inUATUserid = config.GetSection("Freightware").GetSection("InUserid").Value;
            inUATPassword = config.GetSection("Freightware").GetSection("InPassword").Value;
        }

        public async Task<FWResponsePacket> GetWaybillUAT(string waybillNo)
        {
            var request = new Vendor.Services.Freightware.UAT.GetWaybill.GetWaybillRequest
            {
                InUserid = inUATUserid,
                InPassword = inUATPassword,
                InRequestCtr = inRequestCtr,
                InWaybillNo = waybillNo
            };

            var client = new Vendor.Services.Freightware.UAT.GetWaybill.FWV6WEBPortClient(Vendor.Services.Freightware.UAT.GetWaybill.FWV6WEBPortClient.EndpointConfiguration.FWV6WEBSOAP11Port);
            var response = new FWResponsePacket();
            try
            {
                var x = await client.GetWaybillAsync(request);

                response.ReturnCode = x.ReturnCode;
                response.ReturnMessage = x.ReturnMessage;
                response.DataObject = x.Waybill;
            }
            catch (Exception x)
            {
                response.ReturnCode = "Internal Error";
                response.ReturnMessage = x.Message + " - " + x.InnerException;
            }
            return response;
        }

        public async Task<FWResponsePacket> GetWaybill(string waybillNo)
        {
            var request = new Vendor.Services.Freightware.PROD.GetWaybill.GetWaybillRequest
            {
                InUserid = inUserid,
                InPassword = inPassword,
                InRequestCtr = inRequestCtr,
                InWaybillNo = waybillNo
            };

            var client = new Vendor.Services.Freightware.PROD.GetWaybill.FWV6WEBPortClient(Vendor.Services.Freightware.PROD.GetWaybill.FWV6WEBPortClient.EndpointConfiguration.FWV6WEBSOAP11Port);
            var response = new FWResponsePacket();
            try
            {
                var waybillObject = await client.GetWaybillAsync(request);

                response.ReturnCode = waybillObject.ReturnCode;
                response.ReturnMessage = waybillObject.ReturnMessage;
                response.DataObject = waybillObject.Waybill;

                var xml = GetXmlFromObject(waybillObject);

                if(response.ReturnCode.Equals(_successfullReturnCode))
                {
                    const string sql = "proc_Waybills_From_WS_Update @xml";
                    await using var connection = DBConnection.GetOpenConnection(_config.GetConnectionString(StringHelpers.Database.Crm));
                    var waybill = await connection.QuerySingleAsync<Waybills>(sql, new { xml });
                }
            
            }

            catch (Exception x)
            {
               // response.ReturnCode = "Internal Error";
                //response.ReturnMessage = x.Message + " - " + x.InnerException;
            }

            return response;
        }

        public async Task<FWResponsePacket> PostWaybillUAT(CustomerWaybillSubmitModels model, string dbName = "CRMTest")
        {
            string spIOSessionData = "";

            Vendor.Services.Freightware.UAT.SetWaybill.FWV6WEBPortClient client = new Vendor.Services.Freightware.UAT.SetWaybill.FWV6WEBPortClient(Vendor.Services.Freightware.UAT.SetWaybill.FWV6WEBPortClient.EndpointConfiguration.FWV6WEBSOAP11Port);
            Vendor.Services.Freightware.UAT.SetWaybill.SetWaybillRequest request = new Vendor.Services.Freightware.UAT.SetWaybill.SetWaybillRequest
            {
                InAction = new Vendor.Services.Freightware.UAT.SetWaybill.SetWaybillInAction
                {
                    InAdd = true,
                    InAddSpecified = true
                },
                InWaybill = new Vendor.Services.Freightware.UAT.SetWaybill.SetWaybillInWaybill
                {
                    DateWaybill = DateTime.Now.ToString("yyyyMMdd"),
                    CustCode = model.CustCode,//quote.CustCode,
                    ServiceType = model.ServiceType,//quote.ServiceType,
                    RecName = model.ReceiverName,
                    RecAdd1 = model.ReceiverAddress1,
                    RecAdd2 = model.ReceiverAddress2,
                    RecAdd3 = model.ReceiverAddress3,
                    RecAdd4 = model.ReceiverSuburb,
                    RecAdd5 = model.ReceiverPostalCode,
                    RecCellNo = model.ReceiverContactCell,
                    RecEmail = model.ReceiverContactEmail,
                    RecContact = model.ReceiverContactName,
                    RecTelNo = model.ReceiverContactCell,
                    RecSite = model.ReceiverCode,
                    SenName = model.SenderName,
                    SenAdd1 = model.SenderAddress1,
                    SenAdd2 = model.SenderAddress2,
                    SenAdd3 = model.SenderAddress3,
                    SenAdd4 = model.SenderSuburb,
                    SenAdd5 = model.SenderPostalCode,
                    SenCell = model.SenderContactCell,
                    SenTelNo = model.SenderContactCell,
                    SenEmail = model.SenderContactEmail,
                    SenContact = model.SenderContactName,
                    SendSite = model.SenderCode,
                    WaybillNo = model.WaybillNo,
                    CustXref = model.CustXRef,
                },
                InUserid = inUATUserid,
                InPassword = inUATPassword,
                InRequestCtr = inRequestCtr
            };

            List<Vendor.Services.Freightware.UAT.SetWaybill.SetWaybillInWaybillWaybillLine> inLines = new List<Vendor.Services.Freightware.UAT.SetWaybill.SetWaybillInWaybillWaybillLine>();
            int lineCounter = 1;
            List<Vendor.Services.Freightware.UAT.SetParcel.SetParcelInParcel> inParcels = new List<Vendor.Services.Freightware.UAT.SetParcel.SetParcelInParcel>();
            int parcelCounter = 1;
            foreach (TransportPriceLineItemModels line in model.Lines)
            {
                inLines.Add(new Vendor.Services.Freightware.UAT.SetWaybill.SetWaybillInWaybillWaybillLine
                {
                    LineBth = line.LineBreadth.ToString(),
                    LineHgt = line.LineHeight.ToString(),
                    LineLen = line.LineLength.ToString(),
                    LineItemQty = line.LineQty.ToString(),
                    LineItemMass = line.LineMass.ToString(),
                    LineNo = lineCounter.ToString(),
                    LineLabelPrinted = model.WaybillNo + " " + lineCounter.ToString("00#"),
                    LineItemDesc = line.Description

                });

                //Add to the parcel collection
                if (line.Parcels == null || line.Parcels.Count == 0) //Create the parcel objects if default
                {
                    for (int i = 1; i <= line.LineQty; i++)
                    {
                        //Create a parcel for every item in the line qty
                        inParcels.Add(new Vendor.Services.Freightware.UAT.SetParcel.SetParcelInParcel
                        {
                            ParcelNo = model.WaybillNo.PadRight(line.TotalParcelNoLength - 7, ' ') + " " + parcelCounter.ToString("00#"),
                            WaybillItemNo = lineCounter.ToString(),
                            WaybillNo = model.WaybillNo
                        });
                        parcelCounter++;
                    }
                }
                else
                {
                    foreach (TransportPriceLineItemParcelModels parcel in line.Parcels)
                    {
                        inParcels.Add(new Vendor.Services.Freightware.UAT.SetParcel.SetParcelInParcel
                        {
                            ParcelNo = parcel.ParcelNo,
                            WaybillNo = model.WaybillNo,
                            WaybillItemNo = lineCounter.ToString(),
                            ParcelBth = parcel.ParcelBreadth.ToString(),
                            ParcelHgt = parcel.ParcelHeight.ToString(),
                            ParcelLen = parcel.ParcelLength.ToString(),
                            ParcelMass = parcel.ParcelMass.ToString()
                        });
                    }
                }
                lineCounter++;
            }
            request.InWaybill.WaybillLines = inLines.ToArray();

            //We need to check if the parcel numbers are deemed valid first
            Vendor.Services.Freightware.UAT.SetParcel.FWV6WEBPortClient parcelClient = new Vendor.Services.Freightware.UAT.SetParcel.FWV6WEBPortClient(Vendor.Services.Freightware.UAT.SetParcel.FWV6WEBPortClient.EndpointConfiguration.FWV6WEBSOAP11Port);

            var postalcode = new PostalCodeBL(_config);

            foreach (Vendor.Services.Freightware.UAT.SetParcel.SetParcelInParcel parcel in inParcels)
            {
                if (model.Lines[Convert.ToInt32(parcel.WaybillItemNo) - 1].Parcels == null)
                    parcel.ParcelNo = (await postalcode.GetPostalListByCode(request.InWaybill.RecAdd5))[0].Branch + parcel.ParcelNo;
                Vendor.Services.Freightware.UAT.SetParcel.SetParcelRequest parcelRequest = new Vendor.Services.Freightware.UAT.SetParcel.SetParcelRequest
                {
                    InAction = new Vendor.Services.Freightware.UAT.SetParcel.SetParcelInAction
                    {
                        InGetSpecified = true,
                        InGet = true
                    },
                    InUserid = inUATUserid,
                    InPassword = inUATPassword,
                    InRequestCtr = inRequestCtr,
                    InParcel = parcel,
                    IoSessionData = spIOSessionData
                };
                var parcelOutput = await parcelClient.SetParcelAsync(parcelRequest);
                switch (parcelOutput.ReturnCode)
                {
                    case "0000":
                        return new FWResponsePacket
                        {
                            ReturnCode = "Error",
                            ReturnMessage = "A parcel with this numeric sequence already exists, not able to add",
                            DataObject = null
                        };
                    case "9998":
                        if (!parcelOutput.ReturnMessage.Contains("does not exist"))
                            return new FWResponsePacket
                            {
                                ReturnCode = "Error",
                                ReturnMessage = "A parcel with this numeric sequence already exists, not able to add",
                                DataObject = null
                            };
                        break;
                    default:
                        return new FWResponsePacket
                        {
                            ReturnCode = parcelOutput.ReturnCode,
                            ReturnMessage = parcelOutput.ReturnMessage,
                            DataObject = null
                        };
                }
            }

            //We can assume if it reaches here that the parcel numbers have passed validation.
            var waybillResponse = await client.SetWaybillAsync(request);

            string ParcelError = "";

            if (waybillResponse.ReturnCode == "0000")
            {
                //Now that the waybill exists, we can create the parcels
                foreach (Vendor.Services.Freightware.UAT.SetParcel.SetParcelInParcel parcel in inParcels)
                {
                    Vendor.Services.Freightware.UAT.SetParcel.SetParcelRequest parcelRequest = new Vendor.Services.Freightware.UAT.SetParcel.SetParcelRequest
                    {
                        InAction = new Vendor.Services.Freightware.UAT.SetParcel.SetParcelInAction
                        {
                            InAddSpecified = true,
                            InAdd = true
                        },
                        InUserid = inUATUserid,
                        InPassword = inUATPassword,
                        InRequestCtr = inRequestCtr,
                        InParcel = parcel,
                        IoSessionData = spIOSessionData
                    };
                    var parcelOutput = await parcelClient.SetParcelAsync(parcelRequest);

                    if (parcelOutput.ReturnCode != "0000")
                    {
                        ParcelError = ParcelError + " Parecl Error :" + parcelOutput.ReturnCode + " - " + parcelOutput.ReturnMessage + ". ";
                    }

                }
            }


            return new FWResponsePacket
            {
                ReturnCode = waybillResponse.ReturnCode,
                ReturnMessage = waybillResponse.ReturnMessage + ParcelError,
                DataObject = waybillResponse.ReturnCode == "0000" ? waybillResponse.Waybill : null
            };
        }

        public async Task<FWResponsePacket> PostWaybill(CustomerWaybillSubmitModels model, string dbName = "CRM")
        {
            string spIOSessionData = "";

            Vendor.Services.Freightware.PROD.SetWaybill.FWV6WEBPortClient client = new Vendor.Services.Freightware.PROD.SetWaybill.FWV6WEBPortClient(Vendor.Services.Freightware.PROD.SetWaybill.FWV6WEBPortClient.EndpointConfiguration.FWV6WEBSOAP11Port);
            Vendor.Services.Freightware.PROD.SetWaybill.SetWaybillRequest request = new Vendor.Services.Freightware.PROD.SetWaybill.SetWaybillRequest
            {
                InAction = new Vendor.Services.Freightware.PROD.SetWaybill.SetWaybillInAction
                {
                    InAdd = true,
                    InAddSpecified = true
                },
                InWaybill = new Vendor.Services.Freightware.PROD.SetWaybill.SetWaybillInWaybill
                {
                    DateWaybill = DateTime.Now.ToString("yyyyMMdd"),
                    CustCode = model.CustCode,//quote.CustCode,
                    ServiceType = model.ServiceType,//quote.ServiceType,
                    RecName = model.ReceiverName,
                    RecAdd1 = model.ReceiverAddress1,
                    RecAdd2 = model.ReceiverAddress2,
                    RecAdd3 = model.ReceiverAddress3,
                    RecAdd4 = model.ReceiverSuburb,
                    RecAdd5 = model.ReceiverPostalCode,
                    RecCellNo = model.ReceiverContactCell,
                    RecEmail = model.ReceiverContactEmail,
                    RecContact = model.ReceiverContactName,
                    RecTelNo = model.ReceiverContactCell,
                    RecSite = model.ReceiverCode,
                    SenName = model.SenderName,
                    SenAdd1 = model.SenderAddress1,
                    SenAdd2 = model.SenderAddress2,
                    SenAdd3 = model.SenderAddress3,
                    SenAdd4 = model.SenderSuburb,
                    SenAdd5 = model.SenderPostalCode,
                    SenCell = model.SenderContactCell,
                    SenTelNo = model.SenderContactCell,
                    SenEmail = model.SenderContactEmail,
                    SenContact = model.SenderContactName,
                    SendSite = model.SenderCode,
                    WaybillNo = model.WaybillNo,
                    CustXref = model.CustXRef,
                },
                InUserid = inUserid,
                InPassword = inPassword,
                InRequestCtr = inRequestCtr
            };

            List<Vendor.Services.Freightware.PROD.SetWaybill.SetWaybillInWaybillWaybillLine> inLines = new List<Vendor.Services.Freightware.PROD.SetWaybill.SetWaybillInWaybillWaybillLine>();
            int lineCounter = 1;
            List<Vendor.Services.Freightware.PROD.SetParcel.SetParcelInParcel> inParcels = new List<Vendor.Services.Freightware.PROD.SetParcel.SetParcelInParcel>();
            int parcelCounter = 1;
            foreach (TransportPriceLineItemModels line in model.Lines)
            {
                inLines.Add(new Vendor.Services.Freightware.PROD.SetWaybill.SetWaybillInWaybillWaybillLine
                {
                    LineBth = line.LineBreadth.ToString(),
                    LineHgt = line.LineHeight.ToString(),
                    LineLen = line.LineLength.ToString(),
                    LineItemQty = line.LineQty.ToString(),
                    LineItemMass = line.LineMass.ToString(),
                    LineNo = lineCounter.ToString(),
                    LineLabelPrinted = model.WaybillNo + " " + lineCounter.ToString("00#"),
                    LineItemDesc = line.Description

                });

                //Add to the parcel collection
                if (line.Parcels == null || line.Parcels.Count == 0) //Create the parcel objects if default
                {
                    for (int i = 1; i <= line.LineQty; i++)
                    {
                        //Create a parcel for every item in the line qty
                        inParcels.Add(new Vendor.Services.Freightware.PROD.SetParcel.SetParcelInParcel
                        {
                            ParcelNo = model.WaybillNo.PadRight(line.TotalParcelNoLength - 7, ' ') + " " + parcelCounter.ToString("00#"),
                            WaybillItemNo = lineCounter.ToString(),
                            WaybillNo = model.WaybillNo
                        });
                        parcelCounter++;
                    }
                }
                else
                {
                    foreach (TransportPriceLineItemParcelModels parcel in line.Parcels)
                    {
                        inParcels.Add(new Vendor.Services.Freightware.PROD.SetParcel.SetParcelInParcel
                        {
                            ParcelNo = parcel.ParcelNo,
                            WaybillNo = model.WaybillNo,
                            WaybillItemNo = lineCounter.ToString(),
                            ParcelBth = parcel.ParcelBreadth.ToString(),
                            ParcelHgt = parcel.ParcelHeight.ToString(),
                            ParcelLen = parcel.ParcelLength.ToString(),
                            ParcelMass = parcel.ParcelMass.ToString()
                        });
                    }
                }
                lineCounter++;
            }
            request.InWaybill.WaybillLines = inLines.ToArray();
            //We need to check if the parcel numbers are deemed valid first
            Vendor.Services.Freightware.PROD.SetParcel.FWV6WEBPortClient parcelClient = new Vendor.Services.Freightware.PROD.SetParcel.FWV6WEBPortClient(Vendor.Services.Freightware.PROD.SetParcel.FWV6WEBPortClient.EndpointConfiguration.FWV6WEBSOAP11Port);

            var postalcode = new PostalCodeBL(_config);

            foreach (Vendor.Services.Freightware.PROD.SetParcel.SetParcelInParcel parcel in inParcels)
            {
                if (model.Lines[Convert.ToInt32(parcel.WaybillItemNo) - 1].Parcels == null)
                    parcel.ParcelNo = (await postalcode.GetPostalListByCode(request.InWaybill.RecAdd5))[0].Branch + parcel.ParcelNo;
                Vendor.Services.Freightware.PROD.SetParcel.SetParcelRequest parcelRequest = new Vendor.Services.Freightware.PROD.SetParcel.SetParcelRequest
                {
                    InAction = new Vendor.Services.Freightware.PROD.SetParcel.SetParcelInAction
                    {
                        InGetSpecified = true,
                        InGet = true
                    },
                    InUserid = inUserid,
                    InPassword = inPassword,
                    InRequestCtr = inRequestCtr,
                    InParcel = parcel,
                    IoSessionData = spIOSessionData
                };
                var parcelOutput = await parcelClient.SetParcelAsync(parcelRequest);
                switch (parcelOutput.ReturnCode)
                {
                    case "0000":
                        return new FWResponsePacket
                        {
                            ReturnCode = "Error",
                            ReturnMessage = "A parcel with this numeric sequence already exists, not able to add",
                            DataObject = null
                        };
                    case "9998":
                        if (!parcelOutput.ReturnMessage.Contains("does not exist"))
                            return new FWResponsePacket
                            {
                                ReturnCode = "Error",
                                ReturnMessage = "A parcel with this numeric sequence already exists, not able to add",
                                DataObject = null
                            };
                        break;
                    default:
                        return new FWResponsePacket
                        {
                            ReturnCode = parcelOutput.ReturnCode,
                            ReturnMessage = parcelOutput.ReturnMessage,
                            DataObject = null
                        };
                }
            }

            //We can assume if it reaches here that the parcel numbers have passed validation.
            var waybillResponse = await client.SetWaybillAsync(request);

            string ParcelError = "";

            if (waybillResponse.ReturnCode == "0000")
            {

                //Now that the waybill exists, we can create the parcels
                foreach (Vendor.Services.Freightware.PROD.SetParcel.SetParcelInParcel parcel in inParcels)
                {
                    Vendor.Services.Freightware.PROD.SetParcel.SetParcelRequest parcelRequest = new Vendor.Services.Freightware.PROD.SetParcel.SetParcelRequest
                    {
                        InAction = new Vendor.Services.Freightware.PROD.SetParcel.SetParcelInAction
                        {
                            InAddSpecified = true,
                            InAdd = true
                        },
                        InUserid = inUserid,
                        InPassword = inPassword,
                        InRequestCtr = inRequestCtr,
                        InParcel = parcel,
                        IoSessionData = spIOSessionData
                    };
                    var parcelOutput = await parcelClient.SetParcelAsync(parcelRequest);

                    if (parcelOutput.ReturnCode != "0000")
                    {
                        ParcelError = ParcelError + " Parecl Error :" + parcelOutput.ReturnCode + " - " + parcelOutput.ReturnMessage + ". ";
                    }

                }
            }


            return new FWResponsePacket
            {
                ReturnCode = waybillResponse.ReturnCode,
                ReturnMessage = waybillResponse.ReturnMessage + ParcelError,
                DataObject = waybillResponse.ReturnCode == "0000" ? waybillResponse.Waybill : null
            };
        }

        public async Task<FWResponsePacket> GetWaybillDeliverys(string waybillNo)
        {
            var request = new GetWaybillDelivery.GetWaybillDeliveryRequest
            {
                InUserid = inUATUserid,
                InPassword = inUATPassword,
                InRequestCtr = inRequestCtr,
                InWaybillNo = waybillNo
            };

            var client = new GetWaybillDelivery.FWV6WEBPortClient(GetWaybillDelivery.FWV6WEBPortClient.EndpointConfiguration.FWV6WEBSOAP11Port);
            var response = new FWResponsePacket();
            try
            {
                var x = await client.GetWaybillDeliveryAsync(request);

                response.ReturnCode = x.ReturnCode;
                response.ReturnMessage = x.ReturnMessage;
                response.DataObject = x.Waybill;
            }
            catch (Exception x)
            {
                response.ReturnCode = "Internal Error";
                response.ReturnMessage = x.Message + " - " + x.InnerException;
            }
            return response;
        }
    }
}
