using ECommerce.ApiItegration;
using ECommerce.Models.Request.Orders;
using ECommerce.Utilities;
using ECommerce.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ECommerce.WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductApiClient _productApiClient;

        private readonly IOrderApiClient _orderApiClient;

        public CartController(IProductApiClient productApiClient, IOrderApiClient orderApiClient)
        {
            _productApiClient = productApiClient;
            _orderApiClient = orderApiClient;
        }

        #region function for handle cart

        private List<CartVM> GetCartItems()
        {
            var jsonCart = HttpContext.Session.GetString(SystemConstants.CartSessionKey);
            if (jsonCart != null)
            {
                return JsonConvert.DeserializeObject<List<CartVM>>(jsonCart);
            }
            return new List<CartVM>();
        }

        private void ClearCart()
        {
            HttpContext.Session.Remove(SystemConstants.CartSessionKey);
        }

        private void SaveCartSession(List<CartVM> lst)
        {
            var jsonCart = JsonConvert.SerializeObject(lst);
            HttpContext.Session.SetString(SystemConstants.CartSessionKey, jsonCart);
        }

        #endregion function for handle cart

        [Route("/cart", Name = "cart")]
        public IActionResult Cart()
        {
            return View(GetCartItems());
        }

        [Route("{culture}/addcart/{productId:int}/{quantity:int?}")]
        public async Task<IActionResult> AddToCart(string culture, int productId, int? quantity)
        {
            var product = await _productApiClient.GetProductById(culture, productId);
            if (product == null)
                return NotFound("Cannot find the product");

            var cart = GetCartItems();
            var cartItem = cart.Find(p => p.Product.Id == productId);
            if (cartItem != null)
            {
                // had cart
                if (quantity != null)
                {
                    cartItem.Quantity = (int)quantity;
                }
                else
                {
                    cartItem.Quantity++;
                }
            }
            else
            {
                // new cart
                cart.Add(new CartVM { Product = product, Quantity = (quantity == null ? 1 : (int)quantity) });
            }

            // save cart to session
            SaveCartSession(cart);

            // redirect to cart view
            return Json(new { numOfItems = cart.Count() });
        }

        [HttpPatch("/{productId}/updateCart/{quantity}")]
        public IActionResult UpdateCart(int productId, int quantity)
        {
            // update quantity
            var carts = GetCartItems();
            var cartItem = carts.Find(p => p.Product.Id == productId);
            if (cartItem != null)
            {
                // had cart
                cartItem.Quantity = quantity;
            }
            SaveCartSession(carts);

            var cartTotalVM = new CartTotalVM()
            {
                SubTotal = 0,
                Delivery = 0,
                Discount = 0,
            };

            var newCarts = GetCartItems();
            foreach (var cart in newCarts)
            {
                cartTotalVM.SubTotal += cart.GetTotalPrice();
            }

            // return non-variable, only ajax call to handle
            return Json(new { redirectToUrl = Url.Action("cart", "cart") });
        }

        [HttpDelete("/cart/{productId}")]
        public IActionResult RemoveCart(int productId)
        {
            var cart = GetCartItems();
            var cartItem = cart.Find(p => p.Product.Id == productId);
            if (cartItem != null)
            {
                // had cart
                cart.Remove(cartItem);
            }

            SaveCartSession(cart);

            // for ajax call
            return Json(new { redirectToUrl = Url.Action("cart", "cart") });
        }

        public async Task<IActionResult> Checkout(CreateOrderRequest request)
        {
            // loop get all product in current order
            var allItems = GetCartItems();
            request.OrderProduct = new List<OrderProductRequest>();
            foreach (var item in allItems)
            {
                request.OrderProduct.Add(new OrderProductRequest()
                {
                    Price = item.Product.Price,
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity
                });
            }

            var orderVM = await _orderApiClient.CreateOrder(request);
            if (orderVM == null) throw new Exception("Order is unsuccess!!!");
            ClearCart();
            return View(orderVM);
        }
    }
}