using BusinessLayer.Services;
using DataAccessLayer.Mappers;

var builder = WebApplication.CreateBuilder(args); //�⺻ �� ���ø����̼� ����
string? connStr = builder.Configuration.GetConnectionString("MSSQL") ?? "";
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ILoginService, LoginServiceImpl>();
builder.Services.AddTransient<IStockService, StockService>();
builder.Services.AddTransient<ILoginMapper, LoginMapper>(provider => new LoginMapper(connStr));
builder.Services.AddTransient<IStockMapper, StockMapper>(provider => new StockMapper(connStr));
builder.Services.AddTransient<ProcCall>(provider => new ProcCall(connStr));
builder.Services.AddSession();
var app = builder.Build();

app.UseSession();
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

app.Run();