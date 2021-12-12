using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XieCheng.DtoS;
using XieCheng.Services;

namespace XieCheng.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristRoutesController : ControllerBase
    {
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public TouristRoutesController(ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetTouristRoutes()
        {
            var routes = _touristRouteRepository.GetTouristRoutes();
            var routesDtos = _mapper.Map<IEnumerable<TouristRouteDto>>(routes);

            return Ok(routesDtos);
        }

        [HttpGet("{touristId:Guid}")]
        public IActionResult GetTouristRouteById(Guid touristId)
        {
            var routes = _touristRouteRepository.GetTouristRoute(touristId);

            //var touristRouteDto = new TouristRouteDto()
            //{
            //    Id = routes.Id,
            //    Title = routes.Title,
            //    Description = routes.Description,
            //    Price = routes.OriginalPrice * (decimal)routes.DiscountPresent,
            //    CreateTime = routes.CreateTime,
            //    UpdateTime = routes.UpdateTime,
            //    DepartureTime = routes.DepartureTime,
            //    Features = routes.Features,
            //    Fees = routes.Fees,
            //    Notes = routes.Notes,
            //    Rating = routes.Rating,
            //    TravelDays = routes.TravelDays.ToString(),
            //    TripType = routes.TripType.ToString(),
            //    DepartureCity = routes.DepartureCity.ToString()
            //};
            var touristRouteDto = _mapper.Map<TouristRouteDto>(routes);

            return Ok(touristRouteDto);
        }

    }
}
