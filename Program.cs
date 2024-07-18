using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TestMVC.Data;
using TestMVC.Repository;
using TestMVC.Services.ItemService;
using TestMVC.Services.UserItemService;
using TestMVC.Services.UserService;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "MarketplaceMVC API", Version = "v1" });
    });

    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseMySQL(builder.Configuration.GetConnectionString("MVCConnection") ?? string.Empty);
    });

    builder.Services.AddDefaultIdentity<User>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = false;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>();

    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    builder.Services.AddScoped<IItemService, ItemService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IUserItemService, UserItemService>();
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
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MarketplaceMVC API V1"));

    app.UseAuthentication(); // Ensure this is before UseAuthorization
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.MapRazorPages();

    using (var roleScope = app.Services.CreateScope())
    {
        var roleManager = roleScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roleNames = ["Admin", "User", "PremiumUser"];
        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    using (var adminScope = app.Services.CreateScope())
    {
        var userManager = adminScope.ServiceProvider.GetRequiredService<UserManager<User>>();

        const string email = "admin@admin.com";
        const string password = "Admin123!";
        await Task.Run(async () =>
        {
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new User
                {
                    Name = "Admin",
                    Surname = "0",
                    Email = email,
                    UserName = "admin"
                };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        });
    }

    app.Run();
}