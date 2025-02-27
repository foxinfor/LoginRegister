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

            //builder.Services.AddScoped<MailRepository>();
            builder.Services.AddScoped<GoodsRepository>();
            builder.Services.AddScoped<MessageRepository>();
            builder.Services.AddScoped<CategoryRepository>();

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

            await InitialiseCategories(context);
            await InitialiseGoods(context);
        }

        private static async Task InitialiseCategories(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Одежда" },
                    new Category { Name = "Обувь" }
                };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }
        }

        private static async Task InitialiseGoods(ApplicationDbContext context)
        {
            if (!context.Goods.Any())
            {
                var goodsList = new List<Goods>
                {
                    new Goods { Name = "Куртка", Count = 10, ImageSrc = "https://encrypted-tbn0.gstatic.com/shopping?q=tbn:ANd9GcRVQHyhlYKuD1Iye86vZ3uzqqFkSQsY9yAyug_0APjACX0RRDK6bYj1JeOtFAYln2U0BfQEL6cpfCb6wuufQOEtbYPTZ9wHffNmc0VT079hEEVYoTufwcOOoihfcCK0ujYXjIjtflQ&usqp=CAc", CategoryId = 1, Color = "Blue", Size = 45, Gender = "Мужской" },
                    new Goods { Name = "Шапка", Count = 20, ImageSrc = "data:image/jpeg;base64,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", CategoryId = 1, Color = "Black", Size = 43, Gender = "Мужской" },
                    new Goods { Name = "Кимоно", Count = 30, ImageSrc = "data:image/jpeg;base64,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", CategoryId = 1, Color = "White", Size = 40, Gender = "Унисекс" }
                };

                context.Goods.AddRange(goodsList);
                await context.SaveChangesAsync();
            }
        }
    }
}
