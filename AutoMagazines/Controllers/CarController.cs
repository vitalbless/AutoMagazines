using AutoMagazines.Data.DbContext;
using AutoMagazines.Data.Models;
using AutoMagazines.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoMagazines.Controllers
{
    public class CarController : Controller
    {
        StoreContext db;
        IWebHostEnvironment env;
        public CarController(StoreContext context, IWebHostEnvironment environment)
        {
            db = context;
            env = environment;
        }

        public IActionResult SingleCar(int carId)
        {
            return View(db.CarTable.Find(carId));
        }
        public IActionResult CarList()
        {
            return View(db.CarTable.ToList());
        }

        [HttpGet]
        public IActionResult AddCar()
        {
            return View(new AddCarViewModel
            {
                Car = new Car(),
                Categories = db.CategoryTable.ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddCar(Car car, IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                //сформировали путь к файлу
                string path = $"/img/{uploadedFile.FileName}";
                car.Img = path;
                //сохраняем файл в папку на сервере
                using (var filestream = new FileStream(env.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(filestream);
                }
            }

            db.CarTable.Add(car);
            await db.SaveChangesAsync();

            return RedirectToRoute(new { controller = "Car", action = "CarList" });
        }

        [HttpGet]
        public IActionResult EditCar(int? carId)
        {
            if (carId == null)
            {
                return RedirectToAction("Table");
            }
            else
            {
                return View(new AddCarViewModel
                {
                    Car = db.CarTable.Find(carId),
                    Categories = db.CategoryTable.ToList()
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditCar(Car car, IFormFile uploadedFile)
        {
            // Получаем существующий объект из базы данных
            var existingCar = await db.CarTable.FindAsync(car.CarId);
            if (existingCar == null)
            {
                return NotFound();
            }

            // Если файл загружен, обновляем фотографию
            if (uploadedFile != null)
            {
                string path = $"/img/{uploadedFile.FileName}";
                car.Img = path;
                using (var filestream = new FileStream(env.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(filestream);
                }
            }
            else
            {
                // Если файл не загружен, сохраняем старую фотографию
                car.Img = existingCar.Img;
            }

            // Обновляем данные автомобиля
            db.Entry(existingCar).CurrentValues.SetValues(car);
            await db.SaveChangesAsync();

            return RedirectToAction("CarList");
        }


        public IActionResult DeleteCar(int? carId)
        {
            if (carId != null)
            {
                db.CarTable.Remove(db.CarTable.Find(carId));
                db.SaveChanges();
            }
            return RedirectToAction("CarList");
        }
    }
}
