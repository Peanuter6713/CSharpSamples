using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XieCheng.DataBase;
using XieCheng.Models;

namespace XieCheng.Services
{
    public class TouristRouteRepository : ITouristRouteRepository
    {
        private readonly AppDbContext dbContext;

        public TouristRouteRepository(AppDbContext context)
        {
            dbContext = context;
        }


        public TouristRoute GetTouristRoute(Guid touristRouteId)
        {
            return dbContext.TouristRoutes.FirstOrDefault(a => a.Id.Equals(touristRouteId));
        }

        public IEnumerable<TouristRoutePicture> GetTouristRoutePictures(Guid touristRouteId)
        {
            return dbContext.TouristRoutePictures.Where(t => t.TouristRouteId == touristRouteId).ToList();
        }

        public IEnumerable<TouristRoute> GetTouristRoutes()
        {
            return dbContext.TouristRoutes;
        }

        public bool TouristRouteExists(Guid touristRouteId)
        {
            return dbContext.TouristRoutes.Any(t => t.Id == touristRouteId);
        }

    }
}
