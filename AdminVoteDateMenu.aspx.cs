using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdminVoteDateMenu : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["startDate"] == null && Session["endDate"] == null)
        {
            lblVoteDateRange.Text = "A voting date range has not been set.";
            buttonUpdate.Enabled = false;
        }
        else
        {
            int dateStart = Convert.ToInt32(Session["startDate"]);
            int dateEnd = Convert.ToInt32(Session["endDate"]);
            DateTime dtStart;
            DateTime dtEnd;

            //converts int values to DateTime
            if (DateTime.TryParseExact(dateStart.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtStart) && DateTime.TryParseExact(dateEnd.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtEnd))
            {
                lblVoteDateRange.Text = "The current voting date range is: " + dtStart.ToString() + " - " + dtEnd.ToString();
            }
            buttonAdd.Enabled = false;
        }
    }

    protected void buttonAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("AdminVoteDateAdd.aspx");
    }

    protected void buttonUpdate_Click(object sender, EventArgs e)
    {
        Response.Redirect("AdminVoteDateUpdate.aspx");
    }

    protected void buttonView_Click(object sender, EventArgs e)
    {
        Response.Redirect("AdminVoteDateView.aspx");
    }
}