using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DBLibrary.Entities;

namespace DBLibrary.DAL
{
    /// <summary>
    /// Order DAO Class
    /// </summary>
    public class OrderDAO
    {
        /// <summary>
        /// SQL connection string 
        /// </summary>
        string cnnString;

        /// <summary>
        /// cons
        /// </summary>
        /// <param name="cnnString">db connection string</param>
        public OrderDAO(string cnnString)
        {
            this.cnnString = cnnString;
        }
        


        /// <summary>
        /// Add record to Order table with paramaters
        /// </summary>
        public void CreateOrder(int userAccountID, OrderStatus orderStatus, string address, string city, string postalCode, string phone, float shippingCost, float tax)
        {

            // open connection  
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                cnn.Open();

                // OrderID is increased automatically by sql
                SqlCommand cmd = new SqlCommand($"Insert into Orders " +
                    $"(userAccountID, orderStatus,inDate,shippedDate, deliveredDate,address,city,postalCode, phone,shippingCost,tax) " +
                    $"values (@UserAccountID, @OrderStatus, GETDATE(), NULL, NULL, @Address, @City, @PostalCode, @Phone, @ShippingCost, @Tax)");

                cmd.Parameters.AddWithValue("@UserAccountID", userAccountID);
                cmd.Parameters.AddWithValue("@OrderStatus", orderStatus.ToString());
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@City", city);
                cmd.Parameters.AddWithValue("@PostalCode", postalCode);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@ShippingCost", shippingCost);
                cmd.Parameters.AddWithValue("@Tax", tax);

                // execute query
                cmd.Connection = cnn;
                cmd.ExecuteNonQuery();


            }
        }

