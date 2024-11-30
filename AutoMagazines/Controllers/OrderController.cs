using AutoMagazines.Data.DbContext;
using AutoMagazines.Data.Models;
using AutoMagazines.Data.Models.Cart;
using Microsoft.AspNetCore.Mvc;

namespace AutoMagazines.Controllers
{
    public class OrderController : Controller
    {
        private StoreContext db;
        private ShopCart shopCart;

        public OrderController(StoreContext context, ShopCart cart)
        {
            db = context;
            shopCart = cart;
        }

        public IActionResult Checkout()
        {
            shopCart.ListShopItems = shopCart.GetShopItems();
            ViewBag.CartItems = shopCart.ListShopItems;
            return View();
        }


      
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            shopCart.ListShopItems = shopCart.GetShopItems(); // Загрузить товары из корзины
            ViewBag.CartItems = shopCart.ListShopItems; // Повторно передать в ViewBag

            if (shopCart.ListShopItems.Count == 0)
            {
                ModelState.AddModelError("", "Корзина пуста. Добавьте товары перед оформлением заказа.");
            }

            if (ModelState.IsValid)
            {
                CreateOrder(order);
                return RedirectToAction("Complete");
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).ToList();

            return View(order);
        }


        public IActionResult Complete()
        {
            ViewBag.Message = "Заказ обработан";
            return View();
        }

        private void CreateOrder(Order order)
        {
            order.OrderTime = DateTime.Now;
            db.OrderTable.Add(order);
            db.SaveChanges();

            var items = shopCart.ListShopItems;

            foreach (var el in items)
            {
                var orderDetail = new OrderDetail()
                {
                    CarId = el.Car.CarId,
                    OrderId = order.OrderId,
                    Price = el.Car.Price
                };
                db.OrderDetailTable.Add(orderDetail);
                db.SaveChanges();
            }
        }
    }
}
