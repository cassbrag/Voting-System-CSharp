using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Remove("staffName");
        Session.Remove("adminName");
        Response.Write("<script language='javascript'> alert('You have been logged out of the system.');location.href='Home.aspx'</script>");
    }
}