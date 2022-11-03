using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Menu_브랜드상품관리 : System.Web.UI.Page
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

        if (su.Req(this.Page, "debug") != "")
        {
            선택_브랜드코드.Visible = true;
            선택_상품코드.Visible = true;
        }

        if (!IsPostBack)
        {
            bt새로고침_Click(null, null);
        }
    }

    #region 새로고침 검색
    protected void bt새로고침_Click(object sender, EventArgs e)
    {

        sql = @"SELECT a.idx,a.brand_name,convert(varchar(10),a.regist_date,121) regist_date,convert(varchar(10),a.modify_date,121) modify_date, b.cnt FROM coffee_brand a  with(nolock) left outer join (select distinct brand_idx, count(*) cnt from coffee with(nolock) where isusing=1 and machine_idx=0 group by brand_idx) b on a.idx = b.brand_idx where a.machine_idx=0 and a.isusing=1 order by  brand_name ; ";
        su.binding( gv브랜드, sql);

        String 브랜드코드 = 선택_브랜드코드.Text.Trim();

        sql = "SELECT a.idx,a.brand_idx,a.unit,a.NAME,a.comt,a.imagebase64_s,a.imageext, convert(int,a.imagesize/1000) imagesize";
        sql += " FROM coffee a left outer join coffee_brand b on a.brand_idx = b.idx ";
        sql += " where a.brand_idx='" + 브랜드코드 + "' and a.isusing=1 and a.machine_idx=0";
        sql += " order by b.brand_name,a.NAME";

        //Response.Write(sql);
        su.binding(gv상품, sql);

    }

    #endregion

    #region gv브랜드 선택시
    protected void gv브랜드_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String ename = e.CommandName;
        String strval = (String)e.CommandArgument;

        if (ename == "Select")
        {
            선택_브랜드코드.Text = strval;
            bt새로고침_Click(null, null);
        }

        if(ename== "brand_up")
        {
            브랜드수정_idx.Text = strval;
            선택_브랜드코드.Text = strval;
            bt브랜드수정_Click(null, null);
        }
    }
    #endregion

    #region gv상품 선택시
    protected void gv상품_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String ename = e.CommandName;
        String strval = (String)e.CommandArgument;

        if (ename == "Select")
        {
            상품수정_상품코드.Text = strval;
            선택_상품코드.Text = strval;
            div_상품수정.Visible = true;
            bt상품수정_Click(null, null);
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

    #region 브랜드등록 
    protected void bt브랜드등록_Click(object sender, EventArgs e)
    {
        div_브랜드등록.Visible = true; 
    }

    protected void bt브랜드등록닫기_Click(object sender, EventArgs e)
    {
        div_브랜드등록.Visible = false;
    }

    protected void bt브랜드등록완료_Click(object sender, EventArgs e)
    {
        String 브랜드명 = su.sg_db_query(브랜드등록_브랜드명.Text.TrimStart().TrimEnd());

        if (브랜드명 == "" || su.isSptext(브랜드명)==true)
        {
            브랜드등록_결과.InnerText = "브랜드명이 없거나 특수문자가 포함되어 있습니다. ";
            return;
        }

        sql = "select count(*) from coffee_brand where brand_name='" + 브랜드명+"' and machine_idx=0 ";
        String chk1 = su.SqlFieldQuery(sql);

        if (chk1 != "0")
        {
            브랜드등록_결과.InnerText = "이미 사용하고 있는 브랜드명 입니다.";
            return;
        }

        sql = "insert into coffee_brand(brand_name,regist_date,modify_date,machine_idx ) ";
        sql += " values('"+ 브랜드명 + "',getdate(),getdate(),0); ";
        su.SqlNoneQuery(sql);

        브랜드등록_결과.InnerText = "등록완료되었습니다";

        bt새로고침_Click(null, null);
        div_브랜드등록.Visible = false;
    }
    #endregion

    #region 브랜드수정/삭제

    protected void bt브랜드수정_Click(object sender, EventArgs e)
    {
        String 브랜드코드 = 브랜드수정_idx.Text.Trim();
        if (브랜드코드 == "")
        {
            su.sg_alert(this.Page, "브랜드을 선택해주세요");
            return;
        }

        sql = "select brand_name from coffee_brand where idx='"+ 브랜드코드 + "' and machine_idx=0 ";
        DataView dv = su.SqlDvQuery(sql);
        if(dv!=null && dv.Count > 0)
        {
            브랜드수정_브랜드명.Text = dv[0]["brand_name"].ToString();
            div_브랜드수정.Visible = true;
        }
    }

    protected void bt브랜드수정닫기_Click(object sender, EventArgs e)
    {
        div_브랜드수정.Visible = false;
    }

    protected void bt브랜드수정완료_Click(object sender, EventArgs e)
    {
        String 코드 = su.sg_db_query(브랜드수정_idx.Text.Trim()) ;
        String 브랜드명 = su.sg_db_query(브랜드수정_브랜드명.Text.TrimStart().TrimEnd());

        sql = "select count(*) from coffee_brand where brand_name='" + 브랜드명 + "' and machine_idx=0 and idx<>'"+ 코드 + "' ";
        String chk1 = su.SqlFieldQuery(sql);

        if (chk1 != "0")
        {
            브랜드등록_결과.InnerText = "이미 사용하고 있는 브랜드명 입니다.";
            return;
        }

        if (코드 == "")
        {
            브랜드수정_결과.InnerText = "브랜드를 선택해주세요";
            return;
        }

        if (브랜드명 == "" || su.isSptext(브랜드명)==true)
        {
            브랜드수정_결과.InnerText = "브랜드명을 입력해주세요(특수문자 사용불가)";
            return;
        }
        sql = "update coffee_brand set ";
        sql += "  brand_name='" + 브랜드명 + "' ";
        sql += " where idx='" + 코드 + "' ";
        su.SqlNoneQuery(sql);

        브랜드수정_결과.InnerText = "수정완료되었습니다";

        bt새로고침_Click(null, null);

    }

    protected void bt브랜드삭제완료_Click(object sender, EventArgs e)
    {
        String cnt = "";
        String 코드 = su.sg_db_query(브랜드수정_idx.Text.Trim());
        if (코드 == "")
        {
            브랜드수정_결과.InnerText = "브랜드을 선택해주세요";
            return;
        }

        sql = "select count(*) from coffee where brand_idx='"+ 코드 + "' ";
        cnt= su.SqlFieldQuery(sql);

        if (cnt != "0")
        {
            브랜드수정_결과.InnerText = "등록된 상품이 있습니다. 삭제할 수 없습니다. 등록된 상품을 먼저 삭제해주시기 바랍니다";
            return;
        }
        sql = "update coffee_brand set isusing=0 where idx='" + 코드 + "' ";
        su.SqlNoneQuery(sql);

        div_브랜드수정.Visible = false;
        bt새로고침_Click(null, null);        
    }

    #endregion

    #region 상품등록/수정삭제 

    protected void bt상품등록_Click(object sender, EventArgs e)
    {
        String 브랜드코드 = 선택_브랜드코드.Text.Trim();

        sql = "select brand_name,idx from coffee_brand order by brand_name";
        su.binding(상품등록_브랜드코드, sql, "브랜드을 선택해 주세요");

        if (브랜드코드 != "")
        {
            su.setDropDownList(상품등록_브랜드코드, 브랜드코드);
        }


        div_상품등록.Visible = true;
    }

    protected void bt상품등록완료_Click(object sender, EventArgs e)
    {
        String 브랜드코드 = 상품등록_브랜드코드.SelectedValue;

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
            if(확장자!="png" && 확장자 != "jpg")
            {
                상품등록_결과.InnerText = "상품이미지는 png 또는 jpg 만 사용 가능합니다.";
                return;
            }

            if( 상품등록_이미지.FileContent.Length/1000/1000 >1.0)
            {
                상품등록_결과.InnerText = "상품이미지는 1MB를 초과할수 없습니다.";
                return;
            }

            System.Drawing.Image im = System.Drawing.Image.FromStream(상품등록_이미지.FileContent);
            이미지 = su.BitmapToBase64(su.ResizeImage(im, 160, 160));
            이미지s = su.BitmapToBase64(su.ResizeImage(im, 100, 100));
            이미지사이즈 = su.CntBytesLen(이미지).ToString();
        }

        if (브랜드코드 == "")
        {
            상품등록_결과.InnerText = "등록할 브랜드을 선택해 주세요";
            return;
        }

        if (상품명 == "" || su.isSptext(상품명)==true)
        {
            상품등록_결과.InnerText = "상품명을 입력해주세요(특수문자사용불가)";
            return;
        }

        sql = "select count(*) from coffee where brand_idx='"+ 브랜드코드 +"' and name='" + 상품명 + "' ";
        String chk1 = su.SqlFieldQuery(sql);

        if (chk1 != "0")
        {
            상품등록_결과.InnerText = "이미 등록된 상품명 입니다.";
            return;
        }


        sql = "insert into coffee(brand_idx,unit,NAME,comt,imagebase64,imageext,imagesize,imagebase64_s) ";
        sql += " values('" + 브랜드코드 + "','" + 단위 + "','" + 상품명 + "','" + comt + "','" + 이미지 + "','" + 확장자 + "','"+ 이미지사이즈+"','"+이미지s+"'); ";
        su.SqlNoneQuery(sql);

        상품등록_결과.InnerText = "등록완료되었습니다";

        bt새로고침_Click(null, null);
        div_상품등록.Visible = false;
    }

    protected void bt상품등록닫기_Click(object sender, EventArgs e)
    {
        div_상품등록.Visible = false;
    }

    #endregion

    #region 상품삭제

    protected void bt상품삭제완료_Click(object sender, EventArgs e)
    {

        String 브랜드코드 = 선택_브랜드코드.Text.Trim();
        if (브랜드코드 == "")
        {
            div_상품수정결과.InnerText = "브랜드을 선택해주세요";
            return;
        }

        String 상품코드 = 상품수정_상품코드.Text.Trim();
        if (브랜드코드 == "")
        {
            div_상품수정결과.InnerText = "삭제할 상품를 선택해 주세요";
            return;
        }

        sql = "update coffee set isusing=0 where idx='"+ 상품코드 +"' ";
        su.SqlNoneQuery(sql);

        div_상품수정결과.InnerText = "삭제되었습니다";

        div_상품수정.Visible = false;
        bt새로고침_Click(null, null);

    }

    #endregion

    #region 상품수정
    protected void bt상품수정_Click(object sender, EventArgs e)
    {

        String 브랜드코드 = 선택_브랜드코드.Text.Trim();
        if (브랜드코드 == "")
        {
            div_상품수정결과.InnerText = "브랜드을 선택해주세요";
            return;
        }

        String 상품코드 = 선택_상품코드.Text.Trim();
        if (상품코드 == "")
        {
            div_상품수정결과.InnerText = "수정할 상품을 선택해주세요";
            return;
        }

        sql = "select brand_name,idx from coffee_brand order by brand_name";
        su.binding(상품수정_브랜드코드, sql, "브랜드을 선택해 주세요");

        sql = "select a.idx,a.brand_idx,a.unit,a.NAME,a.comt,a.imagebase64,a.imageext, convert(int,a.imagesize/1000) imagesize, a.pr_type from coffee a where idx='" + 상품코드 +"'  ";

        DataView dv = su.SqlDvQuery(sql);
        if (dv != null && dv.Count > 0)
        {

            su.setDropDownList(상품수정_브랜드코드, dv[0]["brand_idx"].ToString());
            su.setDropDownList(상품수정_투출타입, dv[0]["pr_type"].ToString());
            su.setDropDownList(상품수정_단위, dv[0]["unit"].ToString());
            상품수정_상품명.Text = dv[0]["name"].ToString();
            상품수정_비고.Text = dv[0]["comt"].ToString();

            div_상품수정.Visible = true;
        }

    }

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
        sql += " name='"+ 상품명+"',";
        sql += " brand_idx='" + 브랜드코드 + "',";
        sql += " unit='" + 단위 + "',";
        sql += " pr_type='" + 투출타입 + "',";
        sql += " imagebase64='" + 이미지 + "',";
        sql += " imagebase64_s='" + 이미지s + "',";
        sql += " imageext='" + 확장자 + "',";
        sql += " imagesize='" + 이미지사이즈 + "' ";
        sql += " where idx='" + 상품코드 + "' ";
        su.SqlNoneQuery(sql);

        div_상품수정결과.InnerText = "";

        div_상품수정.Visible = false;

        bt새로고침_Click(null, null);
    }

    protected void bt상품수정닫기_Click(object sender, EventArgs e)
    {
        bt새로고침_Click(null, null);
        div_상품수정.Visible = false;
    }
    #endregion

}
