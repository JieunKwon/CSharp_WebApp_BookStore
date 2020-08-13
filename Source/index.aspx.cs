using DBLibrary.DAL;
using DBLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BookTreeWeb
{
    /// <summary>
    /// Index Page for BookTree Web
    /// </summary>
    public partial class index : System.Web.UI.Page
    {
        // connection string
        string cnnString = GlobalData.CnnString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // find new book 
            BookDAO bookDAO = new BookDAO(@cnnString);
            Book newBook = bookDAO.FindByID(16);

            if (newBook != null) {
                imgNewBook.ImageUrl = "images/" + newBook.BookID.ToString() + ".jpg";
                lblNewBookTitle.Text = newBook.Title;
                lblNewBookBy.Text = newBook.Author;
                lblNewBookCate.Text = newBook.Category.ToString();

                if (newBook.Description.Length > 200)
                {
                    lblNewBookDes.Text = newBook.Description.Substring(0, 200) + "....";
                }
                else {
                    lblNewBookDes.Text = newBook.Description;
                }
            }

            // Best Seller 
            Book bestBook = bookDAO.FindByID(10);
            if (bestBook != null)
            {
                imgBestBook.ImageUrl = "images/" + bestBook.BookID.ToString() + ".jpg";
                lblBestBookTitle.Text = bestBook.Title;
                lblBestBookBy.Text = bestBook.Author;
                lblBestBookCate.Text = bestBook.Category.ToString();

                if (bestBook.Description.Length > 200)
                {
                    lblBestBookDes.Text = bestBook.Description.Substring(0, 200) + "....";
                }
                else
                {
                    lblBestBookDes.Text = bestBook.Description;
                }
            }
        }
    }
}