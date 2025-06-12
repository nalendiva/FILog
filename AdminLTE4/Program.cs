using FILog.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Negotiate;

var builder = WebApplication.CreateBuilder(args);

// Konfigurasi Database
builder.Services.AddDbContext<OpsProdDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Tambahkan service autentikasi Negotiate (Windows Auth)
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();

builder.Services.AddAuthorization();

// Tambahkan Controller dan View
builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();

// Konfigurasi Swagger (Hanya Aktif di Development Mode)
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "QualityInspection API",
        Version = "v1",
        Description = "API for Quality Inspection",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Ale",
            Email = "muhammadnalendraz@gmail.com",
            Url = new Uri("https://example.com")
        }
    });
});

var app = builder.Build();

// Konfigurasi Middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "QualityInspection API V1");
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization(); // Tambahkan jika nanti butuh autentikasi

// Rute Default
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
