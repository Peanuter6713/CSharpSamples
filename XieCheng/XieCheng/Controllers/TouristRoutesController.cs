using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XieCheng.DtoS;
using XieCheng.Models;
using XieCheng.ResourceParameters;
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
        public IActionResult GetTouristRoutes(
            //string keyword
            [FromQuery] TouristRouteResourceParameters parameters
            )
        {
            var routes = _touristRouteRepository.GetTouristRoutes(parameters.Keyword, parameters.RatingOperator, parameters.RatingValue);
            var routesDtos = _mapper.Map<IEnumerable<TouristRouteDto>>(routes);

            return Ok(routesDtos);
        }

        [HttpGet("{touristId:Guid}", Name = "GetTouristRouteById")]
        public IActionResult GetTouristRouteById(Guid touristId)
        {
            var routes = _touristRouteRepository.GetTouristRoute(touristId);

            var touristRouteDto = _mapper.Map<TouristRouteDto>(routes);

            return Ok(touristRouteDto);
        }

        [HttpPost]
        public IActionResult CreateTouristRoute([FromBody] TouristRouteForCreationDto touristRouteForCreationDto)
        {
            var model = _mapper.Map<TouristRoute>(touristRouteForCreationDto);

            _touristRouteRepository.AddTouristRoute(model);
            _touristRouteRepository.Save();

            var returnModel = _mapper.Map<TouristRouteDto>(model);

            return CreatedAtRoute("GetTouristRouteById", new { touristId = returnModel.Id }, returnModel);
        }


        [HttpPut("{touristRouteId}")]
        public IActionResult UpdateTouristRoute([FromRoute] Guid touristRouteId, [FromBody] TouristRouteForUpdateDto touristRouteForUpdateDto)
        {
            if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
            {
                return NotFound("旅游路线不存在");
            }

            var touristRouteFromRepo = _touristRouteRepository.GetTouristRoute(touristRouteId);
            //1. 映射DTO  2.更新DTO 3.映射model
            _mapper.Map(touristRouteForUpdateDto, touristRouteFromRepo);
            _touristRouteRepository.Save();

            return NoContent();
        }

    }
}
