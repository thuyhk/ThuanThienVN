using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HGT.Service;
using Repository.Pattern.UnitOfWork;
using HGT.Web.Extensions;
using HGT.Entities.Models;
using System.Net;
using HGT.Web.Models;
using HGT.Core.Enums;
using HGT.Core;
using System.Configuration;
using HGT.Core.Utility;
using HGT.Core.Models;

namespace HGT.Web.Controllers
{
    public class CheckoutController : Controller
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly ICartService _cartService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailQueueService _emailQueueService;
        private readonly IContentService _content;

        #endregion

        #region Contructors

        public CheckoutController(IUnitOfWork unitOfWork, ICategoryService categoryService,
            IOrderService orderService, IOrderDetailService orderDetailService,
            ICartService cartService, IEmailQueueService emailQueueService,
            IProductService productService,
            IEmailTemplateService emailTemplateService, IContentService content)
        {
            this._unitOfWork = unitOfWork;
            this._categoryService = categoryService;
            this._productService = productService;
            this._orderService = orderService;
            this._orderDetailService = orderDetailService;
            this._cartService = cartService;
            this._emailQueueService = emailQueueService;
            this._emailTemplateService = emailTemplateService;
            this._content = content;
        }

        #endregion

        #region Methods

        #region #checkout in form shopping cart

        public ActionResult _CheckoutInFormCart()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult _CheckoutInFormCart(CheckoutModel model)
        {
            if (ModelState.IsValid)
            {
                string message;
                var orderId = CreateOrder(model);
                if (orderId > 0)
                {
                    var order = _orderService.Find(orderId);

                    message = _content.Find((int)ContentType.CreateOrderSuccess).Value;
                    if (!string.IsNullOrEmpty(model.Email))
                    {
                        try
                        {
                            var appSettings = AppSettings.Settings;
                            string emailFrom = appSettings.EmailAdmin;
                            string emailCc = appSettings.EmailAdmin;
                            string emailBcc = appSettings.CompanyEmail;

                            var emailTemplateId = (int)EmailTemplateType.ConfirmOrder;
                            var emaiTemplate = _emailTemplateService.GetEmailByID(emailTemplateId);

                            var subject = emaiTemplate.Subject;

                            string mailbody = emaiTemplate.Content;
                            mailbody = mailbody.Replace(ConstantKeys.UserName, model.FullName);

                            var detail = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"min-width:500px; color:#3A3A3A;\">";
                            detail += "<tbody><tr><td style=\"border: 1px solid #575757; padding: 4px;\"><strong>Tên sản phẩm</strong></td><td style=\"border: 1px solid #575757; padding: 4px;\"><strong>Số lượng</strong></td><td style=\"border: 1px solid #575757; padding: 4px;\"><strong>Giá</strong></td></tr>";
                            var orderDetails = _orderDetailService.ListOrderDetailByOrderId(order.Id);

                            var orderDetailModels = new List<OrderDetailModel>();
                            AutoMapper.Mapper.CreateMap<OrderDetail, OrderDetailModel>();
                            orderDetailModels = AutoMapper.Mapper.Map<List<OrderDetail>, List<OrderDetailModel>>(orderDetails);

                            foreach (var item in orderDetailModels)
                            {
                                item.Product = _productService.GetProductById(item.ProductId);
                                detail += "<tr><td style=\"border: 1px solid #575757; padding: 4px;\">" + item.Product.Name + "</td><td style=\"border: 1px solid #575757;padding: 4px; \">" + item.Quantity + "</td><td style=\"border: 1px solid #575757; padding: 4px;\">" + item.Product.Price.ToString("#,###") + "</td></tr>";
                            }

                            detail += "<tr><td style=\"border: 1px solid #575757; padding: 4px;\">&nbsp;</td><td style=\"border: 1px solid #575757; padding: 4px;\"><strong>TỔNG TIỀN</strong></td><td style=\"border: 1px solid #575757; padding: 4px;\">" + order.Total.Value.ToString("#,###") + "</td></tr>";
                            detail += "</tr></tbody></table>";


                            mailbody += detail;

                            // send mail to guest
                            var status = SendMailUtility.SendMailWithOutCC("", subject, mailbody, model.Email); // subject, content, emailTo, cc, bcc

                            string ip = System.Web.HttpContext.Current.Request.UserHostAddress;
                            var emailqueues = new EmailQueue();

                            emailqueues.EmailFrom = emailFrom;
                            //emailqueues.Department = 0;
                            emailqueues.Active = status;
                            emailqueues.CreatedDate = DateTime.Now;
                            emailqueues.UpdatedDate = DateTime.Now;
                            emailqueues.EmailTo = model.Email;
                            emailqueues.EmailCc = emailFrom;
                            emailqueues.EmailBcc = emailBcc;
                            emailqueues.EmailSubject = subject;
                            emailqueues.SenderIP = ip;
                            emailqueues.DisplayNameFrom = model.FullName;

                            _emailQueueService.Insert(emailqueues);
                            _unitOfWork.SaveChanges();
                        }
                        catch { }
                    }
                }
                else
                    message = _content.Find((int)ContentType.CreateOrderUnSuccess).Value;
                //message = "<div class=\"msgCart\">Xảy ra lỗi trong quá trình đặt hàng. </div>" +
                //   "<div class=\"msgCart\">Vui lòng gọi <span class=\"txtRed\">0904.085.130</span> hoặc <span class=\"txtRed\">0466.808.838</span>. Trở về <a href=\"/\">trang chủ</a></div>";
                message = message.Replace(ConstantKeys.UserName.ToString(), model.FullName);
                return Content(message, "text/html");
            }
            else return View(model);
        }

        #endregion

        #region #checkout in modal popup

        [HttpGet]
        public ActionResult _ModalCheckout()
        {
            var model = new CheckoutModel();
            return PartialView(model);
        }

        #endregion

        #region #private methods

        private int CreateOrder(CheckoutModel model)
        {
            var order = new Order();
            TryUpdateModel(model);
            try
            {
                order.CreatedDate = DateTime.Now;
                order.CustomerName = model.FullName;
                order.Email = model.Email;
                order.Address = model.ShippingAddress;
                order.Phone = model.Phone;
                order.OrderStatusId = (int)OrderStatusType.New;
                order.Note = model.Note;
                order.HomeDelivery = true;
                order.Deleted = false;

                _orderService.Insert(order);
                _unitOfWork.SaveChanges();

                var cart = ShoppingCart.GetCart(this.HttpContext);
                order = cart.CreateOrder(_unitOfWork, _cartService, _orderService, _orderDetailService, _productService, _categoryService, order);

                return order.Id;
            }
            catch
            {
                return 0;
            }
        }

        #endregion

        #endregion
    }
}