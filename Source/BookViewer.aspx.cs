using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBLibrary.Entities;
using DBLibrary.DAL;

namespace BookTreeWeb
{
    public partial class BookViewer : System.Web.UI.Page
    {
        // connection string
        string cnnString = GlobalData.CnnString;

        int bookID;

        Book book; 

        protected void Page_Load(object sender, EventArgs e)
        {
            bookID = Convert.ToInt32(Request.QueryString["bookID"]);

            BookDAO bookDAO = new BookDAO(cnnString);

            book = bookDAO.FindByID(bookID);

            lblTitle.Text = book.Title;
            lblAuthor.Text = book.Author;
            lblCategory.Text = book.Category.ToString();
            lblISBN.Text = book.ISBN;
            lblPublisher.Text = book.Publisher;
            lblYear.Text = book.Year.ToString();
            lblDec.Text = book.Description;
            lblPrice.Text = String.Format("{0:c}", book.Price); 
            imgBook.ImageUrl = "~/images/" + bookID.ToString() + ".jpg";


            ddQty.DataSource = Enumerable.Range(1, 15);
            ddQty.DataBind();

            // Login check, then set button visible
            if (Session["LoginStatus"] != null && Session["UserAccountID"] != null)
            {
                btnAdd.Visible = true;
                btnLogin.Visible = false; 
            }
            else
            {
                btnAdd.Visible = false;
                btnLogin.Visible = true;
                btnCart.Visible = false; 
            }
        }

        /// <summary>
        /// Button Event - to add my cart 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            
            // call DAO 
            ShoppingCartDAO shoppingCartDAO = new ShoppingCartDAO(cnnString);

            // Get User's Cart
            int userID = Convert.ToInt32(Session["UserAccountID"]);
                
                //(Int32)Session["UserAccountID"];
             
            ShoppingCart shoppingCart = shoppingCartDAO.GetCartByUserID(userID);

            // create cart
            if (shoppingCart is null)
            {
                ShoppingCart newshoppingCart = new ShoppingCart(0, userID);
                shoppingCartDAO.CreateShoppingCart(newshoppingCart);

                shoppingCart = shoppingCartDAO.GetCartByUserID(userID);
            }

            try
            {
                //  Convert.ToInt32(ddQty.SelectedValue)
                CartDetail cartDetail = new CartDetail(0, shoppingCart.CartID, book.BookID, Convert.ToInt32(ddQty.SelectedValue), float.Parse(book.Price.ToString()), System.DateTime.Today);

                CartDetailDAO cartDetailDAO = new CartDetailDAO(cnnString);

                CartDetail cartDetailCheck = cartDetailDAO.GetCartDetailByBookID(shoppingCart.CartID, book.BookID);

                // if already the book exists in the cart, qty update 
                if (!(cartDetailCheck is null))
                {
                    // cartDetailDAO.UpdateCartDetailQuantityUp(cartDetailCheck.CartDetailID);

                    cartDetailCheck.Quantity += Convert.ToInt32(ddQty.SelectedValue);
                    cartDetailDAO.UpdateCartDetailQuantity(cartDetailCheck);
                }
                // new data insert
                else
                {
                    cartDetailDAO.AddCartDetail(cartDetail);
                }

            }
            catch (Exception) {
                Response.Redirect("BookList.aspx");
            }
             
            Response.Redirect("Users/MyCartList.aspx");

        }

        /// <summary>
        /// Button to go list
        /// </summary>
        protected void btnBack_Click(object sender, EventArgs e) {
            Response.Redirect("BookList.aspx");
        }

        /// <summary>
        /// Button to go Cart
        /// </summary>
        protected void btnCart_Click(object sender, EventArgs e) {
            Response.Redirect("Users/MyCartList.aspx");
        }

        /// <summary>
        /// Button to go Login
        /// </summary>
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("MyLogin.aspx"); 
        }
    }
}