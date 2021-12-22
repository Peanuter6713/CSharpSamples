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

        public async Task<TouristRoute> GetTouristRouteAsync(Guid touristRouteId)
        {
            return await dbContext.TouristRoutes.Include(t => t.TouristRoutePictures).FirstOrDefaultAsync(a => a.Id.Equals(touristRouteId));
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

        public async Task<IEnumerable<TouristRoute>> GetTouristRoutesAsync(string keyword, string ratingOperator, int? ratingValue)
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

            return await result.ToListAsync();
        }

        public bool TouristRouteExists(Guid touristRouteId)
        {
            return dbContext.TouristRoutes.Any(t => t.Id == touristRouteId);
        }

        public async Task<bool> TouristRouteExistsAsync(Guid touristRouteId)
        {
            return await dbContext.TouristRoutes.AnyAsync(t => t.Id == touristRouteId);
        }

        public IEnumerable<TouristRoutePicture> GetTouristRoutePictures(Guid touristRouteId)
        {
            return dbContext.TouristRoutePictures.Where(t => t.TouristRouteId == touristRouteId).ToList();
        }

        public async Task<IEnumerable<TouristRoutePicture>> GetTouristRoutePicturesAsync(Guid touristRouteId)
        {
            return await dbContext.TouristRoutePictures.Where(t => t.TouristRouteId == touristRouteId).ToListAsync();
        }

        public TouristRoutePicture GetPicture(int pictureId)
        {
            return dbContext.TouristRoutePictures.FirstOrDefault(p => p.Id == pictureId);
        }

        public async Task<TouristRoutePicture> GetPictureAsync(int pictureId)
        {
            return await dbContext.TouristRoutePictures.FirstOrDefaultAsync(p => p.Id == pictureId);
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

        public async Task<IEnumerable<TouristRoute>> GetTouristRoutesByIDListAsync(IEnumerable<Guid> ids)
        {
            return await dbContext.TouristRoutes.Where(t => ids.Contains(t.Id)).ToListAsync();
        }

        public bool Save()
        {
            return dbContext.SaveChanges() >= 0;
        }
        public async Task<bool> SaveAsync()
        {
            return await dbContext.SaveChangesAsync() >= 0;
        }

    }
}
