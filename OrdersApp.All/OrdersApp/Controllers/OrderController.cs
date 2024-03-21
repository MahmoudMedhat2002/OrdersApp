using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrdersApp.Dto;
using OrdersApp.Models;
using OrdersApp.Services;

namespace OrdersApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly IOrderService _orderService;

		public OrderController(IOrderService orderService)
        {
			_orderService = orderService;
		}
        [HttpPost("createorder/{customerId}")]
		public async Task<IActionResult> CreateOrder(int customerId ,[FromBody] List<OrderProduct> products)
		{
			var result = await _orderService.CreateOrder(customerId, products);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (!result.Success)
				return BadRequest(result);

			return Ok(result);
		}
		[HttpGet("getorder/{orderId}")]
		public async Task<IActionResult> GetOrder(int orderId)
		{
			var result = await _orderService.GetOrder(orderId);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (!result.Success)
				return BadRequest(result);

			return Ok(result);
		}
		[HttpPut("orderstatus/{orderId}/{orderStatus}")]
		public async Task<IActionResult> UpdateOrderStatus(int orderId , string orderStatus)
		{
			var result = await _orderService.UpdateOrderStatus(orderId, orderStatus);

			if (!result.Success)
				return BadRequest(result);

			return Ok(result);
		}
		[HttpDelete("cancelorder/{orderId}")]
		public async Task<IActionResult> CancelOrder(int orderId)
		{
			var result = await _orderService.CancelOrder(orderId);

			if (!result.Success)
				return BadRequest(result);

			return Ok(result);
		}
		[HttpGet("getcustomerorders/{customerId}")]
		public async Task<IActionResult> GetCustomerOrders(int customerId)
		{
			var result = await _orderService.GetCustomerOrders(customerId);

			if (!result.Success)
				return BadRequest(result);

			return Ok(result);
		}
		[HttpGet("{limit}")]
		public async Task<IActionResult> GetAllOrders(int limit = 0)
		{
			var result = await _orderService.GetAllOrders(limit);

			if (!result.Success)
				return BadRequest(result);

			return Ok(result);
		}

	}
}
