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
    public partial class UserAccountInput : System.Web.UI.Page
    {
  
        string cnnString = GlobalData.CnnString2;

        protected void Page_Load(object sender, EventArgs e)
        { 
        }

        // cancle to create a user account
        protected void bCancleUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("index.aspx");
        }

        // submit and create a user account
        protected void bSubmitUser_Click(object sender, EventArgs e)
        {
            // int userAccountID, string pwd, string firstName, string lastName, string address,
            // string city, string postalCode, string phone, string email

            UserAccount newUser = new UserAccount(0, txtPassword.Text, txtFirstName.Text, txtLastName.Text, txtAddress.Text, txtCity.Text,
            txtPostalCode.Text, txtPhone.Text, txtEmail.Text);
             
            UserAccountDAO userAccountDAO = new UserAccountDAO(cnnString);

            // Email validarion 
            if (userAccountDAO.ValidateUserAccountExistByEmail(txtEmail.Text))
            {
 
                lblMessage.Text = "Entered Email address already exists";
            }
            else
            {
                // allow to user to add account
                userAccountDAO.AddUser(newUser);
                Server.Transfer("AddUserSuccess.aspx");
            }
        }
    }
}