using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XieCheng.DataBase;
using XieCheng.DtoS;
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
            return dbContext.TouristRoutes.Include(t => t.TouristRoutePictures).FirstOrDefault(a => a.Id.Equals(touristRouteId));
        }

        public IEnumerable<TouristRoute> GetTouristRoutes(string keyword, string ratingOperator, int? ratingValue)
        {
            // Include VS join  连接两张表
            //return dbContext.TouristRoutes.Include(t => t.TouristRoutePictures);

            IQueryable<TouristRoute> result = dbContext.TouristRoutes.Include(t => t.TouristRoutePictures);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                result = result.Where(t => t.Title.Contains(keyword));
            }

            if (ratingValue >= 0)
            {
                switch (ratingOperator)
                {
                    case "largerThan":
                        result = result.Where(t => t.Rating > ratingValue);
                        break;
                    case "lessThan":
                        result = result.Where(t => t.Rating < ratingValue);
                        break;
                    default:
                        result = result.Where(t => t.Rating == ratingValue);
                        break;
                }
            }

            return result.ToList();
        }

        public bool TouristRouteExists(Guid touristRouteId)
        {
            return dbContext.TouristRoutes.Any(t => t.Id == touristRouteId);
        }

        public IEnumerable<TouristRoutePicture> GetTouristRoutePictures(Guid touristRouteId)
        {
            return dbContext.TouristRoutePictures.Where(t => t.TouristRouteId == touristRouteId).ToList();
        }

        public TouristRoutePicture GetPicture(int pictureId)
        {
            return dbContext.TouristRoutePictures.FirstOrDefault(p => p.Id == pictureId);
        }

        public void AddTouristRoute(TouristRoute touristRoute)
        {
            if (touristRoute == null)
            {
                throw new ArgumentNullException(nameof(TouristRoute));
            }

            dbContext.TouristRoutes.Add(touristRoute);
        }

        public void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture)
        {
            if (touristRouteId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(touristRouteId));
            }

            if (touristRoutePicture == null)
            {
                throw new ArgumentNullException(nameof(touristRoutePicture));
            }

            touristRoutePicture.TouristRouteId = touristRouteId;
            dbContext.TouristRoutePictures.Add(touristRoutePicture);
            dbContext.SaveChanges();
        }

        public void DeleteTouristRoute(TouristRoute touristRoute)
        {
            if (touristRoute == null)
            {
                throw new ArgumentNullException(nameof(touristRoute));
            }

            dbContext.TouristRoutes.Remove(touristRoute);
        }

        public void DeletePicture(TouristRoutePicture touristRoutePicture)
        {
            if (touristRoutePicture == null)
            {
                throw new ArgumentNullException(nameof(touristRoutePicture));
            }

            dbContext.TouristRoutePictures.Remove(touristRoutePicture);
        }

        public void DeleteTouristRoutes(IEnumerable<TouristRoute> touristRoutes)
        {
            if (touristRoutes == null)
            {
                throw new ArgumentNullException(nameof(touristRoutes));
            }

            dbContext.TouristRoutes.RemoveRange(touristRoutes);
        }

        public IEnumerable<TouristRoute> GetTouristRoutesByIDList(IEnumerable<Guid> ids)
        {
            return dbContext.TouristRoutes.Where(t => ids.Contains(t.Id)).ToList();
        }

        public bool Save()
        {
            return dbContext.SaveChanges() >= 0;
        }
    }
}
