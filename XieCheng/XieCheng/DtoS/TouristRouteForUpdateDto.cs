using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using XieCheng.ValidationAttributes;

namespace XieCheng.DtoS
{
    public class TouristRouteForUpdateDto : TouristRouteForManipulationDto
    {
        [Required(ErrorMessage = "此更新字段不可缺少")]
        [MaxLength(1500)]
        public override string Description { get; set; }
 

    }
}
