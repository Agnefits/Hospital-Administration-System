using Hospital_Administration_System.Data;
using Hospital_Administration_System.Models;
using Hospital_Administration_System.Repository;
using Hospital_Administration_System.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Hospital_Administration_System
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<PatientService>();
            builder.Services.AddScoped<DoctorService>();
            builder.Services.AddScoped<NurseService>();
            builder.Services.AddScoped<PharmacistService>();
            builder.Services.AddScoped<ReservationService>();
            builder.Services.AddScoped<BillingService>();
            builder.Services.AddScoped<PrescriptionService>();
            builder.Services.AddScoped<LogService>();
            builder.Services.AddScoped<MedicalRecordService>();

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
                options.AccessDeniedPath = "/Auth/AccessDenied";
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();

                SeedRolesAndAdminAsync(services).Wait();
            }

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        async static Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
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

            var adminUser = await userManager.FindByNameAsync("admin");

            if (adminUser == null)
            {
                var user = new User
                {
                    UserName = "admin",
                    CreatedAt = DateTime.UtcNow,
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
}
