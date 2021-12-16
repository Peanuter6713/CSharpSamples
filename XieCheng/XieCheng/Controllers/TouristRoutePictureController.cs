using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XieCheng.DtoS;
using XieCheng.Models;
using XieCheng.Services;

namespace XieCheng.Controllers
{
    [Route("api/touristRoutes/{touristRouteId}/pictures")]
    [ApiController]
    public class TouristRoutePictureController : ControllerBase
    {
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public TouristRoutePictureController(ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {
            _touristRouteRepository = touristRouteRepository ?? throw new ArgumentNullException(nameof(touristRouteRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public IActionResult GetPictureListForTouristRoute(Guid touristRouteId)
        {
            if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
            {
                return NotFound("路线不存在");
            }

            var pictures = _touristRouteRepository.GetTouristRoutePictures(touristRouteId);

            if (pictures == null || pictures.Count() == 0)
            {
                return NotFound("照片不存在");
            }

            return Ok(_mapper.Map<IEnumerable<TouristRoutePictureDto>>(pictures));
        }

        [HttpGet("{pictureId}", Name = "GetPicture")]
        public IActionResult GetPicture(Guid touristRouteId, int pictureId)
        {
            if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
            {
                return NotFound("路线不存在");
            }

            var picture = _touristRouteRepository.GetPicture(pictureId);

            if (picture == null)
            {
                return NotFound("Picture is not exist.");
            }

            return Ok(_mapper.Map<TouristRoutePictureDto>(picture));
        }


        [HttpPost]
        public IActionResult CreateTouristPicture([FromRoute] Guid touristRouteId, [FromBody] TouristRoutePictureForCreationDto touristPictureForCreationDto)
        {
            if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
            {
                return NotFound("旅游路线不存在！");
            }

            var pictureModel = _mapper.Map<TouristRoutePicture>(touristPictureForCreationDto);
            _touristRouteRepository.AddTouristRoutePicture(touristRouteId, pictureModel);
            _touristRouteRepository.Save();

            var pictureToReturn = _mapper.Map<TouristRoutePictureDto>(pictureModel);

            return CreatedAtRoute(nameof(GetPicture), new { touristRouteId = pictureModel.TouristRouteId, pictureId = pictureModel.Id }, pictureToReturn);
        }

        [HttpDelete("pictureId")]
        public IActionResult DeletePicture([FromRoute] Guid touristRouteId, [FromRoute] int pictureId)
        {
            if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
            {
                return NotFound("旅游路线不存在");
            }

            var picture = _touristRouteRepository.GetPicture(pictureId);
            _touristRouteRepository.DeletePicture(picture);
            _touristRouteRepository.Save();

            return NoContent();
        }

    }
}
