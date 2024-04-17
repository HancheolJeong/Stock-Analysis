using BusinessLayer.Services;
using DataAccessLayer.Mappers;

var builder = WebApplication.CreateBuilder(args); //기본 웹 애플리케이션 생성
string? connStr = builder.Configuration.GetConnectionString("MSSQL") ?? "";
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ILoginService, LoginServiceImpl>();
builder.Services.AddTransient<ILoginMapper, LoginMapper>(provider => new LoginMapper(connStr));
builder.Services.AddTransient<IStockMapper, StockMapper>(provider => new StockMapper(connStr));
builder.Services.AddSession();
var app = builder.Build();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

app.Run();
