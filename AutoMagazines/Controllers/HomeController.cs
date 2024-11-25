using AutoMagazines.Data.DbContext;
using AutoMagazines.Data.Models;
using AutoMagazines.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AutoMagazines.Controllers
{
    public class HomeController : Controller
    {
        int pageSize = 6;
        int MaxPage => (int)Math.Ceiling((decimal)db.CarTable.Count() / pageSize);
        private StoreContext db;
        public HomeController(StoreContext context)
        {
            db = context;
        }
        public IActionResult Index(int? catId = null, int page = 1)
        {
            ViewBag.Title = "Главная страница";

            var carsQuery = db.CarTable.AsQueryable();

            if (catId.HasValue)
            {
                carsQuery = carsQuery.Where(c => c.CategoryId == catId.Value);
            }

            return View(
                new HomeIndexViewModel
                {
                    PageName = catId.HasValue ? "Автомобили категории" : "Все автомобили",
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = pageSize,
                        TotalItems = carsQuery.Count()
                    },
                    Cars = carsQuery.OrderBy(c => c.CarId).Skip((page - 1) * pageSize).Take(pageSize),
                    CurrentCategory = catId ?? 0
                }
            );
        }

    }
}
