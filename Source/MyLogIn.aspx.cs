using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBLibrary.Entities;
using DBLibrary.DAL;
using System.Web.Security;

namespace BookTreeWeb
{
    /// <summary>
    /// User Login Page
    /// </summary>
    public partial class MyLogIn : System.Web.UI.Page
    {
        // connection string
        string cnnString = GlobalData.CnnString2;
   
        public void Page_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Submit button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void bSignIn_Click(object sender, EventArgs e)
        {
            UserAccountDAO userAccountDAO = new UserAccountDAO(cnnString);
            UserAccount userAccount = userAccountDAO.FindUserAccountByEmailPassword(txtEmail.Text, txtPassword.Text);

            if (userAccount != null)
            {
                
                // login status = 1
                Session["LoginStatus"] = 1;

                // logined user ID
                Session["UserAccountID"] = userAccount.UserAccountID;

                // logined user email
                Session["Email"] = userAccount.Email;
              
                // userType = 0 (admin), userType = 1 (user)
                if (userAccount.Email.Equals("admin@book.com"))
                {
                    Session["UserType"] = 0;
                    Server.Transfer("Admin/AdminAddBook.aspx");
                }
                else 
                {
                    Session["UserType"] = 1;
                    FormsAuthentication.RedirectFromLoginPage(userAccount.Email, false);
                }
            }
            else
            {   
                // not exist user and go back to log in page
                Server.Transfer("MyLogInFail.aspx");
            }

          
        }

        /// <summary>
        ///    signup for first visit customer 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void bSign_Click(object sender, EventArgs e)
        {
            Server.Transfer("UserAccount.aspx");
        }
 
    }
}