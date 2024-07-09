using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TestMVC.Repository;
using TestMVC.Services.DataService;
using TestMVC.Services.UserService;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllersWithViews();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "TestMVC API", Version = "v1" });
    });

    builder.Services.AddDbContext<Context>(options =>
    {
        options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty);
    });

    builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    builder.Services.AddScoped<IDataService, DataService>();
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
    app.UseAuthorization();
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}