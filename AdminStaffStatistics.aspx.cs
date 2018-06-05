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

public partial class AdminStaffStatistics : System.Web.UI.Page
{
    //connection string
    private string myConnStr = WebConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
    private Staff theStaff = new Staff();
    private VotingController vc = new VotingController();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        lblStaffVoted.Text = "Staff who have voted:";
        lblStaffNotVoted.Text = "Staff who have not voted:";
    }

    public string getStaffVotedTable()
    {
        return vc.getStaffVotedTable();
    }

    public string getStaffNotVotedTable()
    {
        return vc.getStaffNotVotedTable();
    }
}