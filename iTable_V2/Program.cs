using iTable_V2.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// �K�[�A�Ȩ� DI �e��
builder.Services.AddDbContext<ITableDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("iTableDBConnection")));
// �]�w�ϥ� SQL Server �� iTableDbContext�A�s���r��q appsettings.json ���� iTableDBConnection Ū��

// �ҥ� Session
builder.Services.AddSession();

builder.Services.AddControllersWithViews(); // ���U MVC �A��

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
