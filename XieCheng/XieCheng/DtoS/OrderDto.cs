using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XieCheng.DtoS
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }  // 订单所属用户
        public ICollection<LineItemDto> OrderItems { get; set; } // 订单的商品信息
        public string State { get; set; } // 订单状态
        public DateTime CreateDateUTC { get; set; }
        public string TransactionMetadata { get; set; } // 第三方交易的数据
    }
}
