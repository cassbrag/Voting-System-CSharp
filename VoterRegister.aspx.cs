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

public partial class VoterRegister : System.Web.UI.Page
{
    //connection string
    private string myConnStr = WebConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
    private VotingController vc = new VotingController();
    protected void Page_Load(object sender, EventArgs e)
    {
        textboxFName.Focus();
    }

    private void clearTextbox()
    {
        textboxUsername.Text = "";
        textboxPassword.Text = "";
        textboxConfPassword.Text = "";
    }

    protected void buttonRegister_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(myConnStr);

        try
        {
            con.Open();

            //counts number of rows where the value exists
            SqlCommand cmdSelectCount = new SqlCommand("SELECT COUNT(*) FROM tblStaff WHERE staffUsername = @username", con);
            cmdSelectCount.Parameters.AddWithValue("@username", textboxUsername.Text);

            //returns first row and first column value of query result
            vc.matchCount = cmdSelectCount.ExecuteScalar().ToString();

            //if a row in the database contains the entered username
            if (vc.getMatchCount() != "1")
            {
                SqlCommand cmdInsertStaff = new SqlCommand("INSERT INTO tblStaff (staffFirstName, staffLastName, staffUsername, staffPassword, staffVoteStatus) VALUES (@fName,@lName, @username, @password, 0)", con);

                cmdInsertStaff.Parameters.AddWithValue("@fName",textboxFName.Text);
                cmdInsertStaff.Parameters.AddWithValue("@lName", textboxLName.Text);
                cmdInsertStaff.Parameters.AddWithValue("@username", textboxUsername.Text);
                cmdInsertStaff.Parameters.AddWithValue("@password", textboxPassword.Text);
                cmdInsertStaff.ExecuteNonQuery();

                Response.Write("<script language='javascript'> alert('Record has been added successfully!');location.href='VoterLogin.aspx'</script>");
            }
            //if a row in the database doesn't contain the entered username
            else
            {
                Response.Write("<script language='javascript'> alert('Username already exists!');</script>");
                clearTextbox();
                textboxUsername.Focus();
            }
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='VoterRegister.aspx'</script>");
        }
        finally
        {
            con.Close();
        }
    }
}