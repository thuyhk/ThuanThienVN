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

namespace HGT.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly ICartService _cartService;

        #endregion

        #region Contructors

        public ShoppingCartController(IUnitOfWork unitOfWork, ICategoryService categoryService,
            IProductService productService,
            IOrderService orderService, IOrderDetailService orderDetailService,
            ICartService cartService)
        {
            this._unitOfWork = unitOfWork;
            this._categoryService = categoryService;
            this._productService = productService;
            this._orderService = orderService;
            this._orderDetailService = orderDetailService;
            this._cartService = cartService;
        }

        #endregion

        #region Methods

        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var model = new ShoppingCartModel
            {
                CartItems = cart.GetCartItems(_cartService, _productService, _categoryService),
                CartTotal = cart.GetTotal(_cartService)
            };
            return View(model);
        }

        // sequent 1: after click button "add cart", redirect to cart page (include form quick checkout)
        [HttpPost]
        public ActionResult AddToCart(int id)
        {
            //Check price

            var addedProduct = _productService.GetProductById(id);
            var cart = ShoppingCart.GetCart(this.HttpContext);
            int count = cart.AddToCart(_unitOfWork, _cartService, addedProduct);

            var results = new ShoppingCartRemoveModel
            {
                Message = string.Format("Sản phẩm {0} vừa được thêm vào giỏ hàng.", addedProduct.Name),
                CartTotal = cart.GetTotal(_cartService),
                CartCount = cart.GetCount(_cartService),
                ItemTotal = cart.GetItemTotal(_cartService, id),
                ItemCount = count,
                DeleteId = id
            };
            return Json(results);
        }

        // sequent 2: after click button "add cart", show popup (include info cart and button goto cart page (include form quick checkout)
        [HttpPost]
        public ActionResult AddToQuickCart(int id)
        {
            var model = new ShoppingCartModel();
            try
            {
                var addedProduct = _productService.GetProductById(id);
                var cart = ShoppingCart.GetCart(this.HttpContext);
                int count = cart.AddToCart(_unitOfWork, _cartService, addedProduct);

                model.CartItems = cart.GetCartItems(_cartService, _productService, _categoryService);
                model.CartTotal = cart.GetTotal(_cartService);
                ViewData["CountItems"] = model.CartItems.Count();
            }
            catch { }
            return PartialView("_QuickCart", model);
        }

        // AJAX
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            string productName = _productService.GetProductById(id).Name;
            int itemCount = cart.RemoveFromCart(_unitOfWork, _cartService, id);
            var results = new ShoppingCartRemoveModel
            {
                Message = string.Format("Sản phẩm {0} vừa được xóa khỏi giỏ hàng.", productName),
                CartTotal = cart.GetTotal(_cartService),
                CartCount = cart.GetCount(_cartService),
                ItemTotal = cart.GetItemTotal(_cartService, id),
                ItemCount = itemCount,
                DeleteId = id
            };
            if (results.CartCount == 0)
            {
                results.Message = "Không tồn tại sản phẩm trong giỏ hàng.";
            }
            return Json(results);
        }

        // AJAX
        [HttpPost]
        public ActionResult UpdateCart(int id, int quantity)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            string productName = _productService.GetProductById(id).Name;
            int itemCount = cart.UpdateCart(_unitOfWork, _cartService, id, quantity);
            var results = new ShoppingCartRemoveModel
            {
                Message = "Cập nhật giỏ hàng thành công.",
                CartTotal = cart.GetTotal(_cartService),
                CartCount = cart.GetCount(_cartService),
                ItemTotal = cart.GetItemTotal(_cartService, id),
                ItemCount = itemCount,
                DeleteId = id
            };
            if (results.CartCount == 0)
            {
                results.Message = "Không tồn tại sản phẩm trong giỏ hàng.";
            }
            return Json(results);
        }       

        public ActionResult _InfoCartCheckout()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var model = new ShoppingCartModel
            {
                CartItems = cart.GetCartItems(_cartService, _productService, _categoryService),
                CartTotal = cart.GetTotal(_cartService)
            };
            return PartialView(model);
        }        

        // only show total item in cart
        [ChildActionOnly]
        public ActionResult _CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var countItem = cart.GetCount(_cartService);
            ViewData["CartCount"] = "(" + cart.GetCount(_cartService) + ")";
            return PartialView("_CartSummary");
        }
        
        [HttpPost]
        public ActionResult _ShowQuickCart()
        {
            var model = new ShoppingCartModel();
            try
            {
                var cart = ShoppingCart.GetCart(this.HttpContext);
                model.CartItems = cart.GetCartItems(_cartService, _productService, _categoryService);
                model.CartTotal = cart.GetTotal(_cartService);
                ViewData["CountItems"] = model.CartItems.Count();
            }
            catch { }
            return PartialView("_QuickCart", model);
        }

        #endregion
    }
}