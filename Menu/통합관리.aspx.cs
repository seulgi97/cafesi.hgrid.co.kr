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
using System.Net;
using System.IO;

public partial class Menu_총판대리점관리 : System.Web.UI.Page
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
    public string machine_idx = "";

    public String pos_number = "";
    public String usrorg = "";
    public String grp_code = "";
    public String sql = "";
    public String RandomSeed = "";

    private readonly string[] imageMimeTypes = new[] { "image/jpg", "image/jpeg", "image/png" };

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

        if (su.Req(this.Page, "debug") != "")
        {
            선택_총판코드.Visible = true;
            선택_대리점코드.Visible = true;
        }

        String 총판코드3 = 자판기등록_총판.SelectedValue;
        sql = "SELECT name,idx from agency ORDER BY name";
        su.binding(자판기등록_총판, sql, "-총판전체");
        if (총판코드3 == "") { 총판코드3 = 선택_총판코드.Text; }
        su.setDropDownList(자판기등록_총판, 총판코드3);

        String 대리점코드3 = 자판기등록_대리점.SelectedValue;
        sql = "SELECT name,idx from branch ";
        sql += " where agency_idx='" + 총판코드3 + "' ";
        sql += " ORDER BY name";
        su.binding(자판기등록_대리점, sql, "-대리점전체");
        su.setDropDownList(자판기등록_대리점, 대리점코드3);

        String 총판코드2 = 자판기수정_총판.SelectedValue;
        sql = "SELECT name,idx from agency ORDER BY name";
        su.binding(자판기수정_총판, sql, "-총판전체");
        su.setDropDownList(자판기수정_총판, 총판코드2);

        String 대리점코드2 = 자판기수정_대리점.SelectedValue;
        sql = "SELECT name,idx from branch ";
        sql += " where agency_idx='" + 총판코드2 + "' ";
        sql += " ORDER BY name";
        su.binding(자판기수정_대리점, sql, "-대리점전체");
        su.setDropDownList(자판기수정_대리점, 대리점코드2);

        if (!IsPostBack)
        {
            bt새로고침_Click(null, null);

            BindBannerSrchCondition();
            BindDefaultType();
            BindBannerOperatingTime();
            SetDefaultDate();
            SetBannerUseYn();
            
        }
    }

    #region 새로고침 검색
    protected void bt새로고침_Click(object sender, EventArgs e)
    {

        sql = @"select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS rownum, a.*, b.branch_cnt, c.machine_cnt, d.member_cnt  from
( SELECT  idx,name ,chief,telNo,mphone FROM agency ) a 
LEFT OUTER JOIN (select distinct agency_idx, COUNT(*) branch_cnt from branch GROUP BY agency_idx) b ON a.idx= b.agency_idx
LEFT OUTER JOIN (select distinct agency_idx, COUNT(*) machine_cnt from machine GROUP BY agency_idx) c ON a.idx= c.agency_idx
left outer join ( SELECT distinct agency_idx, COUNT(*) member_cnt FROM member GROUP BY agency_idx  ) d on a.idx = d.agency_idx order by a.name
";
        su.binding(gv총판, sql);

        lb총판수.Text = gv총판.Rows.Count.ToString();

        String 총판코드 = 선택_총판코드.Text.Trim();

        if (총판코드 != "")
        {
            sql = @"
select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS rownum, a.*, isnull(c.machine_cnt,0) machine_cnt, isnull(d.member_cnt,0) member_cnt from ( select idx, agency_idx, name ,chief,telNo,mphone FROM branch) a 
LEFT OUTER JOIN ( select distinct branch_idx, COUNT(*) machine_cnt from machine GROUP BY branch_idx) c ON  a.idx= c.branch_idx
left outer join ( SELECT distinct branch_idx, COUNT(*) member_cnt FROM member GROUP BY branch_idx  ) d on  a.idx= d.branch_idx
";
            sql += " where a.agency_idx='" + 총판코드 + "' order by name ";

            su.binding(gv대리점, sql);
        }

        String 대리점코드 = 선택_대리점코드.Text.Trim();

        f_userlist();
        f_자판기리스트();

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
            lb선택명.Text = 총판명;
            gv대리점.SelectedIndex = -1;
            bt새로고침_Click(null, null);

            td대리점.Visible = true;
            td사용자.Visible = true;
        }

        if (ename == "agency_up")
        {
            선택_총판코드.Text = strval;
            bt총판수정_Click(null, null);
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

            선택_대리점코드.Text = strval;
            lb선택명.Text = 대리점명;
            bt새로고침_Click(null, null);
        }

        if (ename == "branch_up")
        {
            선택_대리점코드.Text = strval;
            bt대리점수정_Click(null, null);
        }
    }
    #endregion

    #region gv사용자 선택시
    protected void gv사용자_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String ename = e.CommandName;
        String strval = (String)e.CommandArgument;

        if (ename == "Select")
        {
            sql = "select userid,usernm,usrtel,mphone,lev,agency_idx,branch_idx,pos_number,usrorg from member where userid='" + strval + "' ";
            DataView dv = su.SqlDvQuery(sql);
            if (dv != null && dv.Count > 0)
            {

                String 총판코드3 = dv[0]["agency_idx"].ToString();
                sql = "SELECT name,idx from agency ORDER BY name";
                su.binding(사용자수정_총판선택, sql, "-총판전체");
                su.setDropDownList(사용자수정_총판선택, 총판코드3);

                String 대리점코드3 = dv[0]["branch_idx"].ToString();
                sql = "SELECT name,idx from branch ";
                sql += " where agency_idx='" + 총판코드3 + "' ";
                sql += " ORDER BY name";
                su.binding(사용자수정_대리점선택, sql, "-대리점전체");
                su.setDropDownList(사용자수정_대리점선택, 대리점코드3);

                String 자판기코드3 = 사용자수정_자판기선택.SelectedValue;
                sql = "SELECT install_spot,pos_number from machine ";
                sql += " where agency_idx='" + 총판코드3 + "' ";
                sql += " and branch_idx='" + 대리점코드3 + "' ";
                sql += " ORDER BY install_spot";
                su.binding(사용자수정_자판기선택, sql, "-자판기전체");
                su.setDropDownList(사용자수정_자판기선택, 자판기코드3);

                사용자수정_아이디.Text = strval;
                사용자수정_이름.Text = dv[0]["usernm"].ToString();
                사용자수정_일반전화.Text = dv[0]["usrtel"].ToString();
                사용자수정_휴대폰.Text = dv[0]["mphone"].ToString();
                사용자수정_소속.Text = dv[0]["usrorg"].ToString();

                su.setDropDownList(사용자수정_총판선택, dv[0]["agency_idx"].ToString());
                su.setDropDownList(사용자수정_대리점선택, dv[0]["branch_idx"].ToString());
                su.setDropDownList(사용자수정_자판기선택, dv[0]["pos_number"].ToString());

                bt새로고침_Click(null, null);
                div_사용자수정.Visible = true;
            }

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

    #region 총판등록 
    protected void bt총판등록_Click(object sender, EventArgs e)
    {
        div_총판등록.Visible = true;
    }

    protected void bt총판등록닫기_Click(object sender, EventArgs e)
    {
        div_총판등록.Visible = false;
    }

    protected void bt총판등록완료_Click(object sender, EventArgs e)
    {
        String 총판명 = su.sg_db_query(총판등록_총판명.Text.Trim()).ToUpper();
        String 사업자번호 = su.sg_db_query(총판등록_사업자번호.Text.Trim()).Replace("-", "");
        String 대표자명 = su.sg_db_query(총판등록_대표자명.Text.Trim());
        String 전화번호 = su.sg_db_query(총판등록_전화번호.Text.Trim()).Replace("-", "");
        String 휴대폰번호 = su.sg_db_query(총판등록_휴대폰번호.Text.Trim()).Replace("-", "");
        String 주소 = su.sg_db_query(총판등록_주소.Text.TrimEnd().TrimStart(), true);

        if (총판명 == "")
        {
            총판등록_결과.InnerText = "총판명을 입력해주세요";
            return;
        }
        if (사업자번호 == "")
        {
            총판등록_결과.InnerText = "사업자번호를 입력해주세요";
            return;
        }
        if (su.isNumeric(사업자번호) == false || 사업자번호.Replace("-", "") == "" || 사업자번호.Replace("-", "").Length != 10)
        {
            총판등록_결과.InnerText = "사업자번호는 숫자10자리로 입력해주세요";
            return;
        }

        sql = "insert into agency(company_idx,name,bizNo,chief,telNo,mphone,addr1) ";
        sql += " values('1','" + 총판명 + "','" + 사업자번호 + "','" + 대표자명 + "','" + 전화번호 + "','" + 휴대폰번호 + "','" + 주소 + "'); ";
        su.SqlNoneQuery(sql);

        총판등록_결과.InnerText = "등록완료되었습니다";

        bt새로고침_Click(null, null);
        div_총판등록.Visible = false;
    }
    #endregion

    #region 총판수정/삭제

    protected void bt총판수정_Click(object sender, EventArgs e)
    {
        String 총판코드 = 선택_총판코드.Text.Trim();
        if (총판코드 == "")
        {
            su.sg_alert(this.Page, "총판을 선택해주세요");
            return;
        }

        sql = "select idx,company_idx,name,bizNo,chief,telNo,mphone,addr1 from agency where idx='" + 총판코드 + "' ";
        DataView dv = su.SqlDvQuery(sql);
        if (dv != null && dv.Count > 0)
        {
            총판수정_코드.Text = dv[0]["idx"].ToString();
            총판수정_총판명.Text = dv[0]["name"].ToString();
            총판수정_사업자번호.Text = dv[0]["bizNo"].ToString();
            총판수정_대표자명.Text = dv[0]["chief"].ToString();
            총판수정_전화번호.Text = dv[0]["telNo"].ToString();
            총판수정_휴대폰번호.Text = dv[0]["mphone"].ToString();
            총판수정_주소.Text = dv[0]["addr1"].ToString();
            div_총판수정.Visible = true;
        }
    }

    protected void bt총판수정닫기_Click(object sender, EventArgs e)
    {
        div_총판수정.Visible = false;
    }

    #region 총판 배너관리등록 창 호출
    /// <summary>
    /// 해당 대리점으로 등록된 배너정보 조회
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void bt총판배너등록완료_Click(object sender, EventArgs e)
    {
        var agencyCode = su.sg_db_query(총판수정_코드.Text.Trim());
        GetBannerList(agencyCode);
    }

    /// <summary>
    /// Banner 정보 가져오기
    /// </summary>
    /// <param name="agencyCode"></param>
    private void GetBannerList(string agencyCode)
    {
        div_배너관리.Visible = true;

        var sql = @"select company_idx from dbo.agency where idx = {0}";
        var wSql = string.Format(sql, Convert.ToInt32(agencyCode));

        var dv = su.SqlDvQuery(wSql);
        if (dv != null && dv.Count > 0)
        {
            hCompanyIdx.Value = dv[0]["company_idx"].ToString();
            hAgencyIdx.Value = agencyCode;
            hBranchIdx.Value = null;
            hMachineIdx.Value = null;
        }
        else
        {
            throw new Exception("총판 코드를 찾을 수가 없습니다.");
        }

        try
        {
            sql = @"select ROW_NUMBER() over (order by (select 1)) AS rownum
                        , kid
                        , company_idx
                        , agency_idx
                        , branch_idx
                        , machine_idx
                        , bannertype as btype
                        , title
                        , timeinterval
                        , (startdate + '~' + enddate) as opdate
                        , imgurl
                        , useyn
                        from dbo.tbl_banner
                        where 1 = 1
                        and agency_idx = {0}                        
                        order by startdate ";

            su.binding(gv배너리스트, string.Format(sql, agencyCode));
        }
        catch (Exception ex)
        {
            su.sg_alert(this.Page, ex.Message);
        }
    }
    #endregion

    #region 대리점 배너등록 창 
    protected void bt대리점배너조회_Click(object sender, EventArgs e)
    {   
        div_배너관리.Visible = true;

        hCompanyIdx.Value = hCompanyIdx3.Value;
        hAgencyIdx.Value = hAgencyIdx3.Value;
        hBranchIdx.Value = string.IsNullOrEmpty(hBranchIdx3.Value) == true ? "0" : hBranchIdx3.Value;
        hMachineIdx.Value = string.IsNullOrEmpty(hMachineIdx3.Value) == true ? "0" : hMachineIdx3.Value;

        try
        {
            var sql = @"select ROW_NUMBER() over (order by (select 1)) AS rownum
                        , kid
                        , company_idx
                        , agency_idx
                        , branch_idx
                        , machine_idx
                        , bannertype as btype
                        , title
                        , timeinterval
                        , (startdate + '~' + enddate) as opdate
                        , imgurl
                        , useyn
                        from dbo.tbl_banner
                        where 1 = 1
                        and useyn = ISNULL(useyn, useyn)
                        where company_idx = {0}
                        and agency_idx = {1}
                        and branch_idx = {2}
                        order by startdate ";

            su.binding(gv배너리스트, string.Format(sql, Convert.ToInt32(hCompanyIdx3.Value) , Convert.ToInt32(hAgencyIdx3.Value), Convert.ToInt32(hBranchIdx.Value)));
        }
        catch (Exception ex)
        {
            su.sg_alert(this.Page, ex.Message);
        }
    }
    #endregion

    #region 장비별 배너등록 
    protected void Get장비별배너등록조회(int machineCode)
    {
        div_배너관리.Visible = true;

        var sqlOne = @"select idx, company_idx, agency_idx, branch_idx from dbo.machine where idx = {0}";
        var wSql = string.Format(sqlOne, machineCode);

        var dv = su.SqlDvQuery(wSql);

        var companyCode = 0;
        var agencyCode = 0;
        var branchCode = 0;
       
        if(dv != null && dv.Count > 0)
        {
            companyCode = Convert.ToInt32(dv[0]["company_idx"].ToString());
            agencyCode = Convert.ToInt32(dv[0]["agency_idx"].ToString());
            branchCode = Convert.ToInt32(dv[0]["branch_idx"].ToString());

            hCompanyIdx.Value = companyCode.ToString();
            hAgencyIdx.Value = agencyCode.ToString();
            hBranchIdx.Value = branchCode.ToString();
            hMachineIdx.Value = machineCode.ToString();
        }

        try
        {
            var sql = @"select ROW_NUMBER() over (order by (select 1)) AS rownum
                        , kid
                        , company_idx
                        , agency_idx
                        , branch_idx
                        , machine_idx
                        , bannertype as btype
                        , title
                        , timeinterval
                        , (startdate + '~' + enddate) as opdate
                        , imgurl
                        , useyn
                        from dbo.tbl_banner        
                        where company_idx = {0}
                        and agency_idx = {1}
                        and branch_idx = {2}
                        and machine_idx = {3}
                        order by startdate ";

            su.binding(gv배너리스트, string.Format(sql, companyCode, agencyCode, branchCode, machineCode));
        }
        catch (Exception ex)
        {
            su.sg_alert(this.Page, ex.Message);
        }
    }
    #endregion 


    #region 배너등록 창
    protected void btnBannerRegistration_Click(object sender, EventArgs e)
    {
        div_bannerRegistration.Visible = true;
        hCompanyIdx2.Value = hCompanyIdx.Value;
        hAgencyIdx2.Value = hAgencyIdx.Value;
        hBranchIdx2.Value = hBranchIdx.Value;
        hMachineIdx2.Value = hMachineIdx.Value;

        VisibleForRegisterBanner();
        initRegisterForBanner();
    }

    // 배너등록창 값 초기화 
    private void initRegisterForBanner()
    {
        rdoHome.Checked = true;
        txtBannerTitle.Text = "";
        // lblKid.Text = "";
        //bannerStartDate.Text = "";
        //bannerEndDate.Text = "";
        rdoUseY.Checked = true;
        lblBannerUpload.Text = "";
        lblBannerUpload2.Text = "";
    }
    #endregion
    #region 배너등록 창 
    protected void btnCloseBannerRegistration_Click(object sender, EventArgs e)
    {
        div_배너관리.Visible = false;
    }
    #endregion

    protected void bt총판수정완료_Click(object sender, EventArgs e)
    {
        String 코드 = su.sg_db_query(총판수정_코드.Text.Trim());
        String 총판명 = su.sg_db_query(총판수정_총판명.Text.Trim()).ToUpper();
        String 사업자번호 = su.sg_db_query(총판수정_사업자번호.Text.Trim()).Replace("-", "");
        String 대표자명 = su.sg_db_query(총판수정_대표자명.Text.Trim());
        String 전화번호 = su.sg_db_query(총판수정_전화번호.Text.Trim()).Replace("-", "");
        String 휴대폰번호 = su.sg_db_query(총판수정_휴대폰번호.Text.Trim()).Replace("-", "");
        String 주소 = su.sg_db_query(총판수정_주소.Text.TrimEnd().TrimStart(), true);

        if (총판명 == "")
        {
            총판수정_결과.InnerText = "총판명을 입력해주세요";
            return;
        }
        if (사업자번호 == "")
        {
            총판수정_결과.InnerText = "사업자번호를 입력해주세요";
            return;
        }
        if (su.isNumeric(사업자번호) == false || 사업자번호.Replace("-", "") == "" || 사업자번호.Replace("-", "").Length != 10)
        {
            총판수정_결과.InnerText = "사업자번호는 숫자10자리로 입력해주세요";
            return;
        }
        sql = "update agency set ";
        sql += "  company_idx='1',name='" + 총판명 + "',bizNo='" + 사업자번호 + "',chief='" + 대표자명 + "',telNo='" + 전화번호 + "',mphone='" + 휴대폰번호 + "',addr1='" + 주소 + "'  ";
        sql += " where idx='" + 코드 + "' ";
        su.SqlNoneQuery(sql);

        총판수정_결과.InnerText = "수정완료되었습니다";

        bt새로고침_Click(null, null);

    }

    protected void bt총판삭제완료_Click(object sender, EventArgs e)
    {
        String cnt = "";
        String 총판코드 = 선택_총판코드.Text.Trim();
        if (총판코드 == "")
        {
            su.sg_alert(this.Page, "총판을 선택해주세요");
            return;
        }

        sql = "select count(*) from branch where agency_idx='" + 총판코드 + "' ";
        cnt = su.SqlFieldQuery(sql);

        if (cnt != "0")
        {
            su.sg_alert(this.Page, "하위 대리점이 존재합니다. 삭제할 수 없습니다.");
            return;
        }

        sql = "select count(*) from member where agency_idx='" + 총판코드 + "' ";
        cnt = su.SqlFieldQuery(sql);

        if (cnt != "0")
        {
            su.sg_alert(this.Page, "하위 사용자가 존재합니다. 삭제할 수 없습니다.");
            return;
        }

        sql = "select count(*) from machine where agency_idx='" + 총판코드 + "' ";
        cnt = su.SqlFieldQuery(sql);

        if (cnt != "0")
        {
            su.sg_alert(this.Page, "하위 자판기가 존재합니다. 삭제할 수 없습니다.");
            return;
        }

        sql = "delete from agency where idx='" + 총판코드 + "' ";
        su.SqlNoneQuery(sql);

        div_총판수정.Visible = false;
        bt새로고침_Click(null, null);
    }

    #endregion

    #region 대리점등록/수정삭제 
    protected void bt대리점등록_Click(object sender, EventArgs e)
    {
        String 총판코드 = 선택_총판코드.Text.Trim();

        sql = "select name,idx from agency order by name";
        su.binding(대리점등록_총판코드, sql, "총판을 선택해 주세요");

        if (총판코드 != "")
        {
            su.setDropDownList(대리점등록_총판코드, 총판코드);
        }


        bt새로고침_Click(null, null);
        div_대리점등록.Visible = true;
    }

    protected void bt대리점등록완료_Click(object sender, EventArgs e)
    {
        String 총판코드 = 대리점등록_총판코드.SelectedValue;
        String 대리점명 = su.sg_db_query(대리점등록_대리점명.Text.Trim()).ToUpper();
        String 사업자번호 = su.sg_db_query(대리점등록_사업자번호.Text.Trim()).Replace("-", "");
        String 대표자명 = su.sg_db_query(대리점등록_대표자명.Text.Trim());
        String 전화번호 = su.sg_db_query(대리점등록_전화번호.Text.Trim());
        String 휴대폰번호 = su.sg_db_query(대리점등록_휴대폰번호.Text.Trim());
        String 주소 = su.sg_db_query(대리점등록_주소.Text.TrimEnd().TrimStart(), true);

        if (총판코드 == "")
        {
            대리점등록_결과.InnerText = "등록할 총판을 선택해 주세요";
            return;
        }

        if (대리점명 == "")
        {
            대리점등록_결과.InnerText = "대리점명을 입력해주세요";
            return;
        }
        if (사업자번호 == "")
        {
            대리점등록_결과.InnerText = "사업자번호를 입력해주세요";
            return;
        }
        if (su.isNumeric(사업자번호) == false || 사업자번호.Replace("-", "") == "" || 사업자번호.Replace("-", "").Length != 10)
        {
            대리점등록_결과.InnerText = "사업자번호는 숫자10자리로 입력해주세요";
            return;
        }

        if (전화번호.Replace("-", "") != "" && (전화번호.Replace("-", "").Length > 12 || su.isNumeric(전화번호.Replace("-", "")) == false))
        {
            사용자수정_결과.InnerText = "휴대폰번호를 입력해 주세요";
            return;
        }

        if (휴대폰번호.Replace("-", "") != "" && (휴대폰번호.Replace("-", "").Length > 12 || su.isNumeric(휴대폰번호.Replace("-", "")) == false))
        {
            사용자수정_결과.InnerText = "휴대폰번호를 입력해 주세요";
            return;
        }

        sql = "insert into branch(company_idx,agency_idx,name,bizNo,chief,telNo,mphone,addr1) ";
        sql += " values('1','" + 총판코드 + "','" + 대리점명 + "','" + 사업자번호 + "','" + 대표자명 + "','" + 전화번호 + "','" + 휴대폰번호 + "','" + 주소 + "'); ";
        su.SqlNoneQuery(sql);

        대리점등록_결과.InnerText = "등록완료되었습니다";

        bt새로고침_Click(null, null);
        div_대리점등록.Visible = false;
    }

    protected void bt대리점등록닫기_Click(object sender, EventArgs e)
    {
        bt새로고침_Click(null, null);
        div_대리점등록.Visible = false;
    }

    protected void bt대리점삭제완료_Click(object sender, EventArgs e)
    {
        String cnt = "";
        String 총판코드 = 선택_총판코드.Text.Trim();
        if (총판코드 == "")
        {
            su.sg_alert(this.Page, "총판을 선택해주세요");
            return;
        }

        String 대리점코드 = 대리점수정_코드.Text.Trim();

        sql = "select count(*) from member where branch_idx='" + 대리점코드 + "' ";
        cnt = su.SqlFieldQuery(sql);

        if (cnt != "0")
        {
            su.sg_alert(this.Page, "하위 사용자가 존재합니다. 삭제할 수 없습니다.");
            return;
        }

        sql = "select count(*) from machine where branch_idx='" + 대리점코드 + "' ";
        cnt = su.SqlFieldQuery(sql);

        if (cnt != "0")
        {
            su.sg_alert(this.Page, "하위 자판기가 존재합니다. 삭제할 수 없습니다.");
            return;
        }

        sql = "delete from branch where idx='" + 대리점코드 + "' ";
        su.SqlNoneQuery(sql);

        div_대리점수정.Visible = false;
        bt새로고침_Click(null, null);

    }

    protected void bt대리점수정_Click(object sender, EventArgs e)
    {

        String 총판코드 = 선택_총판코드.Text.Trim();
        if (총판코드 == "")
        {
            su.sg_alert(this.Page, "총판을 선택해주세요");
            return;
        }

        String 대리점코드 = 선택_대리점코드.Text.Trim();
        if (대리점코드 == "")
        {
            su.sg_alert(this.Page, "수정할 대리점을 선택해주세요");
            return;
        }


        sql = "select name,idx from agency order by name";
        su.binding(대리점수정_총판코드, sql, "총판을 선택해 주세요");

        sql = "select idx,company_idx,agency_idx,name,bizNo,chief,telNo,mphone,addr1 from branch where idx='" + 대리점코드 + "'  ";
        DataView dv = su.SqlDvQuery(sql);
        if (dv != null && dv.Count > 0)
        {
            su.setDropDownList(대리점수정_총판코드, 총판코드);
            대리점수정_코드.Text = dv[0]["idx"].ToString();
            대리점수정_대리점명.Text = dv[0]["name"].ToString();
            대리점수정_사업자번호.Text = dv[0]["bizNo"].ToString();
            대리점수정_대표자명.Text = dv[0]["chief"].ToString();
            대리점수정_전화번호.Text = dv[0]["telNo"].ToString();
            대리점수정_휴대폰번호.Text = dv[0]["mphone"].ToString();
            대리점수정_주소.Text = dv[0]["addr1"].ToString();

            // Hidden parameters
            hCompanyIdx3.Value = dv[0]["company_idx"].ToString();
            hAgencyIdx3.Value = dv[0]["agency_idx"].ToString();
            hBranchIdx3.Value = dv[0]["idx"].ToString();
            hMachineIdx3.Value = null;

            div_대리점수정.Visible = true;
        }

        bt새로고침_Click(null, null);
    }

    protected void bt대리점수정완료_Click(object sender, EventArgs e)
    {
        String 총판코드 = su.sg_db_query(선택_총판코드.Text.Trim());
        String 대리점코드 = su.sg_db_query(대리점수정_코드.Text.Trim());
        String 대리점명 = su.sg_db_query(대리점수정_대리점명.Text.Trim()).ToUpper();
        String 사업자번호 = su.sg_db_query(대리점수정_사업자번호.Text.Trim()).Replace("-", "");
        String 대표자명 = su.sg_db_query(대리점수정_대표자명.Text.Trim());
        String 전화번호 = su.sg_db_query(대리점수정_전화번호.Text.Trim()).Replace("-", "");
        String 휴대폰번호 = su.sg_db_query(대리점수정_휴대폰번호.Text.Trim()).Replace("-", "");
        String 주소 = su.sg_db_query(대리점수정_주소.Text.TrimEnd().TrimStart(), true);

        if (총판코드 == "")
        {
            div_대리점수정결과.InnerText = "총판을 선택해주세요";
            return;
        }

        if (대리점코드 == "")
        {
            div_대리점수정결과.InnerText = "수정할 대리점을 선택해주세요";
            return;
        }

        if (전화번호.Replace("-", "") != "" && (전화번호.Replace("-", "").Length > 12 || su.isNumeric(전화번호.Replace("-", "")) == false))
        {
            사용자수정_결과.InnerText = "휴대폰번호를 입력해 주세요";
            return;
        }

        if (휴대폰번호.Replace("-", "") != "" && (휴대폰번호.Replace("-", "").Length > 12 || su.isNumeric(휴대폰번호.Replace("-", "")) == false))
        {
            사용자수정_결과.InnerText = "휴대폰번호를 입력해 주세요";
            return;
        }
        sql = "update branch set ";
        sql += "  company_idx='1',agency_idx='" + 총판코드 + "', name='" + 대리점명 + "',bizNo='" + 사업자번호 + "',chief='" + 대표자명 + "',telNo='" + 전화번호 + "',mphone='" + 휴대폰번호 + "',addr1='" + 주소 + "'  ";
        sql += " where idx='" + 대리점코드 + "' ";
        su.SqlNoneQuery(sql);

        div_대리점수정결과.InnerText = "수정되었습니다.";

        div_대리점수정.Visible = true;
        bt새로고침_Click(null, null);
    }

    protected void bt대리점수정닫기_Click(object sender, EventArgs e)
    {
        bt새로고침_Click(null, null);
        div_대리점수정.Visible = false;
    }
    #endregion

    #region 사용자보기 
    public void f_userlist()
    {
        sql = @"SELECT a.idx,a.userid,a.usernm,a.usrtel,a.mphone,a.active,a.usrorg,a.locktime, a.lst_logintime ";
        sql += " ,case when c.name is null then '총판계정' else c.name end 대리점명 from member a ";
        sql += " left outer join agency b on a.agency_idx = b.idx ";
        sql += " left outer join branch c on a.agency_idx = c.agency_idx and a.branch_idx = c.idx ";
        sql += " where 1=1 ";

        if (선택_총판코드.Text.Trim() != "" && 선택_대리점코드.Text.Trim() == "")
        {
            sql += " and a.agency_idx='" + 선택_총판코드.Text + "' and a.branch_idx=0 ";
        }
        if (선택_대리점코드.Text.Trim() != "")
        {
            sql += " and a.branch_idx='" + 선택_대리점코드.Text + "' ";
        }
        sql += " order by 대리점명, a.usernm";


        su.binding(gv사용자, sql);
    }
    #endregion

    #region 사용자등록 
    protected void bt사용자등록_Click(object sender, EventArgs e)
    {

        String 총판코드2 = 사용자등록_총판선택.SelectedValue;
        sql = "SELECT name,idx from agency ORDER BY name";
        su.binding(사용자등록_총판선택, sql, "-총판전체");
        if (선택_총판코드.Text != "")
        {
            su.setDropDownList(사용자등록_총판선택, 선택_총판코드.Text);
            총판코드2 = 선택_총판코드.Text;
        }

        String 대리점코드2 = 사용자등록_대리점선택.SelectedValue;
        sql = "SELECT name,idx from branch ";
        sql += " where agency_idx='" + 총판코드2 + "' ";
        sql += " ORDER BY name";
        su.binding(사용자등록_대리점선택, sql, "-대리점전체");
        if (선택_대리점코드.Text != "")
        {
            su.setDropDownList(사용자등록_대리점선택, 선택_대리점코드.Text);
            대리점코드2 = 선택_대리점코드.Text;
        }

        sql = "SELECT install_spot,pos_number from machine ";
        sql += " where branch_idx='" + 대리점코드2 + "' ";
        sql += " ORDER BY install_spot";
        su.binding(사용자등록_자판기선택, sql, "-자판기전체");

        bt새로고침_Click(null, null);
        div_사용자등록.Visible = true;
    }

    protected void bt사용자등록닫기_Click(object sender, EventArgs e)
    {
        bt새로고침_Click(null, null);
        div_사용자등록.Visible = false;
    }

    protected void bt사용자등록완료_Click(object sender, EventArgs e)
    {
        String 아이디 = su.sg_db_query(사용자등록_아이디.Text.Trim());
        String 이름 = su.sg_db_query(사용자등록_이름.Text.Trim());
        String 일반전화 = su.sg_db_query(사용자등록_일반전화.Text.Trim());
        String 휴대폰 = su.sg_db_query(사용자등록_휴대폰.Text.Trim());
        String 소속 = su.sg_db_query(사용자등록_소속.Text.Trim());

        String 총판코드 = su.sg_db_query(사용자등록_총판선택.SelectedValue);
        String 대리점코드 = su.sg_db_query(사용자등록_대리점선택.SelectedValue);
        String 자판기코드 = su.sg_db_query(사용자등록_자판기선택.SelectedValue);

        if (아이디 == "" || su.isSptext(아이디) == true || su.isHangul(아이디) == true)
        {
            사용자등록_결과.InnerText = "아이디 영숫자10자리 이하로 입력해주세요. 특수문자 한글 사용 불가";
            return;
        }

        if (총판코드 == "")
        {
            사용자수정_결과.InnerText = "소속 총판을 선택해 주세요";
            return;
        }

        sql = "select count(*) from member where userid='" + 아이디 + "' ";
        String chk1 = su.SqlFieldQuery(sql);

        if (chk1 != "0")
        {
            사용자등록_결과.InnerText = "이미 사용하고 있는 아이디 입니다.";
            return;
        }

        if (이름 == "" || su.isSptext(이름) == true)
        {
            사용자등록_결과.InnerText = "이름을 입력해 주세요. 이름에는 특수문자를 사용할 수 없습니다.";
            return;
        }

        if (일반전화.Replace("-", "") != "" && (일반전화.Replace("-", "").Length > 12 || su.isNumeric(일반전화.Replace("-", "")) == false))
        {
            사용자등록_결과.InnerText = "일반전화번호는 02-555-5555 와 같이 숫자로 입력가능합니다.";
            return;
        }

        if (휴대폰.Replace("-", "") != "" && (휴대폰.Replace("-", "").Length > 12 || su.isNumeric(휴대폰.Replace("-", "")) == false))
        {
            사용자등록_결과.InnerText = "휴대폰번호를 입력해 주세요";
            return;
        }

        sql = "insert into member(userid,usernm,usrtel,mphone,active,company_idx,agency_idx,branch_idx,pos_number,usrorg,locktime,regdate) ";
        sql += " values('" + 아이디 + "','" + 이름 + "','" + 일반전화 + "','" + 휴대폰 + "','1','1','" + 총판코드 + "',dbo.isblink('" + 대리점코드 + "'),dbo.isblink('" + 자판기코드 + "'),'" + 소속 + "',getdate(),getdate()); ";
        su.SqlNoneQuery(sql);

        사용자등록_결과.InnerText = "등록완료되었습니다";

        bt새로고침_Click(null, null);
        div_사용자등록.Visible = false;
    }

    #endregion

    #region 사용자수정/삭제

    protected void bt사용자수정완료_Click(object sender, EventArgs e)
    {
        String 아이디 = su.sg_db_query(사용자수정_아이디.Text.Trim());
        String 이름 = su.sg_db_query(사용자수정_이름.Text.Trim());
        String 일반전화 = su.sg_db_query(사용자수정_일반전화.Text.Trim());
        String 휴대폰 = su.sg_db_query(사용자수정_휴대폰.Text.Trim());
        String 소속 = su.sg_db_query(사용자수정_소속.Text.Trim());

        String 총판코드 = su.sg_db_query(사용자수정_총판선택.SelectedValue);
        String 대리점코드 = su.sg_db_query(사용자수정_대리점선택.SelectedValue);
        String 자판기코드 = su.sg_db_query(사용자수정_자판기선택.SelectedValue);

        if (총판코드 == "")
        {
            사용자수정_결과.InnerText = "소속 총판을 선택해 주세요";
            return;
        }

        사용자수정_결과.InnerText = "시작";
        if (이름 == "" || su.isSptext(이름) == true)
        {
            사용자수정_결과.InnerText = "이름을 입력해 주세요. 이름에는 특수문자를 사용할 수 없습니다.";
            return;
        }

        if (일반전화.Replace("-", "") != "" && (일반전화.Replace("-", "").Length > 12 || su.isNumeric(일반전화.Replace("-", "")) == false))
        {
            사용자수정_결과.InnerText = "일반전화번호를 입력해 주세요";
            return;
        }

        if (휴대폰.Replace("-", "") != "" && (휴대폰.Replace("-", "").Length > 12 || su.isNumeric(휴대폰.Replace("-", "")) == false))
        {
            사용자수정_결과.InnerText = "휴대폰번호를 입력해 주세요";
            return;
        }

        sql = "update member set usernm='" + 이름 + "',usrtel='" + 일반전화 + "',mphone='" + 휴대폰 + "',company_idx='191',agency_idx='" + 총판코드 + "',branch_idx=dbo.isblink('" + 대리점코드 + "'),pos_number=dbo.isblink('" + 자판기코드 + "'),usrorg='" + 소속 + "' where userid='" + 아이디 + "' ";
        su.SqlNoneQuery(sql);

        사용자수정_결과.InnerText = "수정완료되었습니다";

        bt새로고침_Click(null, null);
        div_사용자수정.Visible = false;
    }


    protected void bt사용자수정닫기_Click(object sender, EventArgs e)
    {
        bt새로고침_Click(null, null);
        div_사용자수정.Visible = false;
    }
    protected void bt사용자삭제_Click(object sender, EventArgs e)
    {
        String 아이디 = su.sg_db_query(사용자수정_아이디.Text.Trim());

        sql = "update member set active=0 where userid='" + 아이디 + "'; ";
        su.SqlFieldQuery(sql);


        사용자수정_결과.InnerText = "삭제되었습니다";

        bt새로고침_Click(null, null);
        div_사용자수정.Visible = false;

    }
    #endregion

    #region 자판기리스트
    public void f_자판기리스트()
    {

        sql = "SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS rownum,a.idx ,convert(varchar(10),a.install_date,121) 설치일, a.install_spot 설치장소, a.lte_router, a.관리자연락처, a.machine_stat, datediff(mi,최종보고일시,getdate()) 통신상태 ";
        sql += " FROM machine a where 1=1";

        if (선택_총판코드.Text.Trim() != "" && 선택_대리점코드.Text.Trim() == "")
        {
            sql += " and a.agency_idx='" + 선택_총판코드.Text + "' and a.branch_idx=0 ";
        }
        if (선택_대리점코드.Text.Trim() != "")
        {
            sql += " and a.branch_idx='" + 선택_대리점코드.Text + "' ";
        }
        sql += " ORDER BY 설치장소";


        su.binding(gv단말기리스트, sql);
    }
    #endregion

    #region gv사용자 Pagesing 처리
    protected void gv사용자_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv사용자.PageIndex = e.NewPageIndex;
        bt새로고침_Click(null, null);
    }
    #endregion

    #region 자판기 등록
    protected void bt자판기등록_Click(object sender, EventArgs e)
    {
        bt새로고침_Click(null, null);

        if (선택_총판코드.Text != "")
        {
            su.setDropDownList(자판기등록_총판, 선택_총판코드.Text);
        }

        if (선택_대리점코드.Text != "")
        {
            su.setDropDownList(자판기등록_대리점, 선택_대리점코드.Text);
        }

        div_자판기등록.Visible = true;

    }
    protected void bt자판기등록닫기_Click(object sender, EventArgs e)
    {
        bt새로고침_Click(null, null);
        div_자판기등록.Visible = false;
    }
    protected void bt자판기등록완료_Click(object sender, EventArgs e)
    {
        String 관리자연락처 = su.sg_db_query(자판기등록_관리자연락처.Text.Trim());
        String 자판기타입 = su.sg_db_query(자판기등록_자판기타입.SelectedValue);
        String 설치장소 = su.sg_db_query(자판기등록_설치장소.Text.TrimEnd().TrimStart());
        String 설치일 = su.sg_db_query(자판기등록_설치일.Text.Trim());
        String 총판IDX = 자판기등록_총판.SelectedValue;
        String 대리점IDX = 자판기등록_대리점.SelectedValue;
        String 결제구분 = 자판기등록_결제구분.SelectedValue;
        String 결제업체 = 자판기등록_결제업체.SelectedValue;
        String 결제코드 = su.sg_db_query(자판기등록_결제코드.Text.Trim());
        String 랙수 = su.sg_db_query(자판기등록_랙수.Text.Trim());

        if (랙수 == "" || su.isNumeric(랙수) == false || su.sg_int(랙수) > 65)
        {
            div_자판기등록결과.InnerText = "랙수는 0~65이하의 숫자로 입력해야 합니다.";
            return;
        }

        sql = "insert into machine(company_idx,agency_idx,branch_idx,install_type,install_date,install_spot,lte_router,결제구분,결제코드,결제업체,관리자연락처,랙수) ";
        sql += " values(1,'" + 총판IDX + "' , dbo.isblink('" + 대리점IDX + "'), '" + 자판기타입 + "','" + 설치일 + "' ,'" + 설치장소 + "','','" + 결제구분 + "','" + 결제코드 + "','" + 결제업체 + "','" + 관리자연락처 + "','" + 랙수 + "'); select @@identity; ";

        String 자판기번호 = su.SqlFieldQuery(sql);


        bt새로고침_Click(null, null);
        div_자판기등록.Visible = false;
    }
    #endregion

    #region 자판기 메뉴/로그/수정/쿠폰 클릭시
    
    protected void gv단말기리스트_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String ename = e.CommandName;
        String strval = (String)e.CommandArgument;

        int machineIdx = Convert.ToInt32(strval);
     
        if (ename == "Select")
        {
            자판기수정_자판기번호.Text = strval;
            div_자판기수정결과.InnerText = "";

            sql = "select idx,agency_idx,branch_idx,install_type,convert(varchar(10),install_date,121) install_date,install_spot,lte_router,결제구분,결제코드,결제업체,최종결제일시,최종재부팅일시,최종보고일시,관리자연락처,랙수 from machine with(nolock) where idx='" + strval + "' ";
            DataView dv = su.SqlDvQuery(sql);
            if (dv != null && dv.Count > 0)
            {
                자판기수정_관리자연락처.Text = dv[0]["관리자연락처"].ToString();
                자판기수정_설치장소.Text = dv[0]["install_spot"].ToString();
                자판기수정_설치일.Text = dv[0]["install_date"].ToString();

                String 총판코드2 = dv[0]["agency_idx"].ToString();
                sql = "SELECT name,idx from agency ORDER BY name";
                su.binding(자판기수정_총판, sql, "-총판전체");
                su.setDropDownList(자판기수정_총판, 총판코드2);

                String 대리점코드2 = dv[0]["branch_idx"].ToString();
                sql = "SELECT name,idx from branch ";
                sql += " where agency_idx='" + 총판코드2 + "' ";
                sql += " ORDER BY name";
                su.binding(자판기수정_대리점, sql, "-대리점전체");
                su.setDropDownList(자판기수정_대리점, 대리점코드2);

                su.setDropDownList(자판기수정_자판기타입, dv[0]["install_type"].ToString());

                자판기수정_랙수.Text = dv[0]["랙수"].ToString();

                div_자판기수정.Visible = true;
                bt새로고침_Click(null, null);
            }
        }

        if (ename == "menu")
        {
            lb자판기메뉴관리_자판기번호.Text = strval;
            div_자판기메뉴.Visible = true;
            fn_메뉴정보불러오기();
            bt새로고침_Click(null, null);
        }

        if (ename == "log")
        {
            lb로그자판기번호.Text = strval;
            lb자판기메뉴관리_자판기번호.Text = strval;
            fn_자판기로그검색();
        }

        if (ename == "coupon")
        {
            lb자판기메뉴관리_자판기번호.Text = strval;
            String 자판기번호 = strval;
            fn_쿠폰요약정보불러오기();
            div_쿠폰관리.Visible = true;
        }

        // 기기별 자판기 배너 버튼을 선택했을 경우 
        if(ename == "banner")
        {
            div_배너관리.Visible = true;
            Get장비별배너등록조회(machineIdx);
        }
    }
    #endregion

    #region 쿠폰요약정보 불러오기
    public void fn_쿠폰요약정보불러오기()
    {
        String 자판기번호 = lb자판기메뉴관리_자판기번호.Text;
        sql = "select distinct ";
        sql += "isnull(sum(case when regdate is not null then 1 else 0 end),0) 발행수 ";
        sql += ", isnull(sum(case when regdate is not null then 발행금액 else 0 end),0) 발행금액 ";
        sql += ", isnull(sum(case when regdate is not null and usedate is not null then 1 else 0 end),0) 사용수 ";
        sql += ", isnull(sum(case when regdate is not null and usedate is not null then 발행금액 else 0 end),0) 사용금액 ";
        sql += ", isnull(sum(case when regdate is not null and usedate is null and 만료일 >= convert(varchar(10),getdate(),121) then 1 else 0 end),0) 미사용 ";
        sql += ", isnull(sum(case when regdate is not null and usedate is null and 만료일 >= convert(varchar(10),getdate(),121) then 발행금액 else 0 end),0) 미사용금액 ";
        sql += ", isnull(sum(case when regdate is not null and usedate is null and 만료일 < convert(varchar(10),getdate(),121) then 1 else 0 end),0) 미사용종료 ";
        sql += ", isnull(sum(case when regdate is not null and usedate is null and 만료일 < convert(varchar(10),getdate(),121) then 발행금액 else 0 end),0) 미사용종료금액 ";
        sql += "from coupon with(nolock) ";
        sql += "where machine_idx='" + 자판기번호 + "' ";
        DataView dv = su.SqlDvQuery(sql);
        if (dv != null && dv.Count > 0)
        {
            쿠폰_발행수.Text = su.FormatNumber(dv[0]["발행수"].ToString());
            쿠폰_발행금액.Text = su.FormatNumber(dv[0]["발행금액"].ToString());
            쿠폰_사용수.Text = su.FormatNumber(dv[0]["사용수"].ToString());
            쿠폰_사용금액.Text = su.FormatNumber(dv[0]["사용금액"].ToString());
            쿠폰_미사용.Text = su.FormatNumber(dv[0]["미사용"].ToString());
            쿠폰_미사용금액.Text = su.FormatNumber(dv[0]["미사용금액"].ToString());
            쿠폰_미사용종료.Text = su.FormatNumber(dv[0]["미사용종료"].ToString());
            쿠폰_미사용종료금액.Text = su.FormatNumber(dv[0]["미사용종료금액"].ToString());
        }

        sql = "select distinct ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS rownum,max(쿠폰번호) 쿠폰번호,발행금액, 최소구매금액, convert(varchar(10),만료일,121) 만료일,  Datediff(d, convert(varchar(10), getdate(), 121), 만료일) 잔여일자, case when Datediff(d, convert(varchar(10),getdate(),121),만료일)< 0 then 'Y' else 'N' end[만료여부] , useyn, machine_idx ,convert(varchar(8), regdate, 112) 발행일,count(*) 발행수량 ";
        sql += " from coupon with(nolock) where isusing= 1 and machine_idx='" + 자판기번호 + "' ";
        sql += " group by 발행금액, 최소구매금액, convert(varchar(10),만료일,121),  Datediff(d, convert(varchar(10), getdate(), 121), 만료일), case when Datediff(d, convert(varchar(10),getdate(),121),만료일)< 0 then 'Y' else 'N' end  , useyn, machine_idx ,convert(varchar(8), regdate, 112) ";
        sql += " order by rownum ";
        su.binding(gv쿠폰리스트, sql);

    }
    #endregion

    #region 자판기 수정완료
    protected void bt자판기수정완료_Click(object sender, EventArgs e)
    {
        String 자판기번호 = su.sg_db_query(자판기수정_자판기번호.Text.Trim());
        String 관리자연락처 = su.sg_db_query(자판기수정_관리자연락처.Text.Trim());
        String 자판기타입 = su.sg_db_query(자판기수정_자판기타입.SelectedValue);
        String 설치장소 = su.sg_db_query(자판기수정_설치장소.Text.TrimEnd().TrimStart());
        String 설치일 = su.sg_db_query(자판기수정_설치일.Text.Trim());
        String 총판IDX = 자판기수정_총판.SelectedValue;
        String 대리점IDX = 자판기수정_대리점.SelectedValue;
        String 결제구분 = 자판기수정_결제구분.SelectedValue;
        String 결제업체 = 자판기수정_결제업체.SelectedValue;
        String 결제코드 = su.sg_db_query(자판기수정_결제코드.Text.Trim());
        String 랙수 = su.sg_db_query(자판기수정_랙수.Text.Trim());

        if (랙수 == "" || su.isNumeric(랙수) == false || su.sg_int(랙수) > 64)
        {
            div_자판기수정결과.InnerText = "랙수는 0~65이하의 숫자로 입력해야 합니다.";
            return;
        }

        sql = "update machine set " +
            " agency_idx='" + 총판IDX + "' " +
            " ,branch_idx='" + 대리점IDX + "' " +
            " ,install_type='" + 자판기타입 + "' " +
            " ,install_date='" + 설치일 + "' " +
            " ,install_spot='" + 설치장소 + "' " +
            " ,결제구분='" + 결제구분 + "' " +
            " ,결제코드='" + 결제코드 + "' " +
            " ,결제업체='" + 결제업체 + "' " +
            " ,랙수='" + 랙수 + "' " +
            " ,관리자연락처='" + 관리자연락처 + "' " +
            " where idx='" + 자판기번호 + "' ";
        su.SqlNoneQuery(sql);

        div_자판기수정결과.InnerText = "수정완료 되었습니다";

        bt새로고침_Click(null, null);
    }
    #endregion

    #region 자판기 수정닫기
    protected void bt자판기수정닫기_Click(object sender, EventArgs e)
    {

        div_자판기수정.Visible = false;
        bt새로고침_Click(null, null);
    }
    #endregion

    #region 자판기로그 구분 선택
    protected void 자판기로그_구분_SelectedIndexChanged(object sender, EventArgs e)
    {
        fn_자판기로그검색();
    }
    #endregion

    #region 자판기로그
    public void fn_자판기로그검색()
    {
        String 자판기번호 = lb로그자판기번호.Text.Trim();
        if (자판기로그_시작일.Text == "") { 자판기로그_시작일.Text = DateTime.Now.ToString("yyyy-MM-dd"); }
        if (자판기로그_종료일.Text == "") { 자판기로그_종료일.Text = DateTime.Now.ToString("yyyy-MM-dd"); }

        String 로그시작일 = 자판기로그_시작일.Text;
        String 로그종료일 = 자판기로그_종료일.Text;
        String 로그구분 = 자판기로그_구분.SelectedValue;

        sql = "select idx,";
        sql += " case when etype='1' then '1버전체크' when etype='2' then '2상품조회' when etype='3' then '3결제보고' when etype='4' then '4쿠폰조회' when etype='5' then '5쿠폰사용' when etype='6' then '6쿠폰사용취소' when etype='7' then '7자판기번호조회' when etype='8' then '8자판기정보수정' when etype='9' then '9상태보고' when etype='10' then '10라이브" +
            "체크' when etype='11' then '11문열림보고' when etype='12' then '12결제취소보고' else '미정의' end [구분] , etype ";
        sql += " ,getdata,isnull(senddata,'') senddata,clientip,convert(varchar(19),regdate,121) regdate from machine_log with(nolock) ";
        sql += " where machine_idx='" + 자판기번호 + "' and regdate >= '" + 로그시작일 + "'  ";
        sql += " and regdate < dateadd(d,1,('" + 로그종료일 + "'))  ";
        if (로그구분 != "")
        {
            sql += " and etype='" + 로그구분 + "' ";
        }
        sql += " order by regdate desc";

        su.binding(gv_자판기로그, sql);

        div_자판기로그.Visible = true;
    }

    protected void bt자판기로그닫기_Click(object sender, EventArgs e)
    {
        tb_senddata.Text = "";
        tb_getdata.Text = "";
        gv_자판기로그.SelectedIndex = -1;
        lb로그자판기번호.Text = "";
        div_자판기로그.Visible = false;
    }

    protected void bt자판기로그검색_Click(object sender, EventArgs e)
    {
        fn_자판기로그검색();
    }

    #endregion

    #region 메뉴정보 불러오기
    public void fn_메뉴정보불러오기()
    {
        String 자판기번호 = lb자판기메뉴관리_자판기번호.Text;
        String 선택메뉴 = lb자판기메뉴관리_선택메뉴.Text;



        String 브랜드idx = dd메뉴_브랜드선택.SelectedValue;

        sql = "select a.*,b.상품수 from coffee_menu a with(nolock) left outer join (select distinct count(*) 상품수, coffee_menu_idx from machine_menu with(nolock) where isusing=1 and machine_idx='" + 자판기번호 + "' group by coffee_menu_idx) b on a.idx=b.coffee_menu_idx where a.machine_idx='" + 자판기번호 + "' and a.isusing=1 order by a.dno ";
        su.binding(gv메뉴, sql);

        if (선택메뉴 != "")
        {
            sql = "select c.idx, b.brand_name [브랜드명], c.name [상품명], c.imagebase64_s, c.pr_type from machine_menu a with(nolock) left outer join ";
            sql += " coffee_brand b with(nolock) on a.coffee_brand_idx = b.idx ";
            sql += " left outer join coffee c with(nolock) on a.coffee_idx = c.idx ";
            sql += " where a.isusing = 1 and a.machine_idx = '" + 자판기번호 + "' ";
            sql += " and a.coffee_menu_idx='" + 선택메뉴 + "' ";
            if (브랜드idx != "")
            {
                sql += " and a.coffee_brand_idx='" + 브랜드idx + "' ";
            }
            sql += " order by 브랜드명, 상품명";
            su.binding(gv상품, sql);
        }
    }
    #endregion

    #region 메뉴관리 기본상품추가하기 
    protected void bt기본상품추가하기_Click(object sender, EventArgs e)
    {

        String 자판기번호 = lb자판기메뉴관리_자판기번호.Text.Trim();

        // 기존 메뉴 및 상품 삭제처리
        sql = "update coffee set isusing=0,deletedate=getdate() where machine_idx='" + 자판기번호 + "' ";
        su.SqlNoneQuery(sql);

        sql = "update coffee_menu set isusing=0,deletedate=getdate(),delete_userid='" + userid + "' where machine_idx='" + 자판기번호 + "' ";
        su.SqlNoneQuery(sql);

        sql = "update machine_menu set isusing=0,deletedate=getdate(),delete_userid='" + userid + "' where machine_idx='" + 자판기번호 + "' ";
        su.SqlNoneQuery(sql);

        // 자판기 전용 기본메뉴 추가(캡슐커피)
        sql = "insert into coffee_menu(name,machine_idx,dno) values('캡슐커피','" + 자판기번호 + "',1); select @@identity;";
        String menu_idx = su.SqlFieldQuery(sql);

        // 기본 커피상품 복사처리
        sql = "insert into coffee(brand_idx,unit,name,comt,modify_date,imagebase64,imageext,imagesize,imagebase64_s,pr_type,isusing,machine_idx,deletedate) ";
        sql += " select brand_idx,unit,name,comt,getdate(),imagebase64,imageext,imagesize,imagebase64_s,pr_type,isusing,'" + 자판기번호 + "',null from coffee with(nolock) where isusing=1 and machine_idx=0 ";
        su.SqlNoneQuery(sql);

        // 자판기의 캡슐커피메뉴에 상품 추가
        sql = "insert into machine_menu(machine_idx,coffee_menu_idx,coffee_brand_idx,coffee_idx) ";
        sql += " select " + 자판기번호 + "," + menu_idx + ",brand_idx,idx idx from coffee where isusing = 1 and imageext is not null and machine_idx ='" + 자판기번호 + "'; select @@identity; ";
        String m_menu_idx = su.SqlFieldQuery(sql);

        fn_메뉴정보불러오기();
    }
    #endregion

    #region 자판기메뉴관리 닫기
    protected void bt자판기메뉴관리닫기_Click(object sender, EventArgs e)
    {
        div_자판기메뉴.Visible = false;
    }
    #endregion

    #region 상품등록 열기
    protected void bt상품추가_Click(object sender, EventArgs e)
    {
        상품등록_결과.InnerText = "";
        sql = "select distinct b.brand_name,b.idx from machine_menu a with(nolock) inner join coffee_brand b with(nolock) on a.coffee_brand_idx=b.idx where a.isusing=1 and a.coffee_menu_idx='" + lb자판기메뉴관리_선택메뉴.Text + "' and a.machine_idx='" + lb자판기메뉴관리_자판기번호.Text + "' group by b.brand_name,b.idx order by b.brand_name";
        su.binding(상품등록_브랜드, sql, "브랜드명 직접입력");

        div_상품등록.Visible = true;

        fn_메뉴정보불러오기();
    }

    protected void 상품등록_브랜드_SelectedIndexChanged(object sender, EventArgs e)
    {
        String 브랜드idx = 상품등록_브랜드.SelectedValue;

        if (브랜드idx != "")
        {
            상품등록_브랜드명.Text = 상품등록_브랜드.SelectedItem.Text;
        }
        else
        {
            상품등록_브랜드명.Text = "";
        }
        fn_메뉴정보불러오기();
    }
    #endregion

    #region 상품등록

    protected void bt상품등록완료_Click(object sender, EventArgs e)
    {
        String 브랜드idx = 상품등록_브랜드.SelectedValue;
        String 브랜드명 = su.sg_db_query(상품등록_브랜드명.Text.Trim());
        String 자판기번호 = lb자판기메뉴관리_자판기번호.Text;
        String 메뉴idx = lb자판기메뉴관리_선택메뉴.Text;
        String 투출타입 = 상품등록_투출타입.SelectedValue;

        if (브랜드idx == "" && 브랜드명 != "")
        {
            sql = "select idx from coffee_brand with(nolock) where isusing=1 and machine_idx='" + 자판기번호 + "' and brand_name='" + 브랜드명 + "' ";
            String 검색브랜드idx = su.SqlFieldQuery(sql);

            if (검색브랜드idx != "")
            {
                브랜드idx = 검색브랜드idx;
            }
            else
            {
                // 브랜드 추가후 idx 확보
                sql = "insert into coffee_brand(brand_name,regist_date,isusing,machine_idx) ";
                sql += " values('" + 브랜드명 + "',getdate(),1,'" + 자판기번호 + "'); select @@identity; ";
                브랜드idx = su.SqlFieldQuery(sql);
            }
        }


        // 상품등록 처리
        String 상품명 = su.sg_db_query(상품등록_상품명.Text.Trim());
        String 단위 = su.sg_db_query(상품등록_단위.Text.Trim());
        String comt = su.sg_db_query(상품등록_비고.Text.Trim());
        String 이미지 = "";
        String 이미지s = "";
        String 확장자 = "";
        String 이미지사이즈 = "0";
        if (상품등록_이미지.HasFile == true)
        {
            확장자 = su.FileExt(상품등록_이미지.FileName).ToLower();
            if (확장자 != "png" && 확장자 != "jpg")
            {
                상품등록_결과.InnerText = "상품이미지는 png 또는 jpg 만 사용 가능합니다.";
                return;
            }

            if (상품등록_이미지.FileContent.Length / 1000 / 1000 > 1.0)
            {
                상품등록_결과.InnerText = "상품이미지는 1MB를 초과할수 없습니다.";
                return;
            }
            System.Drawing.Image im = System.Drawing.Image.FromStream(상품등록_이미지.FileContent);

            이미지 = su.BitmapToBase64(su.ResizeImage(im, 160, 160));
            이미지s = su.BitmapToBase64(su.ResizeImage(im, 100, 100));
            이미지사이즈 = su.CntBytesLen(이미지).ToString();
        }

        if (브랜드idx == "")
        {
            상품등록_결과.InnerText = "등록할 브랜드을 선택해 주세요";
            return;
        }

        if (상품명 == "" || su.isSptext(상품명) == true)
        {
            상품등록_결과.InnerText = "상품명을 입력해주세요(특수문자사용불가)";
            return;
        }

        sql = "select count(*) from coffee where brand_idx='" + 브랜드idx + "' and name='" + 상품명 + "' and isusing=1 ";
        String chk1 = su.SqlFieldQuery(sql);

        if (chk1 != "0")
        {
            상품등록_결과.InnerText = "이미 등록된 상품명 입니다.";
            return;
        }

        sql = "select distinct b.pr_type from machine_menu a left outer join coffee b on a.coffee_idx=b.idx where coffee_menu_idx='" + 메뉴idx + "' and a.isusing=1 and a.machine_idx='" + 자판기번호 + "' ";
        String chk2 = su.SqlFieldQuery(sql);

        if (chk2 != "" && chk2 != 투출타입)
        {
            상품등록_결과.InnerText = "해당 메뉴에는 투출타입 [" + chk2 + "] 전시로만 전시상품과 투출상품은 한메뉴에 혼용해서 사용할 수 없습니다.";
            return;
        }


        sql = "insert into coffee(brand_idx,unit,NAME,comt,imagebase64,imageext,imagesize,imagebase64_s,machine_idx,pr_type) ";
        sql += " values('" + 브랜드idx + "','" + 단위 + "','" + 상품명 + "','" + comt + "','" + 이미지 + "','" + 확장자 + "','" + 이미지사이즈 + "','" + 이미지s + "','" + 자판기번호 + "','" + 투출타입 + "'); select @@identity; ";
        String coffee_idx = su.SqlFieldQuery(sql);

        // machine_menu 등록 처리
        sql = "insert into machine_menu(machine_idx,coffee_menu_idx,coffee_brand_idx,coffee_idx) ";
        sql += " values( '" + 자판기번호 + "','" + 메뉴idx + "','" + 브랜드idx + "','" + coffee_idx + "'); select @@identity; ";
        su.SqlNoneQuery(sql);

        상품등록_결과.InnerText = "등록완료되었습니다";

        bt새로고침_Click(null, null);
        fn_메뉴정보불러오기();
    }

    protected void bt상품등록닫기_Click(object sender, EventArgs e)
    {
        상품등록_결과.InnerText = "";
        div_상품등록.Visible = false;
    }

    #endregion

    #region 상품수정

    protected void bt상품수정완료_Click(object sender, EventArgs e)
    {
        String 브랜드코드 = 상품수정_브랜드코드.SelectedValue;
        String 투출타입 = 상품수정_투출타입.SelectedValue;
        String 단위 = 상품수정_단위.SelectedValue;

        String 상품코드 = su.sg_db_query(상품수정_상품코드.Text.Trim());
        String 상품명 = su.sg_db_query(상품수정_상품명.Text.Trim()).ToUpper();

        if (브랜드코드 == "")
        {
            div_상품수정결과.InnerText = "브랜드을 선택해주세요";
            return;
        }

        if (상품코드 == "")
        {
            div_상품수정결과.InnerText = "수정할 상품을 선택해주세요";
            return;
        }

        String 이미지 = "";
        String 이미지s = "";
        String 확장자 = "";
        String 이미지사이즈 = "0";
        if (상품수정_이미지.HasFile == true)
        {
            확장자 = su.FileExt(상품수정_이미지.FileName).ToLower();
            if (확장자 != "png" && 확장자 != "jpg")
            {
                div_상품수정결과.InnerText = "상품이미지는 png 또는 jpg 만 사용 가능합니다.";
                return;
            }

            if (상품수정_이미지.FileContent.Length / 1000 / 1000 > 1.0)
            {
                div_상품수정결과.InnerText = "상품이미지는 1MB를 초과할수 없습니다.";
                return;
            }

            System.Drawing.Image im = System.Drawing.Image.FromStream(상품수정_이미지.FileContent);
            이미지 = su.BitmapToBase64(su.ResizeImage(im, 160, 160));
            이미지s = su.BitmapToBase64(su.ResizeImage(im, 100, 100));
            이미지사이즈 = su.CntBytesLen(이미지).ToString();
        }

        sql = "update coffee set ";
        sql += " name='" + 상품명 + "',";
        sql += " brand_idx='" + 브랜드코드 + "',";
        sql += " unit='" + 단위 + "',";
        if (상품수정_이미지.HasFile == true)
        {
            sql += " imagebase64='" + 이미지 + "',";
            sql += " imagebase64_s='" + 이미지s + "',";
            sql += " imageext='" + 확장자 + "',";
            sql += " imagesize='" + 이미지사이즈 + "', ";
        }
        sql += " pr_type='" + 투출타입 + "' ";
        sql += " where idx='" + 상품코드 + "' ";
        su.SqlNoneQuery(sql);

        div_상품수정결과.InnerText = "상품이 수정되었습니다";
        fn_메뉴정보불러오기();
    }

    protected void bt상품수정닫기_Click(object sender, EventArgs e)
    {
        bt새로고침_Click(null, null);
        div_상품수정.Visible = false;
    }
    #endregion

    #region 메뉴 선택시 
    protected void gv메뉴_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String ename = e.CommandName;
        String strval = (String)e.CommandArgument;

        if (ename == "Select")
        {
            lb자판기메뉴관리_선택메뉴.Text = strval;

            sql = "select distinct b.brand_name, b.idx from machine_menu a with(nolock) inner join coffee_brand b with(nolock) on a.coffee_brand_idx = b.idx where a.isusing=1 and a.coffee_menu_idx = '" + strval + "' order by brand_name ";
            su.binding(dd메뉴_브랜드선택, sql, "브랜드전체");
            fn_메뉴정보불러오기();
        }

        if (ename == "menu_del")
        {
            lb자판기메뉴관리_선택메뉴.Text = strval;

            sql = "update coffee_menu set isusing=0,deletedate=getdate(),delete_userid='" + userid + "' where idx='" + strval + "' ";
            su.SqlNoneQuery(sql);

            sql = "update coffee set isusing=0,deletedate=getdate() where idx in (select coffee_idx from machine_menu where coffee_menu_idx='" + strval + "') ";
            su.SqlNoneQuery(sql);

            sql = "update machine_menu set isusing=0,deletedate=getdate(),delete_userid='" + userid + "' where coffee_menu_idx='" + strval + "' ";
            su.SqlNoneQuery(sql);

            su.binding(dd메뉴_브랜드선택, sql, "브랜드전체");
            fn_메뉴정보불러오기();
        }
    }

    #endregion

    #region 메뉴관리 브랜드 선택시
    protected void dd브랜드선택_SelectedIndexChanged(object sender, EventArgs e)
    {
        fn_메뉴정보불러오기();
    }
    #endregion

    #region 메뉴등록
    protected void bt메뉴등록완료_Click(object sender, EventArgs e)
    {
        String 메뉴명 = su.sg_db_query(메뉴추가_메뉴명.Text.TrimEnd().TrimStart());
        String 자판기번호 = lb자판기메뉴관리_자판기번호.Text;
        String dno = su.SqlFieldQuery("select count(*)+1 from coffee_menu with(nolock) where isusing=1 and machine_idx='" + 자판기번호 + "' ");

        String chk = su.SqlFieldQuery("select count(*) from coffee_menu with(nolock) where isusing=1 and name='" + 메뉴명 + "' and machine_idx='" + 자판기번호 + "' ");
        if (chk != "0")
        {
            div_메뉴등록결과.InnerText = "이미 등록된 메뉴명 입니다";
            return;
        }

        sql = "insert into coffee_menu(name,dno,machine_idx) values('" + 메뉴명 + "','" + dno + "','" + 자판기번호 + "')";
        su.SqlNoneQuery(sql);

        div_메뉴등록결과.InnerText = "등록되었습니다";
        fn_메뉴정보불러오기();
    }

    protected void bt메뉴등록닫기_Click(object sender, EventArgs e)
    {
        div_메뉴등록결과.InnerText = "";
        div_메뉴추가.Visible = false;
        fn_메뉴정보불러오기();
    }

    protected void bt메뉴추가_Click(object sender, EventArgs e)
    {
        div_메뉴등록결과.InnerText = "";
        div_메뉴추가.Visible = true;

    }
    #endregion

    #region 상품 수정/삭제 클릭시
    protected void gv상품_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String ename = e.CommandName;
        String strval = (String)e.CommandArgument;

        if (ename == "menu_del")
        {
            String 자판기번호 = lb자판기메뉴관리_자판기번호.Text;
            String coffee_idx = strval;

            // 기존 메뉴 및 상품 삭제처리
            sql = "update coffee set isusing=0,deletedate=getdate() where idx='" + coffee_idx + "' ";
            su.SqlNoneQuery(sql);

            sql = "update machine_menu set isusing=0,deletedate=getdate(),delete_userid='" + userid + "' where machine_idx='" + 자판기번호 + "' and coffee_idx='" + coffee_idx + "' ";
            su.SqlNoneQuery(sql);

            div_메뉴관리_안내.InnerText = "상품이 삭제되었습니다";
            fn_메뉴정보불러오기();

        }

        if (ename == "menu_up")
        {
            String 자판기번호 = lb자판기메뉴관리_자판기번호.Text;
            String coffee_idx = strval;

            div_상품수정.Visible = true;
            div_상품수정결과.InnerText = "";

            sql = "select brand_idx,unit,NAME,comt,imagebase64,imageext,imagesize,imagebase64_s,machine_idx,pr_type from coffee with(nolock) where machine_idx='" + 자판기번호 + "' and idx='" + coffee_idx + "' ";
            DataView dv = su.SqlDvQuery(sql);

            if (dv != null && dv.Count > 0)
            {
                상품수정_상품코드.Text = coffee_idx;
                su.setDropDownList(상품수정_단위, dv[0]["unit"].ToString());
                su.setDropDownList(상품수정_투출타입, dv[0]["pr_type"].ToString());
                상품수정_상품명.Text = dv[0]["NAME"].ToString();
                상품수정_비고.Text = dv[0]["comt"].ToString();

                sql = "select distinct b.brand_name,b.idx from machine_menu a with(nolock) inner join coffee_brand b with(nolock) on a.coffee_brand_idx=b.idx where a.isusing=1 and a.coffee_menu_idx='" + lb자판기메뉴관리_선택메뉴.Text + "' and a.machine_idx='" + 자판기번호 + "'  group by b.brand_name,b.idx order by b.brand_name";
                su.binding(상품수정_브랜드코드, sql, "브랜드명 직접입력");

                su.setDropDownList(상품수정_브랜드코드, dv[0]["brand_idx"].ToString());
                상품수정_현재이미지.ImageUrl = "data:image/png;base64," + dv[0]["imagebase64_s"].ToString();
            }

            fn_메뉴정보불러오기();

        }
    }
    #endregion

    #region 쿠폰닫기
    protected void bt쿠폰닫기_Click(object sender, EventArgs e)
    {
        div_쿠폰관리.Visible = false;
    }
    #endregion

    #region 쿠폰발행처리
    protected void bt쿠폰발행_Click(object sender, EventArgs e)
    {
        String 자판기번호 = lb자판기메뉴관리_자판기번호.Text;
        String 발행쿠폰수 = su.sg_db_query(쿠폰발행_발행숫자.Text.Trim());
        String 최소구매금액 = su.sg_db_query(쿠폰발행_최소구매금액.Text.Trim());
        String 만료일 = su.sg_db_query(쿠폰발행_만료일.Text.Trim());
        String 발행금액 = su.sg_db_query(쿠폰발행_발행금액.Text.Trim());

        if (발행쿠폰수 == "" || 발행쿠폰수 == "0" || su.isNumeric(발행쿠폰수) == false)
        {
            div_쿠폰결과.InnerText = "발행할 쿠폰수를 숫자로 입력해주세요";
            return;
        }

        int 발행쿠폰수량 = Convert.ToInt32(발행쿠폰수);

        if (발행쿠폰수량 > 1000)
        {
            div_쿠폰결과.InnerText = "한번에 최대 1000개까지 발행가능합니다";
            return;
        }

        if (최소구매금액 == "" || su.isNumeric(최소구매금액) == false)
        {
            div_쿠폰결과.InnerText = "최소구매금액은 0 이상의 숫자로 입력해 주세요";
            return;
        }

        if (발행금액 == "" || su.isNumeric(발행금액) == false || su.sg_int(발행금액) < 10)
        {
            div_쿠폰결과.InnerText = "발행금액은 1 0원 이상의 숫자로 입력해 주세요";
            return;
        }

        if (만료일 == "" || su.isDate(만료일) == false || 만료일 == DateTime.Now.ToString("yyyy-MM-dd"))
        {
            div_쿠폰결과.InnerText = "만료일은 날짜형식으로 입력해야 하며 발행일자보다 커야 합니다.";
            return;
        }

        div_쿠폰결과.InnerText = "";

        for (uint i = 0; i < 발행쿠폰수량; i++)
        {
            String 쿠폰번호 = method2(16);
            div_쿠폰결과.InnerText += method2(16) + Environment.NewLine;

            sql = "select count(*) from coupon with(nolock) where 쿠폰번호='" + 쿠폰번호 + "' ";
            String chk = su.SqlFieldQuery(sql);

            if (chk == "0")
            {
                sql = "insert into coupon(쿠폰번호,machine_idx,발행금액,최소구매금액,만료일,reg_userid) values('" + 쿠폰번호 + "','" + 자판기번호 + "','" + 발행금액 + "','" + 최소구매금액 + "','" + 만료일 + "','" + userid + "'); select @@identity;";
                String 쿠폰idx = su.SqlFieldQuery(sql);
            }
            else
            {
                i--;
            }
        }

        div_쿠폰결과.InnerText = "쿠폰발행이 완료되었습니다";
        fn_쿠폰요약정보불러오기();
    }

    private static Random random = new Random();
    public static string method2(int length)
    {
        const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(characters, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
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
                SetDivision(e, 3, "normal tooltip", "정상", rpt);
            }
            else if (machine_net != "" && su.sg_int(machine_net) > 31 && su.sg_int(machine_net) <= 10000)
            {
                // 점검요청
                SetDivision(e, 3, "reqchk tooltip", "점검요청", "");

                if (su.sg_int(machine_net) > 1440)
                {
                    string rpt = "▶" + (su.sg_int(machine_net) / 60 / 24) + "일전 보고";

                    SetDivision(e, 3, "reqchk tooltip", "점검요청", rpt);
                }
                else
                {
                    string rpt2 = "▶" + (su.sg_int(machine_net) / 60) + "시간 전 보고";
                    SetDivision(e, 3, "reqchk tooltip", "점검요청", rpt2);
                }
            }
            else if (machine_net == "")
            {
                SetDivision(e, 3, "nohistory tooltip", "이력없음", "▶보고 이력이 없습니다");
            }
            else
            {
                // 장기미사용
                string rpt3 = "▶" + (su.sg_int(machine_net) / 60 / 24) + "일전 보고";
                SetDivision(e, 3, "nolonguse tooltip", "장기미사용", rpt3);
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

            if (machine_stat.Length == 22) // 전체 22 자릿수 유지 
            {
                string lackDigits = machine_stat.Substring(0, 18); // lack상태정보 
                string cupDispenser1 = machine_stat.Substring(17, 1);
                string cupDispenser2 = machine_stat.Substring(18, 1); // 현재 사용안함
                string iceMachineStat = machine_stat.Substring(20, 1); // 출빙기 상태 
                string iceMachineCommonStat = machine_stat.Substring(21, 1); // 출빙기 상태 

                int i = 0;
                string rpt = "";
                if (su.CntBytesLen(machine_stat) > 10 && lackDigits.Contains("1") == true) // 투출불량
                {
                    // 점검요청
                    rpt = Get랙상태정보(machine_stat);
                    SetDivision(e, 4, "reqchk tooltip", "점검요청", rpt);
                    i++;

                }
                else if (su.CntBytesLen(machine_stat) > 10 && lackDigits.Contains("1") == false) // 정상
                {
                    rpt = string.Format("▶랙상태(1~18) 정상, 컵투출기 정상,출빙기 정상");
                    SetDivision(e, 4, "normal tooltip", "정상", rpt);
                }
                else // 랙 사용안함
                {
                    rpt = "▶사용중인 랙이 없습니다.";
                    SetDivision(e, 4, "nouse tooltip", "사용안함", rpt);
                    i++;
                }

                if (i > 0)
                {
                    //점검요청
                    SetDivision(e, 4, "reqchk tooltip", "점검요청", rpt);
                }
            }
        }
        catch (Exception ex)
        {
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
        char[] arr = lackInfo.ToCharArray(0, 18);
        int cnt = arr.Length;
        string 투출불량 = "";
        string 미사용랙 = "";
        string 컵투출기 = "";
        string 출빙기 = "";

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < cnt; i++)
        {
            if (arr[i] == '1') // 투출불량
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

        arr = lackInfo.ToCharArray(17, 1); //투불불량
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

        if (status.Length > 0)
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

        if (status.Length > 0)
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

        if (status.Length > 0)
        {
            출빙기 += status;
        }

        return 투출불량 + 미사용랙 + 컵투출기 + 출빙기;
    }
    #endregion

    #region 자판기로그 페이징
    protected void gv_자판기로그_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        tb_getdata.Text = "";
        tb_senddata.Text = "";
        gv_자판기로그.PageIndex = e.NewPageIndex;
        fn_자판기로그검색();
    }
    #endregion

    #region 다른 자판기 메뉴 복사하기 
    protected void bt자판기메뉴_가져오기_Click(object sender, EventArgs e)
    {
        try
        {
            String 가져올자판기번호 = su.sg_db_query(자판기메뉴_가져오기_자판기번호.Text.Trim());
            String 자판기번호 = lb자판기메뉴관리_자판기번호.Text;

            // 기존 메뉴 및 상품 삭제처리
            sql = "update coffee set isusing=0,deletedate=getdate() where machine_idx='" + 자판기번호 + "' ";
            su.SqlNoneQuery(sql);

            sql = "update coffee_brand set isusing=0 where machine_idx='" + 자판기번호 + "' ";
            su.SqlNoneQuery(sql);

            sql = "update coffee_menu set isusing=0,deletedate=getdate(),delete_userid='" + userid + "' where machine_idx='" + 자판기번호 + "' ";
            su.SqlNoneQuery(sql);

            sql = "update machine_menu set isusing=0,deletedate=getdate(),delete_userid='" + userid + "' where machine_idx='" + 자판기번호 + "' ";
            su.SqlNoneQuery(sql);

            sql = "select idx, name," + 자판기번호 + ",dno from coffee_menu with(nolock) where machine_idx='" + 가져올자판기번호 + "'  and isusing=1 ";
            DataView dv = su.SqlDvQuery(sql);

            if (dv != null && dv.Count > 0)
            {
                for (int i = 0; i < dv.Count; i++)
                {
                    String 기존메뉴idx = dv[i]["idx"].ToString();

                    // 자판기 전용 기본메뉴 추가(캡슐커피)
                    sql = "insert into coffee_menu(name,machine_idx,dno) values('" + dv[i]["name"].ToString() + "','" + 자판기번호 + "','" + (i + 1).ToString() + "'); select @@identity; ";
                    String menu_idx = su.SqlFieldQuery(sql);

                    // 기존 메뉴에 있던 커피 정보를 가져와서 새로 넣는다. 
                    sql = "select machine_idx,coffee_menu_idx,coffee_brand_idx,coffee_idx from machine_menu with(nolock) where machine_idx='" + 가져올자판기번호 + "' and coffee_menu_idx='" + 기존메뉴idx + "' and isusing=1 ";
                    DataView dv2 = su.SqlDvQuery(sql);
                    for (int j = 0; j < dv2.Count; j++)
                    {
                        String 커피idx = dv2[j]["coffee_idx"].ToString();
                        String 브랜드idx = dv2[j]["coffee_brand_idx"].ToString();

                        sql = "select brand_name from coffee_brand with(nolock) where idx='" + 브랜드idx + "' ";
                        String 기존브랜드명 = su.SqlFieldQuery(sql);

                        sql = "select idx from coffee_brand with(nolock) where machine_idx='" + 자판기번호 + "' and brand_name='" + 기존브랜드명 + "' and isusing=1 ";
                        String 신규브랜드idx = su.SqlFieldQuery(sql);
                        if (신규브랜드idx == "")
                        {
                            sql = "insert into coffee_brand(brand_name,machine_idx) values('" + 기존브랜드명 + "','" + 자판기번호 + "'); select @@identity; ";
                            신규브랜드idx = su.SqlFieldQuery(sql);
                        }

                        //// 기본 커피상품 복사처리
                        sql = "insert into coffee(brand_idx,unit,name,comt,modify_date,imagebase64,imageext,imagesize,imagebase64_s,pr_type,isusing,machine_idx,deletedate) ";
                        sql += " select '" + 신규브랜드idx + "',unit,name,comt,getdate(),imagebase64,imageext,imagesize,imagebase64_s,pr_type,isusing,'" + 자판기번호 + "',null from coffee with(nolock) where idx='" + 커피idx + "'; select @@identity; ";
                        String 신규커피idx = su.SqlFieldQuery(sql);

                        sql = "insert into machine_menu(machine_idx,coffee_menu_idx,coffee_brand_idx,coffee_idx) ";
                        sql += " values(" + 자판기번호 + "," + menu_idx + ",'" + 신규브랜드idx + "','" + 신규커피idx + "'); select @@identity; ";
                        String m_menu_idx = su.SqlFieldQuery(sql);

                    }

                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(sql);
        }

        fn_메뉴정보불러오기();
    }

    #endregion

    #region 쿠폰 리스트 보기
    protected void gv쿠폰리스트_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        String ename = e.CommandName;
        String strval = (String)e.CommandArgument;

        if (ename == "Select")
        {


            sql = "select 쿠폰번호,발행금액, 최소구매금액, convert(varchar(10),만료일,121) 만료일,  Datediff(d, convert(varchar(10), getdate(), 121), 만료일) 잔여일자, case when Datediff(d, convert(varchar(10),getdate(),121),만료일)< 0 then 'Y' else 'N' end[만료여부] , useyn, machine_idx ,convert(varchar(8), regdate, 112) 발행일 ";
            sql += " from coupon with(nolock) where 쿠폰번호='" + strval + "' ";

            DataView dv = su.SqlDvQuery(sql);
            if (dv != null && dv.Count > 0)
            {
                String machine_idx = dv[0]["machine_idx"].ToString();
                String 발행금액 = dv[0]["발행금액"].ToString();
                String 최소구매금액 = dv[0]["최소구매금액"].ToString();
                String 만료일 = dv[0]["만료일"].ToString();
                String useyn = dv[0]["useyn"].ToString();
                String 발행일 = dv[0]["발행일"].ToString();

                sql = "select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS rownum,쿠폰번호,발행금액, 최소구매금액, convert(varchar(10),만료일,121) 만료일,  Datediff(d, convert(varchar(10), getdate(), 121), 만료일) 잔여일자, case when Datediff(d, convert(varchar(10),getdate(),121),만료일)< 0 then 'Y' else 'N' end[만료여부] , useyn, machine_idx ,convert(varchar(8), regdate, 112) 발행일 ";
                sql += " from coupon with(nolock) where  ";
                sql += " machine_idx='" + machine_idx + "' ";
                sql += " and machine_idx='" + machine_idx + "' ";
                sql += " and 발행금액='" + 발행금액 + "' ";
                sql += " and 최소구매금액='" + 최소구매금액 + "' ";
                sql += " and convert(varchar(10),만료일,121)='" + 만료일 + "' ";
                sql += " and useyn='" + useyn + "' ";
                sql += " and convert(varchar(8), regdate, 112)='" + 발행일 + "' ";
                su.binding(gv쿠폰상세리스트, sql);
            }
        }
    }
    #endregion

    #region 자판기로그 상세 보기
    protected void gv_자판기로그_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        String ename = e.CommandName;
        String strval = (String)e.CommandArgument;

        if (ename == "Select")
        {

            sql = "select idx,";
            sql += " case when etype='1' then '1버전체크' when etype='2' then '2상품조회' when etype='3' then '3결제보고' when etype='4' then '4쿠폰조회' when etype='5' then '5쿠폰사용' when etype='6' then '6쿠폰사용취소' when etype='7' then '7자판기번호조회' when etype='8' then '8자판기정보수정' when etype='9' then '9상태보고' when etype='10' then '10라이브체크' when etype='11' then '11문열림보고' when etype='12' then '12결제취소보고' else '미정의' end [구분] , etype ";
            sql += " ,getdata,isnull(senddata,'') senddata,clientip,convert(varchar(20),regdate,121) regdate from machine_log with(nolock) ";
            sql += " where idx = '" + strval + "'  ";
            DataView dv = su.SqlDvQuery(sql);

            tb_getdata.Text = dv[0]["getdata"].ToString();
            tb_senddata.Text = dv[0]["senddata"].ToString();
        }
    }
    #endregion

    #region 배너정보 관리 
    /// <summary>
    /// 배너 조회조건 Dropdownlist
    /// </summary>
    private void BindBannerSrchCondition()
    {
        try
        {
            var dic = new Dictionary<string, string>();
            dic.Add("all", "전체보기");
            dic.Add("y", "게시여부Y");
            dic.Add("n", "게시여부N");

            su.binding(bannersrchDropDownLst, dic);
        }
        catch { }

    }
    /// <summary>
    /// 배너등록시 기본 구분값 설정
    /// </summary>
    private void BindDefaultType()
    {
        rdoHome.Checked = true;
        divHomeBannerUpload.Visible = true;
    }
    /// <summary>
    /// 배너동작시간
    /// </summary>
    private void BindBannerOperatingTime()
    {
        try
        {
            var dic = new Dictionary<string, string>();
            dic.Add("0", "선택");
            dic.Add("5", "5초");
            dic.Add("10", "10초");
            dic.Add("15", "15초");
            dic.Add("30", "30초");
            dic.Add("60", "60초");

            su.binding(bannerOptimeDropDownList, dic);
        }
        catch { }
    }

    #region 배너이미지 저장 이벤트 
    /// <summary>
    /// 배너이미지 파일 저장하기 
    /// bType =1 (홈광고),  bType = 2 (때배너)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSavBannerFile_Click(object sender, EventArgs e)
    {
        var bType = rdoHome.Checked == true ? "홈광고" : "띠배너";
        var title = txtBannerTitle.Text;
        var optime = bannerOptimeDropDownList.SelectedValue;
        var startDt = bannerStartDate.Text;
        var endDt = bannerEndDate.Text;
        var useYn = rdoUseY.Checked == true ? "Y" : "N";
        var uploadImage1 = "";
        var uploadImage2 = "";

        //var agencyIdx = su.sg_db_query(총판수정_코드.Text.Trim());
        //var branchIdx = su.sg_db_query(대리점수정_코드.Text.Trim());

        var companyCode = hCompanyIdx2.Value.ToString();
        var agencyCode = hAgencyIdx2.Value.ToString();
        var branchCode = hBranchIdx2.Value.ToString();
        var machineCode = hMachineIdx2.Value.ToString();

        var prefix = Guid.NewGuid().ToString(); // companyCode + agencyCode + branchCode + machineCode;

        if (rdoHome.Checked && txtHomebannerUpload.HasFile) // 업로드 컨트롤로 파일을 선택한 게 있다면 
        {
            try
            {
                var fileName = txtHomebannerUpload.FileName;
                if (imageMimeTypes.Contains(txtHomebannerUpload.PostedFile.ContentType))
                {
                    txtHomebannerUpload.SaveAs(Server.MapPath("~/Banner/" + fileName));
                    uploadImage1 = txtHomebannerUpload.PostedFile.FileName;

                    // 파일이름 변경 
                    File.Move(Server.MapPath("~/Banner/" + fileName), Server.MapPath("~/Banner/" + prefix + "_" + fileName));
                    var destifile1 = Path.GetFileName(Server.MapPath("~/Banner/" + prefix + "_" + fileName));

                    if (lblKid.Text.Length == 0) // 신규 등록이면 
                    {
                        if (SaveBannerImage(bType, title, Convert.ToInt32(optime), destifile1, startDt, endDt, useYn, ""))
                        {
                            lblHomeBannerUpload.Text = "이미지 업로드 성공";
                        }
                        else
                        {
                            lblHomeBannerUpload.Text = "이미지 업로드 실패";
                        }
                    }
                    else // 수정이면 
                    {
                        if (UpdateBannerImage(bType, title, Convert.ToInt32(optime), destifile1, startDt, endDt, useYn, ""))
                        {
                            lblHomeBannerUpload.Text = "이미지 업로드 성공";
                            lblKid.Text = "";
                        }
                        else
                        {
                            lblHomeBannerUpload.Text = "이미지 업로드 실패";
                        }
                    }
                }
                else
                {
                    lblHomeBannerUpload.Text = "이미지 형식 에러!!!";
                }
            }
            catch (Exception ex)
            {
                su.sg_alert(this.Page, ex.Message);
            } 
        } else if(rdoHome.Checked && lblKid.Text.Length > 0 && lblHomeBannerUpload.Text.Length > 0) // update only data 
        {
            try
            {
                if (UpdateBannerImage(bType, title, Convert.ToInt32(optime), lblHomeBannerUpload.Text, startDt, endDt, useYn, ""))
                {
                    lblHomeBannerUpload.Text = "이미지 업데이트 성공";
                    lblKid.Text = "";
                }
                else
                {
                    lblHomeBannerUpload.Text = "이미지 업데이트 실패";
                }
            }
            catch (Exception ex)
            {
                su.sg_alert(this.Page, ex.Message);
            }
        }

        if (rdoBand.Checked && txtBandBannerUpload.HasFile && txtBandBannerUpload2.HasFile) // 띠배너 업로드 파일을 선택했다면 
        {
            try
            {
                var fileName1 = txtBandBannerUpload.FileName; // 띠배너 
                var fileName2 = txtBandBannerUpload2.FileName; // 배너상테 

                if (fileName1 == fileName2)
                {
                    lblBannerUpload2.Text = "동일한 파일명 에러!!!";
                    return;
                }

                if (imageMimeTypes.Contains(txtBandBannerUpload.PostedFile.ContentType) && imageMimeTypes.Contains(txtBandBannerUpload2.PostedFile.ContentType))
                {
                    // 띠배너 이미지 
                    txtBandBannerUpload.SaveAs(Server.MapPath("~/Banner/" + fileName1));
                    uploadImage1 = txtBandBannerUpload.PostedFile.FileName;

                    // 띠배너 이미지 저장
                    File.Move(Server.MapPath("~/Banner/" + fileName1), Server.MapPath("~/Banner/" + prefix + "_" + fileName1));
                    var destifile2 = Path.GetFileName(Server.MapPath("~/Banner/" + prefix + "_" + fileName1));

                    // 띠배너 상세 이미지 
                    txtBandBannerUpload2.SaveAs(Server.MapPath("~/Banner/" + fileName2));
                    uploadImage2 = txtBandBannerUpload2.PostedFile.FileName;
                    // 띠배너 상세 이미지 저장
                    File.Move(Server.MapPath("~/Banner/" + fileName2), Server.MapPath("~/Banner/" + prefix + "_" + fileName2));
                    var destifile3 = Path.GetFileName(Server.MapPath("~/Banner/" + prefix + "_" + fileName2));

                    if (lblKid.Text.Length  == 0) // 신규
                    {
                        if (SaveBannerImage(bType, title, Convert.ToInt32(optime), destifile2, startDt, endDt, useYn, destifile3))
                        {
                            lblBannerUpload2.Text = "이미지 업로드 성공";
                        }
                        else
                        {
                            lblBannerUpload2.Text = "이미지 업로드 실패";
                        }
                    }
                    else // 수정 
                    {
                        if (UpdateBannerImage(bType, title, Convert.ToInt32(optime), destifile2, startDt, endDt, useYn, destifile3))
                        {
                            lblKid.Text = "";
                            lblBannerUpload2.Text = "이미지 업로드 성공";
                        }
                        else
                        {
                            lblBannerUpload2.Text = "이미지 업로드 실패";
                        }
                    }
                }
                else
                {
                    lblBannerUpload2.Text = "이미지 형식 에러!!!";
                }
            }
            catch (Exception ex)
            {
                su.sg_alert(this.Page, ex.Message);
            }
        } else if(rdoBand.Checked && lblKid.Text.Length > 0 && lblBannerUpload.Text.Length > 0 && lblBannerUpload2.Text.Length > 0) // update only data.
        {
            try
            {
                if (UpdateBannerImage(bType, title, Convert.ToInt32(optime), lblBannerUpload.Text, startDt, endDt, useYn, lblBannerUpload2.Text))
                {
                    lblKid.Text = "";
                    lblBannerUpload2.Text = "이미지 업로드 성공";
                }
                else
                {
                    lblBannerUpload2.Text = "이미지 업로드 실패";
                }
            }
            catch (Exception ex)
            {
                su.sg_alert(this.Page, ex.Message);
            }
           
        }
    }
    #endregion

    #region 배너이미지 이미지 저장 & 수정하기 
    /// <summary>
    /// 배너 이미지 저장 
    /// </summary>
    /// <param name="bannerType">배너타입</param>
    /// <param name="title">제목</param>
    /// <param name="timeInterval">동작시간</param>
    /// <param name="imageUrl">배너이미지</param>
    /// <param name="startDate">동작시작시간</param>
    /// <param name="endDate">동작종료시간</param>
    /// <param name="useYn">사용여부</param>
    /// <param name="imgUrl2">배너이미지 상세2</param>
    /// <returns></returns>
    private bool SaveBannerImage(string bannerType, string title, int timeInterval, string imageUrl, string startDate, string endDate, string useYn, string imgUrl2)
    {
        bool isSuccess = false;
        try
        {
            //var agencyCode = su.sg_db_query(총판수정_코드.Text.Trim());
            //var branchCode = su.sg_db_query(대리점수정_코드.Text.Trim());

            if (string.IsNullOrEmpty(hCompanyIdx2.Value) || string.IsNullOrEmpty(hAgencyIdx2.Value))
            {
                throw new Exception("유효하지 않은 정보입니다. 다시 확인해 주세요");
            }

            var icompany_idx = Convert.ToInt32(hCompanyIdx2.Value);
            var iagency_idx = Convert.ToInt32(hAgencyIdx2.Value);
            var ibranch_idx = String.IsNullOrEmpty(hBranchIdx2.Value) == true ? 0 :  Convert.ToInt32(hBranchIdx2.Value);
            var imachine_idx = String.IsNullOrEmpty(hMachineIdx2.Value) == true ? 0 : Convert.ToInt32(hMachineIdx2.Value);
 
            var sql = @"insert into dbo.tbl_banner (company_idx, agency_idx, branch_idx, machine_idx, bannertype, title, timeinterval, imgurl, startdate, enddate, useyn, imgurl2) 
                                                values ( {0}, {1}, {2}, {3}, '{4}', '{5}', {6}, '{7}', '{8}', '{9}', '{10}', '{11}');";

            var wSql = string.Format(sql, icompany_idx
                                        , iagency_idx
                                        , ibranch_idx
                                        , imachine_idx
                                        , bannerType
                                        , title
                                        , timeInterval
                                        , imageUrl
                                        , startDate
                                        , endDate
                                        , useYn
                                        , imgUrl2
                                        );

            isSuccess = su.SqlNoneQuery(wSql);
        }
        catch (Exception ex)
        {
            su.sg_alert(this.Page, ex.Message);
        }

        return isSuccess;
    }

    /// <summary>
    /// 배너이미지 수정 
    /// </summary>
    /// <param name="bannerType"></param>
    /// <param name="title"></param>
    /// <param name="timeInterval"></param>
    /// <param name="imageUrl"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="useYn"></param>
    /// <param name="imgUrl2"></param>
    /// <returns></returns>
    private bool UpdateBannerImage(string bannerType, string title, int timeInterval, string imageUrl, string startDate, string endDate, string useYn, string imgUrl2)
    {
        bool isSuccess = false;
        try
        {
            //var agencyCode = su.sg_db_query(총판수정_코드.Text.Trim());
            //var branchCode = su.sg_db_query(대리점수정_코드.Text.Trim());

            if(string.IsNullOrEmpty(hCompanyIdx2.Value) || string.IsNullOrEmpty(hAgencyIdx2.Value))
            {
                throw new Exception("유효하지 않은 정보입니다. 다시 확인해 주세요");
            }

            var icompany_idx = Convert.ToInt32(hCompanyIdx2.Value);
            var iagency_idx = Convert.ToInt32(hAgencyIdx2.Value);
            var ibranch_idx = String.IsNullOrEmpty(hBranchIdx2.Value) == true ? 0 : Convert.ToInt32(hBranchIdx2.Value);
            var imachine_idx = String.IsNullOrEmpty(hMachineIdx2.Value) == true ? 0 : Convert.ToInt32(hMachineIdx2.Value);

            var kid = Convert.ToInt32(lblKid.Text);

            var sql = @"update dbo.tbl_banner 
                            set company_idx = {0}
                            , agency_idx = {1}
                            , branch_idx = {2}
                            , machine_idx = {3}
                            , bannertype = '{4}'
                            , title = '{5}'
                            , timeinterval = {6}
                            , imgurl = '{7}'
                            , startdate = '{8}'
                            , enddate = '{9}'
                            , useyn = '{10}'                        
                            , imgurl2 = '{11}' 
                        from dbo.tbl_banner
                        where kid = {12}";


            var wSql = string.Format(sql, icompany_idx
                                        , iagency_idx
                                        , ibranch_idx
                                        , imachine_idx
                                        , bannerType
                                        , title
                                        , timeInterval
                                        , imageUrl
                                        , startDate
                                        , endDate
                                        , useYn
                                        , imgUrl2
                                        , kid
                                        );

            isSuccess = su.SqlNoneQuery(wSql);

        }
        catch (Exception ex)
        {
            su.sg_alert(this.Page, ex.Message);
        }
        return isSuccess;
    }
    #endregion 

    #region 배너리스트 로우 클릭시 
    protected void gv배너리스트_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        var commandName = e.CommandName;
        if (commandName == "Select")
        {
            lblKid.Text =(String)e.CommandArgument; // kid by tbl_banner 

            div_bannerRegistration.Visible = true;
            VisibleForModifyBanner();
            GetBannerInfo( Convert.ToInt32(lblKid.Text));
        }
        else
        {
            div_bannerRegistration.Visible = false;
        }
    }

    /// <summary>
    /// 배너 이미지 수정버튼들 활성화
    /// </summary>
    private void VisibleForModifyBanner()
    {
        btnSaveBannerImage.Visible = false;
        btnModifyBannerImage.Visible = true;
        btnDeleteBannerImage.Visible = true;
    }

    private void VisibleForRegisterBanner()
    {
        btnSaveBannerImage.Visible = true;
        btnModifyBannerImage.Visible = false;
        btnDeleteBannerImage.Visible = false;
    }

    /// <summary>
    /// 배너정보 불러오기
    /// 그리드에서 선택한 배너정보를 불러온다
    /// </summary>
    /// <param name="kid"></param>
    private void GetBannerInfo(int kid)
    {
        var sql = @"select company_idx, agency_idx, branch_idx
                    , machine_idx, bannertype, title, timeinterval
                    , startdate, enddate , imgurl, imgurl2 
                    , useyn from dbo.tbl_banner with(nolock)
                    where kid = {0}";

        var wSql = string.Format(sql, kid);
        var dv = su.SqlDvQuery(wSql);

        var companyIdx = dv[0]["company_idx"].ToString();
        var agencyIdx = dv[0]["agency_idx"].ToString();
        var branch_idx = dv[0]["branch_idx"].ToString();
        var machine_idx = dv[0]["machine_idx"].ToString();
        var bannertype = dv[0]["bannertype"].ToString();
        var title = dv[0]["title"].ToString();    
        var timeinterval = dv[0]["timeinterval"].ToString();
        var startdate = dv[0]["startdate"].ToString();
        var enddate = dv[0]["enddate"].ToString();
        var useyn = dv[0]["useyn"].ToString();
        var imgurl = dv[0]["imgurl"].ToString();
        var imgurl2 = dv[0]["imgurl2"].ToString(); // 띠배너인 경우 상세 이미지

        hCompanyIdx2.Value = companyIdx;
        hAgencyIdx2.Value = agencyIdx;
        hBranchIdx2.Value = branch_idx;
        hMachineIdx2.Value = machine_idx;

        if (bannertype == "홈광고") // 홈광고
        {
            divHomeBannerUpload.Visible = true;
            divBandBannerUpload.Visible = false;
            rdoHome.Checked = true;
            rdoBand.Checked = false;
            lblHomeBannerUpload.Text = imgurl;
        }
        else // 띠배너
        {
            divHomeBannerUpload.Visible = false;
            divBandBannerUpload.Visible = true;
            rdoHome.Checked = false;
            rdoBand.Checked = true;
            lblBannerUpload.Text = imgurl;
            lblBannerUpload2.Text = imgurl2;
        }

        txtBannerTitle.Text = title;
        bannerOptimeDropDownList.SelectedValue = timeinterval;

        bannerStartDate.Text = startdate;
        bannerEndDate.Text = enddate;

        if (useyn == "Y")
        {
            rdoUseY.Checked = true;
            rdoUseN.Checked = false;
        }
        else
        {
            rdoUseY.Checked = false;
            rdoUseN.Checked = true;
        }
    }

    #endregion

    #region 배너이미지 등록창 닫기
    /// <summary>
    /// 배너이미지 등록창 닫기 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCloseBanner_Click(object sender, EventArgs e)
    {
        div_bannerRegistration.Visible = false;   
        GetBannerList(hAgencyIdx2.Value);
    }
    #endregion

    /// <summary>
    /// 기본 날짜 설정
    /// </summary>
    private void SetDefaultDate()
    {
        bannerStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        bannerEndDate.Text = DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// 배너 게시여부 
    /// </summary>
    private void SetBannerUseYn()
    {
        rdoUseY.Checked = true;
        rdoUseN.Checked = false;
    }

    #region 배너 구분
    protected void rdoBand_CheckedChanged(object sender, EventArgs e)
    {
        if (rdoBand.Checked == true)
        {
            divHomeBannerUpload.Visible = false;
            divBandBannerUpload.Visible = true;
            rdoHome.Checked = false;
        }
    }

    protected void rdoHome_CheckedChanged(object sender, EventArgs e)
    {
        if (rdoHome.Checked == true)
        {
            divHomeBannerUpload.Visible = true;
            divBandBannerUpload.Visible = false;
            rdoBand.Checked = false;
        }
    }

    #region 배너이미지 수정 이벤트 
    /// <summary>
    /// 배너이미지 수정하기 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnModifyBannerImage_Click(object sender, EventArgs e)
    {
        if (lblKid.Text.Length == 0) return;
        btnSavBannerFile_Click(sender, e);
    }
    #endregion

    #region 배너이미지 삭제 이벤트 
    /// <summary>
    /// 배너이미지 삭제하기 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDeleteBannerImage_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblKid.Text.Length == 0) return;
            var sql = @"delete dbo.tbl_banner where kid = {0}";
            var wSql = string.Format(sql, lblKid.Text);

            if (su.SqlNoneQuery(wSql))
            {
                su.sg_alert(this.Page, "선택한 배너이미지를 삭제하였습니다.");
                lblKid.Text = "";
                btnCloseBanner_Click(sender, e);
            }
            else
            {
                su.sg_alert(this.Page, "선택한 배너이미지 삭제 실패!!!");
            }
        }
        catch (Exception ex)
        {
            su.sg_alert(this.Page, ex.Message);
        }
    }

    /// <summary>
    /// 전체보기, 게시여부YN
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void bannersrchDropDownLst_SelectedIndexChanged(object sender, EventArgs e)
    {
        var pCompanyIdx = Convert.ToInt32(hCompanyIdx.Value);
        var pAgencycIdx = Convert.ToInt32(hAgencyIdx.Value);
        var pBranchIdx = string.IsNullOrEmpty(hBranchIdx.Value) == true ? 0 : Convert.ToInt32(hBranchIdx.Value);
        var pMachineIdx = string.IsNullOrEmpty(hMachineIdx.Value) == true ? 0 : Convert.ToInt32(hMachineIdx.Value);

        try
        {
            var sql = @"select ROW_NUMBER() over (order by (select 1)) AS rownum
                        , kid
                        , company_idx
                        , agency_idx
                        , branch_idx
                        , machine_idx
                        , bannertype as btype
                        , title
                        , timeinterval
                        , (startdate + '~' + enddate) as opdate
                        , imgurl
                        , useyn
                        from dbo.tbl_banner
                        where 1 = 1                       
                        and company_idx = {0}
                        and agency_idx = {1} ";

            var item = bannersrchDropDownLst.SelectedValue.ToString();

            if (item == "all")
            {
                if(pBranchIdx > 0 && pMachineIdx == 0)
                {
                    sql += " and branch_idx = {2}";
                } else if( pBranchIdx > 0 && pMachineIdx > 0)
                {
                    sql += " and branch_idx = {2} and machine_idx = {3}";
                }

                sql += " order by startdate";
            }
            else if (item == "y")
            {
                if (pBranchIdx > 0 && pMachineIdx == 0)
                {
                    sql += " and branch_idx = {2}";
                }
                else if (pBranchIdx > 0 && pMachineIdx > 0)
                {
                    sql += " and branch_idx = {2} and machine_idx = {3}";
                }

                sql += " and useyn = 'Y' order by startdate";
            }
            else
            {
                if (pBranchIdx > 0 && pMachineIdx == 0)
                {
                    sql += " and branch_idx = {2}";
                }
                else if (pBranchIdx > 0 && pMachineIdx > 0)
                {
                    sql += " and branch_idx = {2} and machine_idx = {3}";
                }

                sql += " and useyn = 'N' order by startdate";
            }
           
            su.binding(gv배너리스트, string.Format(sql, pCompanyIdx, pAgencycIdx, pBranchIdx, pMachineIdx));
        }
        catch(Exception ex)
        {
            su.sg_alert(this.Page, ex.Message);
        }
    }

    #endregion

    #endregion

    #endregion



}


