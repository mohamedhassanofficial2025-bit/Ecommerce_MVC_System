using EcommerceSystem.BLL;
using EcommerceSystem.DAL;
using EcommerceSystem.DAL.Data.Models;
using EcommerceSystem.DAL.Repositories.ProductRepository;
using EcommerceSystem.DAL.UnitOfWork;
using EcommerceSystem.PL.SystemRoles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EcommerceSystem.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var connectionString = builder.Configuration.GetConnectionString("EcommerceConnection")
                ?? throw new InvalidOperationException("Connection string 'EcommerceConnection' was not found.");

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                });
            });

            builder.Services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            builder.Services.Configure<IdentityOptions>(options => {
                options.Password.RequiredLength = 4;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
            });


            builder.Services.AddScoped<IProductManager, ProductManager>();
            builder.Services.AddScoped<ICategoryManager, CategoryManager>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();




            var app = builder.Build();

            if (builder.Configuration.GetValue<bool>("ApplyMigrationsOnStartup"))
            {
                ApplyDatabaseMigrations(app);
                SeedIdentityRoles(app);
                SeedDefaultUsers(app);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Register}/{id?}")
                .WithStaticAssets();

            app.Run();
        }

        private static void ApplyDatabaseMigrations(WebApplication app)
        {
            const int maxAttempts = 12;
            var logger = app.Services.GetRequiredService<ILogger<Program>>();

            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    using var scope = app.Services.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    dbContext.Database.Migrate();
                    logger.LogInformation("Database migrations applied successfully.");
                    return;
                }
                catch (Exception ex) when (attempt < maxAttempts)
                {
                    logger.LogWarning(
                        ex,
                        "Database is not ready yet. Retrying migration attempt {Attempt}/{MaxAttempts} in 5 seconds.",
                        attempt,
                        maxAttempts);

                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }
            }
        }

        private static void SeedIdentityRoles(WebApplication app)
        {
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            using var scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

            foreach (var roleName in new[] { SysRoles.AdminRole, SysRoles.UserRole })
            {
                if (roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    continue;
                }

                var result = roleManager.CreateAsync(new AppRole { Name = roleName }).GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    logger.LogInformation("Seeded Identity role '{RoleName}'.", roleName);
                    continue;
                }

                var errors = string.Join(", ", result.Errors.Select(error => error.Description));
                throw new InvalidOperationException($"Failed to seed Identity role '{roleName}': {errors}");
            }
        }

        private static void SeedDefaultUsers(WebApplication app)
        {
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            using var scope = app.Services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            SeedDefaultUser(app.Configuration, userManager, logger, "Admin", SysRoles.AdminRole);
            SeedDefaultUser(app.Configuration, userManager, logger, "User", SysRoles.UserRole);
        }

        private static void SeedDefaultUser(
            IConfiguration configuration,
            UserManager<AppUser> userManager,
            ILogger logger,
            string userSectionName,
            string roleName)
        {
            var userSection = configuration.GetSection($"DefaultUsers:{userSectionName}");
            var email = userSection["Email"];
            var password = userSection["Password"];

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                logger.LogInformation("Skipping default {UserSectionName} seed because email or password is not configured.", userSectionName);
                return;
            }

            var user = userManager.FindByEmailAsync(email).GetAwaiter().GetResult();
            if (user is null)
            {
                user = new AppUser
                {
                    FirstName = userSection["FirstName"] ?? userSectionName,
                    LastName = userSection["LastName"] ?? "User",
                    Email = email,
                    UserName = email,
                    EmailConfirmed = true
                };

                var createResult = userManager.CreateAsync(user, password).GetAwaiter().GetResult();
                if (!createResult.Succeeded)
                {
                    var errors = string.Join(", ", createResult.Errors.Select(error => error.Description));
                    throw new InvalidOperationException($"Failed to seed default user '{email}': {errors}");
                }

                logger.LogInformation("Seeded default {UserSectionName} user '{Email}'.", userSectionName, email);
            }
            else
            {
                var userUpdated = false;

                if (!user.EmailConfirmed)
                {
                    user.EmailConfirmed = true;
                    userUpdated = true;
                }

                if (!string.Equals(user.UserName, email, StringComparison.OrdinalIgnoreCase))
                {
                    user.UserName = email;
                    userUpdated = true;
                }

                if (userUpdated)
                {
                    var updateResult = userManager.UpdateAsync(user).GetAwaiter().GetResult();
                    if (!updateResult.Succeeded)
                    {
                        var errors = string.Join(", ", updateResult.Errors.Select(error => error.Description));
                        throw new InvalidOperationException($"Failed to update default user '{email}': {errors}");
                    }
                }
            }

            if (!userManager.CheckPasswordAsync(user, password).GetAwaiter().GetResult())
            {
                var removePasswordResult = userManager.RemovePasswordAsync(user).GetAwaiter().GetResult();
                if (!removePasswordResult.Succeeded)
                {
                    var errors = string.Join(", ", removePasswordResult.Errors.Select(error => error.Description));
                    throw new InvalidOperationException($"Failed to reset password for default user '{email}': {errors}");
                }

                var addPasswordResult = userManager.AddPasswordAsync(user, password).GetAwaiter().GetResult();
                if (!addPasswordResult.Succeeded)
                {
                    var errors = string.Join(", ", addPasswordResult.Errors.Select(error => error.Description));
                    throw new InvalidOperationException($"Failed to set password for default user '{email}': {errors}");
                }

                logger.LogInformation("Reset password for default user '{Email}'.", email);
            }

            if (userManager.IsInRoleAsync(user, roleName).GetAwaiter().GetResult())
            {
                return;
            }

            var roleResult = userManager.AddToRoleAsync(user, roleName).GetAwaiter().GetResult();
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(error => error.Description));
                throw new InvalidOperationException($"Failed to add default user '{email}' to role '{roleName}': {errors}");
            }

            logger.LogInformation("Assigned default user '{Email}' to role '{RoleName}'.", email, roleName);
        }
    }
}
