using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using XieCheng.Models;

namespace XieCheng.DataBase
{
    public class AppDbContext : IdentityDbContext<ApplicationUser> //DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<TouristRoute> TouristRoutes { get; set; }
        public DbSet<TouristRoutePicture> TouristRoutePictures { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
        public DbSet<Order> Orders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<TouristRoute>().HasData(new TouristRoute()
            //{
            //    Id = Guid.NewGuid(),
            //    Title = "I'm titile",
            //    Description = "desss",
            //    DiscountPresent = 0,
            //    CreateTime = DateTime.Now
            //}) ;
            var touristJsonData = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Database/touristRoutesMockData.json");

            IList<TouristRoute> touristRoutes = JsonConvert.DeserializeObject<IList<TouristRoute>>(touristJsonData);
            modelBuilder.Entity<TouristRoute>().HasData(touristRoutes);

            var touristPicJsonData = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Database/touristRoutePicturesMockData.json");

            IList<TouristRoutePicture> touristPictures = JsonConvert.DeserializeObject<IList<TouristRoutePicture>>(touristPicJsonData);
            modelBuilder.Entity<TouristRoutePicture>().HasData(touristPictures);

            // 初始化用户与角色的种子数据
            // 1. 更新用户与角色的外键
            modelBuilder.Entity<ApplicationUser>(u =>
                // 一对多的关系
                u.HasMany(x => x.UserRoles)
                // 映射外键
                .WithOne().HasForeignKey(ur => ur.UserId).IsRequired()
            );

            // 2. 添加管理员角色
            var adminRoleId = "39996F34-013C-4FC6-B1B3-0C1036C47112";
            modelBuilder.Entity<IdentityRole>().HasData(
                    new IdentityRole()
                    {
                        Id = adminRoleId,
                        Name = "Admin",
                        NormalizedName = "Admin".ToUpper()
                    }
                );

            // 3. 添加用户
            var adminUserId = "90184155-dee0-40c9-bb1e-b5ed07afc04e";
            ApplicationUser adminUser = new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin@fakexiecheng.com",
                NormalizedUserName = "admin@fakexiecheng.com".ToUpper(),
                Email = "admin@fakexiecheng.com",
                NormalizedEmail = "admin@fakexiecheng.com".ToUpper(),
                TwoFactorEnabled = false,
                EmailConfirmed = true,
                PhoneNumber = "123456789",
                PhoneNumberConfirmed = false
            };
            PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = ph.HashPassword(adminUser, "Fake123$");
            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

            // 4. 给用户加入管理员角色
            modelBuilder.Entity<IdentityUserRole<string>>()
                    .HasData(new IdentityUserRole<string>()
                    {
                        RoleId = adminRoleId,
                        UserId = adminUserId
                    });
            base.OnModelCreating(modelBuilder);
        }

    }
}
