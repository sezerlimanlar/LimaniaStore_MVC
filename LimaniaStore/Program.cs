using Limania.DataAccess.Data;
using Limania.DataAccess.Repository;
using Limania.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Limania.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;
using Limania.DataAccess.DbInitializer;

namespace LimaniaStore
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


			builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));



			builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders(); ;
			builder.Services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = $"/Identity/Account/Login";
				options.LogoutPath = $"/Identity/Account/Logout";
				options.AccessDeniedPath = $"/Identity/Account/AccessDenied";

			});

			builder.Services.AddAuthentication().AddFacebook(option =>
			{
				option.AppId = "326411533645932";
				option.AppSecret = "a0d8efedcc3e0f6924a6b65034fabc55";
			});


			builder.Services.AddDistributedMemoryCache();
			builder.Services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(100);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
			});





			builder.Services.AddRazorPages();
			builder.Services.AddScoped<IDbInitializer, DbInitializer>();
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			builder.Services.AddScoped<IEmailSender, EmailSender>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
			app.UseRouting();
			app.UseAuthentication();
			app.UseSession();
			SeedDatabase();
			app.UseAuthorization();
			app.MapRazorPages();

			app.MapControllerRoute(
				name: "default",
				pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

			app.Run();
			void SeedDatabase()
			{
				using (var scope = app.Services.CreateScope())
				{
					var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
					dbInitializer.Initialize();
				}
			}
		}
	}
}