        /// <summary>
        /// Add record to Order table
        /// </summary>
        public void CreateOrder(Order order) {

            // open connection  
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                cnn.Open();

                // OrderID is increased automatically by sql
                SqlCommand cmd = new SqlCommand($"Insert into Orders " +
                    $"(userAccountID, orderStatus,inDate,shippedDate, deliveredDate,address,city,postalCode, phone,shippingCost,tax) " +
                    $"values (@UserAccountID, @OrderStatus, GETDATE(), NULL, NULL, @Address, @City, @PostalCode, @Phone, @ShippingCost, @Tax)");

                cmd.Parameters.AddWithValue("@UserAccountID", order.UserAccountID);
                cmd.Parameters.AddWithValue("@OrderStatus", order.OrderStatus);
                cmd.Parameters.AddWithValue("@Address", order.Address);
                cmd.Parameters.AddWithValue("@City", order.City);
                cmd.Parameters.AddWithValue("@PostalCode", order.PostalCode);
                cmd.Parameters.AddWithValue("@Phone", order.Phone);
                cmd.Parameters.AddWithValue("@ShippingCost", order.ShippingCost);
                cmd.Parameters.AddWithValue("@Tax", order.Tax);

                // execute query
                cmd.Connection = cnn;
                cmd.ExecuteNonQuery();

                 
            }
        }

        /// <summary>
        /// Update Order
        /// </summary>
        /// <param name="order"></param>
        public void UpdateOrder(Order order) {

            // open connection  
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                cnn.Open();

                SqlCommand cmd = new SqlCommand($"Update Orders " +
                    $"set UserAccountID = @UserAccountID " +
                    $"orderStatus= @OrderStatus, " +
                    $"inDate = @InDate, " +
                    $"shippedDate= @ShippedDate, " +
                    $"deliveredDate= @DeliveredDate," +
                    $"address= @Address," +
                    $"city= @City," +
                    $"postalCode= @PostalCode, " +
                    $"phone= @Phone," +
                    $"shippingCost= @ShippingCost," +
                    $"tax = @Tax" +
                    $" where OrderID = @OrderID");

                cmd.Parameters.AddWithValue("@UserAccountID", order.UserAccountID);
                cmd.Parameters.AddWithValue("@OrderStatus", order.OrderStatus);
                cmd.Parameters.AddWithValue("@InDate", order.InDate);
                cmd.Parameters.AddWithValue("@ShippedDate", order.ShippedDate);
                cmd.Parameters.AddWithValue("@DeliveredDate", order.DeliveredDate);
                cmd.Parameters.AddWithValue("@Address", order.Address);
                cmd.Parameters.AddWithValue("@City", order.City);
                cmd.Parameters.AddWithValue("@PostalCode", order.PostalCode);
                cmd.Parameters.AddWithValue("@Phone", order.Phone);
                cmd.Parameters.AddWithValue("@ShippingCost", order.ShippingCost);
                cmd.Parameters.AddWithValue("@Tax", order.Tax);
                cmd.Parameters.AddWithValue("@OrderID", order.OrderID);

                // execute query
                cmd.Connection = cnn;
                cmd.ExecuteNonQuery();

                // update Order detail
                foreach (OrderDetail orderDetail in order.OrderDetails)
                {
                    OrderDetailDAO orderDetailDAO = new OrderDetailDAO(cnnString);
                    orderDetailDAO.UpdateOrderDetail(orderDetail);
                }
            }
             
        }
         

        /// <summary>
        /// Update Order status information
        /// </summary>
        public void UpdateOrderStatus(OrderStatus status, int orderID)
        {

            // open connection  
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                cnn.Open();

                // update data for ship and deliver
                string addsql = "";
                if (status.Equals(OrderStatus.DELIVERED)) 
                    addsql = ", deliveredDate = getdate() ";
                else if (status.Equals(OrderStatus.SHIPPED))
                    addsql = ", shippedDate = getdate() ";

                SqlCommand cmd = new SqlCommand($"Update Orders set " +
                    $"orderStatus= @OrderStatus " +
                    addsql +
                    $" where OrderID = @OrderID");

                cmd.Parameters.AddWithValue("@OrderStatus", status.ToString());
                cmd.Parameters.AddWithValue("@OrderID", orderID);

                // execute query
                cmd.Connection = cnn;
                cmd.ExecuteNonQuery();
            }

        }

        /// <summary>
        /// Delete Order
        /// </summary>
        /// <param name="order"></param>
        public void DeleteOrder(Order order)
        {
            // open connection  
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                 
                // firstly, delete all order details by orderID 
                OrderDetailDAO orderDetailDAO = new OrderDetailDAO(cnnString);
                orderDetailDAO.DeleteOrderDetailByOrderID(order.OrderID);

                // then, delete order 
                cnn.Open();
                SqlCommand cmd = new SqlCommand($"delete from Orders " +
                    $" where OrderID = @OrderID");

                cmd.Parameters.AddWithValue("@OrderID", order.OrderID);

                // execute query
                cmd.Connection = cnn;
                cmd.ExecuteNonQuery();
                 
            }
              
        }

        /// <summary>
        /// Delete Order by OrderID
        /// </summary>
        /// <param name="order"></param>
        public void DeleteOrderByID(int orderID)
        {
            // open connection  
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {

                // firstly, delete all order details by orderID 
                OrderDetailDAO orderDetailDAO = new OrderDetailDAO(cnnString);
                orderDetailDAO.DeleteOrderDetailByOrderID(orderID);

                // then, delete order 
                cnn.Open();
                SqlCommand cmd = new SqlCommand($"delete from Orders " +
                    $" where OrderID = @OrderID");

                cmd.Parameters.AddWithValue("@OrderID", orderID);

                // execute query
                cmd.Connection = cnn;
                cmd.ExecuteNonQuery();

            }

        }

        /// <summary>
        /// Find and return top orderID by UserAccountID 
        /// </summary>
        /// <param name="userAccountID"></param>
        /// <returns>List<OrderDetail></returns>
        public int FindOrderByUserID(int userAccountID)
        {
            int orderID = 0 ;
             
            // open connection  
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand($"select TOP 1 orderID from Orders " +
                    $" where userAccountID = @UserID and orderStatus = @OrderStatus" +
                    $" Order by orderID desc");
                cmd.Parameters.AddWithValue("@UserID", userAccountID);
                cmd.Parameters.AddWithValue("@OrderStatus", OrderStatus.NEW.ToString());

                cmd.Connection = cnn;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                { 

                    orderID = Convert.ToInt32(reader["orderID"]);

                }

            }

            return orderID;

        }

        /// <summary>
        /// Find all detail item by OrderID 
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns>List<OrderDetail></returns>
        public Order GetOrderByOrderID(int orderID)
        {
            Order order = null;

            List<OrderDetail> orderDetails = null;

            // open connection  
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand($"select * from Orders where orderID = @OrderID");
                cmd.Parameters.AddWithValue("@OrderID", orderID);

                cmd.Connection = cnn;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    // all order detail by orderID 
                    OrderDetailDAO orderDetailDAO = new OrderDetailDAO(cnnString);
                    orderDetails = orderDetailDAO.GetOrderDetailByOrderID(orderID);
                     
                    OrderStatus.TryParse(reader["orderStatus"].ToString(), out OrderStatus oStatus);
                    DateTime shippedDate = new DateTime();

                    if (!reader.IsDBNull(reader.GetOrdinal("shippedDate")))
                    {
                        
                        shippedDate = Convert.ToDateTime(reader["shippedDate"]);
                    }

                    DateTime deliveredDate = new DateTime();

                    if (!reader.IsDBNull(reader.GetOrdinal("deliveredDate")))
                    {

                        deliveredDate = Convert.ToDateTime(reader["deliveredDate"]);
                    }

                    order = new Order(orderID, Convert.ToInt32(reader["userAccountID"]), 
                         oStatus, 
                        Convert.ToDateTime(reader["inDate"]),
                        shippedDate,
                        deliveredDate,
                        reader["address"].ToString(),
                        reader["city"].ToString(),
                        reader["postalCode"].ToString(),
                        reader["phone"].ToString(),
                        Convert.ToSingle(reader["shippingCost"]),
                        Convert.ToSingle(reader["tax"]),
                        orderDetails
                        );
                     
                }

            }

            return order;

        }

        /// <summary>
        /// Find all Orer List by userID 
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns>List<OrderDetail></returns>
        public List<Order> GetAllOrders()
        {

            List<Order> orders = new List<Order>();

            // open connection  
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand($"select * from Orders order by orderID desc");
          
                cmd.Connection = cnn;
                SqlDataReader reader = cmd.ExecuteReader();

                OrderDetailDAO orderDetailDAO = new OrderDetailDAO(cnnString);

                while (reader.Read())
                {
                    // all order detail by orderID  
                    List<OrderDetail> orderDetails = orderDetailDAO.GetOrderDetailByOrderID(Convert.ToInt32(reader["orderID"]));

                    OrderStatus.TryParse(reader["orderStatus"].ToString(), out OrderStatus oStatus);
                    DateTime shippedDate = new DateTime();

                    if (!reader.IsDBNull(reader.GetOrdinal("shippedDate")))
                    {

                        shippedDate = Convert.ToDateTime(reader["shippedDate"]);
                    }

                    DateTime deliveredDate = new DateTime();

                    if (!reader.IsDBNull(reader.GetOrdinal("deliveredDate")))
                    {

                        deliveredDate = Convert.ToDateTime(reader["deliveredDate"]);
                    }

                    orders.Add(new Order(Convert.ToInt32(reader["orderID"]), Convert.ToInt32(reader["userAccountID"]),
                         oStatus,
                        Convert.ToDateTime(reader["inDate"]),
                        shippedDate,
                        deliveredDate,
                        reader["address"].ToString(),
                        reader["city"].ToString(),
                        reader["postalCode"].ToString(),
                        reader["phone"].ToString(),
                        Convert.ToSingle(reader["shippingCost"]),
                        Convert.ToSingle(reader["tax"]),
                        orderDetails
                        ));

                }

            }

            return orders;

        }

        /// <summary>
        /// Find all Orer List by userID 
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns>List<OrderDetail></returns>
        public List<Order> GetAllOrderByUserID(int userID)
        { 

            List<Order> orders = new List<Order>();

            // open connection  
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand($"select * from Orders where userAccountID = @UserID");
                cmd.Parameters.AddWithValue("@UserID", userID);

                cmd.Connection = cnn;
                SqlDataReader reader = cmd.ExecuteReader();

                OrderDetailDAO orderDetailDAO = new OrderDetailDAO(cnnString);

                while (reader.Read())
                { 
                    // all order detail by orderID  
                    List<OrderDetail> orderDetails = orderDetailDAO.GetOrderDetailByOrderID(Convert.ToInt32(reader["orderID"]));

                    OrderStatus.TryParse(reader["orderStatus"].ToString(), out OrderStatus oStatus);
                    DateTime shippedDate = new DateTime();

                    if (!reader.IsDBNull(reader.GetOrdinal("shippedDate")))
                    {

                        shippedDate = Convert.ToDateTime(reader["shippedDate"]);
                    }

                    DateTime deliveredDate = new DateTime();

                    if (!reader.IsDBNull(reader.GetOrdinal("deliveredDate")))
                    {

                        deliveredDate = Convert.ToDateTime(reader["deliveredDate"]);
                    }

                    orders.Add(new Order(Convert.ToInt32(reader["orderID"]), Convert.ToInt32(reader["userAccountID"]),
                         oStatus,
                        Convert.ToDateTime(reader["inDate"]),
                        shippedDate,
                        deliveredDate,
                        reader["address"].ToString(),
                        reader["city"].ToString(),
                        reader["postalCode"].ToString(),
                        reader["phone"].ToString(),
                        Convert.ToSingle(reader["shippingCost"]),
                        Convert.ToSingle(reader["tax"]),
                        orderDetails
                        ));

                }

            }

            return orders;

        }

        /// <summary>
        /// Find all Orer List by userID 
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns>List<OrderDetail></returns>
        public List<Order> GetAllOrderByStatus(string key)
        {

            List<Order> orders = new List<Order>();

            // open connection  
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand($"select * from Orders where orderStatus = @Status order by orderID desc");
                cmd.Parameters.AddWithValue("@Status", key);

                cmd.Connection = cnn;
                SqlDataReader reader = cmd.ExecuteReader();

                OrderDetailDAO orderDetailDAO = new OrderDetailDAO(cnnString);

                while (reader.Read())
                {
                    // all order detail by orderID  
                    List<OrderDetail> orderDetails = orderDetailDAO.GetOrderDetailByOrderID(Convert.ToInt32(reader["orderID"]));

                    OrderStatus.TryParse(reader["orderStatus"].ToString(), out OrderStatus oStatus);
                    DateTime shippedDate = new DateTime();

                    if (!reader.IsDBNull(reader.GetOrdinal("shippedDate")))
                    {

                        shippedDate = Convert.ToDateTime(reader["shippedDate"]);
                    }

                    DateTime deliveredDate = new DateTime();

                    if (!reader.IsDBNull(reader.GetOrdinal("deliveredDate")))
                    {

                        deliveredDate = Convert.ToDateTime(reader["deliveredDate"]);
                    }

                    orders.Add(new Order(Convert.ToInt32(reader["orderID"]), Convert.ToInt32(reader["userAccountID"]),
                         oStatus,
                        Convert.ToDateTime(reader["inDate"]),
                        shippedDate,
                        deliveredDate,
                        reader["address"].ToString(),
                        reader["city"].ToString(),
                        reader["postalCode"].ToString(),
                        reader["phone"].ToString(),
                        Convert.ToSingle(reader["shippingCost"]),
                        Convert.ToSingle(reader["tax"]),
                        orderDetails
                        ));

                }

            }

            return orders;

        }
    }
}
