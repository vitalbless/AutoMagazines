using AutoMagazines.Data.DbContext;
using AutoMagazines.Data.Models.Cart;
using Microsoft.AspNetCore.Mvc;

namespace AutoMagazines.Controllers
{
    public class ShopCartController : Controller
    {
        StoreContext db;
        ShopCart _shopCart;
        public ShopCartController(StoreContext context, ShopCart shopCart)
        {
            db = context;
            _shopCart = shopCart;
        }

        public ViewResult Index()
        {

            _shopCart.ListShopItems = _shopCart.GetShopItems();
            return View(_shopCart.ListShopItems);
        }
        public void ClearCart()
        {
            // Используем ShopCartId из объекта _shopCart
            var cartItems = db.ShopCartItemTable.Where(c => c.ShopCartId == _shopCart.ShopCartId).ToList();
            db.ShopCartItemTable.RemoveRange(cartItems); // Удаляем все элементы корзины
            db.SaveChanges();
        }


        public RedirectToActionResult AddToCart(int id)
        {
            var item = db.CarTable.Find(id);
            if (item != null)
            {
                _shopCart.AddToCart(item);
            }
            return RedirectToAction("Index");
        }

    }
}
