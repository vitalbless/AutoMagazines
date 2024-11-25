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
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            shopCart.ListShopItems = shopCart.GetShopItems();
            Console.WriteLine(shopCart.ListShopItems.Count);    
            if (shopCart.ListShopItems.Count == 0)
            {
                ModelState.AddModelError("", "Должны быть товары");
            }
            if (ModelState.IsValid)
            {
                CreateOrder(order);
                return RedirectToAction("Complete");
            }
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
