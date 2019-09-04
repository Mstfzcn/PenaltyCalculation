using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace PenaltyCalculation.View
{
    public partial class HomePage : System.Web.UI.Page
    {
        List<DateTime> HolidayList = new List<DateTime>();
        public string query = "", weekend1 = "", weekend2 = "", penaltyCurrency = "";
        int penaltyAmount = 0, holidayCount = 0, dayCount = 0, delayTime = 0,
            countryId = 1, totalPenaltyAmount = 0, businessDays = 0;
        public SqlDataReader dr;
        public SqlCommand cmd;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void DDListCountry_Init(object sender, EventArgs e)
        {
            CountryListFill();
        }

        //Veritabanında kayıtlı olan ülke verileri ile DropDownList'in doldurulduğu metod
        public void CountryListFill()
        {
            SqlConnection con = getCon();

            try
            {
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter("SELECT id, countryName FROM CountryTB", con);

                DataTable tb = new DataTable();

                da.Fill(tb);

                DDListCountry.DataSource = tb;
                DDListCountry.DataTextField = "countryName";
                DDListCountry.DataValueField = "id";
                DDListCountry.DataBind();

                con.Close();
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ERROR", "<script>alert('" + ex + "');</script>");
            }
        }

        public static SqlConnection getCon()
        {
            SqlConnection con = new SqlConnection("Server=DESKTOP-0C197I0; Initial Catalog=PenaltyCalculationDB; Integrated Security=SSPI");

            return con;
        }

        protected void buttonCalc_Click(object sender, EventArgs e)
        {
            if (receiptDate.Text.Trim().Equals("") |
                deliveryDate.Text.Trim().Equals(""))
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ERROR", "<script>alert('You have entered missing parameters . .');</script>");
                return;
            }
            else
            {
                DateTime dtReceiptDate = Convert.ToDateTime(receiptDate.Text);
                DateTime dtDeliveryDate = Convert.ToDateTime(deliveryDate.Text);
                countryId = Convert.ToInt32(DDListCountry.SelectedItem.Value);

                PenaltyCalc(dtReceiptDate, dtDeliveryDate, countryId);

                if((dayCount - holidayCount - 10) > 0)
                {
                    delayTime = (dayCount - holidayCount - 10);

                    totalPenaltyAmount = penaltyAmount * delayTime; 
                }

                businessDays = dayCount - holidayCount;

                Response.Redirect("ResultDisplayPage.aspx?&delayTime=" + delayTime.ToString()
                                              + "&totalPenaltyAmount=" + totalPenaltyAmount.ToString()
                                                 + "&penaltyCurrency=" + penaltyCurrency
                                                    + "&businessDays=" + businessDays.ToString());
            }

        }

        public void PenaltyCalc(DateTime receiptDate, DateTime deliveryDate, int countryId)
        {
            SqlConnection con = getCon();

            query = "Select weekend1, weekend2, penaltyAmount, penaltyCurrency "
                  + "  From CountryTB "
                  + " Where id = @countryId ";

            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@countryId", countryId);

            try
            {
                con.Open();

                dr = cmd.ExecuteReader();

                //Seçilen ülkenin hafta sonu tatil günlerinin, ceza miktarının 
                //ve para biriminin veritabanından çekildiği kısım
                while (dr.Read())
                {
                    weekend1 = dr["weekend1"].ToString().Trim();
                    weekend2 = dr["weekend2"].ToString().Trim();
                    penaltyAmount = Convert.ToInt32(dr["penaltyAmount"].ToString().Trim());
                    penaltyCurrency = dr["penaltyCurrency"].ToString().Trim();
                }

                cmd.Parameters.Clear();
                dr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ERROR", "<script>alert('" + ex + "');</script>");
            }

            HolidayListFill(countryId);

            foreach (DateTime holiday in HolidayList)
            {
                if ((holiday >= receiptDate && holiday <= deliveryDate) &&
                    !(holiday.ToString("dddd").Equals(weekend1) &&
                    !holiday.ToString("dddd").Equals(weekend2)))
                {
                    holidayCount++; // Hafta içine denkgelen resmi tatil günlerinin hesaplandığı kısım
                }
            }

            DateTime dateTemp = receiptDate;
            string day = string.Empty;
            while (dateTemp <= deliveryDate)
            {
                day = dateTemp.ToString("dddd");
                if (!day.Equals(weekend1) && !day.Equals(weekend2))
                {
                    dayCount++;
                }
                dateTemp = dateTemp.AddDays(1);
            }
        }

        //Seçilen ülkenin resmi tatil günlerinin veritabanından bir listeye aktarıldığı metod
        public void HolidayListFill(int countryId)
        {
            SqlConnection con = getCon();

            query = "Select holiday "
                  + "  From HolidayTB "
                  + " Where countryId = @countryId ";

            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@countryId", countryId);

            try
            {
                con.Open();

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    HolidayList.Add(Convert.ToDateTime(dr["holiday"].ToString().Trim())); 
                }

                cmd.Parameters.Clear();
                dr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ERROR", "<script>alert('" + ex + "');</script>");
            }
        }
    }
}