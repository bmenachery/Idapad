using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public class FirmOrder: BaseEntity
    {

            public string BuyerEmail { get; set; }

            public string BasketId { get; set; }
            public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

            public FirmAddress ShipToAddress { get; set; }

            public OrderDelivery DeliveryMethod { get; set; }

            public List<FirmOrderItem> OrderItems { get; set; }
            
            public decimal Subtotal { get; set; }

            public OrderStatus Status { get; set; } = OrderStatus.Pending;

            public string PaymentInterestId { get; set; }

            public decimal GetTotal()
            {
                return Subtotal + DeliveryMethod.Price;
            }
    }
        
}