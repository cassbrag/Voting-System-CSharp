using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Text;
using EpicConstruction;

public partial class HelpMenu : System.Web.UI.Page
{
    //connection string
    private string myConnStr = WebConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
    private VotingController vc = new VotingController();

    protected void Page_Load(object sender, EventArgs e)
    {
      
    }

    public string getHelpData()
    {
        return vc.getHelpTable();
    }
}