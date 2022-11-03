using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Menu_월별매출통계 : System.Web.UI.Page
{
    SgFramework.SgUtil su = new SgFramework.SgUtil();

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



        String 업체 = dd업체명.SelectedValue;

        sql = "SELECT distinct replace(b.`name`,'(주)','') 업체명, a.agency_code FROM machines a LEFT OUTER JOIN agencies b ON a.company_code = b.company_code AND a.agency_code = b.code LEFT OUTER JOIN branches c ON a.company_code = c.company_code AND a.agency_code = c.agency_code AND a.branch_code = c.code";
        sql += " where 1=1 ";
        if (agency_code != "00")
        {
            sql += " and a.agency_code='" + agency_code + "'  ";
        }
        if (branch_code != "0000")
        {
            sql += " and a.branch_code='" + branch_code + "'  ";
        }
        sql += " group by replace(b.`name`,'(주)','') order by 업체명";
        su.binding(dd업체명, sql, "-업체전체");

        su.setDropDownList(dd업체명, 업체);

        String 년도 = dd매출년도.SelectedValue;
        dd매출년도.Items.Clear();
        for(int i = 0; i < 3; i++)
        {
            dd매출년도.Items.Add(Convert.ToInt32(DateTime.Now.AddYears(i * -1).ToString("yyyy")).ToString());
        }

        su.setDropDownList(dd매출년도, 년도);

        if (!IsPostBack)
        {

            bt새로고침_Click(null, null);

            #region TOP

            if (agency_code == "00" && branch_code == "0000")
            {
                sql = " SELECT distinct 총판명, sum(투출) 갯수, sum(가격) 매출 from (  ";
                for (int i = 1; i < 9; i++)
                {
                    //sql += " SELECT DISTINCT 총판명,대리점명,자판기번호,상품" + i.ToString() + " 상품, sum(상품" + i.ToString() + "투출)투출, sum(상품" + i.ToString() + "가격)가격 FROM tbl_pay_log WHERE  결제일시 >= STR_TO_DATE('" + DateTime.Now.ToString("yyyy-MM") + "-01', '%Y-%m-%d') AND 결제일시 < STR_TO_DATE('" + DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-01', '%Y-%m-%d')  ";
                    sql += " SELECT DISTINCT 총판명,대리점명,자판기번호,상품" + i.ToString() + " 상품, sum(상품" + i.ToString() + "투출)투출, sum(상품" + i.ToString() + "가격)가격 FROM tbl_pay_log WHERE  결제일시 >= Convert(datetime, DATEFROMPARTS(year(GETDATE()),month(GETDATE()),'01'))  AND 결제일시 <  DATEADD(mm,1,  Convert(datetime, DATEFROMPARTS(year(getdate()),month(GETDATE()),'01')))  ";
                    if (agency_code != "00")
                    {
                        sql += " and 총판코드='" + agency_code + "'  ";
                    }
                    if (branch_code != "0000")
                    {
                        sql += " and 대리점코드='" + branch_code + "'  ";
                    }
                    sql += " GROUP BY 상품" + i.ToString() + " ";
                    if (i < 8)
                    {
                        sql += " union all ";
                    }
                }
                sql += " ) aa group by 총판명 order by 갯수 desc limit 10";
            }
            else if (agency_code != "00" && branch_code == "0000")
            {
                sql = " SELECT distinct 대리점명, sum(투출) 갯수, sum(가격) 매출 from (  ";
                for (int i = 1; i < 9; i++)
                {
                    //sql += " SELECT DISTINCT 총판명,대리점명,자판기번호,상품" + i.ToString() + " 상품, sum(상품" + i.ToString() + "투출)투출, sum(상품" + i.ToString() + "가격)가격 FROM tbl_pay_log WHERE  결제일시 >= STR_TO_DATE('" + DateTime.Now.ToString("yyyy-MM") + "-01', '%Y-%m-%d') AND 결제일시 < STR_TO_DATE('" + DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-01', '%Y-%m-%d')  ";
                    sql += " SELECT DISTINCT 총판명,대리점명,자판기번호,상품" + i.ToString() + " 상품, sum(상품" + i.ToString() + "투출)투출, sum(상품" + i.ToString() + "가격)가격 FROM tbl_pay_log WHERE  결제일시 >= Convert(datetime, DATEFROMPARTS(year(GETDATE()),month(GETDATE()),'01')) AND 결제일시 < DATEADD(mm,1,  Convert(datetime, DATEFROMPARTS(year(GETDATE()),month(GETDATE()),'01')))  ";
                    if (agency_code != "00")
                    {
                        sql += " and 총판코드='" + agency_code + "'  ";
                    }
                    if (branch_code != "0000")
                    {
                        sql += " and 대리점코드='" + branch_code + "'  ";
                    }
                    sql += " GROUP BY 상품" + i.ToString() + " ";
                    if (i < 8)
                    {
                        sql += " union all ";
                    }
                }
                sql += " ) aa group by 대리점명 order by 갯수 desc limit 10";
            }
            else if (agency_code != "00" && branch_code != "0000")
            {
                sql = " SELECT distinct 자판기번호, sum(투출) 갯수, sum(가격) 매출 from (  ";
                for (int i = 1; i < 9; i++)
                {
                    //sql += " SELECT DISTINCT 총판명,대리점명,자판기번호,상품" + i.ToString() + " 상품, sum(상품" + i.ToString() + "투출)투출, sum(상품" + i.ToString() + "가격)가격 FROM tbl_pay_log WHERE  결제일시 >= STR_TO_DATE('" + DateTime.Now.ToString("yyyy-MM") + "-01', '%Y-%m-%d') AND 결제일시 < STR_TO_DATE('" + DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-01', '%Y-%m-%d')  ";
                    sql += " SELECT DISTINCT 총판명,대리점명,자판기번호,상품" + i.ToString() + " 상품, sum(상품" + i.ToString() + "투출)투출, sum(상품" + i.ToString() + "가격)가격 FROM tbl_pay_log WHERE  결제일시 >= Convert(datetime, DATEFROMPARTS(year(GETDATE()),month(GETDATE()),'01')) AND 결제일시 < DATEADD(mm,1,  Convert(datetime, DATEFROMPARTS(year(GETDATE()),month(GETDATE()),'01'))) ";
                    if (agency_code != "00")
                    {
                        sql += " and 총판코드='" + agency_code + "'  ";
                    }
                    if (branch_code != "0000")
                    {
                        sql += " and 대리점코드='" + branch_code + "'  ";
                    }
                    sql += " GROUP BY 상품" + i.ToString() + " ";
                    if (i < 8)
                    {
                        sql += " union all ";
                    }
                }
                sql += " ) aa group by 자판기번호 order by 갯수 desc limit 10";
            }

            su.binding(gv당월대리점, sql);

            if (agency_code == "00" && branch_code == "0000")
            {
                sql = " SELECT distinct 총판명, sum(투출) 갯수, sum(가격) 매출 from (  ";
                for (int i = 1; i < 9; i++)
                {
                    //sql += " SELECT DISTINCT 총판명,대리점명,자판기번호,상품" + i.ToString() + " 상품, sum(상품" + i.ToString() + "투출)투출, sum(상품" + i.ToString() + "가격)가격 FROM tbl_pay_log WHERE  결제일시 >= STR_TO_DATE('" + DateTime.Now.AddMonths(-12).ToString("yyyy-MM") + "-01', '%Y-%m-%d') AND 결제일시 < STR_TO_DATE('" + DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-01', '%Y-%m-%d')  ";
                    sql += " SELECT DISTINCT 총판명,대리점명,자판기번호,상품" + i.ToString() + " 상품, sum(상품" + i.ToString() + "투출)투출, sum(상품" + i.ToString() + "가격)가격 FROM tbl_pay_log WHERE  결제일시 >= DATEADD(mm,-12,  Convert(datetime, DATEFROMPARTS(year(GETDATE()),month(GETDATE()),'01'))) AND 결제일시 < DATEADD(mm,1,Convert(datetime, DATEFROMPARTS(year(getdate()),month(GETDATE()),'01'))) ";
                    if (agency_code != "00")
                    {
                        sql += " and 총판코드='" + agency_code + "'  ";
                    }
                    if (branch_code != "0000")
                    {
                        sql += " and 대리점코드='" + branch_code + "'  ";
                    }
                    sql += " GROUP BY 상품" + i.ToString() + " ";
                    if (i < 8)
                    {
                        sql += " union all ";
                    }
                }
                sql += " ) aa group by 총판명 order by 갯수 desc limit 10";
            }
            else if (agency_code != "00" && branch_code == "0000")
            {
                sql = " SELECT distinct 대리점명, sum(투출) 갯수, sum(가격) 매출 from (  ";
                for (int i = 1; i < 9; i++)
                {
                    //sql += " SELECT DISTINCT 총판명,대리점명,자판기번호,상품" + i.ToString() + " 상품, sum(상품" + i.ToString() + "투출)투출, sum(상품" + i.ToString() + "가격)가격 FROM tbl_pay_log WHERE  결제일시 >= STR_TO_DATE('" + DateTime.Now.AddMonths(-12).ToString("yyyy-MM") + "-01', '%Y-%m-%d') AND 결제일시 < STR_TO_DATE('" + DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-01', '%Y-%m-%d')  ";
                    sql += " SELECT DISTINCT 총판명,대리점명,자판기번호,상품" + i.ToString() + " 상품, sum(상품" + i.ToString() + "투출)투출, sum(상품" + i.ToString() + "가격)가격 FROM tbl_pay_log WHERE  결제일시 >= DATEADD(mm,-12,  Convert(datetime, DATEFROMPARTS(year(GETDATE()),month(GETDATE()),'01'))) AND 결제일시 < DATEADD(mm,1,Convert(datetime, DATEFROMPARTS(year(getdate()),month(GETDATE()),'01'))) ";
                    if (agency_code != "00")
                    {
                        sql += " and 총판코드='" + agency_code + "'  ";
                    }
                    if (branch_code != "0000")
                    {
                        sql += " and 대리점코드='" + branch_code + "'  ";
                    }
                    sql += " GROUP BY 상품" + i.ToString() + " ";
                    if (i < 8)
                    {
                        sql += " union all ";
                    }
                }
                sql += " ) aa group by 대리점명 order by 갯수 desc limit 10";
            }
            else if (agency_code != "00" && branch_code != "0000")
            {
                sql = " SELECT distinct 자판기번호, sum(투출) 갯수, sum(가격) 매출 from (  ";
                for (int i = 1; i < 9; i++)
                {
                    // sql += " SELECT DISTINCT 총판명,대리점명,자판기번호,상품" + i.ToString() + " 상품, sum(상품" + i.ToString() + "투출)투출, sum(상품" + i.ToString() + "가격)가격 FROM tbl_pay_log WHERE  결제일시 >= STR_TO_DATE('" + DateTime.Now.AddMonths(-12).ToString("yyyy-MM") + "-01', '%Y-%m-%d') AND 결제일시 < STR_TO_DATE('" + DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-01', '%Y-%m-%d')  ";
                    sql += " SELECT DISTINCT 총판명,대리점명,자판기번호,상품" + i.ToString() + " 상품, sum(상품" + i.ToString() + "투출)투출, sum(상품" + i.ToString() + "가격)가격 FROM tbl_pay_log WHERE  결제일시 >= DATEADD(mm,-12,  Convert(datetime, DATEFROMPARTS(year(GETDATE()),month(GETDATE()),'01'))) AND 결제일시 < DATEADD(mm,1,Convert(datetime, DATEFROMPARTS(year(getdate()),month(GETDATE()),'01'))) ";
                    if (agency_code != "00")
                    {
                        sql += " and 총판코드='" + agency_code + "'  ";
                    }
                    if (branch_code != "0000")
                    {
                        sql += " and 대리점코드='" + branch_code + "'  ";
                    }
                    sql += " GROUP BY 상품" + i.ToString() + " ";
                    if (i < 8)
                    {
                        sql += " union all ";
                    }
                }
                sql += " ) aa group by 자판기번호 order by 갯수 desc limit 10";
            }

            su.binding(gv년간대리점, sql);
            #endregion

        }
    }
    protected void bt새로고침_Click(object sender, EventArgs e)
    {
        String 검색어 = su.sg_db_query(tb검색어.Text.Trim(), true);
        String 년도 = dd매출년도.SelectedValue;

        sql = " SELECT distinct b.name as 총판명,c.name as 대리점명, d.idx as 자판기번호 ";
        for(int i = 1; i < 13; i++)
        {
            sql += " , sum(CASE WHEN 결제월 = '"+ su.AddByteZero(i.ToString(),2)+"' THEN 총판매갯수 ELSE 0 END) AS '"+ i.ToString()+"월' ";
            sql += " , sum(CASE WHEN 결제월 = '" + su.AddByteZero(i.ToString(), 2) + "' THEN 총결제금액 ELSE 0 END) AS '" + i.ToString() + "금액' ";
        }

        sql += " , (0 ";
        for (int i = 1; i < 13; i++)
        {
            sql += " + CASE WHEN 결제월 = '" + su.AddByteZero(i.ToString(), 2) + "' THEN 총판매갯수 ELSE 0 END ";
        }
        sql += " ) 총건수";

        sql += " , (0 ";
        for (int i = 1; i < 13; i++)
        {
            sql += " + CASE WHEN 결제월 = '" + su.AddByteZero(i.ToString(), 2) + "' THEN 총결제금액 ELSE 0 END ";
        }
        sql += " ) 총금액";

        sql += " FROM ( ";
        // sql += " SELECT DISTINCT 자판기번호,총판코드, 대리점코드, 포스코드, date_format(결제일시, '%m') 결제월, SUM(판매갯수) 총판매갯수, ";
        sql += " SELECT DISTINCT 자판기번호,총판코드, 대리점코드, 포스코드, MONTH(결제일시) 결제월, SUM(판매갯수) 총판매갯수, ";
        sql += " SUM(총결제금액) 총결제금액 ";
        // sql += " FROM tbl_pay_log WHERE 총판명 IS NOT NULL AND 결제일시 >= STR_TO_DATE('"+년도+"-01-01', '%Y-%m-%d') ";
        // sql += " AND 결제일시 < DATE_ADD(STR_TO_DATE('" + 년도 + "-01-01', '%Y-%m-%d'), INTERVAL 1 YEAR) ";
        sql += " FROM tbl_pay_log WHERE 총판명 IS NOT NULL AND 결제일시 >= CONVERT(DATETIME, '"+년도+"')";
        sql += " AND 결제일시 < DATEADD(yy, 1, CONVERT(DATETIME, '" + 년도 + "'))";

        if (dd업체명.SelectedValue != "")
        {
            sql += " and 총판코드 = '" + dd업체명.SelectedValue + "' ";
        }
        if (검색어 != "")
        {
            sql += " and ( 총판명 like '%" + 검색어 + "%' or 자판기번호 like '%" + 검색어 + "%' or 대리점명 like '%" + 검색어 + "%' or 설치장소 like '%" + 검색어 + "%' or 승인번호 = '" + 검색어 + "'  ) ";
        }
        if (agency_code != "00")
        {
            sql += " and 총판코드='" + agency_code + "'  ";
        }
        if (branch_code != "0000")
        {
            sql += " and 대리점코드='" + branch_code + "'  ";
        }

        //sql += " GROUP BY 자판기번호,총판코드, 대리점코드, 포스코드, date_format(결제일시, '%m'), tid ";
        sql += " GROUP BY 자판기번호,총판코드, 대리점코드, 포스코드, MONTH(결제일시), tid ";
        sql += " ) z left outer join dbo.agency b ON z.총판코드 = b.company_idx";
        sql += "  left outer join dbo.branch c ON z.총판코드 = c.company_idx and z.대리점코드= c.agency_idx";
        sql += "  left outer join dbo.machine d ON z.총판코드 = d.company_idx and z.대리점코드= d.branch_idx";
       // sql += " and z.포스코드=d.pos_number ";
        sql += " GROUP BY b.name ,c.name , d.idx,z.결제월,  z.총판매갯수, z.총결제금액 ";
        hexcel3.Text = sql;
        su.binding(gv월매출리스트, sql);

        String sql1 = "select distinct 총판명, 대리점명, sum('1월') '1월', sum('1금액') '1금액', sum('2월') '2월', sum('2금액') '2금액', sum('3월') '3월', sum('3금액') '3금액', sum('4월') '4월', sum('4금액') '4금액', sum('5월') '5월', sum('5금액') '5금액', sum('6월') '6월', sum('6금액') '6금액', sum('7월') '7월', sum('7금액') '7금액', sum('8월') '8월', sum('8금액') '8금액', sum('9월') '9월', sum('9금액') '9금액', sum('10월') '10월', sum('10금액') '10금액', sum('11월') '11월', sum('11금액') '11금액', sum('12월') '12월', sum('12금액') '12금액', sum('1월'+'2월'+'3월'+'4월'+'5월'+'6월'+'7월'+'8월'+'9월'+'10월'+'11월'+'12월') 총건수, sum('1금액'+'2금액'+'3금액'+'4금액'+'5금액'+'6금액'+'7금액'+'8금액'+'9금액'+'10금액'+'11금액'+'12금액') 총금액 from (" + sql + ") a group by 총판명, 대리점명 ";
        hexcel2.Text = sql1;
        su.binding(gv대리점월매출, sql1);

        String sql2 = "select distinct 총판명, sum('1월') '1월', sum('1금액') '1금액', sum('2월') '2월', sum('2금액') '2금액', sum('3월') '3월', sum('3금액') '3금액', sum('4월') '4월', sum('4금액') '4금액', sum('5월') '5월', sum('5금액') '5금액', sum('6월') '6월', sum('6금액') '6금액', sum('7월') '7월', sum('7금액') '7금액', sum('8월') '8월', sum('8금액') '8금액', sum('9월') '9월', sum('9금액') '9금액', sum('10월') '10월', sum('10금액') '10금액', sum('11월') '11월', sum('11금액') '11금액', sum('12월') '12월', sum('12금액') '12금액', sum('1월'+'2월'+'3월'+'4월'+'5월'+'6월'+'7월'+'8월'+'9월'+'10월'+'11월'+'12월') 총건수, sum('1금액'+'2금액'+'3금액'+'4금액'+'5금액'+'6금액'+'7금액'+'8금액'+'9금액'+'10금액'+'11금액'+'12금액') 총금액 from (" + sql + ") a group by 총판명  ";
        hexcel1.Text = sql2;
        su.binding(gv총판월매출, sql2);

    }   
    #region gv비교2 클릭시
    protected void gv단말기리스트_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String ename = e.CommandName;
        String strval = (String)e.CommandArgument;

        if (ename == "log")
        {

        }
    }
    #endregion


    #region 빠른엑셀다운로드

    protected void bt엑셀다운로드_Click(object sender, EventArgs e)
    {
        fn_엑셀("매출조회", tbexcel.Text);
    }
    protected void fn_엑셀(String 임시파일명, String sql)
    {
        if (sql != "")
        {
            SgFramework.SgExcel se = new SgFramework.SgExcel();
            se.sql2excel(this.Page, sql, 임시파일명);
        }
        else
        {

            Response.Write("없다");
        }
    }

    #endregion

    #region 엑셀다운버튼
    protected void bt총판엑셀_Click(object sender, EventArgs e)
    {
        fn_엑셀("총판월별매출", hexcel1.Text);
    }

    protected void bt대리점엑셀_Click(object sender, EventArgs e)
    {
        fn_엑셀("대리점월별매출", hexcel2.Text);
    }

    protected void bt자판기엑셀_Click(object sender, EventArgs e)
    {
        fn_엑셀("자판기월별매출", hexcel3.Text);
    }
    #endregion
}
