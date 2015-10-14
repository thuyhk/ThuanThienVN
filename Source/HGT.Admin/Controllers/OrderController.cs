using HGT.Admin.Models;
using HGT.Core;
using HGT.Core.Enums;
using HGT.Core.Providers;
using HGT.Service;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HGT.Admin.Extensions;
using System.IO;
using Repository.Pattern.Infrastructure;
using Kendo.Mvc.UI;
using Kendo.Mvc;
using System.ComponentModel;
using HGT.Entities.Models;
using HGT.Admin.Filters;
using System.Linq.Expressions;
using Newtonsoft.Json;
using HGT.Core.Utility;
using HGT.Core.Extensions;
using HGT.Core.Models;


namespace HGT.Admin.Controllers
{
    public class OrderController : AdminController
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IProductService _productService;

        //private UserInfo _userInfo = System.Web.HttpContext.Current.Session[ConstantKeys.UserInfo] as UserInfo;

        //private static LogoModel _logoModel = new LogoModel();

        #endregion

        #region Contructors

        public OrderController(IUnitOfWork unitOfWork, IOrderService orderService, IOrderDetailService orderDetailService,
             IProductService productService)
        {
            this._unitOfWork = unitOfWork;
            this._orderService = orderService;
            this._orderDetailService = orderDetailService;
            this._productService = productService;
        }

        #endregion

        #region Methods

        #region #index
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List([DataSourceRequest] DataSourceRequest request)
        {
            int _total = 0;
            SortDescriptor sortDescriptor = (request.Sorts != null && request.Sorts.Count > 0) ? request.Sorts.FirstOrDefault() : new SortDescriptor("Id", ListSortDirection.Descending);

            sortDescriptor.Member = sortDescriptor.Member ?? "CreatedDate";
            Func<IQueryable<Order>, IOrderedQueryable<Order>> order;
            //Expression<Func<Product, bool>> filter = x => x.IsActive == true;
            //if (!string.IsNullOrEmpty(keyword))
            //{
            //    filter = x => x.Name.Contains(keyword);
            //}
            var data = new List<Order>();
            switch (sortDescriptor.Member)
            {
                case "CreatedDate":
                    if (sortDescriptor.SortDirection == ListSortDirection.Descending)
                    {
                        order = x => x.OrderByDescending(y => y.CreatedDate);
                    }
                    else
                    {
                        order = x => x.OrderBy(y => y.CreatedDate);
                    }
                    break;
                case "OrderStatusId":
                    if (sortDescriptor.SortDirection == ListSortDirection.Ascending)
                    {
                        order = x => x.OrderBy(y => y.OrderStatusId);
                    }
                    else
                    {
                        order = x => x.OrderByDescending(y => y.OrderStatusId);
                    }
                    break;
                default:
                    if (sortDescriptor.SortDirection == ListSortDirection.Ascending)
                    {
                        order = x => x.OrderBy(y => y.Id);
                    }
                    else
                    {
                        order = x => x.OrderByDescending(y => y.Id);
                    }
                    break;
            }

            data = _orderService.Select(null, order, null, request.Page, request.PageSize).ToList();

            var orders = new List<OrderModel>();
            AutoMapper.Mapper.CreateMap<Order, OrderModel>();
            orders = AutoMapper.Mapper.Map<List<Order>, List<OrderModel>>(data);            

            _total = _orderService.Select(null, order, null, null, null).Count();
            ViewBag.Total = _total;
            var result = new DataSourceResult()
            {
                Data = orders,
                Total = _total
            };
            return Json(result);
        }

        #endregion

        #region #detail
        //public ActionResult Details(int id = 0)
        //{
        //    if (id == 0)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var model = new CategoryModel();
        //    model = _bannerService.Find(id).ToModel();
        //    return Json(model, JsonRequestBehavior.AllowGet);
        //}

        #endregion

        #region #edit

        public ActionResult Edit(int id = 0)
        {
            if (id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var entity = _orderService.Find(id);
            if (entity == null)
                return RedirectToAction("AccessDeny", "Role");
            //var model = entity.ToModel();
            var orderDetails = _orderDetailService.ListOrderDetailByOrderId(entity.Id);

            var orderDetailModels = new List<OrderDetailModel>();
            AutoMapper.Mapper.CreateMap<OrderDetail, OrderDetailModel>();
            orderDetailModels = AutoMapper.Mapper.Map<List<OrderDetail>, List<OrderDetailModel>>(orderDetails);

            foreach (var item in orderDetailModels)
            {
                item.Product = _productService.GetProductById(item.ProductId);                
            }
            ViewBag.OrderDeteails = orderDetailModels;
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string Edit(Order model)
        {
            if (ModelState.IsValid)
            {
                var entity = _orderService.Find(model.Id);
                if (entity != null)
                {
                    if (entity.OrderStatusId != model.OrderStatusId)
                        entity.OrderStatusId = model.OrderStatusId;
                    if (entity.CustomerName != model.CustomerName)
                        entity.CustomerName = model.CustomerName;
                    if (entity.Email != model.Email)
                        entity.Email = model.Email;
                    if (entity.Phone != model.Phone)
                        entity.Phone = model.Phone;
                    if (entity.Address != model.Address)
                        entity.Address = model.Address;

                    entity.ObjectState = ObjectState.Modified;
                    _orderService.Update(entity);
                    _unitOfWork.SaveChanges();
                }
                return JsonConvert.SerializeObject(new { Status = 1, Message = "Update success." });
            }
            else
            {
                return JsonConvert.SerializeObject(new { Status = 0, Message = "Update unsuccess." });
            }
        }

        #endregion

        #region #delete

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var entity = _orderService.Find(id);

                if (entity != null)
                {
                    //delete order detail
                    var orderDetails = _orderDetailService.ListOrderDetailByOrderId(entity.Id);
                    foreach (var item in orderDetails)
                        _orderDetailService.Delete(item);

                    //delete data
                    _orderService.Delete(entity);
                    _unitOfWork.SaveChanges();

                    return Json(new { Status = 1, Message = "Delete success!" });
                }

                return Json(new { Status = 0, Message = "Delete error!" });
            }
            catch
            {
                return Json(new { Status = 0, Message = "Delete error!" });
            }
        }

        #endregion

        #endregion
    }
}
