using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using XieCheng.DtoS;

namespace XieCheng.ValidationAttributes
{
    public class TouristRouteTitleMustDifferWithDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var touristRouteDto = (TouristRouteForManipulationDto)validationContext.ObjectInstance;
            
            if (touristRouteDto.Title == touristRouteDto.Description)
            {
                return new ValidationResult("Title cannot equals Description class", new[] { nameof(TouristRouteForCreationDto) });
            }

            return ValidationResult.Success;
        }
    }
}
