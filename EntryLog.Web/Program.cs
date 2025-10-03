using EntryLog.Business;
using EntryLog.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseRouting();


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=RegisterEmployeeUser}");

app.Run();