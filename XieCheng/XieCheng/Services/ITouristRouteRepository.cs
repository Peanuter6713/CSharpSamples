using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XieCheng.Models;

namespace XieCheng.Services
{
    public interface ITouristRouteRepository
    {
        IEnumerable<TouristRoute> GetTouristRoutes();
        TouristRoute GetTouristRoute(Guid touristRouteId);
        bool TouristRouteExists(Guid touristRouteId);
        IEnumerable<TouristRoutePicture> GetTouristRoutePictures(Guid touristRouteId);
    }
}
