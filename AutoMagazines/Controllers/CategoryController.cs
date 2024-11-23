using AutoMagazines.Data.DbContext;
using AutoMagazines.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoMagazines.Controllers
{
    public class CategoryController : Controller
    {
        StoreContext db;
        public CategoryController(StoreContext context)
        {
            db = context;
        }
        public IActionResult CategoryList()
        {
            return View(db.CategoryTable.ToList());
        }
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            if (category != null)
            {
                db.CategoryTable.Add(category);
                db.SaveChanges();
            }
            return RedirectToAction("CategoryList");
        }
        public IActionResult EditCategory(int? categoryId)
        {
            if (categoryId == null)
            {
                return RedirectToAction("CategoryTable");
            }
            else
            {
                return View(db.CategoryTable.Find(categoryId));
            }
        }

        [HttpPost]
        public IActionResult EditCategory(Category category)
        {
            if (category != null)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("CategoryList");
        }

        public IActionResult DeleteCategory(int? categoryId)
        {
            if (categoryId != null)
            {
                db.CategoryTable.Remove(db.CategoryTable.Find(categoryId));
                db.SaveChanges();
            }
            return RedirectToAction("CategoryList");
        }
    }
}
