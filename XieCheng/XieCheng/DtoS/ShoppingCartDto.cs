using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XieCheng.Models;

namespace XieCheng.DtoS
{
    public class ShoppingCartDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public ICollection<LineItemDto> ShoppingCartItems { get; set; } // 购物车商品列表
    }
}
