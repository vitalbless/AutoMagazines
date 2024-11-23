using AutoMagazines.Data.Models;

namespace AutoMagazines.Data.ViewModels
{
    public class AddCarViewModel
    {
        public Car Car { get; set; }
        public List<Category> Categories { get; set; }
    }
}
