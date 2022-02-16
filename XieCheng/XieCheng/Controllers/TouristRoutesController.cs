using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
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
        private readonly IUrlHelper _urlHepler;
        private readonly IPropertyMappingService _propertyMappingService;

        public TouristRoutesController(ITouristRouteRepository touristRouteRepository, IMapper mapper,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor,
            IPropertyMappingService propertyMappingService)
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
            _urlHepler = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _propertyMappingService = propertyMappingService;
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

        private string GenerateTouristRouteResourceURL(
            TouristRouteResourceParameters parameters,
            PaginationResourceParameters paginationParameters,
            ResourceUriType type)
        {
            return type switch
            {
                ResourceUriType.PreviousPage => _urlHepler.Link("GetTouristRoutes",
                new
                {
                    fields = parameters.Fields,
                    orderBy = parameters.OrderBy,
                    keyword = parameters.Keyword,
                    rating = parameters.Rating,
                    pageNumber = paginationParameters.PageNumber,
                    pageSize = paginationParameters.PageSize
                }),
                ResourceUriType.NextPage => _urlHepler.Link("GetTouristRoutes",
                new
                {
                    fields = parameters.Fields,
                    orderBy = parameters.OrderBy,
                    keyword = parameters.Keyword,
                    rating = parameters.Rating,
                    pageNumber = paginationParameters.PageNumber + 1,
                    pageSize = paginationParameters.PageSize
                }),
                _ => _urlHepler.Link("GetTouristRoutes",
                new
                {
                    fields = parameters.Fields,
                    orderBy = parameters.OrderBy,
                    keyword = parameters.Keyword,
                    rating = parameters.Rating,
                    pageNumber = paginationParameters.PageNumber,
                    pageSize = paginationParameters.PageSize
                })
            };
        }

        #region Asynchronic ActionResult
        [HttpGet(Name = "GetTouristRoutes")]
        [HttpHead]
        public async Task<IActionResult> GetTouristRoutes(
            //string keyword
            [FromQuery] TouristRouteResourceParameters parameters,
            [FromQuery] PaginationResourceParameters paginationParameters
            )
        {
            if (!_propertyMappingService.IsMappingExists<TouristRouteDto, TouristRoute>(parameters.OrderBy))
            {
                return BadRequest("请输入正确的排序参数");
            }

            if (!_propertyMappingService.IsPropertiesExists<TouristRouteDto>(parameters.Fields))
            {
                return BadRequest("请输入正确的塑形参数");
            }

            var routes = await _touristRouteRepository.GetTouristRoutesAsync(
                parameters.Keyword,
                parameters.RatingOperator,
                parameters.RatingValue,
                paginationParameters.PageSize,
                paginationParameters.PageNumber,
                parameters.OrderBy
                );
            var routesDtos = _mapper.Map<IEnumerable<TouristRouteDto>>(routes);

            var previousPageLink = routes.HasPrievous ? GenerateTouristRouteResourceURL(parameters, paginationParameters, ResourceUriType.PreviousPage) : null;

            var nextPageLink = routes.HasNext ? GenerateTouristRouteResourceURL(parameters, paginationParameters, ResourceUriType.NextPage) : null;

            // x-pagination
            var paginationMetaData = new
            {
                previousPageLink,
                nextPageLink,
                totalCount = routes.TotalCount,
                pageSize = routes.PageSize,
                currentPage = routes.CurrentPage,
                totalPages = routes.TotalPages
            };

            Response.Headers.Add("x-pagination", JsonConvert.SerializeObject(paginationMetaData));

            //return Ok(routesDtos.ShapeData(parameters.Fields));
            var shapeDtoList = routesDtos.ShapeData(parameters.Fields);

            var linkDto = CreateLinksForTouristRouteList(parameters, paginationParameters);
            var shapedDtoWithLinkList = shapeDtoList.Select(t =>
           {
               var touristRouteDictionary = t as IDictionary<string, object>;
               var links = CreateLinkForTouristRoute((Guid)touristRouteDictionary["Id"], null);
               touristRouteDictionary.Add("links", links);

               return touristRouteDictionary;
           });

            var result = new
            {
                value = shapedDtoWithLinkList,
                links = linkDto
            };

            return Ok(result);
        }

        [HttpGet("{touristId:Guid}", Name = "GetTouristRouteById")]
        public async Task<IActionResult> GetTouristRouteById(Guid touristId, string fields)
        {
            var routes = await _touristRouteRepository.GetTouristRouteAsync(touristId);

            var touristRouteDto = _mapper.Map<TouristRouteDto>(routes);

            //return Ok(touristRouteDto.ShapeData(fields));

            var linkDtos = CreateLinkForTouristRoute(touristId, fields);

            var result = touristRouteDto.ShapeData(fields) as IDictionary<string, object>;

            result.Add("links", linkDtos);

            return Ok(result);
        }

        private IEnumerable<LinkDto> CreateLinkForTouristRoute(Guid touristRouteId, string fields)
        {
            var links = new List<LinkDto>();

            links.Add(
                new LinkDto(
                    Url.Link("GetTouristRouteById", new { touristRouteId, fields }),
                    "self",
                    "GET"
                    )
                );

            // Update
            links.Add(
                new LinkDto(
                    Url.Link("UpdateTouristRoute", new { touristRouteId }),
                    "update",
                    "PUT"
                    )
                );

            // partially update
            links.Add(
                new LinkDto(
                    Url.Link("PariallyUpdateTouristRoute", new { touristRouteId }),
                    "partially_update",
                    "PATCH"
                    )
                );

            // delete
            links.Add(
                new LinkDto(
                    Url.Link("DeleteTouristRoute", new { touristRouteId }),
                    "delete",
                    "DELETE"
                    )
                );

            // get picture of current tourist route
            links.Add(
                new LinkDto(
                    Url.Link("GetPictureListForTouristRoute", new { touristRouteId }),
                    "get_pictures",
                    "GET"
                    )
                );

            // add new picture
            links.Add(
                new LinkDto(
                    Url.Link("CreateTouristPicture", new { touristRouteId }),
                    "create_pictures",
                    "POST"
                    )
                );

            return links;
        }

        [HttpPost(Name = "CreateTouristRoute")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTouristRoute([FromBody] TouristRouteForCreationDto touristRouteForCreationDto)
        {
            var model = _mapper.Map<TouristRoute>(touristRouteForCreationDto);

            _touristRouteRepository.AddTouristRoute(model);
            await _touristRouteRepository.SaveAsync();

            var returnModel = _mapper.Map<TouristRouteDto>(model);

            var links = CreateLinkForTouristRoute(model.Id, null);

            var result = returnModel.ShapeData(null) as IDictionary<string, object>;

            result.Add("links", links);

            // return CreatedAtRoute("GetTouristRouteById", new { touristId = returnModel.Id }, returnModel);
            return CreatedAtRoute("GetTouristRouteById", new { touristId = result["Id"] }, result);
        }


        [HttpPut("{touristRouteId}", Name = "UpdateTouristRoute")]
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

        [HttpPatch("{touristRouteId}", Name = "PariallyUpdateTouristRoute")]
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

        [HttpDelete("{touristRouteId}", Name = "DeleteTouristRoute")]
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

        private IEnumerable<LinkDto> CreateLinksForTouristRouteList(
            TouristRouteResourceParameters parameters,
            PaginationResourceParameters parameters2)
        {
            var links = new List<LinkDto>();

            // add self link
            links.Add(
                new LinkDto(
                    GenerateTouristRouteResourceURL(parameters, parameters2, ResourceUriType.CurrentPage),
                    "self",
                    "GET"
                    )
                );

            // api/touristRoutes
            links.Add(
                new LinkDto(
                    Url.Link("CreateTouristRoute", null),
                    "create_tourist_route",
                    "POST"
                    )
                );

            return links;
        }

    }
}
