using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestMVC.Filters;
using TestMVC.Models.Entity;
using TestMVC.Repository;
using TestMVC.Services.CartService;
using TestMVC.Services.ItemService;
using TestMVC.Services.NotificationService;
using TestMVC.Services.PurchaseService;
using TestMVC.Services.UserItemService;
using TestMVC.Services.UserService;

var builder = WebApplication.CreateBuilder(args);
{
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

    builder.Services.AddControllersWithViews(options => { options.Filters.Add<ModelFilter>(); });


    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.LoginPath = "/Identity/Account/Login";
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        options.SlidingExpiration = true;
    });

    builder.Services.AddRazorPages();
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    builder.Services.AddScoped<IItemService, ItemService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IUserItemService, UserItemService>();
    builder.Services.AddScoped<ICartService, CartService>();
    builder.Services.AddScoped<IPurchaseService, PurchaseService>();
    builder.Services.AddScoped<INotificationService, NotificationService>();
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
    app.UseCors();
    app.UseAuthentication(); // Ensure this is before UseAuthorization
    app.UseAuthorization();
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.MapRazorPages();

    using (var roleScope = app.Services.CreateScope())
    {
        var roleManager = roleScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roleNames = ["Admin", "BasicUser", "PremiumUser"];
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