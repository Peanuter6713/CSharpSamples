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

        public async Task<IEnumerable<TouristRoute>> GetTouristRoutesAsync(string keyword, string ratingOperator, int? ratingValue,int pageSize, int pageNumber)
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

            // pagination
            // skip
            var skip = (pageNumber - 1) * pageSize;
            result = result.Skip(skip);
            result = result.Take(pageSize);

            return await result.ToListAsync();
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

        public async Task<ShoppingCart> GetShoppingCartByUserIdAsync(string userId)
        {
            return await dbContext.ShoppingCarts
                // ShoppingCart Table join ApplicationUser Table
                .Include(s => s.User)
                // ShoppingCart Table
                .Include(s => s.ShoppingCartItems)
                // LineItem Table
                .ThenInclude(line => line.TouristRoute) 
                .Where(s => s.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task CreateShoppingCartAsync(ShoppingCart shoppingCart)
        {
            await dbContext.ShoppingCarts.AddAsync(shoppingCart);
        }

        public async Task AddShoppingCartItemAsync(LineItem lineItem)
        {
            await dbContext.LineItems.AddAsync(lineItem);
        }

        public async Task<LineItem> GetShoppingCartItemByItemIdAsync(int lineItemId)
        {
            return await dbContext.LineItems.FirstOrDefaultAsync(li => li.Id == lineItemId);
        }

        public void DeleteShoppingCartItem(LineItem lineItem)
        {
            dbContext.LineItems.Remove(lineItem);
        }

        public bool Save()
        {
            return dbContext.SaveChanges() >= 0;
        }
        public async Task<bool> SaveAsync()
        {
            return await dbContext.SaveChangesAsync() >= 0;
        }

        public async Task<IEnumerable<LineItem>> GetShoppingCartsByIdListAsync(IEnumerable<int> ids)
        {
            return await dbContext.LineItems.Where(li => ids.Contains(li.Id)).ToListAsync();
        }

        public void DeleteShoppingCartItems(IEnumerable<LineItem> lineItems)
        {
            dbContext.LineItems.RemoveRange(lineItems);
        }

        public async Task AddOrderAsync(Order order)
        {
            await dbContext.AddAsync(order);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await dbContext.Orders.Where(o => o.UserId == userId).ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            //return await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
            return await dbContext.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.TouristRoute)
                .Where(o => o.Id == orderId).FirstOrDefaultAsync();
        }
    }
}
