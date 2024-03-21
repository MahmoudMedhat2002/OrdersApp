using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OrdersApp.Dto;
using OrdersApp.Models;
using OrdersApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersApp.Tests.Services
{
	public class OrderServiceTests
	{
		public async Task<AppDbContext> GetDB()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

			var appDbcontext = new AppDbContext(options);

			appDbcontext.Database.EnsureCreated();

			if (!appDbcontext.Customers.Any())
			{
				appDbcontext.Customers.AddRange
				(
					new Customer
					{
						Id = 1,
						Name = "Mahmoud",
						Address = "Tanta",
						Email = "dodomado2015@gmail.com"
					},
				  new Customer
				  {
					  Id = 2,
					  Name = "Mostafa",
					  Address = "Cairo",
					  Email = "mostafa@gmail.com"
				  }
				);
			}

			if (!appDbcontext.Products.Any())
			{
				appDbcontext.Products.AddRange
				(
					new Product
					{
						Id = 1,
						Name = "T-shirt",
						Description = "One of the best types of T-shirts",
						Price = 9.99m,
						StockQuantity = 30
					}
					,
					new Product
					{
						Id = 2,
						Name = "Shoes",
						Description = "One of the best types of Shoes",
						Price = 5.99m,
						StockQuantity = 10
					}
				);
			}

			if(!appDbcontext.Orders.Any())
			{
				appDbcontext.Orders.AddRange
				(
					new Order
					{
						CustomerId = 1,
						OrderDate = DateTime.Now,
						OrderItems = new List<OrderItem>
						{
							new OrderItem
							{
								OrderId = 1,
								ProductId = 1,
								Quantity = 5
							}
						},
						OrderStatus = Data.Enums.OrderStatus.SHIPPED,
						Id = 1,
						TotalPrice = 50m
					}
				);
			}

			await appDbcontext.SaveChangesAsync();

			return appDbcontext;
		}
		[Fact]
		public async void OrderService_CancelOrder_ReturnsServiceResponse()
		{
			//arrange
			var orderId = 1;
			var context = await GetDB();
			var orderService = new OrderService(context);

			//act

			var result = await orderService.CancelOrder( orderId );

			//assert

			result.Should().NotBeNull();
			result.Should().BeOfType<ServiceResponse<Task>>();
		}

		[Fact]
		public async void OrderService_CreateOrder_ReturnsOrder()
		{
			//arrange
			var customerId = 1;
			var context = await GetDB();
			var products = A.Fake<List<OrderProduct>>();
			var orderService = new OrderService(context);

			//act

			var result = await orderService.CreateOrder(customerId , products);

			//assert

			result.Should().NotBeNull();
			result.Should().BeOfType<ServiceResponse<Order>>();
		}

		[Fact]
		public async void OrderService_GetAllOrders_ReturnsListOfOrderResponse()
		{
			//arrange
			var limit = 1;
			var context = await GetDB();
			var orderService = new OrderService(context);

			//act

			var result = await orderService.GetAllOrders(limit);

			//assert

			result.Should().NotBeNull();
			result.Should().BeOfType<ServiceResponse<List<OrderResponse>>>();
		}

		[Fact]
		public async void OrderService_GetCustomerOrders_ReturnsListOfOrderResponse()
		{
			//arrange
			var customerId = 1;
			var context = await GetDB();
			var orderService = new OrderService(context);

			//act

			var result = await orderService.GetCustomerOrders(customerId);

			//assert

			result.Should().NotBeNull();
			result.Should().BeOfType<ServiceResponse<List<OrderResponse>>>();
		}

		[Fact]
		public async void OrderService_GetOrder_ReturnsOrderResponse()
		{
			//arrange
			var orderId = 1;
			var context = await GetDB();
			var orderService = new OrderService(context);

			//act

			var result = await orderService.GetOrder(orderId);

			//assert

			result.Should().NotBeNull();
			result.Should().BeOfType<ServiceResponse<OrderResponse>>();
		}

		[Fact]
		public async void OrderService_UpdateOrderStatus_ReturnsServiceResponse()
		{
			//arrange
			var orderId = 1;
			var status = "shipped";
			var context = await GetDB();
			var orderService = new OrderService(context);

			//act

			var result = await orderService.UpdateOrderStatus(orderId , status);

			//assert

			result.Should().NotBeNull();
			result.Should().BeOfType<ServiceResponse<Task>>();
		}


	}
}
