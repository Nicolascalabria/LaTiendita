using LaTiendita.Stock;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

Microsoft.AspNetCore.Authentication.AuthenticationBuilder authenticationBuilder = builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(ConfiguracionCookie);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BaseDeDatos>(options => 
  options.UseSqlite(@"filename=Datos/DB.db")); 
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
