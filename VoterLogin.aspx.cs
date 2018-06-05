using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using EpicConstruction;

public partial class VoterLogin : System.Web.UI.Page
{
    private string myConnStr = WebConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
    private VotingController vc = new VotingController();
    private Staff theStaff = new Staff();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["staffName"] != null)
        {
            //query string to send staff ID information to next page
            Response.Redirect("VoterMenu.aspx?staffID=" + Session["staffID"]);
        }
        else
        {
            textboxUsername.Focus();
        }
    }

    private void clearTextbox()
    {
        textboxUsername.Text = "";
        textboxPassword.Text = "";
    }

    protected void buttonLogin_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(myConnStr);

        try
        {
            con.Open();

            //counts number of rows where both values exist
            SqlCommand cmdSelectCount = new SqlCommand("SELECT COUNT(*) FROM tblStaff WHERE staffUsername = '" + textboxUsername.Text + "' AND staffPassword = '" + textboxPassword.Text + "'", con);
            //returns first row and first column value of query result
            vc.matchCount = cmdSelectCount.ExecuteScalar().ToString();

            //if a row in the database contains the entered username and password
            if (vc.getMatchCount() == "1")
            {
                SqlCommand cmdSelectStaffVoted = new SqlCommand("SELECT staffVoteStatus FROM tblStaff WHERE staffUsername = '" + textboxUsername.Text + "' AND staffPassword = '" + textboxPassword.Text + "'", con);
                //returns first row and first column value of query result
                vc.voteCount = cmdSelectStaffVoted.ExecuteScalar().ToString();

                //if the staff hasn't voted
                if (vc.getVoteCount() != "1")
                {
                    SqlCommand cmdSelectStaff = new SqlCommand("SELECT * FROM tblStaff WHERE staffUsername = '" + textboxUsername.Text + "' AND staffPassword = '" + textboxPassword.Text + "'", con);
                    SqlDataReader read = cmdSelectStaff.ExecuteReader();
                    
                    while (read.Read())
                    {
                        //stores firstName, lastName and staffID values in string variables
                        theStaff.staffName = (read["staffFirstName"].ToString() + " " + read["staffLastName"].ToString());
                        theStaff.staffID = Convert.ToInt32(read["staffID"]);
                    }
                    read.Close();

                    //creates a session for user
                    Session["staffName"] = theStaff.staffName;
                    Session["staffID"] = theStaff.staffID;

                    //query string to send staff ID information to next page
                    Response.Redirect("VoterMenu.aspx?staffID=" + Session["staffID"]);
                }
                //if the staff has voted
                else
                {
                    Response.Write("<script language='javascript'> alert('You have already voted!');location.href='Home.aspx'</script>");
                }
            }
            //if a row in the database doesn't contain the entered username and password
            else
            {
                lblErrorMsg.Visible = true;
                clearTextbox();
                textboxUsername.Focus();
            }    
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please enter your details again.');</script>");
            clearTextbox();
            textboxUsername.Focus();
        }
        finally
        {
            con.Close();
        }
    }

    protected void buttonRegister_Click(object sender, EventArgs e)
    {
        Response.Redirect("VoterRegister.aspx");
    }
}