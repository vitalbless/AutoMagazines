using AutoMagazines.Data.DbContext;
using Microsoft.AspNetCore.Mvc;

namespace AutoMagazines.Views.Shared.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        public StoreContext db;
        public NavigationMenuViewComponent(StoreContext context)
        {
            db = context;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["catId"];
            return View(db.CategoryTable.ToList());
        }
    }
}
