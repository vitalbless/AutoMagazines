namespace AutoMagazines.Data.Models.Cart
{
    public class ShopCartItem
    {
        public int ShopCartItemId { get; set; }
        public Car Car { get; set; }
        public int Price { get; set; }
        public string ShopCartId { get; set; }
    }
}
