using LoginRegister.Models;
using LoginRegister.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LoginRegister
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DBConnection");
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;

                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddScoped<MailRepository>();
            builder.Services.AddScoped<GoodsRepository>();
            builder.Services.AddScoped<MessageRepository>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                InitialiseAdmin(services).Wait();
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
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
        }

        private static async Task InitialiseAdmin(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var context = services.GetRequiredService<ApplicationDbContext>();

            var adminEmail = "admin@tut.by";
            var adminPassword = "qwQW0!";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if(adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "Admin",
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if(result.Succeeded)
                {
                    if (!await roleManager.RoleExistsAsync("Администратор"))
                    {
                        await roleManager.CreateAsync(new IdentityRole("Администратор"));
                    }
                    await userManager.AddToRoleAsync(adminUser, "Администратор");
                }
            }

            await InitialiseGoods(context);
        }

        private static async Task InitialiseGoods(ApplicationDbContext context)
        {
            if (!context.Goods.Any())
            {
                var goodsList = new List<Goods>
                {
                    new Goods { Name = "Куртка", Count = 10 },
                    new Goods { Name = "Шапка", Count = 20 },
                    new Goods { Name = "Кимоно", Count = 30 }
                };

                context.Goods.AddRange(goodsList);
                await context.SaveChangesAsync();
            }
        }
    }
}
