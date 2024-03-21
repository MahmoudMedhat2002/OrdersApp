using OrdersApp.Data.Enums;

namespace OrdersApp.Dto
{
	public class OrderResponse
	{
		public int OrderId { get; set; }
		public string CustomerName { get; set; }
		public string CustomerEmail { get; set; }
		public string CustomerAddress { get; set; }
		public DateTime OrderDate { get; set; } = DateTime.Now;
		public OrderStatus OrderStatus { get; set; } = OrderStatus.PENDING;
		public decimal TotalPrice { get; set; }
		public List<OrderItemResponse> OrderItemResponses { get; set; } = new List<OrderItemResponse>();
	}
}
