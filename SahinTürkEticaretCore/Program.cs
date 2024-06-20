using Microsoft.CodeAnalysis.Differencing;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession(option =>

{

    option.IdleTimeout = TimeSpan.FromMinutes(1);

});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();//Logine girilen mail almak i�in eklendi default sayfas�nda Accessoru kullanmak i�in yaz�ld� 
var app = builder.Build();



//biz ekledik layout da session login g�r�n�m� i�in





// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();//biz ekledik sepete ekle
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
