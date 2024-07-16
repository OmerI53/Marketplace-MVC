using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TestMVC.Data;
using TestMVC.Repository;
using TestMVC.Services.ItemService;
using TestMVC.Services.UserService;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "TestMVC API", Version = "v1" });
    });

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseMySQL(builder.Configuration.GetConnectionString("MVCConnection") ?? string.Empty);
    });

    builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
           .AddRoles<IdentityRole>()
           .AddEntityFrameworkStores<ApplicationDbContext>();

    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    builder.Services.AddScoped<IItemService, ItemService>();
    builder.Services.AddScoped<IUserService, UserService>();
}

var app = builder.Build();
{
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseSwagger();
    app.UseCors();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestMVC API V1"));

    app.UseAuthentication(); // Ensure this is before UseAuthorization
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.MapRazorPages();
    app.Run();
}
