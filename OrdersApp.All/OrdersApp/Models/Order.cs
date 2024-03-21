using OrdersApp.Data.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrdersApp.Models
{
	public class Order
	{
        public int Id { get; set; }
		[ForeignKey("Customer")]
		public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public OrderStatus OrderStatus { get; set; } = OrderStatus.PENDING;
        public decimal TotalPrice { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
