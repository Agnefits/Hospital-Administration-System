using Microsoft.AspNetCore.Identity;

namespace Hospital_Administration_System;

public static class DependencyInjection
{
    public static IServiceCollection AddDependency(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddAuthConfig();
        services.AddControllersWithViews();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEmailRepository, EmailService>();
        services.AddScoped<IAuthRepository, AuthService>();
        services.AddLogging();

        return services;
    }

    private static IServiceCollection AddAuthConfig(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
        }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Auth/Login";
            options.AccessDeniedPath = "/Auth/AccessDenied";
        });
        return services;
    }

    public static async Task SeedRolesAndAdminAsync(this IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        string[] roles = { "Admin", "Doctor", "Nurse", "Pharmacist", "Patient" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        var adminUser = await userManager.FindByNameAsync("admin@email.com");

        if (adminUser == null)
        {
            var user = new User
            {
                Email = "admin@email.com",
                UserName = "admin@email.com",
                CreatedAt = DateTime.UtcNow,
                EmailConfirmed = true,
                AdditionalData = null,
                Deleted = false
            };
            var result = await userManager.CreateAsync(user, "admin123");

            if (result.Succeeded)
                await context.AddAsync(new Admin
                {
                    UserID = user.Id,
                    FullName = "Admin",
                    ContactNumber = "01123456789",
                    AdditionalData = null,
                    Deleted = false
                });
            await context.SaveChangesAsync();

            if (result.Succeeded)
                await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}
