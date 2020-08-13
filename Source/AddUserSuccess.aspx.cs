using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BookTreeWeb
{
    public partial class MyAddUserSuccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void bGoHome_Click(object sender, EventArgs e)
        {
            Server.Transfer("index.aspx");
        }

        protected void bGoSignin_Click(object sender, EventArgs e)
        {
            Server.Transfer("MyLogIn.aspx");
        }

        protected void bGoBook_Click(object sender, EventArgs e)
        {
            Server.Transfer("BookList.aspx");
        }
    }
}