using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdminManageAdmin : System.Web.UI.Page
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
        SqlCommand cmdSelectAdmin = new SqlCommand("SELECT * FROM tblAdmin", con);
        SqlDataReader myDataReader;
        ddlAdmins.Items.Clear();

        //creates placeholder for dropdown list
        ListItem placeholder = new ListItem();
        placeholder.Text = "--Click to Select--";
        ddlAdmins.Items.Add(placeholder);

        try
        {
            con.Open();

            myDataReader = cmdSelectAdmin.ExecuteReader();
            while (myDataReader.Read())
            {
                //creates new list item with staff first and last name
                ListItem newitem = new ListItem();
                newitem.Text = myDataReader["adminFirstName"] + " " + myDataReader["adminLastName"];
                //sets value of each item to be equivalent to the staff ID
                newitem.Value = myDataReader["adminID"].ToString();
                ddlAdmins.Items.Add(newitem);
            }
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageAdmin.aspx'</script>");
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
        textboxAdminID.Text = "";
        textboxFName.Text = "";
        textboxLName.Text = "";
        textboxUsername.Text = "";
        textboxPassword.Text = "";
    }

    protected void buttonEdit_Click(object sender, EventArgs e)
    {
        fieldActive(true, false, true, false, false, false);

        SqlConnection con = new SqlConnection(myConnStr);
        SqlCommand cmdSelectAdmin = new SqlCommand("SELECT * FROM tblAdmin WHERE adminID = '" + ddlAdmins.SelectedValue + "'", con);
        SqlDataReader myDataReader;

        try
        {
            con.Open();

            myDataReader = cmdSelectAdmin.ExecuteReader();
            myDataReader.Read();

            textboxAdminID.Text = myDataReader["adminID"].ToString();
            textboxFName.Text = myDataReader["adminFirstName"].ToString();
            textboxLName.Text = myDataReader["adminLastName"].ToString();
            textboxUsername.Text = myDataReader["adminUsername"].ToString();
            textboxPassword.Text = myDataReader["adminPassword"].ToString();
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageAdmin.aspx'</script>");
        }
        finally
        {
            con.Close();
        }
    }

    protected void buttonUpdate_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(myConnStr);
        SqlCommand cmdUpdateAdmin = new SqlCommand("UPDATE tblAdmin SET adminFirstName = @firstName, adminLastName = @lastName, adminUsername = @username, adminPassword = @password WHERE adminID = '" + ddlAdmins.SelectedValue + "'", con);

        cmdUpdateAdmin.Parameters.AddWithValue("@firstName", textboxFName.Text);
        cmdUpdateAdmin.Parameters.AddWithValue("@lastName", textboxLName.Text);
        cmdUpdateAdmin.Parameters.AddWithValue("@username", textboxUsername.Text);
        cmdUpdateAdmin.Parameters.AddWithValue("@password", textboxPassword.Text);

        try
        {
            con.Open();

            cmdUpdateAdmin.ExecuteNonQuery();
            Response.Write("<script language='javascript'> alert('Record has been updated successfully!');</script>");
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageAdmin.aspx'</script>");
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
        SqlCommand cmdInsertAdmin = new SqlCommand("INSERT INTO tblAdmin (adminFirstName, adminLastName, adminUsernane, adminPassword) VALUES (@firstName, @lastName, @username, @password)", con);

        cmdInsertAdmin.Parameters.AddWithValue("@firstName", textboxFName.Text);
        cmdInsertAdmin.Parameters.AddWithValue("@lastName", textboxLName.Text);
        cmdInsertAdmin.Parameters.AddWithValue("@username", textboxUsername.Text);
        cmdInsertAdmin.Parameters.AddWithValue("@password", textboxPassword.Text);

        try
        {
            con.Open();
            cmdInsertAdmin.ExecuteNonQuery();
            Response.Write("<script language='javascript'> alert('Record has been added successfully!');</script>");
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageAdmin.aspx'</script>");
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
        SqlCommand cmdDeleteAdmin = new SqlCommand("DELETE FROM tblAdmin WHERE adminID = '" + ddlAdmins.SelectedValue + "'", con);

        try
        {
            con.Open();

            cmdDeleteAdmin.ExecuteNonQuery();
            Response.Write("<script language='javascript'> alert('Record has been deleted successfully!');</script>");
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageAdmin.aspx'</script>");
        }
        finally
        {
            con.Close();
            fieldActive(false, false, false, true, false, false);
            fillList();
            clearPanel();
        }
    }

    protected void ddlAdmins_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAdmins.SelectedValue == "--Click to Select--")
        {
            fieldActive(false, false, false, true, false, false);
            clearPanel();
        }
        else
        {
            fieldActive(false, true, false, true, false, true);

            SqlConnection con = new SqlConnection(myConnStr);
            SqlCommand cmdSelectAdmin = new SqlCommand("SELECT * FROM tblAdmin WHERE adminID = '" + ddlAdmins.SelectedValue + "'", con);
            SqlDataReader myDataReader;

            try
            {
                con.Open();

                myDataReader = cmdSelectAdmin.ExecuteReader();
                myDataReader.Read();

                textboxAdminID.Text = myDataReader["adminID"].ToString();
                textboxFName.Text = myDataReader["adminFirstName"].ToString();
                textboxLName.Text = myDataReader["adminLastName"].ToString();
                textboxUsername.Text = myDataReader["adminUsername"].ToString();
                textboxPassword.Text = myDataReader["adminPassword"].ToString();
            }
            catch (Exception er)
            {
                Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageAdmin.aspx'</script>");
            }
            finally
            {
                con.Close();
            }
        }
    }
}