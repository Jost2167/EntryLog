using EntryLog.Business;
using EntryLog.Data;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar autenticación por cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Ruta a la que se redirige si no está autenticado
        options.LogoutPath = "/Account/Logout"; // Ruta para cerrar sesión
        options.AccessDeniedPath = "/Account/AccessDenied"; // Ruta a la que se redirige si no tiene permiso
    });

// Configurar sesión
builder.Services.AddSession();

// Agregar servicios de otras capas
builder.Services.AddBusinessDependencies(builder.Configuration);
builder.Services.AddDataDependencies(builder.Configuration);


// Servicio de autenticación
// Especifica que el esquema de autenticación por defecto es el de cookies


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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=RegisterEmployeeUser}");

app.Run();