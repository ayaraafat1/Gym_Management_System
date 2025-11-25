
using GymManagementBLL.BusinessServices.Implementation;
using GymManagementBLL.BusinessServices.Interfaces;
using GymManagementBLL.Mapping;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Data.SeedData;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Implementation;
using GymManagementDAL.Repositories.Interfaces;
using GymManagementDAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace GymManagementPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region DI Registration

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            builder.Services.AddScoped(typeof(ISessionRepository), typeof(SessionRepository));
            builder.Services.AddScoped<IAnalyticService,AnalyticService>();
            builder.Services.AddScoped<IMemberService,MemberService>();
            builder.Services.AddScoped<IPlanService,PlanService>();
            builder.Services.AddScoped<ISessionService,SessionService>();
            builder.Services.AddScoped<ITrainerService,TrainerService>();
            builder.Services.AddAutoMapper(X => X.AddProfile(new MappingProfile()));
            
            #endregion

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();

            var pendingMigrations = dbContext.Database.GetPendingMigrations();

            if(pendingMigrations?.Any()??false)
                dbContext.Database.Migrate();   

            GymDbContextSeeding.SeedData(dbContext);

            #region Configure PipeLine [Middlewares]

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets(); 
            #endregion

            app.Run();
        }
    }
}
