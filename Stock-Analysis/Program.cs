using BusinessLayer.Services;
using DataAccessLayer.Mappers;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Stock_Analysis.Controllers;
using Stock_Analysis.Middlewares;

var builder = WebApplication.CreateBuilder(args); //기본 웹 애플리케이션 생성
// 구글 인증 OAUTH 2.0
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/"; // 로그인 경로를 루트로 변경
})
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
    options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
});

string? connStr = builder.Configuration.GetConnectionString("MSSQL") ?? "";

// 로깅
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<StockService>();
builder.Services.AddSingleton<IndexService>();
builder.Services.AddSingleton<ETFService>();
builder.Services.AddControllersWithViews();





// 의존성 주입
builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddTransient<IStockService, StockService>();
builder.Services.AddTransient<IIndexService, IndexService>();
builder.Services.AddTransient<IETFService, ETFService>();
builder.Services.AddTransient<IPortfolioService, PortfolioService>();
builder.Services.AddTransient<IStockMapper, StockMapper>(provider => new StockMapper(connStr));
builder.Services.AddTransient<IIndexMapper, IndexMapper>(provider => new IndexMapper(connStr));
builder.Services.AddTransient<IPortfolioMapper, PortfolioMapper>(provider => new PortfolioMapper(connStr));
builder.Services.AddTransient<IProcCall, ProcCall>(provider => new ProcCall(connStr));
builder.Services.AddTransient<IETFMapper, ETFMapper>(provider => new ETFMapper(connStr));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(120); // 세션 유지 시간 설정
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


//서버점검 객체 서비스
builder.Services.AddHostedService<MaintenanceService>();

var app = builder.Build();
var stockService = app.Services.GetRequiredService<StockService>();
var indexService = app.Services.GetRequiredService<IndexService>();
var etfService = app.Services.GetRequiredService<ETFService>();
await stockService.LoadDataAsync();
await indexService.LoadDataAsync();
await etfService.LoadDataAsync();

app.UseSession();
app.UseStaticFiles();
app.UseMiddleware<ServerMaintenance>();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

app.Run();
