using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Inventory.DTOModel;
using ERPBO.Production.DTOModel;
using ERPBO.Production.ViewModels;
using ERPWeb.Filters;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ERPWeb.Controllers
{
    public class ProductionController : BaseController
    {
        // GET: Production
        private IWarehouseBusiness _warehouseBusiness;
        private IRequsitionInfoBusiness _requsitionInfoBusiness;
        private IRequsitionDetailBusiness _requsitionDetailBusiness;
        private IProductionLineBusiness _productionLineBusiness;
        private IProductionStockInfoBusiness _productionStockInfoBusiness;
        private IProductionStockDetailBusiness _productionStockDetailBusiness;

        private IItemBusiness _itemBusiness;
        private IItemTypeBusiness _itemTypeBusiness;
        private IUnitBusiness _unitBusiness;
        private IDescriptionBusiness _descriptionBusiness;

        private readonly long UserId = 1;
        private readonly long OrgId = 1;
        public ProductionController(IRequsitionInfoBusiness requsitionInfoBusiness,IWarehouseBusiness warehouseBusiness, IRequsitionDetailBusiness requsitionDetailBusiness,IProductionLineBusiness productionLineBusiness, IItemBusiness itemBusiness,IItemTypeBusiness itemTypeBusiness, IUnitBusiness unitBusiness, IProductionStockDetailBusiness productionStockDetailBusiness, IProductionStockInfoBusiness productionStockInfoBusiness,IDescriptionBusiness descriptionBusiness)
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
            this._descriptionBusiness = descriptionBusiness;
        }

        #region ProductionLine
        public ActionResult GetProductionLineList(int? page)
        {
            IPagedList<ProductionLineViewModel> productionLineDTO = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId).Select(line => new ProductionLineViewModel
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
            }).OrderBy(p => p.LineId).ToPagedList(page ?? 1, 15);
            IEnumerable<ProductionLineViewModel> productionLineViewModelForPage = new List<ProductionLineViewModel>();
            //List<ProductionLineViewModel> productionLineViewModels = new List<ProductionLineViewModel>();
           // AutoMapper.Mapper.Map(productionLineDTO, productionLineViewModels);
            return View(productionLineDTO);
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
        public ActionResult GetReqInfoList(int? page)
        {
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).Select(ware => new SelectListItem { Text = ware.WarehouseName, Value = ware.Id.ToString() }).ToList();

            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();
            ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(OrgId).Select(line => new SelectListItem { Text = line.DescriptionName, Value = line.DescriptionId.ToString() }).ToList();

            ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(status => 1==1
            //status.value != RequisitionStatus.Pending
            ).Select(st => new SelectListItem
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

            ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(OrgId).Select(line => new SelectListItem { Text = line.DescriptionName, Value = line.DescriptionId.ToString() }).ToList();
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
        public ActionResult GetReqInfoParitalList(string reqCode,long? warehouseId,string status, long? line,long? modelId,string fromDate, string toDate,int? page)
        {
            IPagedList<RequsitionInfoViewModel> requsitionInfoViewModels = _requsitionInfoBusiness.GetAllReqInfoByOrgId(OrgId).Where(req =>
                //req.StateStatus != RequisitionStatus.Pending &&
                (reqCode == null || reqCode.Trim() == "" || req.ReqInfoCode.Contains(reqCode)) &&
                (warehouseId == null || warehouseId <= 0 || req.WarehouseId == warehouseId) &&
                (status == null || status.Trim() == "" || req.StateStatus == status.Trim()) &&
                (line == null || line <= 0 || req.LineId == line) &&
                (modelId==null || modelId <=0 || req.DescriptionId==modelId) &&
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
            ).Select(info => new RequsitionInfoViewModel
            {
                ReqInfoId = info.ReqInfoId,
                ReqInfoCode = info.ReqInfoCode,
                LineId = info.LineId,
                LineNumber = (_productionLineBusiness.GetProductionLineOneByOrgId(info.LineId, OrgId).LineNumber),
                DescriptionId=info.DescriptionId,
                ModelName=(_descriptionBusiness.GetDescriptionOneByOrdId(info.DescriptionId,OrgId).DescriptionName),
                StateStatus = info.StateStatus,
                Remarks = info.Remarks,
                OrganizationId = info.OrganizationId,
                EntryDate = info.EntryDate,
                WarehouseId = info.WarehouseId,
                WarehouseName = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId, OrgId).WarehouseName),
                Qty = _requsitionDetailBusiness.GetRequsitionDetailByReqId(info.ReqInfoId, OrgId).Select(s => s.ItemId).Distinct().Count(),
            }).OrderBy(p => p.ReqInfoId).ToPagedList(page ?? 1, 15);
            IEnumerable<RequsitionInfoViewModel> requsitionInfoViewModelForPage = new List<RequsitionInfoViewModel>();
            //List<RequsitionInfoViewModel> requsitionInfoViewModels = new List<RequsitionInfoViewModel>();
            //AutoMapper.Mapper.Map(requsitionInfoDTO, requsitionInfoViewModels);
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


        #endregion

        #region Stock
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
        public ActionResult GetProductionStockInfoPartialList(long? WarehouseId, long? ItemTypeId, long? ItemId, int? page)
        {
            IEnumerable<ProductionStockInfoDTO> productionStockInfoDTO = _productionStockInfoBusiness.GetAllProductionStockInfoByOrgId(OrgId).Select(info => new ProductionStockInfoDTO
            {
                StockInfoId = info.ProductionStockInfoId,
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

            productionStockInfoDTO = productionStockInfoDTO.Where(ws => (WarehouseId == null || WarehouseId == 0 || ws.WarehouseId == WarehouseId) && (ItemTypeId == null || ItemTypeId == 0 || ws.ItemTypeId == ItemTypeId) && (ItemId == null || ItemId == 0 || ws.ItemId == ItemId)).OrderBy(p => p.StockInfoId).ToPagedList(page ?? 1, 15);

            // List<ProductionStockInfoViewModel> productionStockInfoViews = new List<ProductionStockInfoViewModel>();
            //AutoMapper.Mapper.Map(productionStockInfoDTO, productionStockInfoViews);
            return PartialView("_productionStockInfoList", productionStockInfoDTO);
        }
        #endregion

        #region Description
        public ActionResult GetDescriptionList(int? page)
        {
            IPagedList<DescriptionViewModel> descriptionViewModels = _descriptionBusiness.GetDescriptionByOrgId(OrgId).Select(des => new DescriptionViewModel
            {
                DescriptionId = des.DescriptionId,
                DescriptionName = des.DescriptionName,
                SubCategoryId = des.SubCategoryId,
                StateStatus = (des.IsActive == true ? "Active" : "Inactive"),
                Remarks = des.Remarks,
                OrganizationId = des.OrganizationId,
                EUserId = des.EUserId,
                EntryDate = des.EntryDate,
                UpUserId = des.UpUserId,
                UpdateDate = des.UpdateDate

            }).OrderBy(des => des.DescriptionId).ToPagedList(page ?? 1, 15);
            IEnumerable<DescriptionViewModel> descriptionViewModelForPage = new List<DescriptionViewModel>();
            return View(descriptionViewModels);
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}