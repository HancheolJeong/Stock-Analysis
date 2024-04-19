using BusinessLayer.Services;
using DataAccessLayer.Mappers;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Stock_Analysis.Controllers;

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
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<StockService>();
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ILoginService, LoginServiceImpl>();
builder.Services.AddTransient<IStockService, StockService>();
builder.Services.AddTransient<ILoginMapper, LoginMapper>(provider => new LoginMapper(connStr));
builder.Services.AddTransient<IStockMapper, StockMapper>(provider => new StockMapper(connStr));
//builder.Services.AddTransient<ProcCall>(provider => new ProcCall());
//builder.Services.AddTransient<ProcCall>(provider => new ProcCall(connStr));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(120); // 세션 유지 시간 설정
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();
var cacheService = app.Services.GetRequiredService<StockService>();
await cacheService.LoadDataAsync();
app.UseSession();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
//app.UseMiddleware<AuthenticationMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

app.Run();
