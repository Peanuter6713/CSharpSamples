using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XieCheng.DtoS;
using XieCheng.Helper;
using XieCheng.Models;
using XieCheng.ResourceParameters;
using XieCheng.Services;

namespace XieCheng.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TouristRoutesController : ControllerBase
    {
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public TouristRoutesController(ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }

        #region Synchronic ActionResult
        //[HttpGet]
        //public IActionResult GetTouristRoutes(
        //    //string keyword
        //    [FromQuery] TouristRouteResourceParameters parameters
        //    )
        //{
        //    var routes = _touristRouteRepository.GetTouristRoutes(parameters.Keyword, parameters.RatingOperator, parameters.RatingValue);
        //    var routesDtos = _mapper.Map<IEnumerable<TouristRouteDto>>(routes);

        //    return Ok(routesDtos);
        //}

        //[HttpGet("{touristId:Guid}", Name = "GetTouristRouteById")]
        //public IActionResult GetTouristRouteById(Guid touristId)
        //{
        //    var routes = _touristRouteRepository.GetTouristRoute(touristId);

        //    var touristRouteDto = _mapper.Map<TouristRouteDto>(routes);

        //    return Ok(touristRouteDto);
        //}

        //[HttpPost]
        //public IActionResult CreateTouristRoute([FromBody] TouristRouteForCreationDto touristRouteForCreationDto)
        //{
        //    var model = _mapper.Map<TouristRoute>(touristRouteForCreationDto);

        //    _touristRouteRepository.AddTouristRoute(model);
        //    _touristRouteRepository.Save();

        //    var returnModel = _mapper.Map<TouristRouteDto>(model);

        //    return CreatedAtRoute("GetTouristRouteById", new { touristId = returnModel.Id }, returnModel);
        //}


        //[HttpPut("{touristRouteId}")]
        //public IActionResult UpdateTouristRoute([FromRoute] Guid touristRouteId, [FromBody] TouristRouteForUpdateDto touristRouteForUpdateDto)
        //{
        //    if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
        //    {
        //        return NotFound("旅游路线不存在");
        //    }

        //    var touristRouteFromRepo = _touristRouteRepository.GetTouristRoute(touristRouteId);
        //    //1. 映射DTO  2.更新DTO 3.映射model
        //    _mapper.Map(touristRouteForUpdateDto, touristRouteFromRepo);
        //    _touristRouteRepository.Save();

        //    return NoContent();
        //}

        //[HttpPatch("{touristRouteId}")]
        //public IActionResult PariallyUpdateTouristRoute([FromRoute]Guid touristRouteId, [FromBody]JsonPatchDocument<TouristRouteForUpdateDto> patchDocument)
        //{
        //    if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
        //    {
        //        return NotFound("旅游路线不存在");
        //    }

        //    var touristRouteForRepo = _touristRouteRepository.GetTouristRoute(touristRouteId);
        //    var touristRouteToPatch = _mapper.Map<TouristRouteForUpdateDto>(touristRouteForRepo);
        //    patchDocument.ApplyTo(touristRouteToPatch, ModelState);

        //    if (!TryValidateModel(touristRouteToPatch))
        //    {
        //        return ValidationProblem(ModelState);
        //    }

        //    _mapper.Map(touristRouteToPatch, touristRouteForRepo);
        //    _touristRouteRepository.Save();

        //    return NoContent();
        //}

        //[HttpDelete("{touristRouteId}")]
        //public IActionResult DeleteTouristRoute([FromRoute]Guid touristRouteId)
        //{
        //    if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
        //    {
        //        return NotFound("旅游路线不存在");
        //    }

        //    var touristRoute = _touristRouteRepository.GetTouristRoute(touristRouteId);
        //    _touristRouteRepository.DeleteTouristRoute(touristRoute);
        //    _touristRouteRepository.Save();

        //    return NoContent(); 
        //}

        //[HttpDelete("({ids})")]
        //public IActionResult DeleteByIDs([ModelBinder(BinderType =typeof(ArrayModelBinder))][FromRoute] IEnumerable<Guid> ids)
        //{
        //    if (ids == null)
        //    {
        //        return BadRequest();
        //    }

        //    var touristRoutes = _touristRouteRepository.GetTouristRoutesByIDList(ids);
        //    _touristRouteRepository.DeleteTouristRoutes(touristRoutes);
        //    _touristRouteRepository.Save();

        //    return NoContent();
        //}
        #endregion

        #region Asynchronic ActionResult
        [HttpGet]
        public async Task<IActionResult> GetTouristRoutes(
            //string keyword
            [FromQuery] TouristRouteResourceParameters parameters
            )
        {
            var routes = await _touristRouteRepository.GetTouristRoutesAsync(
                parameters.Keyword,
                parameters.RatingOperator,
                parameters.RatingValue,
                parameters.PageSize,
                parameters.PageNumber
                );
            var routesDtos = _mapper.Map<IEnumerable<TouristRouteDto>>(routes);

            return Ok(routesDtos);
        }

        [HttpGet("{touristId:Guid}", Name = "GetTouristRouteById")]
        public async Task<IActionResult> GetTouristRouteById(Guid touristId)
        {
            var routes = await _touristRouteRepository.GetTouristRouteAsync(touristId);

            var touristRouteDto = _mapper.Map<TouristRouteDto>(routes);

            return Ok(touristRouteDto);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTouristRoute([FromBody] TouristRouteForCreationDto touristRouteForCreationDto)
        {
            var model = _mapper.Map<TouristRoute>(touristRouteForCreationDto);

            _touristRouteRepository.AddTouristRoute(model);
            await _touristRouteRepository.SaveAsync();

            var returnModel = _mapper.Map<TouristRouteDto>(model);

            return CreatedAtRoute("GetTouristRouteById", new { touristId = returnModel.Id }, returnModel);
        }


        [HttpPut("{touristRouteId}")]
        public async Task<IActionResult> UpdateTouristRoute([FromRoute] Guid touristRouteId, [FromBody] TouristRouteForUpdateDto touristRouteForUpdateDto)
        {
            if (!await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId))
            {
                return NotFound("旅游路线不存在");
            }

            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            //1. 映射DTO  2.更新DTO 3.映射model
            _mapper.Map(touristRouteForUpdateDto, touristRouteFromRepo);
            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{touristRouteId}")]
        public async Task<IActionResult> PariallyUpdateTouristRoute([FromRoute] Guid touristRouteId, [FromBody] JsonPatchDocument<TouristRouteForUpdateDto> patchDocument)
        {
            if (!await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId))
            {
                return NotFound("旅游路线不存在");
            }

            var touristRouteForRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            var touristRouteToPatch = _mapper.Map<TouristRouteForUpdateDto>(touristRouteForRepo);
            patchDocument.ApplyTo(touristRouteToPatch, ModelState);

            if (!TryValidateModel(touristRouteToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(touristRouteToPatch, touristRouteForRepo);
            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{touristRouteId}")]
        public async Task<IActionResult> DeleteTouristRoute([FromRoute] Guid touristRouteId)
        {
            if (!await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId))
            {
                return NotFound("旅游路线不存在");
            }

            var touristRoute = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            _touristRouteRepository.DeleteTouristRoute(touristRoute);
            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }

        [HttpDelete("({ids})")]
        public async Task<IActionResult> DeleteByIDs([ModelBinder(BinderType = typeof(ArrayModelBinder))][FromRoute] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var touristRoutes = await _touristRouteRepository.GetTouristRoutesByIDListAsync(ids);
            _touristRouteRepository.DeleteTouristRoutes(touristRoutes);
            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }
        #endregion

    }
}
