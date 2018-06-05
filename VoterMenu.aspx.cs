using System;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using EpicConstruction;

public partial class VoterMenu : System.Web.UI.Page
{
    //connection string
    private string myConnStr = WebConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
    private VotingController vc = new VotingController();
    protected void Page_Load(object sender, EventArgs e)
    {
        textboxEnterCode.Focus();
        lblShowName.Text = "Welcome " + Session["staffName"].ToString() + "!";
        lblShowCurrentDate.Text = "The current date and time is: " + DateTime.Now;
        lblVotingTitle.Text = "<b>Voting:</b>";

        if (Session["startDate"] == null && Session["endDate"] == null)
        {
            lblShowDateRange.Text = "<b>A vote date range has not been set.</b>" + "</br>" + "<i><u>You will not be able to vote until the Admin has set a voting date range.</u></i>";
            lblEnterCode.Text = "<b>Voting System is currently closed.</b>";
            textboxEnterCode.Visible = false;
            buttonSelect.Visible = false;
        }
        else
        {
            int dateStart = Convert.ToInt32(Session["startDate"]);
            int dateEnd = Convert.ToInt32(Session["endDate"]);
            DateTime dtNow = DateTime.Now;
            DateTime dtStart;
            DateTime dtEnd;

            //converts int values to DateTime
            if (DateTime.TryParseExact(dateStart.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtStart) && DateTime.TryParseExact(dateEnd.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtEnd))
            {
                lblShowDateRange.ForeColor = System.Drawing.Color.Black;
                lblShowDateRange.Text = "<b>The current voting date range is: " + dtStart.ToString() + " - " + dtEnd.ToString() + "</b>";

                int result1 = DateTime.Compare(dtNow, dtStart);
                int result2 = DateTime.Compare(dtNow, dtEnd);
                int relationship = 0;

                // if current date is earlier than start date and later than end date (out of range)
                if (result1 < 0 && result2 > 0)
                {
                    relationship = 0;
                }
                //if current date is equal to start date or end date (in range)
                else if (result1 == 0 || result2 == 0)
                {
                    relationship = 1;
                }
                //if current date is later than start date and earlier than end date (in range)
                else if (result1 > 0 && result2 < 0)
                {
                    relationship = 1;
                }

                //if the date is not in range
                if (relationship == 0)
                {
                    lblInRange.ForeColor = System.Drawing.Color.Red;
                    lblInRange.Text = "<b><i>(out of range)</i></b>";
                    lblEnterCode.Text = "<b>Voting System is currently closed.</b>";
                    textboxEnterCode.Visible = false;
                    buttonSelect.Visible = false;
                }
                //if the date is in range
                else if (relationship == 1)
                {
                    lblInRange.ForeColor = System.Drawing.Color.ForestGreen;
                    lblInRange.Text = "<b><i>(in range)</i></b>";
                    lblEnterCode.Text = "Enter the code of the Candidate you would like to vote for: ";
                    textboxEnterCode.Visible = true;
                    buttonSelect.Visible = true;
                }
            }
        }
    }

    public string getCandidateData()
    {
        return vc.getCandidateDataTable();
    }

    protected void buttonSelect_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(myConnStr);
        //counts number of rows where value exists
        SqlCommand cmdSelectCount = new SqlCommand("SELECT COUNT(*) FROM tblCandidate WHERE candCode = '" + textboxEnterCode.Text + "'", con);
        try
        {
            con.Open();

            //returns first row and first column value of query result
           vc.matchCount = cmdSelectCount.ExecuteScalar().ToString();

            //if a row in the database contains the entered candidate code
            if (vc.getMatchCount() == "1")
            {
                SqlCommand cmdSelectCand = new SqlCommand("SELECT * FROM tblCandidate WHERE candCode = '" + textboxEnterCode.Text + "'", con);
                SqlDataReader read = cmdSelectCand.ExecuteReader();
                while (read.Read())
                {
                    //stores firstName and lastName value in label
                    lblConfirmMsg.Text = "You have chosen " + (read["candFirstName"].ToString() + " " + read["candLastName"].ToString() + ". Please confirm your choice below:");
                }
                read.Close();

                if (lblConfirmMsg.Text != "")
                {
                    buttonYes.Visible = true;
                    buttonNo.Visible = true;
                }
            }
            //if a row in the database doesn't contain the entered username
            else
            {
                lblErrorMsg.Visible = true;
                textboxEnterCode.Text = "";
                textboxEnterCode.Focus();
            }
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');</script>");
            textboxEnterCode.Text = "";
            textboxEnterCode.Focus();
        }
        finally
        {
            con.Close();
        }
    }

    protected void buttonYes_Click(object sender, EventArgs e)
    {
        if (Session["startDate"] != null && Session["endDate"] != null)
        {
            DateTime dtNow = DateTime.Now;
            DateTime dtStart;
            DateTime dtEnd;
            int dateStart = Convert.ToInt32(Session["startDate"]);
            int dateEnd = Convert.ToInt32(Session["endDate"]);

            //converts int values to DateTime
            if (DateTime.TryParseExact(dateStart.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtStart) && DateTime.TryParseExact(dateEnd.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtEnd))
            {
                lblStartDate.Text = dtStart.ToString();
                lblEndDate.Text = dtEnd.ToString();

                int result1 = DateTime.Compare(dtNow, dtStart);
                int result2 = DateTime.Compare(dtNow, dtEnd);
                int relationship = 0;

                // if current date is earlier than start date and later than end date (out of range)
                if (result1 < 0 && result2 > 0)
                {
                    relationship = 0;
                }
                //if current date is equal to start date or end date (in range)
                else if (result1 == 0 || result2 == 0)
                {
                    relationship = 1;
                }
                //if current date is later than start date and earlier than end date (in range)
                else if (result1 > 0 && result2 < 0)
                {
                    relationship = 1;
                }

                //if the date is in range
                if (relationship == 1)
                {
                    SqlConnection con = new SqlConnection(myConnStr);
                    try
                    {
                        con.Open();

                        SqlCommand cmdUpdateCand = new SqlCommand("UPDATE tblCandidate SET candVoteCount = candVoteCount + 1 WHERE candCode = '" + textboxEnterCode.Text + "'", con);
                        SqlCommand cmdUpdateStaff = new SqlCommand("UPDATE tblStaff SET staffVoteTime = @voteTime, staffVoteStatus = 1 WHERE staffID = '" + Request.QueryString["staffID"] + "'", con);

                        cmdUpdateStaff.Parameters.AddWithValue("@voteTime", DateTime.Now);
                        cmdUpdateCand.ExecuteNonQuery();
                        cmdUpdateStaff.ExecuteNonQuery();

                        Session.Remove("staffName");
                        Session.Remove("staffID");
                        Response.Write("<script language='javascript'> alert('Your vote has been recorded!\\nYou will automatically be logged out.');location.href='Home.aspx'</script>");
                    }
                    catch (Exception er)
                    {
                        Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');</script>");
                        textboxEnterCode.Text = "";
                        textboxEnterCode.Focus();
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
        }
    }

    protected void buttonNo_Click(object sender, EventArgs e)
    {
        buttonYes.Visible = false;
        buttonNo.Visible = false;
        lblConfirmMsg.Visible = false;
        textboxEnterCode.Text = "";
        textboxEnterCode.Focus();
    }
}