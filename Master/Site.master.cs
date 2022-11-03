using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class SiteMaster : System.Web.UI.MasterPage
{

    public SgFramework.SgUtil su = new SgFramework.SgUtil();
    public String 타임아웃시간 = "0";
    public String UserID = "";
    public String PathInfo = "";
    public String QueryString = "";

    public String sql = "";
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
    public String AccessClientip = "";
    
    protected void Page_Load(object sender, EventArgs e)
    {

        String ServerPort = Request.ServerVariables["SERVER_PORT"].ToString();
        String HTTP_HOST = Request.ServerVariables["HTTP_HOST"].ToString();
        PathInfo = Request.ServerVariables["PATH_INFO"].ToString();
        QueryString = Request.ServerVariables["QUERY_STRING"].ToString();
        String HTTPSYN = Request.ServerVariables["HTTPS"].ToString();
        String UserID = su.ReqS(this.Page, "userid");
        AccessClientip = su.GetIP(this.Page);

        if (UserID == "")
        {
            Response.Redirect("/Account/Login.aspx?ReturnUrl="+Server.UrlEncode(su.GetFullURL(this.Page)));
            return;
        }

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
        MenuItem m = null;

        NavigationMenu.Items.Clear();

        sql = "select distinct 메뉴그룹, max(id) id from ( select distinct 메뉴그룹,max(b.id) id from t_메뉴권한 a left outer join t_메뉴 b on a.메뉴ID = b.ID where a.직원ID='" + UserID + "' and b.구분='메뉴' group by 메뉴그룹 ";
        sql += " union all ";
        sql += " select distinct b.메뉴그룹,max(b.id) from t_메뉴 b where  b.공통메뉴여부='Y' and b.구분='메뉴' group by 메뉴그룹 ) aa group by 메뉴그룹";

        DataView dv = su.SqlDvQuery(sql);
        if (dv != null && dv.Count > 0)
        {

            for (int i = 0; i < dv.Count; i++)
            {
                MenuItem mn = new MenuItem(dv[i]["메뉴그룹"].ToString(), dv[i]["id"].ToString(), null, null);
                NavigationMenu.Items.Add(mn);
            }

        }

        dv = null;

        sql = "select * from (select b.메뉴명, b.메뉴경로,b.메뉴그룹,b.id from t_메뉴권한 a left outer join t_메뉴 b on a.메뉴ID = b.ID where ( a.직원ID='" + UserID + "' ) and b.구분='메뉴' ";
        sql += " union all ";
        sql += " select b.메뉴명, b.메뉴경로,b.메뉴그룹,b.id from t_메뉴 b where  b.공통메뉴여부='Y' and b.구분='메뉴') aa order by 메뉴명 ";

        dv = su.SqlDvQuery(sql);

        if (dv != null && dv.Count > 0)
        {
            for (int i = 0; i < dv.Count; i++)
            {
                MenuItem sm = new MenuItem(dv[i]["메뉴명"].ToString() + " " + dv[i]["id"].ToString(), dv[i]["메뉴경로"].ToString(), null, dv[i]["메뉴경로"].ToString());
                NavigationMenu.Items[FindMenu(NavigationMenu, dv[i]["메뉴그룹"].ToString())].ChildItems.Add(sm);
            }

        }

        NavigationMenu.StaticDisplayLevels = 1;
        NavigationMenu.StaticSubMenuIndent = 95;
        NavigationMenu.DynamicVerticalOffset = 0;
        NavigationMenu.DynamicHorizontalOffset = 0;
        NavigationMenu.MaximumDynamicDisplayLevels = 3;
        NavigationMenu.StaticEnableDefaultPopOutImage = false;
        NavigationMenu.ItemWrap = true;
        NavigationMenu.TabIndex = 0;


        m = new MenuItem("로그아웃", "menu9", null, "/Account/Logout.aspx", "_self");
        NavigationMenu.Items.Add(m);

    }
    protected void bt_alert닫기_Click(object sender, EventArgs e)
    {
        su.sg_alert_c(this.Page);
    }


    public int FindMenu(Menu mn, String ms)
    {
        int retVal = 100;
        try
        {
            for (int i = 0; i < mn.Items.Count; i++)
            {
                if (mn.Items[i].Text == ms)
                {
                    retVal = i;
                    break;
                }
            }
        }
        catch (Exception ex)
        {

        }
        return retVal;
    }

}
