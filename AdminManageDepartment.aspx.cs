using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdminManageDepartment : System.Web.UI.Page
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
        SqlCommand cmdSelectDepartment = new SqlCommand("SELECT * FROM tblDepartment", con);
        SqlDataReader myDataReader;
        ddlDepartments.Items.Clear();

        //creates placeholder for dropdown list
        ListItem placeholder = new ListItem();
        placeholder.Text = "--Click to Select--";
        ddlDepartments.Items.Add(placeholder);

        try
        {
            con.Open();

            myDataReader = cmdSelectDepartment.ExecuteReader();
            while (myDataReader.Read())
            {
                //creates new list item with department name
                ListItem newitem = new ListItem();
                newitem.Text = myDataReader["departmentName"].ToString();
                //sets value of each item to be equivalent to the department ID
                newitem.Value = myDataReader["departmentID"].ToString();
                ddlDepartments.Items.Add(newitem);
            }
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageDepartment.aspx'</script>");
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
        textboxDeptID.Text = "";
        textboxDeptName.Text = "";
    }

    protected void buttonEdit_Click(object sender, EventArgs e)
    {
        fieldActive(true, false, true, false, false);

        SqlConnection con = new SqlConnection(myConnStr);
        SqlCommand cmdSelectDepartment = new SqlCommand("SELECT * FROM tblDepartment WHERE departmentID = '" + ddlDepartments.SelectedValue + "'", con);
        SqlDataReader myDataReader;

        try
        {
            con.Open();

            myDataReader = cmdSelectDepartment.ExecuteReader();
            myDataReader.Read();

            textboxDeptID.Text = myDataReader["departmentID"].ToString();
            textboxDeptName.Text = myDataReader["departmentName"].ToString();
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageDepartment.aspx'</script>");
        }
        finally
        {
            con.Close();
        }
    }

    protected void buttonUpdate_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(myConnStr);
        SqlCommand cmdUpdateDept = new SqlCommand("UPDATE tblDepartment SET departmentName = @deptName WHERE departmentID = '" + ddlDepartments.SelectedValue + "'", con);
        SqlCommand cmdUpdateCandDept = new SqlCommand("UPDATE tblCandidate SET candDeptName = @candDeptName WHERE departmentID = '" + ddlDepartments.SelectedValue + "'", con);

        cmdUpdateDept.Parameters.AddWithValue("@deptName", textboxDeptName.Text);
        cmdUpdateCandDept.Parameters.AddWithValue("@candDeptName", textboxDeptName.Text);
        
        try
        {
            con.Open();

            cmdUpdateDept.ExecuteNonQuery();
            cmdUpdateCandDept.ExecuteNonQuery();
            Response.Write("<script language='javascript'> alert('Record has been updated successfully!');</script>");
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageDepartment.aspx'</script>");
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
        textboxDeptName.Focus();
        clearPanel();
    }

    protected void buttonInsert_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(myConnStr);
        SqlCommand cmdInsertDept = new SqlCommand("INSERT INTO tblDepartment (departmentName) VALUES (@deptName)", con);

        cmdInsertDept.Parameters.AddWithValue("@deptName", textboxDeptName.Text);

        try
        {
            con.Open();
            cmdInsertDept.ExecuteNonQuery();
            Response.Write("<script language='javascript'> alert('Record has been added successfully!');</script>");
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageDepartment.aspx'</script>");
        }
        finally
        {
            con.Close();
            fieldActive(false, false, false, true, false);
            fillList();
            clearPanel();
        }
    }

    protected void ddlDepartments_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDepartments.SelectedValue == "--Click to Select--")
        {
            fieldActive(false, false, false, true, false);
            clearPanel();
        }
        else
        {
            fieldActive(false, true, false, true, false);

            SqlConnection con = new SqlConnection(myConnStr);
            SqlCommand cmdSelectDepartment = new SqlCommand("SELECT * FROM tblDepartment WHERE departmentID = '" + ddlDepartments.SelectedValue + "'", con);
            SqlDataReader myDataReader;

            try
            {
                con.Open();

                myDataReader = cmdSelectDepartment.ExecuteReader();
                myDataReader.Read();

                textboxDeptID.Text = myDataReader["departmentID"].ToString();
                textboxDeptName.Text = myDataReader["departmentName"].ToString();
            }
            catch (Exception er)
            {
                Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageDepartment.aspx'</script>");
            }
            finally
            {
                con.Close();
            }
        }
    }
}