using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XieCheng.DtoS;
using XieCheng.Models;

namespace XieCheng.Services
{
    public interface ITouristRouteRepository
    {
        IEnumerable<TouristRoute> GetTouristRoutes(string keyword, string ratingOperator, int? ratingValue);
        Task<IEnumerable<TouristRoute>> GetTouristRoutesAsync(string keyword, string ratingOperator, int? ratingValue);
        TouristRoute GetTouristRoute(Guid touristRouteId);
        Task<TouristRoute> GetTouristRouteAsync(Guid touristRouteId);
        bool TouristRouteExists(Guid touristRouteId);
        Task<bool> TouristRouteExistsAsync(Guid touristRouteId);
        IEnumerable<TouristRoutePicture> GetTouristRoutePictures(Guid touristRouteId);
        Task<IEnumerable<TouristRoutePicture>> GetTouristRoutePicturesAsync(Guid touristRouteId);
        TouristRoutePicture GetPicture(int pictureId);
        Task<TouristRoutePicture> GetPictureAsync(int pictureId);
        void AddTouristRoute(TouristRoute touristRoute);
        void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture);
        void DeleteTouristRoute(TouristRoute touristRoute);
        void DeleteTouristRoutes(IEnumerable<TouristRoute> touristRoutes);
        void DeletePicture(TouristRoutePicture picture);
        IEnumerable<TouristRoute> GetTouristRoutesByIDList(IEnumerable<Guid> touristRouteIds);
        Task<IEnumerable<TouristRoute>> GetTouristRoutesByIDListAsync(IEnumerable<Guid> touristRouteIds);
        bool Save();
        Task<bool> SaveAsync();
    }
}
