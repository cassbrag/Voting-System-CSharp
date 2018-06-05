using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdminManageHelp : System.Web.UI.Page
{
    //connection string
    private string myConnStr = WebConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            fillList();
        }

        fieldActive(false, false, false, true, false);
    }

    private void fillList()
    {
        SqlConnection con = new SqlConnection(myConnStr);
        SqlCommand cmdSelectHelp = new SqlCommand("SELECT helpID FROM tblHelp", con);
        SqlDataReader myDataReader;
        ddlHelp.Items.Clear();

        //creates placeholder for dropdown list
        ListItem placeholder = new ListItem();
        placeholder.Text = "--Click to Select--";
        ddlHelp.Items.Add(placeholder);

        try
        {
            con.Open();

            myDataReader = cmdSelectHelp.ExecuteReader();
            while (myDataReader.Read())
            {
                //creates new list item with help ID
                ListItem newitem = new ListItem();
                newitem.Text = myDataReader["helpID"].ToString();
                //sets value of each item to be equivalent to the help ID
                newitem.Value = myDataReader["helpID"].ToString();
                ddlHelp.Items.Add(newitem);
            }
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageHelp.aspx'</script>");
        }
        finally
        {
            con.Close();
        }
    }

    protected void fieldActive(bool Panel, bool btnEdit, bool btnUpdate, bool btnAdd, bool btnInsert)
    {
        panelEdit.Enabled = Panel;
        buttonEdit.Enabled = btnEdit;
        buttonUpdate.Enabled = btnUpdate;
        buttonAdd.Enabled = btnAdd;
        buttonInsert.Enabled = btnInsert;
    }

    private void clearPanel()
    {
        textboxHelpID.Text = "";
        textboxQuestion.Text = "";
        textboxAnswer.Text = "";
    }

    protected void buttonEdit_Click(object sender, EventArgs e)
    {
        fieldActive(true, false, true, false, false);

        SqlConnection con = new SqlConnection(myConnStr);
        SqlCommand cmdSelectHelp = new SqlCommand("SELECT * FROM tblHelp WHERE helpID = '" + ddlHelp.SelectedValue + "'", con);
        SqlDataReader myDataReader;

        try
        {
            con.Open();

            myDataReader = cmdSelectHelp.ExecuteReader();
            myDataReader.Read();

            textboxHelpID.Text = myDataReader["helpID"].ToString();
            textboxQuestion.Text = myDataReader["helpQuestion"].ToString();
            textboxAnswer.Text = myDataReader["helpAnswer"].ToString();
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageHelp.aspx'</script>");
        }
        finally
        {
            con.Close();
        }
    }

    protected void buttonUpdate_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(myConnStr);
        SqlCommand cmdUpdateHelp = new SqlCommand("UPDATE tblHelp SET helpQuestion = @question, helpAnswer = @answer WHERE helpID = '" + ddlHelp.SelectedValue + "'", con);

        cmdUpdateHelp.Parameters.AddWithValue("@question", textboxQuestion.Text);
        cmdUpdateHelp.Parameters.AddWithValue("@answer", textboxAnswer.Text);

        try
        {
            con.Open();

            cmdUpdateHelp.ExecuteNonQuery();
            Response.Write("<script language='javascript'> alert('Record has been updated successfully!');</script>");
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageHelp.aspx'</script>");
        }
        finally
        {
            con.Close();
            fieldActive(false, false, false, true, false);
            fillList();
            clearPanel();
        }
    }

    protected void buttonAdd_Click(object sender, EventArgs e)
    {
        fieldActive(true, false, false, false, true);
        textboxQuestion.Focus();
        clearPanel();
    }

    protected void buttonInsert_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(myConnStr);
        SqlCommand cmdInsertHelp = new SqlCommand("INSERT INTO tblHelp (helpQuestion, helpAnswer) VALUES (@question, @answer)", con);

        cmdInsertHelp.Parameters.AddWithValue("@question", textboxQuestion.Text);
        cmdInsertHelp.Parameters.AddWithValue("@answer", textboxAnswer.Text);

        try
        {
            con.Open();
            cmdInsertHelp.ExecuteNonQuery();
            Response.Write("<script language='javascript'> alert('Record has been added successfully!');</script>");
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageHelp.aspx'</script>");
        }
        finally
        {
            con.Close();
            fieldActive(false, false, false, true, false);
            fillList();
            clearPanel();
        }
    }

    protected void ddlHelp_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlHelp.SelectedValue == "--Click to Select--")
        {
            fieldActive(false, false, false, true, false);
            clearPanel();
        }
        else
        {
            fieldActive(false, true, false, true, false);

            SqlConnection con = new SqlConnection(myConnStr);
            SqlCommand cmdSelectHelp = new SqlCommand("SELECT * FROM tblHelp WHERE helpID = '" + ddlHelp.SelectedValue + "'", con);
            SqlDataReader myDataReader;

            try
            {
                con.Open();

                myDataReader = cmdSelectHelp.ExecuteReader();
                myDataReader.Read();

                textboxHelpID.Text = myDataReader["helpID"].ToString();
                textboxQuestion.Text = myDataReader["helpQuestion"].ToString();
                textboxAnswer.Text = myDataReader["helpAnswer"].ToString();
            }
            catch (Exception er)
            {
                Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageHelp.aspx'</script>");
            }
            finally
            {
                con.Close();
            }
        }
    }
}