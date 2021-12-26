using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using XieCheng.DtoS;
using XieCheng.Helper;
using XieCheng.Models;
using XieCheng.Services;

namespace XieCheng.Controllers
{
    [ApiController]
    [Route("api/shoppingCart")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ITouristRouteRepository touristRouteRepository;
        private readonly IMapper mapper;

        public ShoppingCartController(IHttpContextAccessor httpContextAccessor, ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.touristRouteRepository = touristRouteRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetShoppingCart()
        {
            // 1. Gets the current user id.
            var userId = this.httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // 2. Gets shopping cart by UserId
            var shoppingCart = await touristRouteRepository.GetShoppingCartByUserIdAsync(userId);

            return Ok(mapper.Map<ShoppingCartDto>(shoppingCart));
        }

        /// <summary>
        /// 向购物车中添加商品订单
        /// </summary>
        /// <param name="addShoppingCartItemDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes ="Bearer")]
        public async Task<IActionResult> AddShoppingCartItem([FromBody] AddShoppingCartItemDto addShoppingCartItemDto)
        {
            // 1. Gets the current user id.
            var userId = this.httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // 2. Gets shopping cart by UserId
            var shoppingCart = await touristRouteRepository.GetShoppingCartByUserIdAsync(userId);

            // 3. Create LineItem
            var touristRoute = await this.touristRouteRepository.GetTouristRouteAsync(addShoppingCartItemDto.TouristRouteId);

            if (touristRoute == null)
            {
                return NotFound("旅游路线不存在");
            }

            var lineItem = new LineItem()
            {
                TouristRouteId = addShoppingCartItemDto.TouristRouteId,
                ShoppingCartId = shoppingCart.Id,
                OriginalPrice = touristRoute.OriginalPrice,
                DiscountPresent = touristRoute.DiscountPresent
            };

            await this.touristRouteRepository.AddShoppingCartItemAsync(lineItem);
            await this.touristRouteRepository.SaveAsync();

            return Ok(mapper.Map<ShoppingCartDto>(shoppingCart));
        }

        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> DeleteShoppingCartItem([FromRoute] int itemId)
        {
            var lineItem = await this.touristRouteRepository.GetShoppingCartItemByItemIdAsync(itemId);

            if (lineItem == null)
            {
                return NotFound("商品未找到");
            }

            this.touristRouteRepository.DeleteShoppingCartItem(lineItem);
            await this.touristRouteRepository.SaveAsync();

            return NoContent();
        }


        [HttpDelete("items/({itemIDs})")]
        public async Task<IActionResult> DeleteShoppingCarteItems([ModelBinder(typeof(ArrayModelBinder))][FromBody] IEnumerable<int> itemIDs)
        {
            var lineItems = await this.touristRouteRepository.GetShoppingCartsByIdListAsync(itemIDs);

            this.touristRouteRepository.DeleteShoppingCartItems(lineItems);
            await this.touristRouteRepository.SaveAsync();

            return NoContent();
        }


        [HttpPost("checkout")]
        public async Task<IActionResult> CheckOut()
        {
            // 1. Gets the current user id.
            var userId = this.httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // 2. Gets shopping cart by UserId
            var shoppingCart = await touristRouteRepository.GetShoppingCartByUserIdAsync(userId);

            // 3. Create order
            var order = new Order()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                State = OrderStateEnum.Pending,
                OrderItems = shoppingCart.ShoppingCartItems,
                CreateDateUTC = DateTime.UtcNow
            };

            // 清空购物车
            shoppingCart.ShoppingCartItems = null;

            await this.touristRouteRepository.AddOrderAsync(order);
            await this.touristRouteRepository.SaveAsync();

            return Ok(mapper.Map<OrderDto>(order));
        }

    }
}
