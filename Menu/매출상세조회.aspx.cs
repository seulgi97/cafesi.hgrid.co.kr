using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Web.UI.HtmlControls;

public partial class Menu_매출상세조회 : System.Web.UI.Page
{
    SgFramework.SgUtil su = new SgFramework.SgUtil();

    public String idx = "";
    public String userid = "";
    public String level = "";
    public String passwd = "";
    public String usrtel = "";
    public String company_idx = "";
    public String agency_idx = "";
    public String branch_idx = "";
    public String pos_number = "";
    public String usrorg = "";
    public String grp_code = "";
    public String sql = "";
    public String RandomSeed = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        RandomSeed = this.ClientID;

        idx = su.ReqS(this.Page, "idx");
        userid = su.ReqS(this.Page, "userid");
        level = su.ReqS(this.Page, "level");
        passwd = su.ReqS(this.Page, "passwd");
        usrtel = su.ReqS(this.Page, "usrtel");
        company_idx = su.ReqS(this.Page, "company_idx");
        agency_idx = su.ReqS(this.Page, "agency_idx");
        branch_idx = su.ReqS(this.Page, "branch_idx");
        pos_number = su.ReqS(this.Page, "pos_number");
        usrorg = su.ReqS(this.Page, "usrorg");
        grp_code = su.ReqS(this.Page, "grp_code");

        btnFooter.Visible = false;

        if (su.Req(this.Page, "debug") != "")
        {
            선택_총판코드.Visible = true;
            선택_대리점코드.Visible = true;
        }

