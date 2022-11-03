using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Menu_사용자관리 : System.Web.UI.Page
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
    public String grp_idx = "";
    public String sql = "";

    #region PageLoad 

    protected void Page_Load(object sender, EventArgs e)
    {

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
        grp_idx = su.ReqS(this.Page, "grp_idx");


        String 총판코드 = dd총판.SelectedValue;
        sql = "SELECT name,idx from agency ORDER BY name";
        su.binding(dd총판, sql, "-총판전체");
        su.setDropDownList(dd총판, 총판코드);

        String 대리점코드 = dd대리점.SelectedValue;
        sql = "SELECT name,idx from branch ";
        if (총판코드 != "")
        {
            sql += " where agency_idx='"+ 총판코드 +"' ";
        }
        sql += " ORDER BY name";
        su.binding(dd대리점, sql, "-대리점전체");
        su.setDropDownList(dd대리점, 대리점코드);

        String 총판코드2 = 사용자등록_총판선택.SelectedValue;
        sql = "SELECT name,idx from agency ORDER BY name";
        su.binding(사용자등록_총판선택, sql, "-총판전체");
        su.setDropDownList(사용자등록_총판선택, 총판코드2);

        String 대리점코드2 = 사용자등록_대리점선택.SelectedValue;
        sql = "SELECT name,idx from branch ";
        sql += " where agency_idx='" + 총판코드2 + "' ";
        sql += " ORDER BY name";
        su.binding(사용자등록_대리점선택, sql, "-대리점전체");
        su.setDropDownList(사용자등록_대리점선택, 대리점코드2);

        String 자판기코드2 = 사용자등록_자판기선택.SelectedValue;
        sql = "SELECT install_spot,pos_number from machine ";
        sql += " where branch_idx='" + 대리점코드2 + "' ";
        sql += " ORDER BY install_spot";
        su.binding(사용자등록_자판기선택, sql, "-자판기전체");
        su.setDropDownList(사용자등록_자판기선택, 자판기코드2);


        String 총판코드3 = 사용자수정_총판선택.SelectedValue;
        sql = "SELECT name,idx from agency ORDER BY name";
        su.binding(사용자수정_총판선택, sql, "-총판전체");
        su.setDropDownList(사용자수정_총판선택, 총판코드3);

        String 대리점코드3 = 사용자수정_대리점선택.SelectedValue;
        sql = "SELECT name,idx from branch ";
        sql += " where agency_idx='" + 총판코드3 + "' ";
        sql += " ORDER BY name";
        su.binding(사용자수정_대리점선택, sql, "-대리점전체");
        su.setDropDownList(사용자수정_대리점선택, 대리점코드3);

        String 자판기코드3 = 사용자수정_자판기선택.SelectedValue;
        sql = "SELECT install_spot,pos_number from machine ";
        sql += " where branch_idx='" + 대리점코드3 + "' ";
        sql += " ORDER BY install_spot";
        su.binding(사용자수정_자판기선택, sql, "-자판기전체");
        su.setDropDownList(사용자수정_자판기선택, 자판기코드3);

        if (!IsPostBack)
        {
            bt새로고침_Click(null, null);
        }
    }

    #endregion

    #region 새로고침 검색
    protected void bt새로고침_Click(object sender, EventArgs e)
    {
        String 사용여부 = dd사용여부.SelectedValue;
        String 총판코드 = dd총판.SelectedValue;
        String 대리점코드 = dd대리점.SelectedValue;
        String 검색어 = su.sg_db_query(tb검색어.Text.TrimEnd().TrimStart().Replace(" ","%") );
        sql = @"SELECT a.idx,a.userid,a.usernm,a.usrtel,a.mphone,case when a.lev='s' then '시스템' when a.lev='b' then '대리점' when a.lev='a' then '총판'  when a.lev='c' then '회사' else a.lev end level,a.active,a.company_idx,a.agency_idx,a.branch_idx,a.pos_number,a.usrorg,a.locktime, a.lst_logintime ";
        sql += " ,case when a.branch_idx is null then '관리자' else b.name end 총판명, c.name 대리점명";
        sql += " FROM member a ";
        sql += " left outer join agency b on a.agency_idx = b.idx ";
        sql += " left outer join branch c on a.agency_idx = c.agency_idx and a.branch_idx = c.idx ";
        sql += " where 1=1 ";

        if (총판코드 != "")
        {
            sql += " and a.agency_idx='" + 총판코드 + "' ";
        }
        if (대리점코드 != "")
        {
            sql += " and a.branch_idx='" + 대리점코드 + "' ";
        }
        if (사용여부 != "") 
        {
            sql += " and a.active='"+ 사용여부+"' ";
        }
        if (검색어 != "")
        {
            sql += " and ( a.usernm like '%" + 검색어 + "%' or b.name like '%" + 검색어+ "%' or c.name like '%" + 검색어 + "%' or a.userid like '%" + 검색어 + "%') ";

        }
        sql += " order by a.company_idx,a.agency_idx,a.branch_idx,a.pos_number";

        su.binding(gv사용자, sql);



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
            if(dv!=null && dv.Count > 0)
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

    #region 사용자등록 
    protected void bt사용자등록_Click(object sender, EventArgs e)
    {
        div_사용자등록.Visible = true;
    }

    protected void bt사용자등록닫기_Click(object sender, EventArgs e)
    {
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

        if (아이디 == "" || su.isSptext(아이디)==true || su.isHangul(아이디)==true)
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

        if (이름 =="" || su.isSptext(이름)==true)
        {
            사용자등록_결과.InnerText = "이름을 입력해 주세요. 이름에는 특수문자를 사용할 수 없습니다.";
            return;
        }

        if (일반전화.Replace("-","") == "" || su.isSptext(일반전화.Replace("-", "")) == true || 일반전화.Replace("-","").Length>12 || su.isNumeric(일반전화.Replace("-", "")) == false)
        {
            사용자등록_결과.InnerText = "일반전화번호를 입력해 주세요";
            return;
        }

        if (휴대폰.Replace("-", "") == "" || su.isSptext(휴대폰.Replace("-", "")) == true || 휴대폰.Replace("-", "").Length > 12 || su.isNumeric(휴대폰.Replace("-", ""))==false)
        {
            사용자등록_결과.InnerText = "휴대폰번호를 입력해 주세요";
            return;
        }

        sql = "insert into member(userid,usernm,usrtel,mphone,active,company_idx,agency_idx,branch_idx,pos_number,usrorg,locktime,regdate) ";
        sql += " values('" + 아이디 + "','" + 이름  + "','" + 일반전화 + "','" + 휴대폰 + "','1','1','" + 총판코드 + "',dbo.isblink('" + 대리점코드 + "'),dbo.isblink('" + 자판기코드 + "'),'" + 소속 + "',getdate(),getdate()); ";
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

        if (일반전화.Replace("-", "") == "" || su.isSptext(일반전화.Replace("-", "")) == true || 일반전화.Replace("-", "").Length > 12 || su.isNumeric(일반전화.Replace("-", "")) == false)
        {
            사용자수정_결과.InnerText = "일반전화번호를 입력해 주세요";
            return;
        }

        if (휴대폰.Replace("-", "") == "" || su.isSptext(휴대폰.Replace("-", "")) == true || 휴대폰.Replace("-", "").Length > 12 || su.isNumeric(휴대폰.Replace("-", "")) == false)
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

    #region 총판코드/대리점코드 선택시 
    protected void dd총판_SelectedIndexChanged(object sender, EventArgs e)
    {

        bt새로고침_Click(null, null);
    }
    #endregion

    #region gv사용자 Pagesing 처리
    protected void gv사용자_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv사용자.PageIndex = e.NewPageIndex;
        bt새로고침_Click(null, null);
    }
    #endregion


}
