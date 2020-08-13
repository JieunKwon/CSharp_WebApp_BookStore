using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BookTreeWeb
{
    /// <summary>
    /// Master page for admin
    /// </summary>
    public partial class SiteAdmin : System.Web.UI.MasterPage
    {
        public string userEmail = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["LoginStatus"] != null) { 
                if ((int)Session["LoginStatus"] == 1 && (int)Session["UserType"] == 0)
                {
                    userEmail = (string)Session["Email"];
                }
                else
                {
                    userEmail = "";
                }
            }
        }
    }
}