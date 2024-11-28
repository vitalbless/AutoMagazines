using AutoMagazines.Data.DbContext;
using AutoMagazines.Data.Models.Cart;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ����������� ���� ������
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer(@"Server=localhost;Database=AutoMagazine;Trusted_Connection=True;TrustServerCertificate=True;"));

// ���������� ��������� ������������ � �������������
builder.Services.AddControllersWithViews();

// ��������� ������
builder.Services.AddSession(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // �������� ������ �� HTTPS
    options.Cookie.SameSite = SameSiteMode.Lax; // ������������ Lax ��� Strict ��� SameSite
    options.IdleTimeout = TimeSpan.FromMinutes(30); // ���������� �������� ����� ����� ������
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// ����������� ������� ��� scoped-�������
builder.Services.AddScoped(sp => ShopCart.GetCart(sp));

var app = builder.Build();

// ������������� ���� ������
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

// �������� ��������� ��������
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection(); // �������� � HTTP �� HTTPS
app.UseStaticFiles();

app.UseRouting();

// ����������� ������
app.UseSession();

app.UseAuthorization();

// �������� ������������
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
