using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace XieCheng.Models
{
    public class TouristRoutePicture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // 自增
        public int Id { get; set; }
        [MaxLength(100)]
        public string Url { get; set; }
        [ForeignKey("TouristRouteId")] // 映射 TouristRoute 表中的Id  (类名加上属性 TouristRoute  +  Id)
        public Guid TouristRouteId { get; set; }
        public TouristRoute TouristRoute { get; set; }
    }
}
