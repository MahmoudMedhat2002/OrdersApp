using OrdersApp.Dto;
using OrdersApp.Models;

namespace OrdersApp.Services
{
	public interface IOrderService
	{
		Task<ServiceResponse<Order>> CreateOrder(int customerId, List<OrderProduct> products);
		Task<ServiceResponse<OrderResponse>> GetOrder(int orderId);
		Task<ServiceResponse<Task>> UpdateOrderStatus(int orderId , string status);
		Task<ServiceResponse<Task>> CancelOrder(int orderId);
		Task<ServiceResponse<List<OrderResponse>>> GetCustomerOrders(int customerId);
		Task<ServiceResponse<List<OrderResponse>>> GetAllOrders(int limit = 0);

	}
}
