using ColegioData.Configuracion;
using ColegioData.Contrato;
using ColegioData.Implementacion;
using ColegioWeb.Models.DTOs;
using ColegioWeb.Services;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddTransient<IMessage, Message>();
builder.Services.Configure<GmailSettings>(builder.Configuration.GetSection("GmailSettings"));



builder.Services.AddScoped<IAsesoriaRepositorio, AsesoriaRepositorio>();
builder.Services.AddScoped<IDocenteRepositorio, DocenteRepositorio>();
builder.Services.AddScoped<ICursoRepositorio, CursoRepositorio>();
builder.Services.AddScoped<IRolUsuarioRepositorio, RolUsuarioRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IDashboardRepositorio, DashboardRepositorio>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Acceso/Login";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        option.AccessDeniedPath = "/Acceso/Denegado";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=Login}/{id?}");

app.Run();
