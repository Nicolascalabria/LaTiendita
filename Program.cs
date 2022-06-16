using LaTiendita.Stock;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Microsoft.AspNetCore.Authentication.AuthenticationBuilder authenticationBuilder = builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(ConfiguracionCookie);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BaseDeDatos>(options => 
  options.UseSqlite(@"filename=C:\Temp\LaTiendita.db")); 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


 static void ConfiguracionCookie(CookieAuthenticationOptions opciones)
{
    opciones.LoginPath = "/Home/Index";
    opciones.AccessDeniedPath = "/Usuarios/NoAutorizado";
    opciones.LogoutPath = "/Login/Logout";
    opciones.ExpireTimeSpan = System.TimeSpan.FromMinutes(10);   



}

app.UseCookiePolicy();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
