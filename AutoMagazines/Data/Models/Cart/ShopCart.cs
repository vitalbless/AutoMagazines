using AutoMagazines.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace AutoMagazines.Data.Models.Cart
{
    public class ShopCart
    {
        private StoreContext db;
        public ShopCart(StoreContext context)
        {
            db = context;
        }

        public string ShopCartId { get; set; } //идентификатор всей корзины
        public List<ShopCartItem> ListShopItems { get; set; } //список всех товаров в корзине
        /// <summary>
        /// будет определять новая корзина или нет. Если новая, то определит новий идентификатор
        /// </summary>
        /// <returns></returns>
        public static ShopCart GetCart(IServiceProvider service)
        {
            //Переменная, через которую будем работать с сессиями
            ISession session = service.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = service.GetService<StoreContext>();
            //вернет нам ID корзины
            string shopCartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", shopCartId); //привязываем к каждому товару сгенерированный ключ
            return new ShopCart(context)
            {
                ShopCartId = shopCartId
            };
        }

        public void AddToCart(Car car)
        {
            //db.ShopCartTable.Add(new ShopCart(db) { ShopCartId = ShopCartId, ListShopItems = ListShopItems });
            db.ShopCartItemTable.Add(new ShopCartItem
            {
                ShopCartId = ShopCartId,
                Car = car,
                Price = car.Price,
            });
            db.SaveChanges();
        }

        public List<ShopCartItem> GetShopItems()
        {
            return db.ShopCartItemTable.Where(c => c.ShopCartId == ShopCartId).Include(cr => cr.Car).ToList();
        }
    }
}
