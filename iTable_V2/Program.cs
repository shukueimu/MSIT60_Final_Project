using iTable_V2.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// 添加服務到 DI 容器
builder.Services.AddDbContext<ITableDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("iTableDBConnection")));
// 設定使用 SQL Server 的 iTableDbContext，連接字串從 appsettings.json 中的 iTableDBConnection 讀取

// 啟用 Session
builder.Services.AddSession();

builder.Services.AddControllersWithViews(); // 註冊 MVC 服務

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Booking}/{action=BookingPage}/{restaurantID?}");

app.Run();
