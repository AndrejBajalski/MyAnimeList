using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.DomainModels;
using Repository.Data;
using Repository.Interface;
using Repository.Implementation;
using Service.Implementation;
using Service.Interface;
using Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString,
                         b => b.MigrationsAssembly("Repository"))
);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<DataFetchService>();
builder.Services.AddScoped<IAnimeService, AnimeService>();
builder.Services.AddScoped<IUnwatchedAnimeService, UnwatchedAnimeService>();
builder.Services.AddHostedService<ShutdownCleanupService>();

var app = builder.Build();

// Register shutdown event
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

lifetime.ApplicationStopping.Register(() =>
{
    using var scope = scopeFactory.CreateScope();
    var animeRepository = scope.ServiceProvider.GetRequiredService<IRepository<Anime>>();
    animeRepository.Clear();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.UseAuthorization();

app.MapRazorPages();

app.Run();
