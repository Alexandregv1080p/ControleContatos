<<<<<<< HEAD
using ControleContatos.Data;
using ControleContatos.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DataBase");


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEntityFrameworkSqlServer().AddDbContext<BancoContext>(o => o.UseSqlServer(connectionString));
builder.Services.AddScoped<IContatoRepository, ContatoRepository>();
=======
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

>>>>>>> 0fecd126d7d3dede56a6b7e02d294da4fd6f42a2
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
