using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdminVoteDateUpdate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblInstructions.Text = "<i>*The supplementary date automatically sets to 7 days after/before the selected date (Dates are inclusive).*</i>";
    }

    protected void calendarChooseStart_SelectionChanged(object sender, EventArgs e)
    {
        lblCheckNewEnd.Text = calendarChooseStart.SelectedDate.AddDays(6).ToString("yyyyMMdd");

        int newStartDate = calendarChooseStart.SelectedDate.Year * 10000 + calendarChooseStart.SelectedDate.Month * 100 + calendarChooseStart.SelectedDate.Day;
        int newEndDate = calendarChooseStart.SelectedDate.Year * 10000 + calendarChooseStart.SelectedDate.Month * 100 + calendarChooseStart.SelectedDate.AddDays(6).Day;

        lblNewStartDate.Text = "" + newStartDate;
        lblNewEndDate.Text = "" + lblCheckNewEnd.Text;

        Session["startDate"] = lblNewStartDate.Text;
        Session["endDate"] = lblNewEndDate.Text;

        int dateStart = Convert.ToInt32(Session["startDate"]);
        int dateEnd = Convert.ToInt32(Session["endDate"]);
        DateTime dtStart;
        DateTime dtEnd;

        //converts int values to DateTime
        if (DateTime.TryParseExact(dateStart.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtStart) && DateTime.TryParseExact(dateEnd.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtEnd))
        {
            lblVoteDateRange.Text = "You are setting the dates to: " + dtStart.ToString() + " - " + dtEnd.ToString();
        }

        //if a value has been selected from the other calendar
        if (calendarChooseEnd.SelectedDates.Count > 0)
        {
            //clear selected value
            calendarChooseEnd.SelectedDates.Remove(calendarChooseEnd.SelectedDates[0]);
        }

        buttonConfirm.Visible = true;
    }

    protected void calendarChooseEnd_SelectionChanged(object sender, EventArgs e)
    {
        lblCheckNewStart.Text = calendarChooseEnd.SelectedDate.AddDays(-6).ToString("yyyyMMdd");

        int newStartDate = calendarChooseEnd.SelectedDate.Year * 10000 + calendarChooseEnd.SelectedDate.Month * 100 + calendarChooseEnd.SelectedDate.AddDays(-6).Day;
        int newEndDate = calendarChooseEnd.SelectedDate.Year * 10000 + calendarChooseEnd.SelectedDate.Month * 100 + calendarChooseEnd.SelectedDate.Day;

        lblNewStartDate.Text = "" + lblCheckNewStart.Text;
        lblNewEndDate.Text = "" + newEndDate;

        Session["startDate"] = lblNewStartDate.Text;
        Session["endDate"] = lblNewEndDate.Text;

        int dateStart = Convert.ToInt32(Session["startDate"]);
        int dateEnd = Convert.ToInt32(Session["endDate"]);
        DateTime dtStart;
        DateTime dtEnd;

        //converts int value to DateTime
        if (DateTime.TryParseExact(dateStart.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtStart) && DateTime.TryParseExact(dateEnd.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtEnd))
        {
            lblVoteDateRange.Text = "You are setting the dates to: " + dtStart.ToString() + " - " + dtEnd.ToString();
        }

        //if a value has been selected from the otehr calendar
        if (calendarChooseStart.SelectedDates.Count > 0)
        {
            //clear selected value
            calendarChooseStart.SelectedDates.Remove(calendarChooseStart.SelectedDates[0]);
        }

        buttonConfirm.Visible = true;
    }

    protected void buttonConfirm_Click(object sender, EventArgs e)
    {
        Response.Write("<script language='javascript'> alert('The voting date range has successfully been updated!');location.href='AdminVoteDateMenu.aspx'</script>");
    }
}