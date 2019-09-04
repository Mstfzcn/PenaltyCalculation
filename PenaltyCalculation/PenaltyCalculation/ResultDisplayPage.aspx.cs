using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PenaltyCalculation.View
{
    public partial class ResultDisplayPage : System.Web.UI.Page
    {
        public string businessDays, delayTime, totalPenaltyAmount, penaltyCurrency;

        protected void Page_Load(object sender, EventArgs e)
        {
            businessDays = Request.QueryString["businessDays"];

            delayTime = Request.QueryString["delayTime"];

            totalPenaltyAmount = Request.QueryString["totalPenaltyAmount"];

            penaltyCurrency = Request.QueryString["penaltyCurrency"];

            lblResultMessage.Text = "Business Days : " + businessDays
                       + "<br/><br/> Delay Time    : " + delayTime
                       + "<br/><br/> Penalty       : " + totalPenaltyAmount + " " + penaltyCurrency;
        }
    }
}