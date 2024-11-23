namespace AutoMagazines.Data.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public ICollection<Car> Cars { get; set; }
    }
}
