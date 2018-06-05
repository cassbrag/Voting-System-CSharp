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

public partial class AdminLogin : System.Web.UI.Page
{
    //connection string
    private string myConnStr = WebConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
    private VotingController vc = new VotingController();
    private Admin theAdmin = new Admin();
    protected void Page_Load(object sender, EventArgs e)
    {
        textboxUsername.Focus();
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
            SqlCommand cmdSelectCount = new SqlCommand("SELECT COUNT(*) FROM tblAdmin WHERE adminUsername = '" + textboxUsername.Text + "' AND adminPassword = '" + textboxPassword.Text + "'", con);
            //returns first row and first column value of query result
            vc.matchCount = cmdSelectCount.ExecuteScalar().ToString();

            //if a row in the database contains the entered username and password
            if (vc.getMatchCount() == "1")
            {
                SqlCommand select2Cmd = new SqlCommand("SELECT * FROM tblAdmin WHERE adminUsername = '" + textboxUsername.Text + "' AND adminPassword = '" + textboxPassword.Text + "'", con);
                SqlDataReader read = select2Cmd.ExecuteReader();

                while (read.Read())
                {
                    //stores firstName and lastName value in string variable
                    theAdmin.adminName = (read["adminFirstName"].ToString() + " " + read["adminLastName"].ToString());
                }
                read.Close();

                //creates session for user
                Session["adminName"] = theAdmin.adminName;
                Response.Redirect("AdminMenu.aspx");
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
            Response.Write("<script language='javascript'> alert(''Error! Database connection failed. Please try again.');location.href='AdminLogin.aspx'');</script>");
        }
        finally
        {
            con.Close();
        }
    }
}