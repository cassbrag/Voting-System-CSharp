using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdminManageStaff : System.Web.UI.Page
{
    //connection string
    private string myConnStr = WebConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            fillList();
        }

        fieldActive(false, false, false, true, false, false);
    }

    private void fillList()
    {
        SqlConnection con = new SqlConnection(myConnStr);
        SqlCommand cmdSelectStaff = new SqlCommand("SELECT * FROM tblStaff", con);
        SqlDataReader myDataReader;
        ddlStaff.Items.Clear();

        //creates placeholder for dropdown list
        ListItem placeholder = new ListItem();
        placeholder.Text = "--Click to Select--";
        ddlStaff.Items.Add(placeholder);

        try
        {
            con.Open();

            myDataReader = cmdSelectStaff.ExecuteReader();
            while (myDataReader.Read())
            {
                //creates new list item with staff first and last name
                ListItem newitem = new ListItem();
                newitem.Text = myDataReader["staffFirstName"] + " " + myDataReader["staffLastName"];
                //sets value of each item to be equivalent to the staff ID
                newitem.Value = myDataReader["staffID"].ToString();
                ddlStaff.Items.Add(newitem);
            }
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageStaff.aspx'</script>");
        }
        finally
        {
            con.Close();
        }
    }

    protected void fieldActive(bool Panel, bool btnEdit, bool btnUpdate, bool btnAdd, bool btnInsert, bool btnDelete)
    {
        panelEdit.Enabled = Panel;
        buttonEdit.Enabled = btnEdit;
        buttonUpdate.Enabled = btnUpdate;
        buttonAdd.Enabled = btnAdd;
        buttonInsert.Enabled = btnInsert;
        buttonDelete.Enabled = btnDelete;
    }

    private void clearPanel()
    {
        textboxStaffID.Text = "";
        textboxFName.Text = "";
        textboxLName.Text = "";
        textboxUsername.Text = "";
        textboxPassword.Text = "";
    }

    protected void buttonEdit_Click(object sender, EventArgs e)
    {
        fieldActive(true, false, true, false, false, false);

        SqlConnection con = new SqlConnection(myConnStr);
        SqlCommand cmdSelectStaff = new SqlCommand("SELECT * FROM tblStaff WHERE staffID = '" + ddlStaff.SelectedValue + "'", con);
        SqlDataReader myDataReader;

        try
        {
            con.Open();

            myDataReader = cmdSelectStaff.ExecuteReader();
            myDataReader.Read();

            textboxStaffID.Text = myDataReader["staffID"].ToString();
            textboxFName.Text = myDataReader["staffFirstName"].ToString();
            textboxLName.Text = myDataReader["staffLastName"].ToString();
            textboxUsername.Text = myDataReader["staffUsername"].ToString();
            textboxPassword.Text = myDataReader["staffPassword"].ToString();
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageStaff.aspx'</script>");
        }
        finally
        {
            con.Close();
        }
    }

    protected void buttonUpdate_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(myConnStr);
        SqlCommand cmdUpdateStaff = new SqlCommand("UPDATE tblStaff SET staffFirstName = @firstName, staffLastName = @lastName, staffUsername = @username, staffPassword = @password WHERE staffID = '" + ddlStaff.SelectedValue + "'", con);

        cmdUpdateStaff.Parameters.AddWithValue("@firstName", textboxFName.Text);
        cmdUpdateStaff.Parameters.AddWithValue("@lastName", textboxLName.Text);
        cmdUpdateStaff.Parameters.AddWithValue("@username", textboxUsername.Text);
        cmdUpdateStaff.Parameters.AddWithValue("@password", textboxPassword.Text);

        try
        {
            con.Open();

            cmdUpdateStaff.ExecuteNonQuery();
            Response.Write("<script language='javascript'> alert('Record has been updated successfully!');location.href='AdminManageStaff.aspx'</script>");
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageStaff.aspx'</script>");
        }
        finally
        {
            con.Close();
            fieldActive(false, false, false, true, false, false);
            fillList();
            clearPanel();
        }
    }

    protected void buttonAdd_Click(object sender, EventArgs e)
    {
        fieldActive(true, false, false, false, true, false);
        textboxFName.Focus();
        clearPanel();
    }

    protected void buttonInsert_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(myConnStr);
        SqlCommand cmdInsertStaff = new SqlCommand("INSERT INTO tblStaff (staffFirstName, staffLastName, staffUsername, staffPassword, staffVoteStatus) VALUES (@firstName, @lastName, @username, @password, 0)", con);

        cmdInsertStaff.Parameters.AddWithValue("@firstName", textboxFName.Text);
        cmdInsertStaff.Parameters.AddWithValue("@lastName", textboxLName.Text);
        cmdInsertStaff.Parameters.AddWithValue("@username", textboxUsername.Text);
        cmdInsertStaff.Parameters.AddWithValue("@password", textboxPassword.Text);

        try
        {
            con.Open();

            cmdInsertStaff.ExecuteNonQuery();
            Response.Write("<script language='javascript'> alert('Record has been added successfully!');</script>");
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageStaff.aspx'</script>");
        }
        finally
        {
            con.Close();
            fieldActive(false, false, false, true, false, false);
            fillList();
            clearPanel();
        }
    }

    protected void buttonDelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(myConnStr);
        SqlCommand cmdDeleteStaff = new SqlCommand("DELETE FROM tblStaff WHERE staffID = '" + ddlStaff.SelectedValue + "'", con);

        try
        {
            con.Open();

            cmdDeleteStaff.ExecuteNonQuery();
            Response.Write("<script language='javascript'> alert('Record has been deleted successfully!');location.href='AdminManageStaff.aspx'</script>");
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageStaff.aspx'</script>");
        }
        finally
        {
            con.Close();
            fieldActive(false, false, false, true, false, false);
            fillList();
            clearPanel();
        }
    }

    protected void ddlStaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlStaff.SelectedValue == "--Click to Select--")
        {
            fieldActive(false, false, false, true, false, false);
            clearPanel();
        }
        else
        {
            fieldActive(false, true, false, true, false, true);

            SqlConnection con = new SqlConnection(myConnStr);
            SqlCommand cmdSelectStaff = new SqlCommand("SELECT * FROM tblStaff WHERE staffID = '" + ddlStaff.SelectedValue + "'", con);
            SqlDataReader myDataReader;

            try
            {
                con.Open();

                myDataReader = cmdSelectStaff.ExecuteReader();
                myDataReader.Read();

                textboxStaffID.Text = myDataReader["staffID"].ToString();
                textboxFName.Text = myDataReader["staffFirstName"].ToString();
                textboxLName.Text = myDataReader["staffLastName"].ToString();
                textboxUsername.Text = myDataReader["staffUsername"].ToString();
                textboxPassword.Text = myDataReader["staffPassword"].ToString();
            }
            catch (Exception er)
            {
                Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageStaff.aspx'</script>");
            }
            finally
            {
                con.Close();
            }
        }
    }
}