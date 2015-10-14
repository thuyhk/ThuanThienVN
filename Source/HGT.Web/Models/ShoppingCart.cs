using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HGT.Entities.Models;
using HGT.Service;
using Repository.Pattern.UnitOfWork;

namespace HGT.Web.Models
{
    public partial class ShoppingCart
    {
        #region Fields

        public string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";

        #endregion

        #region Methods

        public static ShoppingCart GetCart(HttpContextBase ctx)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(ctx);
            return cart;
        }

        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public int AddToCart(IUnitOfWork unitOfWork, ICartService cartService, Product product)
        {
            var cartItem = cartService.ODataQueryable().SingleOrDefault(
                    c => c.CartId == ShoppingCartId
                    && c.ProductId == product.Id);

            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    CartId = ShoppingCartId,
                    ProductId = product.Id,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                cartService.Insert(cartItem);
            }
            else
            {
                cartItem.Count++;
                cartService.Update(cartItem);
            }

            //save changes
            unitOfWork.SaveChanges();
            return cartItem.Count;
        }

        // remove item from cart
        public int RemoveFromCart(IUnitOfWork unitOfWork, ICartService cartService, int productId)
        {

            var cartItem = cartService.ODataQueryable().Single(
                c => c.CartId == ShoppingCartId
                    && c.ProductId == productId);
            //int itemCount = 0;
            //if (cartItem != null)
            //{
            //    if (cartItem.Count > 1)
            //    {
            //        cartItem.Count--;
            //        itemCount = cartItem.Count;
            //        cartService.Update(cartItem);
            //    }
            //    else
            //    {
            cartService.Delete(cartItem);
            //}
            unitOfWork.SaveChanges();
            //}
            return 0;
        }

        // update cart
        public int UpdateCart(IUnitOfWork unitOfWork, ICartService cartService, int itemId, int quantity)
        {            
            var cartItem = cartService.ODataQueryable().Single(
                c => c.CartId == ShoppingCartId
                    && c.ProductId == itemId);
            if (cartItem != null && cartItem.Count > 0)
            {
                if (quantity.Equals(0))
                    cartService.Delete(cartItem);
                else
                {
                    cartItem.Count = quantity;
                    cartService.Update(cartItem);
                }
                unitOfWork.SaveChanges();
            }
            return quantity;
        }

        // empty cart
        public void EmptyCart(IUnitOfWork unitOfWork, ICartService cartService)
        {
            var cartItems = cartService.ODataQueryable().Where(x => x.CartId == ShoppingCartId);
            foreach (var cartItem in cartItems)
            {
                cartService.Delete(cartItem);
            }
            unitOfWork.SaveChanges();
        }

        public List<Cart> GetCartItems(ICartService cartService, IProductService productService, ICategoryService categoryService)
        {
            var results = new List<Cart>();
            var cartItems = cartService.ODataQueryable().Where(x => x.CartId == ShoppingCartId).ToList();
            foreach (var item in cartItems)
            {
                item.Product = productService.GetProductById(item.ProductId);
                item.CategoryAlias = categoryService.GetCategoryById(item.Product.CategoryId).Alias;
                results.Add(item);
            }
            return results;
        }

        public int GetCount(ICartService cartService)
        {
            int? count = cartService.ODataQueryable().Where(x => x.CartId == ShoppingCartId).Count();
            return count ?? 0;
        }

        public decimal GetTotal(ICartService cartService)
        {
            decimal? total = (from k in cartService.ODataQueryable()
                              where k.CartId == ShoppingCartId
                              select (int?)k.Count * k.Product.Price).Sum();
            return total ?? decimal.Zero;

        }

        public decimal GetItemTotal(ICartService cartService, int productId)
        {
            decimal? total = (from k in cartService.ODataQueryable()
                              where k.CartId == ShoppingCartId && k.ProductId == productId
                              select (int?)k.Count * k.Product.Price).Sum();
            return total ?? decimal.Zero;

        }

        public Order CreateOrder(IUnitOfWork unitOfWork, ICartService cartService, IOrderService orderService, IOrderDetailService orderDetailService,
            IProductService productService, ICategoryService categoryService, Order order)
        {
            decimal orderTotal = 0;
            var cartItems = GetCartItems(cartService, productService, categoryService);
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    OrderId = order.Id,
                    UnitPrice = item.Product.Price,
                    Quantity = item.Count
                };
                orderTotal += (item.Count * item.Product.Price);
                orderDetailService.Insert(orderDetail);
            }
            // update total into order
            order.Total = orderTotal;
            orderService.Update(order);

            // commit to changes
            unitOfWork.SaveChanges();

            //emty cart session
            EmptyCart(unitOfWork, cartService);

            return order;

        }

        public string GetCartId(HttpContextBase ctx)
        {
            if (ctx.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(ctx.User.Identity.Name))
                {
                    ctx.Session[CartSessionKey] = ctx.User.Identity.Name;
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    ctx.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return ctx.Session[CartSessionKey].ToString();
        }

        public void MigrateCart(IUnitOfWork unitOfWork, ICartService cartService, string userName)
        {
            var shoppingCart = cartService.ODataQueryable().Where(x => x.CartId == ShoppingCartId);
            foreach (var item in shoppingCart)
            {
                item.CartId = userName;
            }
            unitOfWork.SaveChanges();
        }

        #endregion

    }
}