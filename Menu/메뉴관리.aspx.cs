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

public partial class Menu_메뉴관리 : System.Web.UI.Page
{

    SgFramework.SgUtil su = new SgFramework.SgUtil();
    String AgencyID = "";
    String UserID = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        UserID = su.ReqS(this.Page, "userid");

        // 권한 체크
        su.PermitPage(this.Page, "1");

        if (!IsPostBack)
        {
            Button1_Click(null, null);
        }
    }

    // 검색 처리 내역
    protected void Button1_Click(object sender, EventArgs e)
    {
        String 검색어 = su.sg_db_query(tb검색어.Text,true).TrimEnd().TrimStart().Replace(" ","%");
        String sql = "select * from dbo.t_메뉴 where 1=1 ";
        if (검색어 != "")
        {
            sql += " and (메뉴그룹='" + 검색어 + "' or 메뉴명 like '%"+ 검색어 +"%' or 메뉴경로 like '%"+ 검색어 +"%') ";
        }
        sql += " order by id desc ";
        su.binding(GridView1, sql);
    }

	protected void bt추가_Click(object sender, EventArgs e)
	{
        Div_InsertForm.Visible = true;
	}

	protected void bt입력_Click(object sender, EventArgs e)
    {
        String sql = "";
        String 메뉴그룹 = su.sg_db_query(in메뉴그룹.Text.Trim(), true);
        String 메뉴명 = su.sg_db_query(in메뉴명.Text.Trim(), true);
        String 메뉴경로 = su.sg_db_query(in메뉴경로.Text.Trim(), true);
        String 구분 = su.sg_db_query(in구분.Text.Trim(), true);
        String 공통메뉴여부 = su.sg_db_query(in공통메뉴여부.Text.Trim(), true);

        sql = "insert into dbo.t_메뉴(메뉴그룹,메뉴명,메뉴경로,구분,공통메뉴여부) ";
        sql += "values('" + 메뉴그룹 + "','" + 메뉴명 + "','" + 메뉴경로 + "','" + 구분 + "','" + 공통메뉴여부 + "'); select @@identity;";
        String id = su.SqlFieldQuery(sql);
        if (id == "")
        {
            su.alert(this.Page, "메뉴 추가시 오류가 발생하였습니다.");
            return;
        }

        sql = "insert into dbo.t_메뉴권한(메뉴id,직원id) values('"+ id +"' ,'"+ UserID +"'); ";
        su.SqlNoneQuery(sql);

        Div_InsertForm.Visible = false;
        Button1_Click(null, null);

    }

    protected void bt수정_Click(object sender, EventArgs e)
    {
        String seq = upseq.Text.Trim();

        String sql = "";
        String 메뉴그룹 = su.sg_db_query(up메뉴그룹.Text.Trim(), true);
        String 메뉴명 = su.sg_db_query(up메뉴명.Text.Trim(), true);
        String 메뉴경로 = su.sg_db_query(up메뉴경로.Text.Trim(), true);
        String 구분 = su.sg_db_query(up구분.Text.Trim(), true);
        String 공통메뉴여부 = su.sg_db_query(up공통메뉴여부.Text.Trim(), true);

        sql = "update dbo.t_메뉴 set "
            + " 메뉴그룹='" + 메뉴그룹 + "' "
            + ",메뉴명='" + 메뉴명 + "' "
            + ",메뉴경로='" + 메뉴경로 + "' "
            + ",구분='" + 구분 + "' "
            + ",공통메뉴여부='" + 공통메뉴여부 + "' "
            + " where id='" + seq + "' ";
        su.SqlNoneQuery(sql);

        Div_updateForm.Visible = false;

        Button1_Click(null, null);

    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String ename = e.CommandName;
        String id = (String)e.CommandArgument;
        String sql = "";

        if (ename == "ngo")
        {
            Response.Redirect("/menu/메뉴권한관리.aspx?mid="+ id +"");
            return;
        }

        if (ename == "del")
        {
            sql = "delete from dbo.t_메뉴 where id='" + id + "' ";
            su.SqlNoneQuery(sql);

            su.alert(this.Page, "삭제되었습니다");
            Button1_Click(null, null);
            return;
        }

        if (ename == "up")
        {
            sql = "select * from  dbo.t_메뉴 where id='" + id + "' ";
            DataView dv = su.SqlDvQuery(sql);

            if (dv != null && dv.Count > 0)
            {
                upseq.Text = dv[0]["id"].ToString();
                up메뉴그룹.Text = dv[0]["메뉴그룹"].ToString();
                up메뉴명.Text = dv[0]["메뉴명"].ToString();
                up메뉴경로.Text = dv[0]["메뉴경로"].ToString();
                su.setDropDownList(up구분, dv[0]["구분"].ToString());
                su.setDropDownList(up공통메뉴여부, dv[0]["공통메뉴여부"].ToString());
            }

            Div_updateForm.Visible = true;
        }
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        Button1_Click(null, null);
    }

    //플러스 세금계산서 발행 엑셀
    protected void Button4_Click(object sender, EventArgs e)
    {
        GridView1.AllowSorting = false;
        GridView1.AllowPaging = false;
        Button1_Click(null, null);
        if (GridView1.Rows.Count < 1) return;
        GridView2Excel(GridView1, @"메뉴");
        GridView1.AllowSorting = true;
        GridView1.AllowPaging = true;
    }

    #region ★★ GridView2Excel (Page에 EnableEventValidation="false" 도 추가)
    private void GridView2Excel(GridView gv, string sFilename)
    {
        System.Web.HttpResponse Response = HttpContext.Current.Response;

        Response.Clear();
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(sFilename) + ".xls");
        Response.Write(@"<meta http-equiv='Content-Type' content='text/html; charset=utf-8'>");

        gv.AllowSorting = false;
        gv.AllowPaging = false;

        gv.HeaderStyle.BackColor = Color.White;
        gv.HeaderStyle.ForeColor = Color.Black;
        gv.RowStyle.BackColor = Color.White;
        gv.RowStyle.ForeColor = Color.Black;
        gv.FooterStyle.BackColor = Color.White;
        gv.FooterStyle.ForeColor = Color.Black;
        gv.PagerStyle.BackColor = Color.White;
        gv.PagerStyle.ForeColor = Color.Black;
        gv.SelectedRowStyle.BackColor = Color.White;
        gv.SelectedRowStyle.ForeColor = Color.Black;
        gv.AlternatingRowStyle.BackColor = Color.White;
        gv.AlternatingRowStyle.ForeColor = Color.Black;

        PrepareGridViewForExport(gv);
        //gv.DataBind();
        System.IO.StringWriter stringWriter = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
        // HTML 추출 후 바로 저장
        gv.RenderControl(htmlWriter);
        Response.Write("<style> td { mso-number-format:\\@; } </style>" +stringWriter.ToString());

        Response.Flush();
        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    private void PrepareGridViewForExport(Control gv)
    {
        LinkButton lb = new LinkButton();
        Literal l = new Literal();
        string name = String.Empty;
        for (int i = 0; i < gv.Controls.Count; i++)
        {

            if (gv.Controls[i].GetType() == typeof(LinkButton))
            {
                l.Text = (gv.Controls[i] as LinkButton).Text;
                gv.Controls.Remove(gv.Controls[i]);
                gv.Controls.AddAt(i, l);
            }
            else if (gv.Controls[i].GetType() == typeof(DropDownList))
            {
                l.Text = (gv.Controls[i] as DropDownList).SelectedItem.Text;
                gv.Controls.Remove(gv.Controls[i]);
                gv.Controls.AddAt(i, l);
            }
            else if (gv.Controls[i].GetType() == typeof(CheckBox))
            {
                l.Text = (gv.Controls[i] as CheckBox).Checked ? "True" : "False";
                gv.Controls.Remove(gv.Controls[i]);
                gv.Controls.AddAt(i, l);
            }
            else if (gv.Controls[i].GetType() == typeof(HyperLink))
            {
                l.Text = (gv.Controls[i] as HyperLink).Text;
                gv.Controls.Remove(gv.Controls[i]);
                gv.Controls.AddAt(i, l);
            }

            if (gv.Controls[i].HasControls())
            {
                PrepareGridViewForExport(gv.Controls[i]);
            }
        }

    }
    #endregion

    protected void bt입력닫기_Click(object sender, EventArgs e)
    {
        Div_InsertForm.Visible = false;
    }
    protected void bt수정닫기_Click(object sender, EventArgs e)
    {
        Div_updateForm.Visible = false;
    }
}