using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdminManageCandidate : System.Web.UI.Page
{
    //connection string
    private string myConnStr = WebConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            fillListCandidates();
            fillListCandidateDept();
        }

        fieldActive(false, false, false, true, false, false);
    }

    private void fillListCandidates()
    {
        SqlConnection con = new SqlConnection(myConnStr);
        SqlCommand cmdSelectCand = new SqlCommand("SELECT * FROM tblCandidate", con);
        SqlDataReader myDataReader;
        ddlCandidates.Items.Clear();
       
        //creates placeholder for dropdown list
        ListItem placeholder = new ListItem();
        placeholder.Text = "--Click to Select--";
        ddlCandidates.Items.Add(placeholder);

        try
        {
            con.Open();

            myDataReader = cmdSelectCand.ExecuteReader();
            while (myDataReader.Read())
            {
                //creates new list item with staff first and last name
                ListItem newitem = new ListItem();
                newitem.Text = myDataReader["candFirstName"] + " " + myDataReader["candLastName"];
                //sets value of each item to be equivalent to the staff ID
                newitem.Value = myDataReader["candCode"].ToString();
                ddlCandidates.Items.Add(newitem);
            }
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageCandidate.aspx'</script>");
        }
        finally
        {
            con.Close();
        }
    }

    private void fillListCandidateDept()
    {
        SqlConnection con = new SqlConnection(myConnStr);
        SqlCommand cmdSelectCand = new SqlCommand("SELECT * FROM tblDepartment", con);
        SqlDataReader myDataReader;
        ddlCandDept.Items.Clear();

        try
        {
            con.Open();

            myDataReader = cmdSelectCand.ExecuteReader();
            while (myDataReader.Read())
            {
                //creates new list item with staff first and last name
                ListItem newitem = new ListItem();
                newitem.Text = myDataReader["departmentName"].ToString();
                //sets value of each item to be equivalent to the staff ID
                newitem.Value = myDataReader["departmentID"].ToString();
                ddlCandDept.Items.Add(newitem);
            }
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageCandidate.aspx'</script>");
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
        textboxCandCode.Text = "";
        textboxFName.Text = "";
        textboxLName.Text = "";
        ddlCandDept.ClearSelection();
    }

    protected void buttonEdit_Click(object sender, EventArgs e)
    {
        fieldActive(true, false, true, false, false, false);

        SqlConnection con = new SqlConnection(myConnStr);
        SqlCommand cmdSelectCand = new SqlCommand("SELECT * FROM tblCandidate WHERE candCode = '" + ddlCandidates.SelectedValue + "'", con);
        SqlDataReader myDataReader;

        try
        {
            con.Open();

            myDataReader = cmdSelectCand.ExecuteReader();
            myDataReader.Read();

            textboxCandCode.Text = myDataReader["candCode"].ToString();
            textboxFName.Text = myDataReader["candFirstName"].ToString();
            textboxLName.Text = myDataReader["candLastName"].ToString();
            ddlCandDept.SelectedValue = myDataReader["departmentID"].ToString();
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageCandidate.aspx'</script>");
        }
        finally
        {
            con.Close();
        }
    }

    protected void buttonUpdate_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(myConnStr);
        SqlCommand cmdUpdateCand = new SqlCommand("UPDATE tblCandidate SET candFirstName = @firstName, candLastName = @lastName, candDeptName = @departmentname, departmentID = @departmentid WHERE candCode = '" + ddlCandidates.SelectedValue + "'", con);

        cmdUpdateCand.Parameters.AddWithValue("@firstName", textboxFName.Text);
        cmdUpdateCand.Parameters.AddWithValue("@lastName", textboxLName.Text);
        cmdUpdateCand.Parameters.AddWithValue("@departmentname", ddlCandDept.SelectedItem.ToString());
        cmdUpdateCand.Parameters.AddWithValue("@departmentid", ddlCandDept.SelectedValue);

        try
        {
            con.Open();

            cmdUpdateCand.ExecuteNonQuery();
            Response.Write("<script language='javascript'> alert('Record has been updated successfully!');</script>");
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageCandidate.aspx'</script>");
        }
        finally
        {
            con.Close();
            fieldActive(false, false, false, true, false, false);
            fillListCandidates();
            clearPanel();
        }
    }

    protected void buttonAdd_Click(object sender, EventArgs e)
    {
        fieldActive(true, false, false, false, true, false);
        clearPanel();
        textboxFName.Focus();
    }

    protected void buttonInsert_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(myConnStr);
        SqlCommand cmdInsertCand = new SqlCommand("INSERT INTO tblCandidate (candFirstName, candLastName, candDeptName, candVoteCount, departmentID) VALUES (@firstName, @lastName, @departmentname, 0, @departmentid)", con);

        cmdInsertCand.Parameters.AddWithValue("@firstName", textboxFName.Text);
        cmdInsertCand.Parameters.AddWithValue("@lastName", textboxLName.Text);
        cmdInsertCand.Parameters.AddWithValue("@departmentname", ddlCandDept.SelectedItem.ToString());
        cmdInsertCand.Parameters.AddWithValue("@departmentid", ddlCandDept.SelectedValue);

        try
        {
            con.Open();

            cmdInsertCand.ExecuteNonQuery();
            Response.Write("<script language='javascript'> alert('Record has been added successfully!');</script>");
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageCandidate.aspx'</script>");
        }
        finally
        {
            con.Close();
            fieldActive(false, false, false, true, false, false);
            fillListCandidates();
            clearPanel();
        }
    }

    protected void buttonDelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(myConnStr);
        SqlCommand cmdDeleteCand = new SqlCommand("DELETE FROM tblCandidate WHERE candCode = '" + ddlCandidates.SelectedValue + "'", con);

        try
        {
            con.Open();

            cmdDeleteCand.ExecuteNonQuery();
            Response.Write("<script language='javascript'> alert('Record has been deleted successfully!');</script>");
        }
        catch (Exception er)
        {
            Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageCandidate.aspx'</script>");
        }
        finally
        {
            con.Close();
            fieldActive(false, false, false, true, false, false);
            fillListCandidates();
            clearPanel();
        }
    }

    protected void ddlCandidates_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCandidates.SelectedValue == "--Click to Select--")
        {
            fieldActive(false, false, false, true, false, false);
            clearPanel();
        }
        else
        {
            fieldActive(false, true, false, true, false, true);

            SqlConnection con = new SqlConnection(myConnStr);
            SqlCommand cmdSelectCand = new SqlCommand("SELECT * FROM tblCandidate WHERE candCode = '" + ddlCandidates.SelectedValue + "'", con);
            SqlDataReader myDataReader;

            try
            {
                con.Open();

                myDataReader = cmdSelectCand.ExecuteReader();
                myDataReader.Read();

                textboxCandCode.Text = myDataReader["candCode"].ToString();
                textboxFName.Text = myDataReader["candFirstName"].ToString();
                textboxLName.Text = myDataReader["candLastName"].ToString();
                ddlCandDept.SelectedValue = myDataReader["departmentID"].ToString();
            }
            catch (Exception er)
            {
                Response.Write("<script language='javascript'> alert('Error! Database connection failed. Please try again.');location.href='AdminManageCandidate.aspx'</script>");
            }
            finally
            {
                con.Close();
            }
        }
    }
}