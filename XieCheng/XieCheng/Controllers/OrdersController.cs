using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using XieCheng.DtoS;
using XieCheng.ResourceParameters;
using XieCheng.Services;

namespace XieCheng.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ITouristRouteRepository touristRouteRepository;
        private readonly IMapper mapper;
        private readonly IHttpClientFactory httpClientFactory;

        public OrdersController(IHttpContextAccessor httpContextAccessor,
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper,
            IHttpClientFactory httpClientFactory)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.touristRouteRepository = touristRouteRepository;
            this.mapper = mapper;
            this.httpClientFactory = httpClientFactory;
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes ="Bearer")]
        public async Task<IActionResult> GetOrders([FromQuery] PaginationResourceParameters parameters)
        {
            // 1. Gets the current user id. 
            var userId = this.httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // 2. Gets shopping cart by UserId
            var shoppingCart = await touristRouteRepository.GetShoppingCartByUserIdAsync(userId);

            var orders = await this.touristRouteRepository.GetOrdersByUserIdAsync(userId, parameters.PageNumber, parameters.PageSize);

            return Ok(mapper.Map<IEnumerable<OrderDto>>(orders));
        }

        [HttpGet("{orderId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOrderById([FromRoute] Guid orderId)
        {
            // 1. Gets the current user id.
            var userId = this.httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var order = await this.touristRouteRepository.GetOrderByIdAsync(orderId);

            return Ok(mapper.Map<OrderDto>(order));
        }

        [HttpPost("{orderId}/placeOrder")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PlaceOrder([FromRoute] Guid orderId)
        {
            // 1. Gets the current user id.
            var userId = this.httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // 2. 开始处理支付
            var order = await this.touristRouteRepository.GetOrderByIdAsync(orderId);
            order.PaymentProcessing();
            await this.touristRouteRepository.SaveAsync();

            // 3. 向第三方提交支付请求，等待第三方响应
            var httpClient = httpClientFactory.CreateClient();
            string url = "http://123.56.149.216/api/FakePaymentProcess?icode={0}&orderNumber={1}&returnFault={2}";
            var response = await httpClient.PostAsync(string.Format(url, "8EDEC5DA75DFA5D6", order.Id, false), null);

            // 4. 提取支付结果，以及支付信息
            bool isApproved = false;
            string transactionMetadata = "";
            if (response.IsSuccessStatusCode)
            {
                transactionMetadata = await response.Content.ReadAsStringAsync();
                var jsonObject = (JObject)JsonConvert.DeserializeObject(transactionMetadata);
                isApproved = jsonObject["approved"].Value<bool>();
            }

            // 5. 如果第三方支付成功，完成订单
            if (isApproved)
            {
                order.PaymentApprove();
            }
            else
            {
                order.PaymentReject();
            }

            order.TransactionMetadata = transactionMetadata;
            await touristRouteRepository.SaveAsync();  

            return Ok(mapper.Map<OrderDto>(order));
        }

    }
}
