using AutoMagazines.Data.DbContext;
using AutoMagazines.Data.Models.Cart;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Подключение базы данных
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer(@"Server=localhost;Database=AutoMagazine;Trusted_Connection=True;TrustServerCertificate=True;"));

// Добавление поддержки контроллеров и представлений
builder.Services.AddControllersWithViews();

// Включение сессий
builder.Services.AddSession();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Регистрация корзины как scoped-сервиса
builder.Services.AddScoped(sp => ShopCart.GetCart(sp));

var app = builder.Build();

// Инициализация базы данных
using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<StoreContext>();
        dbContext.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database initialization error: {ex.Message}");
    }
}

// Конвейер обработки запросов
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Подключение сессий
app.UseSession();

app.UseAuthorization();

// Маршруты контроллеров
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
