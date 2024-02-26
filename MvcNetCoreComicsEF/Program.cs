using Microsoft.EntityFrameworkCore;
using MvcNetCoreComicsEF.Data;
using MvcNetCoreComicsEF.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// SQLSERVER
//string connectionString = builder.Configuration.GetConnectionString("SqlServerConnection");
//builder.Services.AddTransient<IRepositoryComics, RepositoryComicsSQLServer>();
//builder.Services.AddDbContext<ComicContext>
//    (options => options.UseSqlServer(connectionString));

// ORACLE
string connectionString = builder.Configuration.GetConnectionString("OracleConnection");
builder.Services.AddTransient<IRepositoryComics, RepositoryComicsOracle>();
builder.Services.AddDbContext<ComicContext>
    (options => options.UseOracle(connectionString,
    options => options.UseOracleSQLCompatibility("11")));

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
