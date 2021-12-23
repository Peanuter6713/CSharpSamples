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
    public class AppDbContext : IdentityDbContext<IdentityUser> //DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<TouristRoute> TouristRoutes { get; set; }
        public DbSet<TouristRoutePicture> TouristRoutePictures { get; set; }

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

            base.OnModelCreating(modelBuilder);
        }

    }
}
