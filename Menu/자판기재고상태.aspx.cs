using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Menu_자판기재고상태 : System.Web.UI.Page
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

        String 대리점 = dd대리점명.SelectedValue;
        sql = "SELECT distinct replace(c.`name`,'(주)','') 업체명, a.branch_code FROM machines a LEFT OUTER JOIN agencies b ON a.company_code = b.company_code AND a.agency_code = b.code LEFT OUTER JOIN branches c ON a.company_code = c.company_code AND a.agency_code = c.agency_code AND a.branch_code = c.code";
        sql += " where a.agency_code='" + 업체 + "'  ";
        sql += " group by replace(c.`name`,'(주)','') , a.branch_code order by 업체명";
        su.binding(dd대리점명, sql, "-대리점전체");
        su.setDropDownList(dd대리점명, 대리점);


        String 자판기 = dd자판기명.SelectedValue;
        sql = "SELECT distinct concat(machine_code, ' (',a.install_spot,')'), a.machine_code FROM machines a LEFT OUTER JOIN agencies b ON a.company_code = b.company_code AND a.agency_code = b.code LEFT OUTER JOIN branches c ON a.company_code = c.company_code AND a.agency_code = c.agency_code AND a.branch_code = c.code";
        sql += " where a.agency_code='" + 업체 + "'  ";
        sql += " and a.branch_code='" + 대리점 + "'  ";
        sql += " group by  concat(machine_code, ' (',a.install_spot,')'), a.machine_code order by a.install_spot";

        su.binding(dd자판기명, sql, "-자판기전체");
        su.setDropDownList(dd자판기명, 자판기);


        if (!IsPostBack)
        {
            bt새로고침_Click(null, null);
        }
    }

    #region 자판기 리스트 조회
    protected void bt새로고침_Click(object sender, EventArgs e)
    {
        String 검색어 = su.sg_db_query(tb검색어.Text.Trim(), true);

        sql = "SELECT a.idx, a.machine_code `자판기번호` ,date_format(a.install_date,'%Y%m%d') `설치일`,b.`name` 업체명, c.name `대리점명`,  a.install_spot `설치장소`,b.addr `설치주소`, a.lte_router  ";
        sql += ", date_format(d.up_time,'%Y%m%d %H%i%s') 최종보고, date_format(d.ma_time,'%Y%m%d %H%i%s') 최종결제 , date_format(d.m8_time,'%Y%m%d %H%i%s') 최종가격, date_format(d.m9_time,'%Y%m%d %H%i%s') 최종재고  ";
        sql += " ,TIMESTAMPDIFF(HOUR,d.up_time, now()) 경과시간, m8,m9,ma ";
        sql += " , case when substring(m9,1,1)= '0' then '정상' else '오류' end VAN상태 ";
        sql += ", case when substring(m9,2,1)='0' then '정상' else '오류' end 태블릿상태 ";
        sql += ", case when substring(m9,3,1)='0' then '정상' else '오류' end RS232상태 ";
        sql += ", case when substring(m9,4,1)='0' then '정상' else '오류' end 컨트롤러상태 ";
        sql += " FROM machines a LEFT OUTER JOIN agencies b ON a.company_code = b.company_code AND a.agency_code = b.code LEFT OUTER JOIN branches c ON a.company_code = c.company_code AND a.agency_code = c.agency_code AND a.branch_code = c.code ";
        sql += " left outer join machines_last_status d on a.machine_code=d.machine_code";
        sql += " where 1=1";
        if (agency_code != "00")
        {
            sql += " and a.agency_code='" + agency_code + "'  ";
        }
        if (branch_code != "0000")
        {
            sql += " and a.branch_code='" + branch_code + "'  ";
        }
        if (cb상태오류건.Checked == true)
        {
            sql += " and ( substr(m9,9,1)=1 ) ";
        }
        if (dd업체명.SelectedValue != "")
        {
            sql += " and a.agency_code = '"+ dd업체명.SelectedValue +"' ";
        }
        if (dd대리점명.SelectedValue != "")
        {
            sql += " and a.branch_code = '" + dd대리점명.SelectedValue + "' ";
        }
        if (dd자판기명.SelectedValue != "")
        {
            sql += " and a.machine_code = '" + dd자판기명.SelectedValue + "' ";
        }
        if (검색어 != "")
        {
            sql += " and ( a.machine_code like '%" + 검색어 + "%' or b.`name` like '%" + 검색어 + "%' or c.`name` like '%" + 검색어 + "%' or a.install_spot like '%" + 검색어 + "%' or b.addr like '%" + 검색어 + "%'  ) ";
        }
        sql += " ORDER BY 자판기번호";
        su.binding(gv단말기리스트, sql);

        lb단말기수.Text = gv단말기리스트.Rows.Count.ToString();
    }
    #endregion

    #region gv비교2 클릭시
    protected void gv단말기리스트_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String ename = e.CommandName;
        String strval = (String)e.CommandArgument;

    }
    #endregion


    #region 자판기 투출구 상태 
    protected void gv단말기리스트_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label r0 = (Label)e.Row.FindControl("r0");
                Label r1 = (Label)e.Row.FindControl("r1");
                Label r2 = (Label)e.Row.FindControl("r2");
                Label r3 = (Label)e.Row.FindControl("r3");
                Label r4 = (Label)e.Row.FindControl("r4");
                Label r5 = (Label)e.Row.FindControl("r5");
                Label r6 = (Label)e.Row.FindControl("r6");
                Label r7 = (Label)e.Row.FindControl("r7");
                Label r8 = (Label)e.Row.FindControl("r8");
                Label r9 = (Label)e.Row.FindControl("r9");

                r1.ForeColor = (r0.Text.Substring(9, 1) == "0" ? System.Drawing.Color.Black : System.Drawing.Color.Red);
                r2.ForeColor = (r0.Text.Substring(15, 1) == "0" ? System.Drawing.Color.Black : System.Drawing.Color.Red);
                r3.ForeColor = (r0.Text.Substring(21, 1) == "0" ? System.Drawing.Color.Black : System.Drawing.Color.Red);
                r4.ForeColor = (r0.Text.Substring(27, 1) == "0" ? System.Drawing.Color.Black : System.Drawing.Color.Red);
                r5.ForeColor = (r0.Text.Substring(33, 1) == "0" ? System.Drawing.Color.Black : System.Drawing.Color.Red);
                r6.ForeColor = (r0.Text.Substring(39, 1) == "0" ? System.Drawing.Color.Black : System.Drawing.Color.Red);
                r7.ForeColor = (r0.Text.Substring(45, 1) == "0" ? System.Drawing.Color.Black : System.Drawing.Color.Red);
                r8.ForeColor = (r0.Text.Substring(51, 1) == "0" ? System.Drawing.Color.Black : System.Drawing.Color.Red);
                r9.ForeColor = (r0.Text.Substring(57, 1) == "0" ? System.Drawing.Color.Black : System.Drawing.Color.Red);
            }

        }
        catch (Exception ex) {
            Response.Write(ex.Message.ToString());
        }

    }

    #endregion
}
