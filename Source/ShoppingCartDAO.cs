using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DBLibrary.Entities;

namespace DBLibrary.DAL
{
    public class ShoppingCartDAO
    {
        /// <summary>
        /// SQL connection string 
        /// </summary>
        string cnnString;

        /// <summary>
        /// cons
        /// </summary>
        /// <param name="cnnString">db connection string</param>
        public ShoppingCartDAO(string cnnString)
        {
            this.cnnString = cnnString;
        }

        /// <summary>
        /// Add record to ShoppingCart table
        /// </summary>
        public void CreateShoppingCart(ShoppingCart shoppingCart) {

            // open connection  
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                cnn.Open();

                // if Database/ShoppingCart/CartID is NOT auto-increase, change this part 
                // SqlCommand cmd = new SqlCommand($"Insert into ShoppingCart values (@CartID,@UserAccountID)");
                // cmd.Parameters.AddWithValue("@CartID", shoppingCart.CartID);
                // cmd.Parameters.AddWithValue("@UserAccountID", shoppingCart.UserAccountID);

                // cartID is increased automatically by sql
                SqlCommand cmd = new SqlCommand($"Insert into ShoppingCart values (@UserAccountID)");
                cmd.Parameters.AddWithValue("@UserAccountID", shoppingCart.UserAccountID);

                // execute query
                cmd.Connection = cnn;
                cmd.ExecuteNonQuery();

            }
        }

        /// <summary>
        /// delete record to ShoppingCart table
        /// </summary>
        public void DeleteShoppingCart(int cartID)
        {

            // open connection  
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                cnn.Open();
   
                SqlCommand cmd = new SqlCommand($"Delete from ShoppingCart where cartID=@CartID");
                cmd.Parameters.AddWithValue("@CartID", cartID);

                // execute query
                cmd.Connection = cnn;
                cmd.ExecuteNonQuery();

            }
        }

        /// <summary>
        /// Update Shopping Cart
        /// </summary>
        /// <param name="shoppingCart"></param>
        //public void UpdateShoppingCart(ShoppingCart shoppingCart) {

        //    // open connection  
        //    using (SqlConnection cnn = new SqlConnection(cnnString))
        //    {
        //        cnn.Open();

        //        // ShoppingCart table, don't need to update
        //        //SqlCommand cmd = new SqlCommand($"Update ShoppingCart set UserAccountID = @UserAccountID where cartID = @CartID");
        //        //cmd.Parameters.AddWithValue("@CartID", shoppingCart.CartID);
        //        //cmd.Parameters.AddWithValue("@UserAccountID", shoppingCart.UserAccountID);

        //        // update cart detail
        //        foreach (CartDetail cartDetail in shoppingCart.CartDetails)
        //        {
        //            CartDetailDAO cartDetailDAO = new CartDetailDAO(cnnString);
        //            cartDetailDAO.UpdateCartDetail(cartDetail);
        //        }
        //    }
        //}

        /// <summary>
        /// Find all detail item by CartID 
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns>List<OrderDetail></returns>
        public ShoppingCart GetCartByCartID(int cartID)
        {
            ShoppingCart shoppingCart = null;
            //List<CartDetail> cartDetails = null;

            // open connection  
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand($"select userAccountID from ShoppingCart where cartID = @CartID");
                cmd.Parameters.AddWithValue("@CartID", cartID);

                cmd.Connection = cnn;
                SqlDataReader reader = cmd.ExecuteReader();

                // if (reader.Read())
                //  {

                while (reader.Read())
                {
                    shoppingCart = new ShoppingCart(cartID, Convert.ToInt32(reader["userAccountID"]));

                }
                // }
            }

            return shoppingCart;

        }

        /// <summary>
        /// Find all detail item by userID 
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns>List<OrderDetail></returns>
        public ShoppingCart GetCartByUserID(int userID)
        { 
            ShoppingCart shoppingCart = null;
            //List<CartDetail> cartDetails = null;

            // open connection  
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand($"select cartID from ShoppingCart where userAccountID = @UserID");
                cmd.Parameters.AddWithValue("@UserID", userID);

                cmd.Connection = cnn;
                SqlDataReader reader = cmd.ExecuteReader();

               // if (reader.Read())
              //  {
                     
                    while (reader.Read())
                    {
                        shoppingCart = new ShoppingCart(Convert.ToInt32(reader["cartID"]), userID);

                   }
               // }
            }

            return shoppingCart;

        }

        /// <summary>
        /// Find all items item with book title by CartID 
        /// </summary>
        /// <param name="cartID"></param>
        /// <returns>List<ShoppingCartItem></returns>
        public List<ShoppingCartItem> GetCartItemListByCartID(int cartID)
        {
              
            List<ShoppingCartItem> shoppingCartItems = new List<ShoppingCartItem>();

            // open connection  
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand($"select c.cartDetailID, c.bookID, c.price, c.quantity, b.author, b.title " +
                    $"from CartDetail c inner join Book b ON c.bookID = b.bookID " +
                    $"where c.cartID = @CartID");
                cmd.Parameters.AddWithValue("@CartID", cartID);

                cmd.Connection = cnn;
                SqlDataReader reader = cmd.ExecuteReader();

             
                while (reader.Read())
                {
                    shoppingCartItems.Add(new ShoppingCartItem(
                        Convert.ToInt32(reader["cartDetailID"]),
                        cartID,
                        Convert.ToInt32(reader["bookID"]),
                        Convert.ToInt32(reader["quantity"]),
                        Convert.ToSingle(reader["price"]),
                        reader["title"].ToString(),
                        reader["author"].ToString() ));
                }

            }

            return shoppingCartItems;

        }
    }
}
