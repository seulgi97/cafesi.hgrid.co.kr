using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Menu_상품매출통계 : System.Web.UI.Page
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
            sql = " SELECT distinct bb.`name` 상품명, sum(투출) 갯수, sum(가격) 매출 from (  ";
            for (int i = 1; i < 9; i++)
            {
                sql += " SELECT DISTINCT 총판명,대리점명,자판기번호,상품" + i.ToString() + " 상품, sum(상품" + i.ToString() + "투출)투출, sum(상품" + i.ToString() + "가격)가격 FROM tbl_pay_log WHERE  결제일시 >= STR_TO_DATE('" + DateTime.Now.ToString("yyyy-MM") + "-01', '%Y-%m-%d') AND 결제일시 < STR_TO_DATE('" + DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-01', '%Y-%m-%d')  ";
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
            sql += " ) aa left outer join coffees bb on aa.상품=bb.code group by 상품,bb.`name` order by 갯수 desc limit 10";
            su.binding(gv당월상품, sql);

            sql = " SELECT distinct bb.`name` 상품명, sum(투출) 갯수, sum(가격) 매출 from (  ";
            for (int i = 1; i < 9; i++)
            {
                sql += " SELECT DISTINCT 총판명,대리점명,자판기번호,상품" + i.ToString() + " 상품, sum(상품" + i.ToString() + "투출)투출, sum(상품" + i.ToString() + "가격)가격 FROM tbl_pay_log WHERE  결제일시 >= STR_TO_DATE('" + DateTime.Now.AddMonths(-12).ToString("yyyy-MM") + "-01', '%Y-%m-%d')  ";
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
            sql += " ) aa left outer join coffees bb on aa.상품=bb.code group by 상품,bb.`name` order by 갯수 desc limit 10";
            su.binding(gv년간상품, sql);

            sql = " SELECT distinct bb.`name` 상품명, sum(투출) 갯수, sum(가격) 매출 from (  ";
            for (int i = 1; i < 9; i++)
            {
                sql += " SELECT DISTINCT 총판명,대리점명,자판기번호,상품" + i.ToString() + " 상품, sum(상품" + i.ToString() + "투출)투출, sum(상품" + i.ToString() + "가격)가격 FROM tbl_pay_log WHERE  결제일시 >= STR_TO_DATE('" + DateTime.Now.AddMonths(-3).ToString("yyyy-MM") + "-01', '%Y-%m-%d')  ";
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
            sql += " ) aa left outer join coffees bb on aa.상품=bb.code group by 상품,bb.`name` order by 갯수 desc limit 10";
            su.binding(gv3개월상품, sql);

            bt새로고침_Click(null, null);
        }
        else
        {

        }
    }
    protected void bt새로고침_Click(object sender, EventArgs e)
    {
        String 검색어 = su.sg_db_query(tb검색어.Text.Trim(), true);
        String 년도 = dd매출년도.SelectedValue;

        sql = " SELECT distinct 총판명,대리점명,자판기번호,상품,bb.`name` 상품명, sum(투출) 투출, sum(가격) 가격 from (  ";
        for(int i = 1; i < 9; i++)
        {
            sql += " SELECT DISTINCT 총판명,대리점명,자판기번호,상품"+i.ToString()+ " 상품, sum(상품" + i.ToString() + "투출)투출, sum(상품" + i.ToString() + "가격)가격 FROM tbl_pay_log WHERE  결제일시 >= STR_TO_DATE('" + 년도 + "-01-01', '%Y-%m-%d') AND 결제일시 < DATE_ADD(STR_TO_DATE('" + 년도 + "-01-01', '%Y-%m-%d'), INTERVAL 1 YEAR)  ";
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
            sql += " GROUP BY 상품" + i.ToString() + " ";
            if (i < 8)
            {
                sql += " union all ";
            }
        }
        sql += " ) aa left outer join coffees bb on aa.상품=bb.code group by 총판명,대리점명,자판기번호,상품,bb.`name` order by 총판명,대리점명,가격 desc";

        hexcel3.Text = sql;
        su.binding(gv월매출리스트, sql);

        String sql1 = "select distinct 총판명,대리점명,자판기번호,상품,상품명, sum(투출) 투출, sum(가격) 가격 from (" + sql+ ") a group by 총판명,대리점명,상품,상품명 order by 총판명,대리점명,가격 desc ";
        hexcel2.Text = sql1;
        su.binding(gv대리점월매출, sql1);

        String sql2 = "select distinct 총판명,상품,상품명, sum(투출) 투출, sum(가격) 가격 from (" + sql + ") a group by 총판명,상품,상품명 order by 총판명,가격 desc ";
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
