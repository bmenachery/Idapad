using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.OrderAggregate;

namespace Infrastructure.DataAccess
{
    public static class OrderDataAccess
    {
        
        public static async Task<int> CreateOrderAsync(this IdapadDataAccess dataAccess, RedisDataAccess basketAccess,
             Order order, string Email, int DeliveryMethodId, int AddressId)
        {
            var basket = await basketAccess.GetBasketAsync(order.BasketId);
            decimal orderSubTotal = 0;
            var orderCreate = new 
            {
                BuyerEmail = Email,
                OrderDate = order.OrderDate,
                DeliveryMethodId = DeliveryMethodId,
                ShipAddressId = AddressId,
                Status = order.Status
            };

            var orderid = await dataAccess.ExecuteScalarAsync<int>(
                         "Insert Orders (BuyerEmail, OrderDate, DeliveryMethodId, ShipAddressId, Status) " +
                         "output inserted.Id " +
                         "values(@BuyerEmail, @OrderDate, @DeliveryMethodId, @ShipAddressId, @Status)", orderCreate);


            var items = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                //var productItem = await dataAccess.GetFirmProductItemAsync(item.Id);
                var orderItemCreate = new 
                {
                    FirmProductId  = item.Id,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    orderid = orderid,
                    
                };

                var orderitemid = await dataAccess.ExecuteScalarAsync<int>(
                            "INSERT INTO[dbo].[OrderItems]([FirmProductId],[Price],[Quantity],[OrderId]) " +
                            " VALUES( @FirmProductId, @Price, @Quantity, @OrderId)", orderItemCreate);

                orderSubTotal = orderSubTotal + (decimal)(item.Quantity * item.Price);

            }
            await dataAccess.ExecuteAsync(
                "UPDATE Orders SET SubTotal = " + orderSubTotal +
                " WHERE Id = " + orderid );
            
            return orderid;


        }

        public static async Task<IEnumerable<Order>> GetOrderAsync(this IdapadDataAccess dataAccess, string email)
        {
            string SQL = "SELECT o.[BuyerEmail], o.[OrderDate], o.[DeliveryMethodId],o.[ShipAddressId],  " +
	                    "o.[Subtotal],o.[Status], o.[PaymentInterestId], i.[FirmProductId], i.[Price], " +
                        "i.[Quantity], i.[SizeId], i.[OrderId] FROM [dbo].[Orders] o " +
	                    "INNER JOIN [dbo].[OrderItems] i ON o.Id = i.OrderId " + 
                        " WHERE o.BuyerEmail = '" + email + "'";

            var productItemsOrdered = await dataAccess.QueryAsync<Order>(SQL);

            return productItemsOrdered;   
        }

        public static async Task<IEnumerable<Order>> GetOrderByIdAsync(this IdapadDataAccess dataAccess, string email, int id)
        {
            string SQL = "SELECT o.[BuyerEmail], o.[OrderDate], o.[DeliveryMethodId],o.[ShipAddressId],  " +
                        "o.[Subtotal],o.[Status], o.[PaymentInterestId], i.[FirmProductId], i.[Price], " +
                        "i.[Quantity], i.[SizeId], i.[OrderId] FROM [dbo].[Orders] o " +
                        "INNER JOIN [dbo].[OrderItems] i ON o.Id = i.OrderId " +
                        " WHERE o.BuyerEmail = '" + email + "'" +
                        " AND o.id = " + id ;

            var productItemOrdered = await dataAccess.QueryAsync<Order>(SQL);

            return productItemOrdered;
        }
             
  

        public static async Task<IEnumerable<DeliveryMethod>> GetDeliveryMethodsAsync(this IdapadDataAccess dataAccess)
        {
            var sql = "SELECT[Id],[ShortName],[DeliveryTime],[Description],[Price] " +
                    " FROM[dbo].[DeliveryMethod] ";


            var deliveryMethod = await dataAccess.QueryAsync<DeliveryMethod>(sql);

            return deliveryMethod;
        }

    }
}