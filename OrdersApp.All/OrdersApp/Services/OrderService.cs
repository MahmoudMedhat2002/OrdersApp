using Microsoft.EntityFrameworkCore;
using OrdersApp.Models;
using OrdersApp.Data.Enums;
using OrdersApp.Dto;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

namespace OrdersApp.Services
{
	public class OrderService : IOrderService
	{
		private readonly AppDbContext _context;

		public OrderService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<ServiceResponse<Task>> CancelOrder(int orderId)
		{
			var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(x => x.Id == orderId);

			if (order == null)
				return new ServiceResponse<Task> { Message = "Order not found!!!", Success = false };

			foreach(var orderitem in order.OrderItems)
			{
				var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == orderitem.ProductId);

				product.StockQuantity += orderitem.Quantity;
			}

			_context.Orders.Remove(order);
			await _context.SaveChangesAsync();

			return new ServiceResponse<Task> { Message = "Order Cancelled Successfully" };


		}

		public async Task<ServiceResponse<Order>> CreateOrder(int customerId, List<OrderProduct> products)
		{
			Customer? customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == customerId);

			if (customer == null)
				return new ServiceResponse<Order> { Message = "Customer not found!!", Success = false };

			decimal total = 0;

			var orderItems = new List<OrderItem>();

			foreach (var item in products)
			{
				var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);

				if (product == null)
					return new ServiceResponse<Order> { Message = $"Product with {item.ProductId} not found !!", Success = false };

				if (item.Quantity > product.StockQuantity || item.Quantity <= 0)
					return new ServiceResponse<Order> { Message = $"Not correct amount for {product.Name}", Success = false };
			}

			foreach(var item in products)
			{
				var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);

				product.StockQuantity -= item.Quantity;
				total += product.Price * item.Quantity;

				orderItems.Add(new OrderItem
				{
					ProductId = product.Id,
					Quantity = item.Quantity
				});
			}

			Order order = new Order
			{
				CustomerId = customerId,
				OrderDate = DateTime.Now,
				OrderStatus = OrderStatus.PENDING,
				TotalPrice = total,
				OrderItems = orderItems
			};

			await _context.Orders.AddAsync(order);
			await _context.SaveChangesAsync();
			return new ServiceResponse<Order> {Data = order , Message = "Order Created Successfully" };
		}

		public async Task<ServiceResponse<List<OrderResponse>>> GetAllOrders(int limit = 0)
		{
			if(limit == 0)
			{
				var orders = await _context.Orders.ToListAsync();
				var orderResponses = new List<OrderResponse>();	
				foreach(var order in orders)
				{
					orderResponses.Add((await GetOrder(order.Id)).Data);
				}
				return new ServiceResponse<List<OrderResponse>> { Data = orderResponses };
			}
			else
			{
				var orders = await _context.Orders.Take(limit).ToListAsync();
				var orderResponses = new List<OrderResponse>();
				foreach (var order in orders)
				{
					orderResponses.Add((await GetOrder(order.Id)).Data);
				}
				return new ServiceResponse<List<OrderResponse>> { Data = orderResponses };
			}

		}

		public async Task<ServiceResponse<List<OrderResponse>>> GetCustomerOrders(int customerId)
		{
			var Customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == customerId);

			if(Customer == null)
				return new ServiceResponse<List<OrderResponse>> { Message = "Customer not found" , Success = false };

			var orders = await _context.Orders.Where(o => o.CustomerId == customerId).ToListAsync();

			var orderResponses = new List<OrderResponse>();

			foreach(var order in orders)
			{
				orderResponses.Add((await GetOrder(order.Id)).Data);
			}

			return new ServiceResponse<List<OrderResponse>> { Data = orderResponses };
		}

		public async Task<ServiceResponse<OrderResponse>> GetOrder(int orderId)
		{
			var order = await _context.Orders.Include(o => o.Customer).Include(o => o.OrderItems)
				.ThenInclude(oi => oi.Product).FirstOrDefaultAsync(x => x.Id == orderId);

			if(order == null)
				return new ServiceResponse<OrderResponse> { Message = "Order not found!!!" , Success=false };

			var orderItems = new List<OrderItemResponse>();

			order.OrderItems.ForEach(oi => orderItems.Add(new OrderItemResponse
			{
				ProductId = oi.ProductId,
				Name = oi.Product.Name,
				Description = oi.Product.Description,
				Price = oi.Product.Price,
				Quantity = oi.Quantity	
			}));

			var orderResponse = new OrderResponse
			{
				OrderId = orderId,
				CustomerName = order.Customer.Name,
				CustomerEmail = order.Customer.Email,
				CustomerAddress = order.Customer.Address,
				OrderStatus = order.OrderStatus,
				TotalPrice = order.TotalPrice,
				OrderDate = DateTime.Now,
				OrderItemResponses = orderItems
			};

			return new ServiceResponse<OrderResponse> { Data = orderResponse };
		}

		public async Task<ServiceResponse<Task>> UpdateOrderStatus(int orderId, string status)
		{
			var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);

			if(order == null)
				return new ServiceResponse<Task> { Message = "order not found!!!" , Success = false };

			if(!Enum.TryParse(status.ToUpper() , out OrderStatus dbstatus))
				return new ServiceResponse<Task> { Message = "status must be pending or shipped or delivered", Success = false };

			order.OrderStatus = dbstatus;
			await _context.SaveChangesAsync();

			return new ServiceResponse<Task> { Message = "Order status updated Successfully" };

		}
	}
}