        if (!IsPostBack)
        {
            if (tb시작일.Text == "")
            {
                tb시작일.Text = DateTime.Now.ToString("yyyy-MM-dd");
                tb종료일.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }

            bt새로고침_Click(null, null);
        }
    }

    #region  오늘검색 

    protected void btnToday_Click(object sender, EventArgs e)
    {
        string 시작일 = DateTime.Now.ToString("yyyy-MM-dd");
        string 종료일 = DateTime.Now.ToString("yyyy-MM-dd");
        tb시작일.Text = 시작일;
        tb종료일.Text = 종료일;

        Search(시작일, 종료일);
    }
    #endregion

    #region 이번달 
    protected void btnMonth_Click(object sender, EventArgs e)
    {
        var 시작일 = DateTime.Now.AddDays(1 - DateTime.Now.Day);
        var 종료일 = 시작일.AddMonths(1).AddDays(-1);
        tb시작일.Text = 시작일.ToString("yyyy-MM-dd");
        tb종료일.Text = 종료일.ToString("yyyy-MM-dd");

        Search(tb시작일.Text.Trim(), tb종료일.Text.Trim());
    }

    #endregion

    #region 지난 달 
    protected void btnPrevMonth_Click(object sender, EventArgs e)
    {
        var 시작일 = DateTime.Now.AddMonths(-1).AddDays(1 - DateTime.Now.Day);
        var 종료일 = 시작일.AddMonths(1).AddDays(-1);
        tb시작일.Text = 시작일.ToString("yyyy-MM-dd");
        tb종료일.Text = 종료일.ToString("yyyy-MM-dd");

        Search(tb시작일.Text.Trim(), tb종료일.Text.Trim());
    }
    #endregion

    #region 새로고침 검색
    protected void bt새로고침_Click(object sender, EventArgs e)
    {
        String 시작일 = tb시작일.Text.Trim();
        String 종료일 = tb종료일.Text.Trim();
        Search(시작일, 종료일);
    }

    private void Search(string 시작일, string 종료일)
    {
        sql = @"select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS rownum, a.*, b.branch_cnt, c.machine_cnt, d.member_cnt , isnull(p.cnt,0) pay_cnt, isnull(p.pay_amount,0) pay_amount from
                ( SELECT  idx,name ,chief,telNo,mphone FROM agency ) a 
                LEFT OUTER JOIN (select distinct agency_idx, COUNT(*) branch_cnt from branch GROUP BY agency_idx) b ON a.idx= b.agency_idx
                LEFT OUTER JOIN (select distinct agency_idx, COUNT(*) machine_cnt from machine GROUP BY agency_idx) c ON a.idx= c.agency_idx
                left outer join ( SELECT distinct agency_idx, COUNT(*) member_cnt FROM member GROUP BY agency_idx  ) d on a.idx = d.agency_idx 
                left outer join ( select distinct agency_idx , count(*) as cnt,  sum(pay_amount) pay_amount from machine_pay with(nolock) where pay_result='0000' ";
        if (시작일 != "" && 종료일 != "")
        {
            sql += "and pay_date >='" + 시작일 + "' and pay_date < convert(varchar(10),dateadd(d,1,convert(datetime,'" + 종료일 + "')),121) ";
        }
        sql += @"group by agency_idx) p on a.idx=p.agency_idx
                order by a.name
                ";
        su.binding(gv총판, sql);

        lb총판수.Text = gv총판.Rows.Count.ToString();

        String 총판코드 = 선택_총판코드.Text.Trim();

        if (총판코드 != "")
        {
            sql = @"
                    select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS rownum, a.*, isnull(c.machine_cnt,0) machine_cnt, isnull(d.member_cnt,0) member_cnt, isnull(p.cnt,0) as pay_cnt, isnull(p.pay_amount,0) as pay_amount from ( select idx, agency_idx, name ,chief,telNo,mphone FROM branch) a 
                    LEFT OUTER JOIN ( select distinct branch_idx, COUNT(*) machine_cnt from machine GROUP BY branch_idx) c ON  a.idx= c.branch_idx
                    left outer join ( SELECT distinct branch_idx, COUNT(*) member_cnt FROM member GROUP BY branch_idx  ) d on  a.idx= d.branch_idx
                    left outer join ( select distinct branch_idx , count(*) as cnt, sum(pay_amount) as pay_amount from machine_pay with(nolock) where pay_result='0000' ";
            if (시작일 != "" && 종료일 != "")
            {
                sql += "and pay_date >='" + 시작일 + "' and pay_date < convert(varchar(10),dateadd(d,1,convert(datetime,'" + 종료일 + "')),121) ";
            }
            sql += " group by branch_idx) p on a.idx=p.branch_idx ";
            sql += " where a.agency_idx='" + 총판코드 + "' order by name ";

            su.binding(gv대리점, sql);

        }

        f_자판기리스트();

        if (총판코드 != "")
        {
            fn_매출상세내역();
        }
    }

    #endregion

    #region gv총판 선택시
    protected void gv총판_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String ename = e.CommandName;
        String strval = (String)e.CommandArgument;

        if (ename == "Select")
        {
            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);

            String 총판명 = ((LinkButton)row.FindControl("bt2")).Text;
            lb선택총판명.Text = 총판명;

            lb선택총판idx.Text = strval;
            선택_총판코드.Text = strval;
            선택_대리점코드.Text = "";
            lb선택대리점명.Text = 총판명+" 대리점 전체";
            lb선택대리점코드.Text = "0";
            gv대리점.SelectedIndex = -1;
            gv단말기리스트.SelectedIndex = -1;
            bt새로고침_Click(null, null);

            td대리점.Visible = true;
            td자판기.Visible = true;
        }

    }
    #endregion

    #region gv대리점 선택시
    protected void gv대리점_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String ename = e.CommandName;
        String strval = (String)e.CommandArgument;

        if (ename == "Select")
        {
            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
            String 대리점명 = ((LinkButton)row.FindControl("bt2")).Text;

            gv단말기리스트.SelectedIndex = -1;
            선택_대리점코드.Text = strval;
            bt새로고침_Click(null, null);
            td자판기.Visible = true;
            lb선택대리점명.Text = 대리점명;
            lb선택대리점코드.Text = strval;
        }

    }
    #endregion

    #region 매출상세내역
    /// <summary>
    /// 카드:카드
    /// 쿠폰:쿠폰
    /// 카드+쿠폰:복합
    /// </summary>
    public void fn_매출상세내역()
    {
        String 총판코드 = 선택_총판코드.Text;
        String 대리점코드 = 선택_대리점코드.Text;
        String 자판기코드 = 선택_자판기코드.Text;
        String 시작일 = tb시작일.Text.Trim();
        String 종료일 = tb종료일.Text.Trim();

        sql = "select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS rownum,a.idx,b.install_spot, a.machine_idx, a.pay_id, a.total_amount,case when a.coupon_use_yn='Y' then a.coupon_amount else 0 end coupon_amount, a.pay_amount, ";
        sql += "a.pay_tid,a.pay_result,a.pay_result_msg,a.pay_result_no,convert(varchar(19),a.pay_date,121) pay_date ";
        sql += ",case when a.coupon_use_yn='Y' then a.coupon_amount else 0 end coupon_amount, a.pay_amount";
        sql += ",case when  (a.total_amount > 0 and a.coupon_amount = 0) then '카드'";
        sql += "      when  (a.total_amount > 0 and  a.coupon_amount  > 0) then '복합'";
        sql += "      when  (a.total_amount - a.coupon_amount <= 0 and a.coupon_amount > 0) then '쿠폰'";
        sql += "      else 'N/A' end as 'pay_type'";

        sql += " from machine_pay a with(nolock) left outer join machine b with(nolock) on a.machine_idx=b.idx where a.pay_result= '0000' ";
        if (시작일 != "" && 종료일 != "")
        {
            sql += "and pay_date >='" + 시작일 + "' and pay_date < convert(varchar(10),dateadd(d,1,convert(datetime,'" + 종료일 + "')),121) ";
        }
        if (총판코드 != "" && 대리점코드 == "" && 자판기코드 == "")
        {
            sql += " and a.agency_idx='" + 총판코드 + "' ";
        }
        if (총판코드 != "" && 대리점코드 != "" && 자판기코드 == "")
        {
            sql += " and a.agency_idx='" + 총판코드 + "' and a.branch_idx='" + 대리점코드 + "' ";
        }
        if (총판코드 != "" && 대리점코드 != "" && 자판기코드 != "")
        {
            sql += " and a.machine_idx='" + 자판기코드 + "'  ";
        }
        sql += " order by pay_date desc ";

        su.binding(gv매출내역, sql);

        if (gv매출내역.DataSource != null && gv매출내역.Rows.Count > 0)
            btnFooter.Visible = true;
        else
            btnFooter.Visible = false;
    }

    private string GetSql매출상세내역()
    {
        String 총판코드 = 선택_총판코드.Text;
        String 대리점코드 = 선택_대리점코드.Text;
        String 자판기코드 = 선택_자판기코드.Text;
        String 시작일 = tb시작일.Text.Trim();
        String 종료일 = tb종료일.Text.Trim();

        string sql = "select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS 'No',a.idx as 'MID',b.install_spot as '설치장소', a.machine_idx as '자판기번호'";
        
        sql += ",convert(varchar(19),a.pay_date,121) as '결제일시' ";
        sql += ",case when  (a.total_amount > 0 and a.coupon_amount = 0) then '카드'";
        sql += "      when  (a.total_amount > 0 and  a.coupon_amount  > 0) then '복합'";
        sql += "      when  (a.total_amount - a.coupon_amount <= 0 and a.coupon_amount > 0) then '쿠폰'";
        sql += "      else 'N/A' end as '구분'";
        sql += ",a.pay_tid as TID, a.pay_amount as '카드결제',case when a.coupon_use_yn='Y' then a.coupon_amount else 0 end 쿠폰결제";
        sql += ",a.total_amount as '총 결제금액'";

        sql += " from machine_pay a with(nolock) left outer join machine b with(nolock) on a.machine_idx=b.idx where a.pay_result= '0000' ";
        if (시작일 != "" && 종료일 != "")
        {
            sql += "and pay_date >='" + 시작일 + "' and pay_date < convert(varchar(10),dateadd(d,1,convert(datetime,'" + 종료일 + "')),121) ";
        }
        if (총판코드 != "" && 대리점코드 == "" && 자판기코드 == "")
        {
            sql += " and a.agency_idx='" + 총판코드 + "' ";
        }
        if (총판코드 != "" && 대리점코드 != "" && 자판기코드 == "")
        {
            sql += " and a.agency_idx='" + 총판코드 + "' and a.branch_idx='" + 대리점코드 + "' ";
        }
        if (총판코드 != "" && 대리점코드 != "" && 자판기코드 != "")
        {
            sql += " and a.machine_idx='" + 자판기코드 + "'  ";
        }
        sql += " order by pay_date desc ";
        return sql;
    }
    #endregion

    #region 빠른엑셀다운로드

    protected void bt엑셀다운로드_Click(object sender, EventArgs e)
    {
        fn_엑셀("매출조회", GetSql매출상세내역());
    }
    protected void fn_엑셀(String 임시파일명, String sql)
    {
        if (sql != "")
        {
            SgFramework.SgExcel se = new SgFramework.SgExcel();
            se.sqlToExcel(this.Page, sql, 임시파일명);
        }
        else
        {
            Response.Write("없다");
        }
    }

    #endregion

    #region 자판기리스트
    public void f_자판기리스트()
    {
        String 시작일 = tb시작일.Text.Trim();
        String 종료일 = tb종료일.Text.Trim();

        sql = "SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS rownum, a.idx ,convert(varchar(10),a.install_date,121) 설치일, a.install_spot 설치장소, a.lte_router, a.관리자연락처, a.machine_stat, datediff(mi,최종보고일시,getdate()) 통신상태, ISNULL(p.pay_cnt, 0) AS 'pay_cnt', ISNULL(p.pay_amount,0) AS pay_amount, ISNULL(p.coupon_amount,0) as coupon_amount, ISNULL(p.total_amount,0) as total_amount ";
        sql += " FROM machine a with(nolock) ";
        sql += " left outer join ( select distinct machine_idx , count(*) as pay_cnt, sum(pay_amount) as pay_amount, sum(total_amount) as total_amount, sum(coupon_amount) as coupon_amount from machine_pay with(nolock) where pay_result='0000' ";
        if (시작일 != "" && 종료일 != "")
        {
            sql += "and pay_date >='" + 시작일 + "' and pay_date < convert(varchar(10),dateadd(d,1,convert(datetime,'" + 종료일 + "')),121) ";
        }
        sql += " group by machine_idx) p on a.idx=p.machine_idx ";
        sql += " where 1=1";

        if (선택_총판코드.Text.Trim() != "" && 선택_대리점코드.Text.Trim() == "")
        {
            sql += " and a.agency_idx='" + 선택_총판코드.Text + "' ";
        }
        if (선택_대리점코드.Text.Trim() != "")
        {
            sql += " and a.branch_idx='" + 선택_대리점코드.Text + "' ";
        }
        sql += " ORDER BY 설치장소";
        su.binding(gv단말기리스트, sql);

    }
    #endregion

    #region 단말기리스트 자판기RACK상태 표시
    protected void gv단말기리스트_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            Label lbt = (Label)e.Row.FindControl("machine_net");

            if (lbt == null) return;
            String machine_net = lbt.Text;

            if (machine_net != "" && su.sg_int(machine_net) <= 30)
            {
                // 정상
                string rpt = "▶" + machine_net + "분전 보고";
                SetDivision(e,2, "normal tooltip", "정상", rpt);
            }
            else if (machine_net != "" && su.sg_int(machine_net) > 31 && su.sg_int(machine_net) <= 10000)
            {
                // 점검요청
                SetDivision(e, 2, "reqchk tooltip", "점검요청", "");

                if (su.sg_int(machine_net) > 1440)
                {
                    string rpt  = "▶" +(su.sg_int(machine_net) / 60 / 24) + "일전 보고";

                    SetDivision(e, 2, "reqchk tooltip", "점검요청", rpt);
                }
                else
                {
                    string rpt2 = "▶" + (su.sg_int(machine_net) / 60) + "시간 전 보고";
                    SetDivision(e, 2, "reqchk tooltip", "점검요청", rpt2);
                }
            }
            else if (machine_net == "")
            {
                SetDivision(e, 2, "nohistory tooltip", "이력없음", "▶보고 이력이 없습니다");
            }
            else
            {
                // 장기미사용
                string rpt3 = "▶" + (su.sg_int(machine_net) / 60 / 24) + "일전 보고";
                SetDivision(e, 2, "nolonguse tooltip", "장기미사용", rpt3);
            }
        }
        catch (Exception ex)
        {
        }
        
        try
        {
            // 총 22 자리 
            // 1 ~  18 : 랙상태 , 정상 =0, 투출불량=1, 사용안함 = 2
            // 19 ~ 20 : 컵투출기 상태, 정상=0,불량=1,재고없음=2, 통신불량=3,사용안함 4
            // 21      : 출빙기 상태, 정상=0,급수불량=1,배수불량=2,저온불량=3,고온불량=4,온도과열=5,출구과냉=6,모터이상=7,제빙불량=8,정기점검=9,FAN이상 = F
            // 22      : 출빙기 일반상태,  정상=0, 불량=1, 통신불량 = 2

            Label lbt = (Label)e.Row.FindControl("machine_stat");
            if (lbt == null) return;

            String machine_stat = lbt.Text;

            if(machine_stat.Length == 22) // 전체 22 자릿수 유지 
            {
                string lackDigits = machine_stat.Substring(0,18); // lack상태정보 
                string cupDispenser1 = machine_stat.Substring(17,1); 
                string cupDispenser2 = machine_stat.Substring(18,1); // 현재 사용안함
                string iceMachineStat = machine_stat.Substring(20,1); // 출빙기 상태 
                string iceMachineCommonStat = machine_stat.Substring(21,1); // 출빙기 상태 

                int i = 0;
                string rpt = "";
                if (su.CntBytesLen(machine_stat) > 10 && lackDigits.Contains("1") == true) // 투출불량
                {
                    // 점검요청
                    rpt = Get랙상태정보(machine_stat);
                    SetDivision(e, 3, "reqchk tooltip", "점검요청", rpt);
                    i++;

                }
                else if (su.CntBytesLen(machine_stat) > 10 && lackDigits.Contains("1") == false) // 정상
                {
                    rpt = string.Format("▶랙상태(1~18) 정상, 컵투출기 정상,출빙기 정상");
                    SetDivision(e, 3, "normal tooltip", "정상", rpt);
                }
                else // 랙 사용안함
                {
                    rpt = "▶사용중인 랙이 없습니다.";                    
                    SetDivision(e, 3, "nouse tooltip", "사용안함", rpt);
                    i++;
                }

                if (i > 0)
                {
                    //점검요청
                    SetDivision(e, 3, "reqchk tooltip", "점검요청", rpt);
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    /// <summary>
    /// 통신 및 랙 상태에 대한 정보 표시 
    /// </summary>
    /// <param name="e"></param>
    /// <param name="idx"></param>
    /// <param name="className"></param>
    /// <param name="title"></param>
    /// <param name="tooltipText"></param>
    private static void SetDivision(GridViewRowEventArgs e, int idx, string className, string title, string tooltipText)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TableCell statCell = e.Row.Cells[idx];
            HtmlGenericControl div = new HtmlGenericControl("DIV");
            div.Attributes.Add("class", className);          
            div.InnerHtml = title;

            HtmlGenericControl spanCtrl = new HtmlGenericControl("span");
            spanCtrl.Attributes.Add("class", "tooltiptext tooltip-left");
            spanCtrl.InnerHtml = tooltipText;
            div.Controls.Add(spanCtrl);           
            statCell.Text = string.Empty;
            statCell.Controls.Add(div);
        }
    }

    private string Get랙상태정보(string lackInfo)
    {
        char[] arr = lackInfo.ToCharArray(0,18);
        int cnt = arr.Length;
        string 투출불량 = "";
        string 미사용랙 = "";
        string 컵투출기 = "";
        string 출빙기 = "";

        StringBuilder sb = new StringBuilder();
        
        for(int i=0; i<cnt; i++)
        {
            if(arr[i] == '1') // 투출불량
            {
                sb.Append((i + 1).ToString());
                sb.Append(",");
            }
        }
        if (sb.ToString().Length > 0)
        {
            투출불량 = "▶투출불량 랙:" + sb.ToString().Substring(0, sb.Length - 1);
        }
        sb.Clear();

        for (int i = 0; i < cnt; i++) 
        {
            if (arr[i] == '2') // 미사용랙
            {
                sb.Append((i + 1).ToString());
                sb.Append(",");
            }
        }

        if (sb.ToString().Length > 0)
        {
            미사용랙 = "<br>" + "▶미사용 랙:" + sb.ToString().Substring(0, sb.Length - 1);
        }
        sb.Clear();

        arr = lackInfo.ToCharArray(17,1); //투불불량
        string status = "";
        switch (arr[0])
        {          
            case '1':
                status = "불량";
                break;
            case '2':
                status = "재고없음";
                break;
            case '3':
                status = "통신불량";
                break;
            case '4':
                status = "사용안함";
                break;
        }

        if(status.Length > 0)
        {
            컵투출기 = "<br>" + "▶컵투출기:" + status;
        }

        arr = lackInfo.ToCharArray(20, 1); // 출빙기
        status = "";
        switch (arr[0])
        {
            case '1':
                status = "급수불량";
                break;
            case '2':
                status = "배수불량";
                break;
            case '3':
                status = "저온불량";
                break;
            case '4':
                status = "고온불량";
                break;
            case '5':
                status = "온도과열";
                break;
            case '6':
                status = "출구과냉";
                break;
            case '7':
                status = "모터이상";
                break;
            case '8':
                status = "제빙불량";
                break;
            case '9':
                status = "정기점검";
                break;
            case 'F':
                status = "Fan이상";
                break;
        }

        if(status.Length > 0)
        {
            출빙기 = "<Br>" + "▶출빙기:" + status;
        }

        arr = lackInfo.ToCharArray(21, 1); // 출빙기 일반상태
        status = "";
        switch (arr[0])
        {
            case '1':
                status = "불량";
                break;
            case '2':
                status = "통신불량";
                break;        
        }

        if(status.Length > 0)
        {
            출빙기 += status;
        }

        return 투출불량 + 미사용랙 + 컵투출기 + 출빙기;
    }

    #endregion

    #region 자판기 선택시
    protected void gv단말기리스트_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String ename = e.CommandName;
        String strval = (String)e.CommandArgument;

        if (ename == "Select")
        {
            선택_자판기코드.Text = strval;
            bt새로고침_Click(null, null);
        }

    }
    #endregion

}
