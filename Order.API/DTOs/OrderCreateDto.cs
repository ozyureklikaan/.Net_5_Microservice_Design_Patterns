using System.Collections.Generic;

namespace Order.API.DTOs
{
    public class OrderCreateDto
    {
        public string BuyerId { get; set; }
        public PaymentDto Payment { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
        public AddressDto Address { get; set; }
    }
}
