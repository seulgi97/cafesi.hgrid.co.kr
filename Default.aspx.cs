using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class _Default : System.Web.UI.Page
{
    public SgFramework.SgUtil su = new SgFramework.SgUtil();

    public String idx = "";
    public String userid = "";
    public String level = "";
    public String passwd = "";
    public String usrtel = "";
    public String company_code = "";
    public String agency_code = "";
    public String branch_code = "";
    public String pos_number = "";
    public String usrorg = "";
    public String grp_code = "";
    public String sql = "";

    public String arr1년판매추이 = "";
    public DataView dv1년간판매추이 = null;
    public DataView dv1년간가동율 = null;

    protected void Page_Load(object sender, EventArgs e)
    {

        idx = su.ReqS(this.Page, "idx");
        userid = su.ReqS(this.Page, "userid");
        level = su.ReqS(this.Page, "level");
        passwd = su.ReqS(this.Page, "passwd");
        usrtel = su.ReqS(this.Page, "usrtel");
        company_code = su.ReqS(this.Page, "company_code");
        agency_code = su.ReqS(this.Page, "agency_code");
        branch_code = su.ReqS(this.Page, "branch_code");
        pos_number = su.ReqS(this.Page, "pos_number");
        usrorg = su.ReqS(this.Page, "usrorg");
        grp_code = su.ReqS(this.Page, "grp_code");

        if (!IsPostBack)
        {
            View();
        }
    }

    public void View()
    {
        //#region 기본정보 

        //sql += " SELECT distinct SUM(판매갯수) 총판매갯수, ";
        //sql += " SUM(총결제금액) 총결제금액 ";
        //sql += " FROM tbl_pay_log WHERE 총판명 IS NOT NULL AND 결제일시 >= STR_TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd") + "', '%Y-%m-%d') ";
        //sql += " AND 결제일시 < STR_TO_DATE('" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "', '%Y-%m-%d') ";
        //if (agency_code != "00")
        //{
        //    sql += " and 총판코드='" + agency_code + "'  ";
        //}
        //if (branch_code != "0000")
        //{
        //    sql += " and 대리점코드='" + branch_code + "'  ";
        //}
        //DataView dv = su.SqlDvQuery(sql);
        //if (dv != null)
        //{
        //    lb당일판매건수.Text = su.FormatNumber( dv[0]["총판매갯수"].ToString() );
        //    lb당일판매금액.Text = su.FormatNumber(dv[0]["총결제금액"].ToString() );
        //}

        //sql = " SELECT distinct SUM(판매갯수) 총판매갯수, ";
        //sql += " SUM(총결제금액) 총결제금액 ";
        //sql += " FROM tbl_pay_log WHERE 총판명 IS NOT NULL AND 결제일시 >= STR_TO_DATE('" + DateTime.Now.AddMonths(-2).ToString("yyyy-MM-") + "01', '%Y-%m-%d') ";
        //sql += " AND 결제일시 < STR_TO_DATE('" + DateTime.Now.AddMonths(-1).ToString("yyyy-MM-") + "01', '%Y-%m-%d') ";
        //if (agency_code != "00")
        //{
        //    sql += " and 총판코드='" + agency_code + "'  ";
        //}
        //if (branch_code != "0000")
        //{
        //    sql += " and 대리점코드='" + branch_code + "'  ";
        //}
        //dv = su.SqlDvQuery(sql);
        //if (dv != null)
        //{
        //    lb전전월판매건수.Text = su.FormatNumber(dv[0]["총판매갯수"].ToString());
        //    lb전전월판매금액.Text = su.FormatNumber(dv[0]["총결제금액"].ToString());

        //    try
        //    {
        //        int 날짜수 = Convert.ToInt32(Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-") + "01").AddDays(-1).ToString("dd"));

        //        int 일평균판매건수 = su.sg_int(dv[0]["총판매갯수"].ToString()) / 날짜수;
        //        int 일평균판매금액 = su.sg_int(dv[0]["총결제금액"].ToString()) / 날짜수;
        //        lb전전월일평균건수.Text = su.FormatNumber(일평균판매건수.ToString());
        //        lb전전월일평균금액.Text = su.FormatNumber(일평균판매금액.ToString());
        //    }
        //    catch (Exception ex) { }

        //}

        //sql = " SELECT distinct SUM(판매갯수) 총판매갯수, ";
        //sql += " SUM(총결제금액) 총결제금액 ";
        //sql += " FROM tbl_pay_log WHERE 총판명 IS NOT NULL AND 결제일시 >= STR_TO_DATE('" + DateTime.Now.AddMonths(-1).ToString("yyyy-MM-") + "01', '%Y-%m-%d') ";
        //sql += " AND 결제일시 < STR_TO_DATE('" + DateTime.Now.AddMonths(0).ToString("yyyy-MM-") + "01', '%Y-%m-%d') ";
        //if (agency_code != "00")
        //{
        //    sql += " and 총판코드='" + agency_code + "'  ";
        //}
        //if (branch_code != "0000")
        //{
        //    sql += " and 대리점코드='" + branch_code + "'  ";
        //}
        //dv = su.SqlDvQuery(sql);
        //if (dv != null)
        //{
        //    lb전월판매건수.Text = su.FormatNumber(dv[0]["총판매갯수"].ToString());
        //    lb전월판매금액.Text = su.FormatNumber(dv[0]["총결제금액"].ToString());

        //    try
        //    {
        //        int 날짜수 = Convert.ToInt32(Convert.ToDateTime(DateTime.Now.AddMonths(0).ToString("yyyy-MM-") + "01").AddDays(-1).ToString("dd"));

        //        int 일평균판매건수 = su.sg_int(dv[0]["총판매갯수"].ToString())/날짜수;
        //        int 일평균판매금액 = su.sg_int(dv[0]["총결제금액"].ToString()) / 날짜수;
        //        lb전월일평균건수.Text = su.FormatNumber(일평균판매건수.ToString());
        //        lb전월일평균금액.Text = su.FormatNumber(일평균판매금액.ToString());
        //    }
        //    catch (Exception ex) { }

        //}

        //sql = " SELECT distinct SUM(판매갯수) 총판매갯수, ";
        //sql += " SUM(총결제금액) 총결제금액 ";
        //sql += " FROM tbl_pay_log WHERE 총판명 IS NOT NULL AND 결제일시 >= STR_TO_DATE('" + DateTime.Now.ToString("yyyy-MM-") + "01', '%Y-%m-%d') ";
        //sql += " AND 결제일시 < STR_TO_DATE('" + DateTime.Now.AddMonths(1).ToString("yyyy-MM-") + "01', '%Y-%m-%d') ";
        //if (agency_code != "00")
        //{
        //    sql += " and 총판코드='" + agency_code + "'  ";
        //}
        //if (branch_code != "0000")
        //{
        //    sql += " and 대리점코드='" + branch_code + "'  ";
        //}
        //dv = su.SqlDvQuery(sql);
        //if (dv != null)
        //{
        //    lb당월판매건수.Text = su.FormatNumber(dv[0]["총판매갯수"].ToString());
        //    lb당월판매금액.Text = su.FormatNumber(dv[0]["총결제금액"].ToString());

        //    try
        //    {
        //        int 날짜수 = Convert.ToInt32(DateTime.Now.ToString("dd"));

        //        int 일평균판매건수 = su.sg_int(dv[0]["총판매갯수"].ToString()) / 날짜수;
        //        int 일평균판매금액 = su.sg_int(dv[0]["총결제금액"].ToString()) / 날짜수;
        //        lb당월일평균건수.Text = su.FormatNumber(일평균판매건수.ToString());
        //        lb당월일평균금액.Text = su.FormatNumber(일평균판매금액.ToString());

        //        int 당월전체날짜수 = Convert.ToInt32(Convert.ToDateTime(DateTime.Now.AddMonths(1).ToString("yyyy-MM-") + "01").AddDays(-1).ToString("dd"));

        //        int 당월예상판매건수 = 일평균판매건수 * 당월전체날짜수;
        //        int 당월예상판매금액 = 일평균판매금액 * 당월전체날짜수;

        //        lb당월예상판매건수.Text = su.FormatNumber(당월예상판매건수.ToString());
        //        lb당월예상판매금액.Text = su.FormatNumber(당월예상판매금액.ToString());

        //    }
        //    catch (Exception ex) { }
        //}

        //#endregion

        //#region 1년간 판매추이 

        //sql = " SELECT distinct DATE_FORMAT(결제일시,'%y%m') 매출년월,DATE_FORMAT(결제일시,'%m') 매출월,SUM(판매갯수) 총판매갯수, ";
        //sql += " SUM(총결제금액) 총결제금액 ";
        //sql += " FROM tbl_pay_log WHERE 총판명 IS NOT NULL AND 결제일시 >= STR_TO_DATE('" + DateTime.Now.AddYears(-1).ToString("yyyy-MM-") + "01', '%Y-%m-%d') ";
        //sql += " AND 결제일시 < STR_TO_DATE('" + DateTime.Now.AddMonths(1).ToString("yyyy-MM-") + "01', '%Y-%m-%d') ";
        //if (agency_code != "00")
        //{
        //    sql += " and 총판코드='" + agency_code + "'  ";
        //}
        //if (branch_code != "0000")
        //{
        //    sql += " and 대리점코드='" + branch_code + "'  ";
        //}
        //sql += " group by DATE_FORMAT(결제일시,'%y%m'),DATE_FORMAT(결제일시,'%m') order by 매출년월 desc ";

        //dv = su.SqlDvQuery(sql);

        //if (dv != null && dv.Count > 0)
        //{
        //    for (int i = dv.Count - 1; i > -1; i--)
        //    {
        //        arr1년판매추이 += "  { \"date\" : \"" + dv[i]["매출년월"].ToString().Substring(0,2)+"년"+ dv[i]["매출월"].ToString() + "월\", \"date2\" : \"" + dv[i]["매출월"].ToString() + "\", \"visits\" : " + dv[i]["총결제금액"].ToString() + ", \"cnt\" : \"" + su.FormatNumber(dv[i]["총판매갯수"].ToString()) + "\" } ";
        //        if (i > 0)
        //        {
        //            arr1년판매추이 += ",";
        //        }
        //    }
        //}

        //sql = " SELECT distinct DATE_FORMAT(결제일시,'%y%m') 매출년월,DATE_FORMAT(결제일시,'%m') 매출월,SUM(판매갯수) 총판매갯수, ";
        //sql += " SUM(총결제금액) 총결제금액 ";
        //sql += " FROM tbl_pay_log WHERE 총판명 IS NOT NULL AND 결제일시 >= STR_TO_DATE('" + DateTime.Now.AddYears(-1).ToString("yyyy-MM-") + "01', '%Y-%m-%d') ";
        //sql += " AND 결제일시 < STR_TO_DATE('" + DateTime.Now.AddMonths(1).ToString("yyyy-MM-") + "01', '%Y-%m-%d') ";
        //if (agency_code != "00")
        //{
        //    sql += " and 총판코드='" + agency_code + "'  ";
        //}
        //if (branch_code != "0000")
        //{
        //    sql += " and 대리점코드='" + branch_code + "'  ";
        //}
        //sql += " group by DATE_FORMAT(결제일시,'%y%m'),DATE_FORMAT(결제일시,'%m') order by 매출년월 ";
        //dv1년간판매추이 = su.SqlDvQuery(sql);
        //#endregion

        //#region 가동율

        //sql = "SELECT 가동월,COUNT(*) 가동일  FROM ( ";
        //sql += " SELECT DISTINCT date_format(req_time, '%y-%m') 가동월, date_format(req_time, '%y%m%d') 가동일, COUNT(*) cnt ";
        //sql += " FROM socket_log ";
        //sql += " WHERE agency_code <> '' ";
        //sql += " and req_time >= STR_TO_DATE('" + DateTime.Now.AddYears(-1).ToString("yyyy-MM-") + "01', '%Y-%m-%d') ";
        //sql += " AND req_time < STR_TO_DATE('" + DateTime.Now.AddMonths(1).ToString("yyyy-MM-") + "01', '%Y-%m-%d') ";
        //if (agency_code != "00")
        //{
        //    sql += " and agency_code='" + agency_code + "'  ";
        //}
        //if (branch_code != "0000")
        //{
        //    sql += " and branch_code='" + branch_code + "'  ";
        //}
        //sql += " GROUP BY date_format(req_time, '%y-%m') ,date_format(req_time, '%y%m%d') ) aa GROUP BY 가동월  order by 가동월";

        //dv1년간가동율 = su.SqlDvQuery(sql);
        //#endregion
    }
}
