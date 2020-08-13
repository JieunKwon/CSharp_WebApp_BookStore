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
    /// <summary>
    /// Book List : user can add books to their shopping cart
    /// </summary>
    public partial class BookList : System.Web.UI.Page
    { 
        // connection string
        string cnnString = GlobalData.CnnString;
     
        List<Book> books = new List<Book>();

        int userAccountID;

        protected void Page_Load(object sender, EventArgs e)
        {
            //    categoryFilter = ViewState["CategoryFilter"] == null ? "ALL" : ViewState["CategoryFilter"].ToString();
           
            if (!IsPostBack)
            {
                // category
                ddlBookCategory.DataSource = Enum.GetNames(typeof(Category));
                ddlBookCategory.DataBind(); 
                //add All 
                ddlBookCategory.Items.Insert(0, new ListItem("ALL", "ALL"));

                // generate book list
                this.BindGrid("ALL");
            }

            if (Session["LoginStatus"] != null && Session["UserAccountID"] != null)
            { 
                userAccountID = Convert.ToInt32(Session["UserAccountID"]);

                btnAdd.Visible = true;
                btnLogin.Visible = false;

                ShoppingCartDAO shoppingCartDAO = new ShoppingCartDAO(cnnString);
                ShoppingCart shoppingCart = shoppingCartDAO.GetCartByUserID(userAccountID);
  
            }
            else
            {
                btnAdd.Visible = false;
                btnLogin.Visible = true;
            }
             
        }

        /// <summary>
        /// search a book by category filter
        /// </summary>
        private void BindGrid(string categoryFilter)
        {
            BookDAO bookDAO = new BookDAO(@cnnString);
               
            if (categoryFilter.Equals("ALL"))
            {
                books = bookDAO.ReadAll();
            }
            else
            {
                books = bookDAO.FindByBooksByCategory(categoryFilter);
            }
            gvBooks.DataSource = books;
            gvBooks.DataBind();
            GenerateBookImage();
        }

        /// <summary>
        /// Generate Book Image url for all books
        /// </summary>
        protected void GenerateBookImage()
        {
            // create book image url  
            string path = "";
            foreach (GridViewRow row in gvBooks.Rows)
            {
                // choose with control
                HiddenField bID = (HiddenField)row.FindControl("hdBookID");
                path = "~/images/" + bID.Value + ".jpg";

                Image image = (Image)row.FindControl("imgBook");
                image.ImageUrl = path;

            }
        }

        /// <summary>
        /// Category Changed Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBookCategory_IndexChanged(object sender, EventArgs e)
        {
             
           BindGrid(ddlBookCategory.SelectedItem.Text);

        }


        /// <summary>
        /// Button Event - to link login page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("MyLogIn.aspx");
        }

        /// <summary>
        /// Button Event - to add my cart 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {

            List<int> selectedIDs = new List<int>();

            // call DAO 
            ShoppingCartDAO shoppingCartDAO = new ShoppingCartDAO(cnnString);

            // Get User's Cart
            int userID = (Int32)Session["UserAccountID"];
             

            ShoppingCart shoppingCart = shoppingCartDAO.GetCartByUserID(userID);

            // create cart
            if (shoppingCart is null)
            {
                ShoppingCart newshoppingCart = new ShoppingCart(0, userID);
                shoppingCartDAO.CreateShoppingCart(newshoppingCart);

                shoppingCart = shoppingCartDAO.GetCartByUserID(userID);
            }

           
            // For all checkbox
            foreach (GridViewRow row in gvBooks.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkSelectBook");

                // checked request
                if (chk.Checked)
                {
                    HiddenField hID = (HiddenField)row.FindControl("hdBookID"); 
                    HiddenField hdP = (HiddenField)row.FindControl("hdPrice");
                      
                    int selBookID = Convert.ToInt32(hID.Value);
                    float bPrice = float.Parse(hdP.Value);
                      
                   try
                   {
                        CartDetail cartDetail = new CartDetail(0, shoppingCart.CartID,
                            selBookID, 1, bPrice, System.DateTime.Today);

                        CartDetailDAO cartDetailDAO = new CartDetailDAO(cnnString);

                        CartDetail cartDetailCheck = cartDetailDAO.GetCartDetailByBookID(shoppingCart.CartID, selBookID);

                        // if already the book exists in the cart, aty update 
                        if (!(cartDetailCheck is null))
                        { 
                            cartDetailDAO.UpdateCartDetailQuantityUp(cartDetailCheck.CartDetailID);
                        }
                        // new data insert
                        else {
                            cartDetailDAO.AddCartDetail(cartDetail);
                        }
                        

                   }
                   catch (Exception)
                   {

                   }
                     
                }
            }


            Response.Redirect("Users/MyCartList.aspx");
             
        }

        /// <summary>
        /// Book Title search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BookDAO bookDAO = new BookDAO(@cnnString);

            if (ddlBookCategory.SelectedValue.Equals("ALL"))
            {
                books = bookDAO.FindByBooksTitle(txtKeyword.Text);
            }
            else {
                books = bookDAO.FindByBooksTitle(txtKeyword.Text, ddlBookCategory.SelectedValue);
            }
             
                gvBooks.DataSource = books;
                gvBooks.DataBind();
                GenerateBookImage();
             
        }

        /// <summary>
        /// Go to view detail page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkDetail_Click(object sender, EventArgs e)
        {
            Response.Redirect("BookViewer.aspx?bookID=" + (sender as LinkButton).CommandArgument);
        }

    }
}
