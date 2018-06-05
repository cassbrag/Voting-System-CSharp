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

public partial class AdminVoteStatistics : System.Web.UI.Page
{
    //connection string
    private string myConnStr = WebConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
    private Staff theStaff = new Staff();
    private Candidate theCandidate = new Candidate();
    private VotingController vc = new VotingController();

    protected void Page_Load(object sender, EventArgs e)
    {
        lblTotalStaff.Text = "Numbers on voting list: " + vc.getStaffTotal();
        lblStaffVoted.Text = "Numbers voted: " + vc.getStaffVoted() + " (" + Convert.ToInt32(vc.getPercentStaffVoted()) + "%)";
    }

    public string getCandidateVoteStats()
    {     
        return vc.getCandidateVoteStats();
    }
}