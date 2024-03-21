using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using OrdersApp.Controllers;
using OrdersApp.Models;
using OrdersApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersApp.Tests.Controllers
{
	public class OrderContollerTests
	{
		private readonly IOrderService _orderService;
        public OrderContollerTests()
        {
            _orderService = A.Fake<IOrderService>();
        }
        [Fact]
		public void OrderController_CreateOrder_ReturnsActionResult()
		{
			//arrange
			int customerId = 1;
			List<OrderProduct> products = A.Fake<List<OrderProduct>>();
			var orderController = new OrderController(_orderService);

			//act

			var result = orderController.CreateOrder(customerId, products);

			//assert

			result.Should().NotBeNull();
			result.Should().BeOfType<Task<IActionResult>>();
		}

		[Fact]
		public void OrderController_GetOrder_ReturnsActionResult()
		{
			//arrange
			int orderId = 1;
			var orderController = new OrderController(_orderService);

			//act

			var result = orderController.GetOrder(orderId);

			//assert

			result.Should().NotBeNull();
			result.Should().BeOfType<Task<IActionResult>>();
		}

		[Fact]
		public void OrderController_UpdateOrderStatus_ReturnsActionResult()
		{
			//arrange
			int orderId = 1;
			string orderStatus = "Shipped";
			var orderController = new OrderController(_orderService);

			//act

			var result = orderController.GetOrder(orderId);

			//assert

			result.Should().NotBeNull();
			result.Should().BeOfType<Task<IActionResult>>();
		}

		[Fact]
		public void OrderController_CancelOrder_ReturnsActionResult()
		{
			//arrange
			int orderId = 1;
			var orderController = new OrderController(_orderService);

			//act

			var result = orderController.CancelOrder(orderId);

			//assert

			result.Should().NotBeNull();
			result.Should().BeOfType<Task<IActionResult>>();
		}

		[Fact]
		public void OrderController_GetCustomerOrders_ReturnsActionResult()
		{
			//arrange
			int CustomerId = 1;
			var orderController = new OrderController(_orderService);

			//act

			var result = orderController.GetCustomerOrders(CustomerId);

			//assert

			result.Should().NotBeNull();
			result.Should().BeOfType<Task<IActionResult>>();
		}
	}
}
