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
        public IActionResult Index(int catId = 1, int page = 1)
        {
            ViewBag.Title = "Главная страница";

            return View(
                new HomeIndexViewModel
                {
                    PageName = "Все автомобили",
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = pageSize,
                        TotalItems = db.CarTable.Where(c => c.CategoryId == catId).Count()
                    },
                    Cars = db.CarTable.Where(p => p.CategoryId == catId).OrderBy(c => c.CarId).Skip((page - 1) * pageSize).Take(pageSize),
                    CurrentCategory = catId
                }
            );
        }
    }
}
