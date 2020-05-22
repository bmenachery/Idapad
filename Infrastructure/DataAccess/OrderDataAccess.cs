using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Models;


namespace Infrastructure.DataAccess
{
    public static class OrderDataAccess
    {
        
        public static async Task<int> CreateOrderAsync(this IdapadDataAccess dataAccess, RedisDataAccess basketAccess,
             FirmOrder order, string Email, int DeliveryMethodId, int AddressId)
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


            var items = new List<FirmOrderItem>();
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

        public static async Task<List<FirmOrder>> GetOrderAsync(this IdapadDataAccess dataAccess, string email)
        {
            string SQL = "SELECT o.Id, o.[BuyerEmail], o.[OrderDate], o.[DeliveryMethodId], o.[ShipAddressId], " +
	                    "o.[Subtotal], o.[Status], o.[PaymentInterestId], " +
                        "i.[FirmProductId], i.[FirmProductId], p.Id ProductId, p.[Name] ProductName, " +
	                    "p.[Description] ProductDescription, p.PictureUrl ProductPictureUrl, " +
                        "i.[Price], i.[Quantity], i.[SizeId], i.[OrderId], i.Id OrderItemId, " +
                        "d.ShortName DelShortName, d.[DeliveryTime], d.[Description] DelDescription, d.[Price] DelPrice, " +
                        "a.[StreetAddress], a.[AptAddress], a.[City], a.[State], a.[ZipCode] " +
                        "FROM [dbo].[Orders] o " +
	                    "INNER JOIN [dbo].[OrderItems] i ON o.Id = i.OrderId " +
                        "INNER JOIN [dbo].[FirmProducts] fp ON i.FirmProductId = fp.Id " +
                        "INNER JOIN [dbo].[Products] p ON fp.ProductId = p.Id " +
                        "INNER JOIN [dbo].[Firm] f ON i.FirmProductId = f.Id " +
                        "INNER JOIN [dbo].[DeliveryMethod] d on o.[DeliveryMethodId] = d.Id " +
                        "INNER JOIN FirmAddress a on a.Id = o.Id " +
                        "WHERE o.BuyerEmail = '" + email + "'";

            var ordersflat = await dataAccess.QueryAsync<OrderFlat>(SQL);
             
            List<FirmOrder>orders = new List<FirmOrder>();

            //
            HashSet<int> orderIds = new HashSet<int>();

            foreach(var item in ordersflat)
            {
                if(!orderIds.Contains(item.Id))
                {

                    var order = new FirmOrder();
                    order.Id = item.Id;
                    order.BuyerEmail = item.BuyerEmail;
                    order.OrderDate = item.OrderDate;
                    order.Subtotal = item.Subtotal;
                    

                        FirmAddress shipaddress = new FirmAddress();
                        shipaddress.StreetAddress = item.StreetAddress;
                        shipaddress.AptAddress = item.AptAddress;
                        shipaddress.City = item.City;
                        shipaddress.State = item.State;
                        shipaddress.ZipCode = item.ZipCode;
                    order.ShipToAddress = shipaddress;

                        OrderDelivery delivery = new OrderDelivery();
                        delivery.DeliveryTime = item.DeliveryTime;
                        delivery.ShortName = item.DelShortName;
                        delivery.Description = item.DelDescription;
                        delivery.Price = item.DelPrice;
                    order.DeliveryMethod = delivery;

                    order.OrderItems = new List<FirmOrderItem>();

                    foreach (var orderitem in ordersflat.Where(oi => oi.OrderId == order.Id))
                    {
                        FirmOrderItem firmorderItem = new FirmOrderItem();

                        firmorderItem.OrderId = orderitem.OrderId;
                        firmorderItem.Id = orderitem.OrderItemId;
                        firmorderItem.Price = orderitem.Price;
                        firmorderItem.Quantity = orderitem.Quantity;

                        FirmProductItem firmProductItem = new FirmProductItem();
                            firmProductItem.ProductId = orderitem.FirmProductId;
                            firmProductItem.ProductName = orderitem.ProductName;
                            firmProductItem.PictureUrl = orderitem.ProductPictureUrl;
                        firmorderItem.ItemOrdered = firmProductItem;

                        order.OrderItems.Add(firmorderItem);

                    }
                    
                    orderIds.Add(order.Id);
                    orders.Add(order);
                }

            }
            return orders;
            
            
        }

        
        public static async Task<IEnumerable<FirmOrder>> GetOrderByIdAsync(this IdapadDataAccess dataAccess, string email, int id)
        {
            string SQL = "SELECT o.[BuyerEmail], o.[OrderDate], o.[DeliveryMethodId],o.[ShipAddressId],  " +
                        "o.[Subtotal],o.[Status], o.[PaymentInterestId], i.[FirmProductId], i.[Price], " +
                        "i.[Quantity], i.[SizeId], i.[OrderId] FROM [dbo].[Orders] o " +
                        "INNER JOIN [dbo].[OrderItems] i ON o.Id = i.OrderId " +
                        " WHERE o.BuyerEmail = '" + email + "'" +
                        " AND o.id = " + id ;

            var productItemOrdered = await dataAccess.QueryAsync<FirmOrder>(SQL);

            return productItemOrdered;
        }
             
  

        public static async Task<IEnumerable<OrderDelivery>> GetDeliveryMethodsAsync(this IdapadDataAccess dataAccess)
        {
            var sql = "SELECT[Id],[ShortName],[DeliveryTime],[Description],[Price] " +
                    " FROM[dbo].[DeliveryMethod] ";


            var deliveryMethod = await dataAccess.QueryAsync<OrderDelivery>(sql);

            return deliveryMethod;
        }

    }
}