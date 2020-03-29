﻿using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Inventory.DTOModel;
using ERPBO.Production.DTOModel;
using ERPBO.Production.ViewModels;
using ERPWeb.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ERPWeb.Controllers
{
    public class ProductionController : BaseController
    {
        // GET: Production
        private readonly IWarehouseBusiness _warehouseBusiness;
        private readonly IRequsitionInfoBusiness _requsitionInfoBusiness;
        private readonly IRequsitionDetailBusiness _requsitionDetailBusiness;
        private readonly IProductionLineBusiness _productionLineBusiness;
        private readonly IProductionStockInfoBusiness _productionStockInfoBusiness;
        private readonly IProductionStockDetailBusiness _productionStockDetailBusiness;

        private readonly IItemBusiness _itemBusiness;
        private readonly IItemTypeBusiness _itemTypeBusiness;
        private readonly IUnitBusiness _unitBusiness;
        private readonly IItemReturnInfoBusiness _itemReturnInfoBusiness;
        private readonly IItemReturnDetailBusiness _itemReturnDetailBusiness;

        private readonly long UserId = 1;
        private readonly long OrgId = 1;
        public ProductionController(IRequsitionInfoBusiness requsitionInfoBusiness,IWarehouseBusiness warehouseBusiness, IRequsitionDetailBusiness requsitionDetailBusiness,IProductionLineBusiness productionLineBusiness, IItemBusiness itemBusiness,IItemTypeBusiness itemTypeBusiness, IUnitBusiness unitBusiness, IProductionStockDetailBusiness productionStockDetailBusiness, IProductionStockInfoBusiness productionStockInfoBusiness, IItemReturnInfoBusiness itemReturnInfoBusiness, IItemReturnDetailBusiness itemReturnDetailBusiness)
        {
            this._requsitionInfoBusiness = requsitionInfoBusiness;
            this._warehouseBusiness = warehouseBusiness;
            this._requsitionDetailBusiness = requsitionDetailBusiness;
            this._productionLineBusiness = productionLineBusiness;
            this._itemBusiness = itemBusiness;
            this._itemTypeBusiness = itemTypeBusiness;
            this._unitBusiness = unitBusiness;
            this._productionStockDetailBusiness = productionStockDetailBusiness;
            this._productionStockInfoBusiness = productionStockInfoBusiness;
            this._itemReturnInfoBusiness = itemReturnInfoBusiness;
            this._itemReturnDetailBusiness = itemReturnDetailBusiness;
        }

        #region ProductionLine
        public ActionResult GetProductionLineList()
        {
            IEnumerable<ProductionLineDTO> productionLineDTO = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId).Select(line => new ProductionLineDTO
            {
                LineId = line.LineId,
                LineNumber = line.LineNumber,
                LineIncharge = line.LineIncharge,
                Remarks = line.Remarks,
                StateStatus = (line.IsActive == true ? "Active" : "Inactive"),
                OrganizationId = line.OrganizationId,
                EUserId = line.EUserId,
                EntryDate = line.EntryDate,
                UpUserId = line.UpUserId,
                UpdateDate = line.UpdateDate
            }).ToList();
            List<ProductionLineViewModel> productionLineViewModels = new List<ProductionLineViewModel>();
            AutoMapper.Mapper.Map(productionLineDTO, productionLineViewModels);
            return View(productionLineViewModels);
        }
        [HttpPost]
        public ActionResult SaveProductionLine(ProductionLineViewModel productionLineViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    ProductionLineDTO dto = new ProductionLineDTO();
                    AutoMapper.Mapper.Map(productionLineViewModel, dto);
                    isSuccess = _productionLineBusiness.SaveUnit(dto, UserId, OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region Requsition
        [HttpGet]
        public ActionResult GetReqInfoList()
        {
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).Select(ware => new SelectListItem { Text = ware.WarehouseName, Value = ware.Id.ToString() }).ToList();

            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(status => status.value != RequisitionStatus.Pending).Select(st => new SelectListItem
            {
                Text = st.text,
                Value = st.value
            }).ToList();

          return View();
        }
        public ActionResult CreateRequsition()
        {
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).Select(line => new SelectListItem { Text = line.WarehouseName, Value = line.Id.ToString() }).ToList();
            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveRequsition(VmReqInfo model)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    ReqInfoDTO dto = new ReqInfoDTO();
                    AutoMapper.Mapper.Map(model, dto);
                    isSuccess = _requsitionInfoBusiness.SaveRequisition(dto, UserId, OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }

        // Used By  GetReqInfoList ActionMethod
        public ActionResult GetReqInfoParitalList(string reqCode,long? warehouseId,string status, long? line,string fromDate, string toDate)
        {
            IEnumerable<RequsitionInfoDTO> requsitionInfoDTO = _requsitionInfoBusiness.GetAllReqInfoByOrgId(OrgId).Where(req=>
                req.StateStatus != RequisitionStatus.Pending &&
                (reqCode == null ||reqCode.Trim() == ""|| req.ReqInfoCode.Contains(reqCode)) &&
                (warehouseId == null || warehouseId <= 0 || req.WarehouseId == warehouseId) &&
                (status == null || status.Trim() == "" || req.StateStatus == status.Trim())&&
                (line == null || line <= 0 || req.LineId == line) &&
                (
                    (fromDate == null && toDate == null)
                    ||
                     (fromDate == "" && toDate == "")
                    ||
                    (fromDate.Trim() != "" && toDate.Trim() != "" &&

                        req.EntryDate.Value.Date >= Convert.ToDateTime(fromDate).Date &&
                        req.EntryDate.Value.Date <= Convert.ToDateTime(toDate).Date)
                    ||
                    (fromDate.Trim() != "" && req.EntryDate.Value.Date == Convert.ToDateTime(fromDate).Date)
                    ||
                    (toDate.Trim() != "" && req.EntryDate.Value.Date == Convert.ToDateTime(toDate).Date)
                )
            ).Select(info => new RequsitionInfoDTO
            {
                ReqInfoId = info.ReqInfoId,
                ReqInfoCode = info.ReqInfoCode,
                LineId = info.LineId,
                LineNumber = (_productionLineBusiness.GetProductionLineOneByOrgId(info.LineId, OrgId).LineNumber),
                StateStatus = info.StateStatus,
                Remarks = info.Remarks,
                OrganizationId = info.OrganizationId,
                EntryDate =info.EntryDate,
                WarehouseId = info.WarehouseId,
                WarehouseName = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId, OrgId).WarehouseName),
                Qty = _requsitionDetailBusiness.GetRequsitionDetailByReqId(info.ReqInfoId, OrgId).Select(s => s.ItemId).Distinct().Count(),
            }).ToList();
            List<RequsitionInfoViewModel> requsitionInfoViewModels = new List<RequsitionInfoViewModel>();
            AutoMapper.Mapper.Map(requsitionInfoDTO, requsitionInfoViewModels);
            return PartialView(requsitionInfoViewModels);
        }
        public ActionResult GetRequsitionDetails(long? reqId)
        {
            IEnumerable<RequsitionDetailDTO> requsitionDetailDTO = _requsitionDetailBusiness.GetAllReqDetailByOrgId(OrgId).Where(rqd => reqId == null || reqId == 0 || rqd.ReqInfoId == reqId).Select(d => new RequsitionDetailDTO
            {
                ReqDetailId = d.ReqDetailId,
                ItemTypeId = d.ItemTypeId.Value,
                ItemTypeName = (_itemTypeBusiness.GetItemType(d.ItemTypeId.Value, OrgId).ItemName),
                ItemId = d.ItemId.Value,
                ItemName = (_itemBusiness.GetItemOneByOrgId(d.ItemId.Value, OrgId).ItemName),
                Quantity = d.Quantity.Value,
                UnitName = (_unitBusiness.GetUnitOneByOrgId(d.UnitId.Value, OrgId).UnitSymbol)
            }).ToList();
            List<RequsitionDetailViewModel> requsitionDetailViewModels = new List<RequsitionDetailViewModel>();
            AutoMapper.Mapper.Map(requsitionDetailDTO, requsitionDetailViewModels);

            ViewBag.RequisitionStatus = _requsitionInfoBusiness.GetRequisitionById(reqId.Value, OrgId).StateStatus;

            return PartialView("_GetRequsitionDetails", requsitionDetailViewModels);
        }

        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult SaveRequisitionStatus(long reqId, string status)
        {
            bool IsSuccess = false;
            if(reqId> 0 && !string.IsNullOrEmpty(status) && status == RequisitionStatus.Accepted)
            {
                IsSuccess = _productionStockDetailBusiness.SaveProductionStockInByProductionRequistion(reqId, status, OrgId, UserId);
            }
            else
            {
                IsSuccess = _requsitionInfoBusiness.SaveRequisitionStatus(reqId, status, OrgId);
            }
            return Json(IsSuccess);
        }

        [HttpGet]
        public ActionResult GetProductionStockInfoList()
        {
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).Select(ware => new SelectListItem
            {
                Text = ware.WarehouseName,
                Value = ware.Id.ToString()
            }).ToList();
            return View();
        }

        [HttpGet]
        public ActionResult GetProductionStockInfoPartialList(long? WarehouseId, long? ItemTypeId, long? ItemId)
        {
            IEnumerable<ProductionStockInfoDTO> productionStockInfoDTO = _productionStockInfoBusiness.GetAllProductionStockInfoByOrgId(OrgId).Select(info => new ProductionStockInfoDTO
            {
                StockInfoId = info.ProductionStockInfoId,
                LineId = info.LineId.Value,
                LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(info.LineId.Value,OrgId).LineNumber,
                WarehouseId = info.WarehouseId,
                Warehouse = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId.Value, OrgId).WarehouseName),
                ItemTypeId = info.ItemTypeId,
                ItemType = (_itemTypeBusiness.GetItemType(info.ItemTypeId.Value, OrgId).ItemName),
                ItemId = info.ItemId,
                Item = (_itemBusiness.GetItemOneByOrgId(info.ItemId.Value, OrgId).ItemName),
                UnitId = info.UnitId,
                Unit = (_unitBusiness.GetUnitOneByOrgId(info.UnitId.Value, OrgId).UnitSymbol),
                StockInQty = info.StockInQty,
                StockOutQty = info.StockOutQty,
                Remarks = info.Remarks,
                OrganizationId = info.OrganizationId,
            }).AsEnumerable();

            productionStockInfoDTO = productionStockInfoDTO.Where(ws => (WarehouseId == null || WarehouseId == 0 || ws.WarehouseId == WarehouseId) && (ItemTypeId == null || ItemTypeId == 0 || ws.ItemTypeId == ItemTypeId) && (ItemId == null || ItemId == 0 || ws.ItemId == ItemId)).ToList();

            List<ProductionStockInfoViewModel> productionStockInfoViews = new List<ProductionStockInfoViewModel>();
            AutoMapper.Mapper.Map(productionStockInfoDTO, productionStockInfoViews);
            return PartialView("_productionStockInfoList", productionStockInfoViews);
        }
        #endregion

        #region Item Return
        public ActionResult GetItemReturnList(string flag,string code,long? lineId, long?  warehouseId,string status,string returnType,string faultyCase, string fromDate, string toDate)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ReturnType = Utility.ListOfReturnType().Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.FaultyCase = Utility.ListOfFaultyCase().Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.ListOfLine = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId).Select(l => new SelectListItem
                {
                    Text = l.LineNumber,
                    Value = l.LineId.ToString()
                }).ToList();

                ViewBag.ListOfWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).Select(w => new SelectListItem
                {
                    Text = w.WarehouseName,
                    Value = w.Id.ToString()
                }).ToList();

                ViewBag.Status = Utility.ListOfReqStatus().Where(s => s.text == RequisitionStatus.Approved || s.text == RequisitionStatus.Accepted || s.text== RequisitionStatus.Canceled).Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();
                return View();
            }
            else
            {
                var warehouses = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).ToList();
                var lines = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId).ToList();
                IEnumerable<ItemReturnInfoDTO> itemReturnInfoDTOs = _itemReturnInfoBusiness.GetItemReturnInfos(OrgId).Where(i => 1 == 1
                && (code == null || code.Trim() =="" || i.IRCode.Contains(code))
                && (lineId == null || lineId <= 0 || i.LineId == lineId)
                && (warehouseId == null || warehouseId <= 0 || i.WarehouseId == warehouseId)
                && (status == null || status.Trim() == "" || i.StateStatus == status.Trim()) 
                && (returnType == null || returnType.Trim() == "" || i.ReturnType == returnType.Trim())
                && (faultyCase == null || faultyCase.Trim() == "" || i.FaultyCase == faultyCase.Trim())
                &&
                (
                    (fromDate == null && toDate == null)
                    ||
                     (fromDate == "" && toDate == "")
                    ||
                    (fromDate.Trim() != "" && toDate.Trim() != "" &&

                        i.EntryDate.Value.Date >= Convert.ToDateTime(fromDate).Date &&
                        i.EntryDate.Value.Date <= Convert.ToDateTime(toDate).Date)
                    ||
                    (fromDate.Trim() != "" && i.EntryDate.Value.Date == Convert.ToDateTime(fromDate).Date)
                    ||
                    (toDate.Trim() != "" && i.EntryDate.Value.Date == Convert.ToDateTime(toDate).Date)
                )
                ).Select(i => new ItemReturnInfoDTO
                {
                    IRInfoId = i.IRInfoId,
                    IRCode = i.IRCode,
                    ReturnType = i.ReturnType,
                    FaultyCase = i.FaultyCase,
                    LineId = lines.Where(l=> l.LineId == i.LineId).FirstOrDefault().LineId,
                    LineNumber = lines.Where(l => l.LineId == i.LineId).FirstOrDefault().LineNumber,
                    WarehouseId = warehouses.Where(w=> w.Id == i.WarehouseId).FirstOrDefault().Id,
                    WarehouseName = warehouses.Where(w => w.Id == i.WarehouseId).FirstOrDefault().WarehouseName,
                    Qty = _itemReturnDetailBusiness.GetItemReturnDetailsByReturnInfoId(OrgId, i.IRInfoId).Count(),
                    StateStatus = i.StateStatus,
                    EntryDate = i.EntryDate

                }).ToList();

                List<ItemReturnInfoViewModel> itemReturnInfoViewModels = new List<ItemReturnInfoViewModel>();
                AutoMapper.Mapper.Map(itemReturnInfoDTOs, itemReturnInfoViewModels);
                return PartialView("_GetItemReturnList", itemReturnInfoViewModels);
            }
        }

        [HttpGet]
        public ActionResult CreateFaultyGoodReturn()
        {
            ViewBag.ReturnType = Utility.ListOfReturnType().Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

            ViewBag.FaultyCase = Utility.ListOfFaultyCase().Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

            ViewBag.ListOfLine = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId).Select(l => new SelectListItem
            {
                Text = l.LineNumber,
                Value = l.LineId.ToString()
            }).ToList();

            ViewBag.ListOfWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).Select(w => new SelectListItem
            {
                Text = w.WarehouseName,
                Value = w.Id.ToString()
            }).ToList();

            return View();
        }

        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult SaveFaultyItemOrGoodsReturn(ItemReturnInfoViewModel info,List<ItemReturnDetailViewModel> details)
        {
            bool IsSuccess = false;
            if(ModelState.IsValid && details.Count > 0)
            {
                var dtoInfo = new ItemReturnInfoDTO();
                AutoMapper.Mapper.Map(info, dtoInfo);
                dtoInfo.OrganizationId = OrgId;
                dtoInfo.EUserId = UserId;
                var dtoDetail = new List<ItemReturnDetailDTO>();
                AutoMapper.Mapper.Map(details, dtoDetail);
                IsSuccess =_itemReturnInfoBusiness.SaveFaultyItemOrGoodsReturn(dtoInfo, dtoDetail);
            }
            return Json(IsSuccess);
        }

        public ActionResult GetProductionItemReturnDetails(long itemReturnInfoId)
        {
            var items = _itemBusiness.GetAllItemByOrgId(OrgId);
            var itemTypes = _itemTypeBusiness.GetAllItemTypeByOrgId(OrgId);
            var units = _unitBusiness.GetAllUnitByOrgId(OrgId);
            IEnumerable<ItemReturnDetailDTO> itemReturnDetailDTOs = _itemReturnDetailBusiness.GetItemReturnDetailsByReturnInfoId(OrgId, itemReturnInfoId).Select(s => new ItemReturnDetailDTO
            {
                IRDetailId = s.IRDetailId,
                ItemTypeId = s.ItemTypeId,
                ItemTypeName = itemTypes.FirstOrDefault(i => i.ItemId == s.ItemTypeId).ItemName,
                ItemId = s.ItemId,
                ItemName = items.FirstOrDefault(i => i.ItemId == s.ItemId).ItemName,
                UnitId = s.UnitId,
                UnitName = units.FirstOrDefault(i => i.UnitId == s.UnitId).UnitName,
                Quantity = s.Quantity,
                Remarks = s.Remarks
            }).ToList();

            var info = _itemReturnInfoBusiness.GetItemReturnInfo(OrgId, itemReturnInfoId);
            ItemReturnInfoViewModel itemReturnInfoViewModel = new ItemReturnInfoViewModel()
            {
                IRCode = info.IRCode,
                LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(info.LineId.Value, OrgId).LineNumber,
                WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId.Value, OrgId).WarehouseName
            };

            ViewBag.ReturnInfoViewModel = itemReturnInfoViewModel;

            IEnumerable<ItemReturnDetailViewModel> itemReturnDetailViews = new List<ItemReturnDetailViewModel>();
            AutoMapper.Mapper.Map(itemReturnDetailDTOs, itemReturnDetailViews);
            return PartialView("_GetProductionItemReturnDetails", itemReturnDetailViews);
        }

        ////IRDetailId,IRCode,ReturnType,FaultyCase,LineNumber,WarehouseName,ItemTypeName,ItemName,Quantity,UnitSymbol,StateStatus,Remarks,EntryDate
        public ActionResult GetProductionFaultyOrReturnItemDetailList(string flag, string refNum, string returnType,string faultyCase,long? lineId, long? warehouseId,string status, long? itemTypeId, long? itemId, string fromDate, string toDate)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ReturnType = Utility.ListOfReturnType().Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.FaultyCase = Utility.ListOfFaultyCase().Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.ListOfLine = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId).Select(l => new SelectListItem
                {
                    Text = l.LineNumber,
                    Value = l.LineId.ToString()
                }).ToList();

                ViewBag.ListOfWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).Select(w => new SelectListItem
                {
                    Text = w.WarehouseName,
                    Value = w.Id.ToString()
                }).ToList();

                ViewBag.Status = Utility.ListOfReqStatus().Where(s => s.text == RequisitionStatus.Approved || s.text == RequisitionStatus.Accepted || s.text == RequisitionStatus.Canceled).Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();
                return View();
            }
            else
            {
                IEnumerable<ItemReturnDetailListDTO> listdto = _itemReturnDetailBusiness.GetItemReturnDetailList(refNum, returnType, faultyCase, lineId, warehouseId, status, itemTypeId, itemId, fromDate, toDate);
                IEnumerable<ItemReturnDetailListViewModel> listViewModels = new List<ItemReturnDetailListViewModel>();
                AutoMapper.Mapper.Map(listdto, listViewModels);
                return PartialView("_GetProductionFaultyOrReturnItemDetailList", listViewModels);
            }
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}