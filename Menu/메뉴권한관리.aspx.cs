using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Drawing;

public partial class Menu_사용자메뉴권한관리 : System.Web.UI.Page
{

    SgFramework.SgUtil su = new SgFramework.SgUtil();
    String sql = "";
    String AgencyID = "";
    String UserID = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.tb업체명검색어.Attributes["onkeyPress"] = "if(event.keyCode == 13) {" +
    ClientScript.GetPostBackEventReference(Button3, "") + "; return false;}";

        this.tb사용자.Attributes["onkeyPress"] = "if(event.keyCode == 13) {" +
    ClientScript.GetPostBackEventReference(Button3, "") + "; return false;}";

        this.tb메뉴.Attributes["onkeyPress"] = "if(event.keyCode == 13) {" +
    ClientScript.GetPostBackEventReference(Button3, "") + "; return false;}";

        this.tb부여메뉴.Attributes["onkeyPress"] = "if(event.keyCode == 13) {" +
    ClientScript.GetPostBackEventReference(Button3, "") + "; return false;}";

        UserID = su.ReqS(this.Page, "userid");

        // 권한 체크
        su.PermitPage(this.Page, "4");

        if (!IsPostBack)
        {
            Button1_Click(null, null);
        }
    }

    // 검색 처리 내역
    protected void Button1_Click(object sender, EventArgs e)
    {
        String mid = su.sg_db_query(su.Req(this.Page, "mid"), true);

        #region 사용자기준
        String 업체명 = su.sg_db_query(tb업체명검색어.Text, true);
        String 검색어 = su.sg_db_query(tb사용자.Text, true);

        sql = "select a.*,b.name [총판명] , c.name [대리점명] from dbo.member a left outer join dbo.agency b on a.agency_idx=b.idx  ";
        sql += " left outer join dbo.branch c on a.branch_idx = c.idx ";
        sql += " where a.active=1 ";

        if (검색어 != "")
        {
            sql += " and ( a.이름 like '%"+ 검색어+"%' or a.id='"+ 검색어+"' ) ";       
        }
        if (업체명 != "")
        {
            sql += " and ( b.상호 like '%" + 업체명 + "%' ) ";
        }
        sql += " order by 총판명,대리점명,a.usernm";

        su.binding(gv사용자, sql);

        String 메뉴검색 = su.sg_db_query(tb메뉴.Text);
        sql = "select id,메뉴그룹,메뉴명 from dbo.t_메뉴 where 공통메뉴여부='N' ";
        if (메뉴검색 != "")
        {
            sql += " and ( 메뉴그룹='" + 메뉴검색 + "' or 메뉴명 like '%" + 메뉴검색 + "%') ";
        }
        if (mid != "")
        {
            sql += " and id='" + mid + "' ";
        }
        if (gv사용자.SelectedValue != null && gv사용자.SelectedValue != "")
        {
            sql += " and id not in (select 메뉴id from  dbo.t_메뉴권한 where 직원id='" + gv사용자.SelectedValue.ToString() + "') ";
        }

        sql += " order by 메뉴명 ";
        su.binding(gv선택, sql);

        if (gv사용자.SelectedValue != null)
        {
            
            String 부여메뉴 = su.sg_db_query(tb부여메뉴.Text);
            
            sql = "select a.id, a.메뉴id,a.직원id, b.메뉴그룹, b.메뉴명 ";
            sql += " from dbo.t_메뉴권한 a left outer join dbo.t_메뉴 b on a.메뉴id = b.id where 1=1 ";

            if (부여메뉴 != "")
            {
                sql += " and (b.메뉴그룹='"+ 부여메뉴 +"' or b.메뉴명 like '%"+ 부여메뉴 +"%') ";
            }

            sql += " and a.직원id ='" + gv사용자.SelectedValue + "' order by b.메뉴명 ";
            su.binding(gv메뉴, sql);
        }
        #endregion

    }

    #region 사용자기준 gv
    protected void gv사용자_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String cName = e.CommandName;
        String cVal = (String)e.CommandArgument.ToString();

        if (e.CommandName == "Page")
        {
            gv사용자.PageIndex = Convert.ToInt32(e.CommandArgument)-1;
        }
        if (e.CommandName == "ok")
        {
            gv사용자.SelectedIndex = Convert.ToInt32(e.CommandArgument);
        }

        Button1_Click(null, null);
    }

    protected void gv사용자_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv사용자.PageIndex = e.NewPageIndex;
    }

    protected void gv메뉴_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String cName = e.CommandName;
        String cVal = (String)e.CommandArgument.ToString();
        if (e.CommandName == "ok")
        {
            sql = "insert into dbo.l_권한부여로그(권한부여자ID,부여대상자ID,부여권한seq,부여구분) select '"+ UserID+"',직원ID,메뉴id,'회수' from dbo.t_메뉴권한 where id='" + cVal + "'; ";
            su.SqlNoneQuery(sql);

            sql = "delete from dbo.t_메뉴권한 where id='" + cVal + "' ";
            su.SqlNoneQuery(sql);
        }

        if (e.CommandName == "Page")
        {
            gv메뉴.PageIndex = Convert.ToInt32(e.CommandArgument) - 1;
        }

        Button1_Click(null, null);
    }

    protected void gv메뉴_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv메뉴.PageIndex = e.NewPageIndex;
    }

    protected void gv선택_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String cName = e.CommandName;
        String cVal = (String)e.CommandArgument.ToString().Trim();

        if (e.CommandName == "ok")
        {
            String 부여대상자ID = "";
            try
            {
                부여대상자ID = gv사용자.SelectedValue.ToString().Trim();
            }
            catch (Exception ex) { }

            if (부여대상자ID != "" && cVal!="" )
            {
                String cnt = su.SqlFieldQuery("select count(*) from  dbo.t_메뉴권한 where 직원id='" + 부여대상자ID + "' and 메뉴id='" + cVal + "' ");

                if (cnt == "0")
                {
                    sql = "insert into dbo.l_권한부여로그(권한부여자ID,부여대상자ID,부여권한seq,부여구분) values('" + UserID + "','" + gv사용자.SelectedValue + "','" + cVal + "','부여'); ";
                    su.SqlNoneQuery(sql);

                    sql = "insert into dbo.t_메뉴권한(직원id,메뉴id) values('" + gv사용자.SelectedValue + "','" + cVal + "');";
                    su.SqlNoneQuery(sql);
                }
            }
            else
            {
                su.alert(this.Page, "메뉴 권한 부여대상자를 선택해 주세요");
            }
        }

        if (e.CommandName == "Page")
        {
            gv선택.PageIndex = Convert.ToInt32(e.CommandArgument) - 1;
        }

        Button1_Click(null, null);
    }
    #endregion

    #region 모두체크

    protected void gv선택_on모두체크change(object sender, EventArgs e)
    {
        CheckBox chk_box;

        if (gv선택.Rows.Count > 0)
        {
            CheckBox 사용자기준사용자 = (CheckBox)gv선택.HeaderRow.FindControl("cb모두체크");

            foreach (GridViewRow rowitem in gv선택.Rows)
            {
                chk_box = (CheckBox)(rowitem.Cells[0].FindControl("cb체크"));
                chk_box.Checked = (사용자기준사용자.Checked) ? true : false;
            }
        }
    }

    protected void gv메뉴_on모두체크change(object sender, EventArgs e)
    {
        CheckBox chk_box;

        if (gv메뉴.Rows.Count > 0)
        {
            CheckBox 사용자기준부여사용자 = (CheckBox)gv메뉴.HeaderRow.FindControl("cb모두체크");

            foreach (GridViewRow rowitem in gv메뉴.Rows)
            {
                chk_box = (CheckBox)(rowitem.Cells[0].FindControl("cb체크"));
                chk_box.Checked = (사용자기준부여사용자.Checked) ? true : false;
            }
        }
    }
    #endregion

    #region 사용자기준 일괄부여
    protected void bt사용자일괄부여_OnClick(object sender, EventArgs e) 
    {
        if (gv사용자.SelectedValue == null || gv사용자.SelectedValue == "") 
        {
            su.alert(this.Page, "부여할 사용자를 선택해주세요.");
            return;
        }

        int cnt = 0;

        for (int i = 0; i < gv선택.Rows.Count; i++)
        {
            if (((CheckBox)gv선택.Rows[i].FindControl("cb체크")).Checked == true)
            {
                String lbid = ((Label)gv선택.Rows[i].FindControl("lbid")).Text;

                sql = "insert into dbo.l_권한부여로그(권한부여자ID,부여대상자ID,부여권한seq,부여구분) values('" + UserID + "','" + gv사용자.SelectedValue + "','" + lbid + "','부여'); ";
                su.SqlNoneQuery(sql);

                sql = "insert into dbo.t_메뉴권한(직원id,메뉴id) values('" + gv사용자.SelectedValue + "','" + lbid + "');";
                su.SqlNoneQuery(sql);

                cnt++;
            }
        }

        su.alert(this.Page, "총 " + cnt.ToString() + " 건의 권한이 부여되었습니다");
        Button1_Click(null, null);
        return;
    }
    #endregion

    #region 사용자기준 일괄회수
    protected void bt사용자일괄회수_OnClick(object sender, EventArgs e)
    {
        if (gv사용자.SelectedValue == null || gv사용자.SelectedValue == "")
        {
            su.alert(this.Page, "회수할 사용자를 선택해주세요.");
            return;
        }

        int cnt = 0;

        for (int i = 0; i < gv메뉴.Rows.Count; i++)
        {
            if (((CheckBox)gv메뉴.Rows[i].FindControl("cb체크")).Checked == true)
            {
                String lbid = ((Label)gv메뉴.Rows[i].FindControl("lbid")).Text;


                sql = "insert into dbo.l_권한부여로그(권한부여자ID,부여대상자ID,부여권한seq,부여구분) select '" + UserID + "',직원ID,메뉴id,'회수' from dbo.t_메뉴권한 where id='" + lbid + "'; ";
                su.SqlNoneQuery(sql);

                sql = "delete from dbo.t_메뉴권한 where id='" + lbid + "' ";
                su.SqlNoneQuery(sql);

                cnt++;
            }
        }

        su.alert(this.Page, "총 " + cnt.ToString() + " 건의 권한이 회수되었습니다");
        Button1_Click(null, null);
        return;
    }
    #endregion
}